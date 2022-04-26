using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tech.Weather.Model;
using Tech.Weather.DAL;


namespace Tech.Weather.Entites
{
    public class Request
    {
        public Dictionary<string, WeatherModel> CitiesWeatherDic = new Dictionary<string, WeatherModel>();
        APICityRequset DataCitiesFromApi = new APICityRequset();
        bool keepRunningOrStop = true;

       
        public void LoadCityTableFromAPI()
        {
            CitiesWeatherDic = DataCitiesFromApi.Load();
        }

        public async Task<Dictionary<string, WeatherModel>> AddCity(string cityName)
        {
            var searchCity = await DataCitiesFromApi.GetCityData(cityName);
            if (!CitiesWeatherDic.ContainsKey(cityName))
            {
                CitiesWeatherDic.Add(searchCity.location.name, searchCity);
            }
            else
            {
                CitiesWeatherDic[searchCity.location.name] = searchCity;
                //Update(searchCity);

            }

            return CitiesWeatherDic;
        }

        public void Update(WeatherModel searchCity)
        {
          
            CitiesWeatherDic[searchCity.location.name] = searchCity;

            
        }

        public void Save()
        {
            DataCitiesFromApi.Save(CitiesWeatherDic);
        }

        public void RefreshTableByUser(string cityName,int timeByUser)
        {
            Task.Run(async() =>
            {
                while(keepRunningOrStop)
                {
                    APICityRequset apiRequest = new APICityRequset();
                    WeatherModel oneCityWeather;
                    oneCityWeather = await apiRequest.GetCityData(cityName);
                    apiRequest.SaveOne(oneCityWeather);

                    System.Threading.Thread.Sleep(timeByUser * 1000);
                }
            });
        }

        public void StopRefresh()
        {
            keepRunningOrStop = false;
        }

        public async Task UpdateList()
        {
            var citiesWeatherList = DataCitiesFromApi.Load();
          
            foreach (var WeatherCity in citiesWeatherList)
            {
                
                var cityWeatherData = await DataCitiesFromApi.GetCityData(WeatherCity.Key);

                if (WeatherCity.Value.current.temp_c != cityWeatherData.current.temp_c)
                {
                    Update(cityWeatherData);
                }
            }
            DataCitiesFromApi.Save(CitiesWeatherDic);
        }

        public void Load()
        {
            CitiesWeatherDic = DataCitiesFromApi.Load();
        }




        public static async Task<string> GetCity(string cityname)
        {

            WeatherModel weatherCallResult;
            APICityRequset api = new APICityRequset();

            weatherCallResult = await api.GetCityData(cityname);

            return $"City:{weatherCallResult.location.name}\nTemperature:{weatherCallResult.current.temp_c}";

        }
      
    }
}

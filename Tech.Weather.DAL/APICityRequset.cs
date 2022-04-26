using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Text.Json;
using Tech.Weather.Model;
using System.IO;

namespace Tech.Weather.DAL
{
    public class APICityRequset
    {
        readonly string tempInOneCity = (@"C:\Users\yosef\source\repos\Project.Tech.Weather\Tech.Weather.Model\Data\LastTempInOneCity.txt");
        readonly string WeatherListFromFile = (@"C:\Users\yosef\source\repos\Project.Tech.Weather\Tech.Weather.Model\Data\DicWeatherList.txt");
        
        public async Task<WeatherModel> GetCityData(string nameCity)
        {

            WeatherModel weather;
            using (var httpC = new HttpClient())
            {
                httpC.BaseAddress = new Uri("http://api.weatherapi.com/");
                httpC.DefaultRequestHeaders.Accept.Clear();
                httpC.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage pespones = await httpC.GetAsync($@"v1/current.json?key=b480e7a490374b44be472511222103&q={nameCity}& aqi=no");

                string lines = await pespones.Content.ReadAsStringAsync();
                weather = JsonSerializer.Deserialize<WeatherModel>(lines);

                return weather;
            }



        }

        public Dictionary<string,WeatherModel> Load()
        {
            string citiesWeaterData = File.ReadAllText(WeatherListFromFile);
            var weatherDic = JsonSerializer.Deserialize<Dictionary<string, WeatherModel>>(citiesWeaterData);
            return weatherDic;
        }

     
        public void Save(Dictionary<string, WeatherModel> CitiesWeatherDic)
        {
            string dataFromWeatherDic = JsonSerializer.Serialize(CitiesWeatherDic);

            File.WriteAllText(WeatherListFromFile, dataFromWeatherDic);

        }

        public void SaveOne(WeatherModel oneCityWeather)
        {
            string dataCityWeather = JsonSerializer.Serialize(oneCityWeather);

            File.WriteAllText(tempInOneCity, dataCityWeather);

        }

        public WeatherModel LoadOneCity()
        {
            string oneCityData = File.ReadAllText(tempInOneCity);
            var oneCity = JsonSerializer.Deserialize<WeatherModel>(oneCityData);

            return oneCity;
        }

        public Dictionary<string,WeatherModel> LoadAll()
        {
            string allcitiesData = File.ReadAllText(WeatherListFromFile);
            var allcitiesweatherDic = JsonSerializer.Deserialize<Dictionary<string, WeatherModel>>(allcitiesData);
            return allcitiesweatherDic;
        }
    }
}

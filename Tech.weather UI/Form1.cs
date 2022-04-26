using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tech.Weather.Entites;
using Tech.Weather.DAL;
using System.IO;
using Tech.Weather.Model;

namespace Tech.weather_UI
{
    public partial class Form1 : Form
    {
        Request req = new Request();
        readonly string WeatherListFromFile = (@"C:\Users\yosef\source\repos\Project.Tech.Weather\Tech.Weather.Model\Data\DicWeatherList.txt");
        APICityRequset DataCitiesFromFile = new APICityRequset();

        public Form1()
        {
            InitializeComponent();
            if (File.Exists(WeatherListFromFile))
            {
                weatherTablePopulate();

            }
            

        }



        private async void button1_Click(object sender, EventArgs e)
        {
            string cityName = City_Name.Text;
            int timeRefreshByUser = int.Parse(timeByUser.Text);

            if(cityName!= null  & cityName != "" )
            {
                label1.Text = await Request.GetCity(City_Name.Text);
                await req.AddCity(cityName);
                req.RefreshTableByUser(cityName, timeRefreshByUser);
            }
            else
            {
                MessageBox.Show("please enter name of city!");
            }

            //label1.Text = await Request.GetCity(City_Name.Text);

            //await req.AddCity(City_Name.Text);

            //req.RefreshTableByUser(City_Name.Text, int.Parse(timeByUser.Text));

        }

        private void button3_Click(object sender, EventArgs e)
        {
            req.StopRefresh();
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            WeatherModel w = DataCitiesFromFile.LoadOneCity();
            label1.Text = await Request.GetCity(w.location.name);

        }

        private void button4_Click(object sender, EventArgs e)
        {
            
            req.Save();
            weatherTablePopulate();
        }

        public void weatherTablePopulate()
        {
            DataTable newTable = new DataTable();
            newTable.Columns.Add("City", typeof(string));
            newTable.Columns.Add("Temperature", typeof(decimal));
            req.Load();

            foreach (var city in req.CitiesWeatherDic)
            {
                newTable.Rows.Add(city.Key, city.Value.current.temp_c);
               
            }
            dataGridView1.DataSource = newTable;
        }

        private async void button5_Click(object sender, EventArgs e)
        {
           await req.UpdateList();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void timeByUser_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

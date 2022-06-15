using System;
using System.IO;
using System.ComponentModel;
using System.Globalization;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Xml;


namespace Sensors.Weather
{
    internal class WebWeather : IRefresher, INotifyPropertyChanged
    {
        private const string OPEN_WEATHER_KEY = "adac93f3a057d268edd730c32733714e";
        private const string URL = "http://api.openweathermap.org/data/2.5/weather?q=@LOC@&mode=xml&units=metric&APPID=@API_KEY@";

        private XmlElement _xmlWeather;
        private double _temperature;
        private double _humidity;
        private double _pressure;
        private double _windSpeed;
        private double _windDirection;



        public event PropertyChangedEventHandler PropertyChanged;

        internal WebWeather()
        {
            RefreshValues();
        }

        public void RefreshValues()
        {
            RequestWeather()
                .ContinueWith((result) => ReadValues(result.Result)
                .ContinueWith((result) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Value")));
        }

        private async Task RequestWeather()
        {
            try
            {
                string requestUrl = URL.Replace("@LOC@", "Paris").Replace("@API_KEY@", OPEN_WEATHER_KEY);

                HttpClient client = new();
                HttpResponseMessage response = await client.GetAsync(url);

                if(response.StatusCode == HttpStatusCode.OK)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    
                    XmlDocument xmlDocument = new();
                    xmlDocument.LoadXml(content);
                    _xmlWeather = xmlDocument.DocumentElement;
                }
                else
                {
                    Console.WriteLine($"IOT: error while get weather : {response.ReasonPhrase}");
                    return null;
                }                
            }
            catch(Exception ex)
            {
                Console.WriteLine("IOT: Exception during reading Weather: " + ex.Message);
                return null;
            }
        }

        private double ReadNodeValue(string nodeName)
        {
            XmlNode node = _xmlWeather.SelectSingleNode(nodeName);

            string? stringValue =  node.Attributes["value"].Value;

            double value = double.NaN;

            double.TryParse(stringValue, out value);

            return  value; 
        }

        
        private Task ReadValues()
        {
            _temperature = ReadNodeValue("temperature");
            _humidity = ReadNodeValue("humidity");
            _pressure = ReadNodeValue("pressure");
            _windSpeed = ReadNodeValue("wind/speed");
            _windDirection = ReadNodeValue("wind/direction");

            return Task.CompletedTask;
        }

        public double GetTemperature()
        {
            return _temperature;
        }

        public double GetHumidity()
        {
            string humidity = GetNodeValue(new string[] { "humidity" }, "value");

            return humidity != null ? double.Parse(humidity, CultureInfo.InvariantCulture) : double.NaN;
        }

        public double GetPressure()
        {
            return _pressure;
        }

        public double GetWindSpeed()
        {
            return _windSpeed;
        }

        public double GetWindDirection()
        {
            return _windDirection;
        }
    }
}

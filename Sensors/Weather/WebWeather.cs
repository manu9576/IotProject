using System;
using System.ComponentModel;
using System.Globalization;
using System.Net;
using System.Xml;

namespace Sensors.Weather
{
    internal class WebWeather : IRefresher, INotifyPropertyChanged
    {
        private const string OpenWeatherKey = "adac93f3a057d268edd730c32733714e";
        private const string URL = "http://api.openweathermap.org/data/2.5/weather?q=@LOC@&mode=xml&units=metric&APPID=@API_KEY@";

        private XmlDocument xmlDocument;

        public event PropertyChangedEventHandler PropertyChanged;

        internal WebWeather()
        {
            Refresh();
        }

        public void Refresh()
        {
            ReadWeather(URL.Replace("@LOC@", "Paris").Replace("@API_KEY@", OpenWeatherKey));
        }

        // Return the XML result of the URL.
        private void ReadWeather(string url)
        {
            try
            {
                // Create a web client.
                using WebClient client = new WebClient();
                // Get the response string from the URL.
                string xml = client.DownloadString(url);

                xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xml);

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Value"));
            }
            catch(Exception ex)
            {
                Console.WriteLine("IOT: Exception during reading Weather: " + ex.Message);
            }
        }

        private string GetNodeValue(string[] nodesName, string attribute)
        {
            var xpath = string.Join('/', nodesName);

            var node = xmlDocument.DocumentElement.SelectSingleNode(xpath);

            return node.Attributes[attribute]?.Value;
        }

        public double GetTemperature()
        {
            var temperature = GetNodeValue(new string[] { "temperature" }, "value");

            return temperature != null ? double.Parse(temperature, CultureInfo.InvariantCulture) : double.NaN;
        }

        public double GetHumidity()
        {
            var humidity = GetNodeValue(new string[] { "humidity" }, "value");

            return humidity != null ? double.Parse(humidity, CultureInfo.InvariantCulture) : double.NaN;
        }

        public double GetPressure()
        {
            var pressure = GetNodeValue(new string[] { "pressure" }, "value");

            return pressure != null ? double.Parse(pressure, CultureInfo.InvariantCulture) : double.NaN;
        }

        public double GetWindSpeed()
        {
            var windSpeed = GetNodeValue(new string[] { "wind", "speed" }, "value");

            return windSpeed != null ? double.Parse(windSpeed, CultureInfo.InvariantCulture) : double.NaN;
        }

        public double GetWindDirection()
        {
            var windDirection = GetNodeValue(new string[] { "wind", "direction" }, "value");

            return windDirection != null ? double.Parse(windDirection, CultureInfo.InvariantCulture) : double.NaN;
        }
    }
}

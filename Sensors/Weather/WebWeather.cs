using System.Globalization;
using System.Net;
using System.Xml;

namespace Sensors.Weather
{
    internal class WebWeather : IRefresher
    {
        private const string OpenWeatherKey = "adac93f3a057d268edd730c32733714e";
        private const string URL = "http://api.openweathermap.org/data/2.5/weather?q=@LOC@&mode=xml&units=metric&APPID=@API_KEY@";

        private XmlDocument xmlDocument;

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
            // Create a web client.
            using WebClient client = new WebClient();
            // Get the response string from the URL.
            string xml = client.DownloadString(url);

            xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xml);
        }

        private string GetNodeValue(string[] nodesName, string attribute)
        {
            var xpath = string.Join('/', nodesName);

            var node = xmlDocument.DocumentElement.SelectSingleNode(xpath);

            return node.Attributes[attribute].Value;
        }

        public double GetTemperature()
        {
            var temperature = GetNodeValue(new string[] { "temperature" }, "value");

            return double.Parse(temperature, CultureInfo.InvariantCulture);
        }

        public double GetHumidity()
        {
            var humidity = GetNodeValue(new string[] { "humidity" }, "value");

            return double.Parse(humidity, CultureInfo.InvariantCulture);
        }

        public double GetPressure()
        {
            var pressure = GetNodeValue(new string[] { "pressure" }, "value");

            return double.Parse(pressure, CultureInfo.InvariantCulture);
        }

        public double GetWindSpeed()
        {
            var windSpeed = GetNodeValue(new string[] { "wind", "speed" }, "value");

            return double.Parse(windSpeed, CultureInfo.InvariantCulture);
        }

        public double GetWindDirection()
        {
            var windDirection = GetNodeValue(new string[] { "wind", "direction" }, "value");

            return double.Parse(windDirection, CultureInfo.InvariantCulture);
        }
    }
}

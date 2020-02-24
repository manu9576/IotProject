using System.Globalization;
using System.Net;
using System.Xml;

namespace Sensors.Weather
{
    internal class WebWeather : IRefresher
    {
        internal WebWeather()
        {
            Refresh();
        }

        private const string OpenWeatherKey = "adac93f3a057d268edd730c32733714e";
        private const string URL = "http://api.openweathermap.org/data/2.5/weather?q=@LOC@&mode=xml&units=metric&APPID=@API_KEY@";

        private XmlDocument xmlDocument;

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

        private string GetNodeValue(string nodeName, string attribute)
        {
            var nodes = xmlDocument.DocumentElement.SelectNodes(nodeName);

            return  nodes[0].Attributes[attribute].Value;

        }


        public double GetTemperature()
        {
            var temperature = GetNodeValue("temperature", "value");

            return double.Parse(temperature, CultureInfo.InvariantCulture);
        }


        /** Format : 
          <?xml version="1.0" encoding="UTF-8"?>
            <current>
              <city id="2988507" name="Paris">
                <coord lon="2.35" lat="48.85">
                </coord>
                <country>FR</country>
                <timezone>3600</timezone>
                <sun rise="2020-02-16T06:58:23" set="2020-02-16T17:11:18">
                </sun>
              </city>
              <temperature value="16.2" min="14.44" max="17.22" unit="celsius">
              </temperature>
              <feels_like value="9.36" unit="celsius">
              </feels_like>
              <humidity value="72" unit="%">
              </humidity>
              <pressure value="1007" unit="hPa">
              </pressure>
              <wind>
                <speed value="10.3" unit="m/s" name="Fresh Breeze">
                </speed>
                <gusts value="19.5">
                </gusts>
                <direction value="220" code="SW" name="Southwest">
                </direction>
              </wind>
              <clouds value="90" name="overcast clouds">
              </clouds>
              <visibility value="10000">
              </visibility>
              <precipitation mode="no">
              </precipitation>
              <weather number="500" value="light rain" icon="10n">
              </weather>
              <lastupdate value="2020-02-16T17:32:07">
              </lastupdate>
            </current>
    **/
    }
}

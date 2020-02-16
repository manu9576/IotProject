using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace Sensors.Weather
{
    public class Weather : ISensor
    {
        public Weather()
        {
            cancellationTokenSource = new CancellationTokenSource();
            Start();
        }

        private const string OpenWeatherKey = "adac93f3a057d268edd730c32733714e";
        private const string URL = "http://api.openweathermap.org/data/2.5/weather?q=@LOC@&mode=xml&units=metric&APPID=@API_KEY@";
        private CancellationTokenSource cancellationTokenSource;

        public string Name => "Meteo";

        private double _value;
        private bool IsRunning;

        public double Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }

        public string Unit => "°C";


        public void Refresh()
        {
            ReadWeather(URL.Replace("@LOC@", "Paris").Replace("@API_KEY@", OpenWeatherKey));
        }

        // Return the XML result of the URL.
        private void ReadWeather(string url)
        {
            // Create a web client.
            using (WebClient client = new WebClient())
            {
                // Get the response string from the URL.
                string xml = client.DownloadString(url);

                // Load the response into an XML document.
                XmlDocument xml_document = new XmlDocument();
                xml_document.LoadXml(xml);

                // Format the XML.
                using (StringWriter string_writer = new StringWriter())
                {
                    XmlTextWriter xml_text_writer =
                        new XmlTextWriter(string_writer);
                    xml_text_writer.Formatting = Formatting.Indented;
                    xml_document.WriteTo(xml_text_writer);

                    var nodes = xml_document.GetElementsByTagName("temperature");
                    string val = nodes[0].Attributes["value"]?.InnerText.Replace(".", ",");

                    double.TryParse(val, out _value);

                }
            }
        }

        private void PeriodicRefreshTask(int intervalInMS, CancellationToken cancellationToken)
        {

            Task.Run(async () =>
            {
                while (true)
                {

                    Refresh();

                    await Task.Delay(intervalInMS, cancellationToken);

                    if (cancellationToken.IsCancellationRequested)
                        break;
                }
            });
        }


        private void Start(int intervalInMs = 30000)
        {
            if (IsRunning)
            {
                throw new Exception("The refresher is already running");
            }

            IsRunning = true;
            PeriodicRefreshTask(intervalInMs, cancellationTokenSource.Token);
        }

        public void Stop()
        {
            IsRunning = false;
            cancellationTokenSource.Cancel();
            cancellationTokenSource = new CancellationTokenSource();
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

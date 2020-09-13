using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Sensors.Configuration
{
    [Serializable]
    public class SensorsConfiguration
    {
        private const string DEFAULT_CONFIGURATION_FILE = "DefaultConfiguration.xml";
        private const string CONFIGURATION_FILE = "Configuration.xml";

        private readonly static string _configPath;
        private readonly static string _defaultConfigPath;

        public List<SensorConfiguration> Sensors { get; set; }

        static SensorsConfiguration()
        {
            var configFolder = AppDomain.CurrentDomain.BaseDirectory;

            _configPath = Path.Combine(configFolder, CONFIGURATION_FILE);
            _defaultConfigPath = Path.Combine(configFolder, DEFAULT_CONFIGURATION_FILE);
        }

        public SensorsConfiguration()
        {
            Sensors = new List<SensorConfiguration>();
        }

        public static SensorsConfiguration Load()
        {
            SensorsConfiguration readedConfiguration = null;

            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(SensorsConfiguration));

                if (File.Exists(_configPath))
                {
                    using (StreamReader streamReader = new StreamReader(_configPath))
                    {
                        readedConfiguration = xmlSerializer.Deserialize(streamReader) as SensorsConfiguration;
                    }
                }
                else
                {
                    using (StreamReader streamReader = new StreamReader(_defaultConfigPath))
                    {
                        readedConfiguration = xmlSerializer.Deserialize(streamReader) as SensorsConfiguration;
                    }

                    readedConfiguration.Save();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while reading configuration file: " + ex.Message);
                readedConfiguration = new SensorsConfiguration();
            }

            Console.WriteLine("IotProject configuration");
            Console.WriteLine(readedConfiguration);

            return readedConfiguration;
        }

        public bool Save()
        {
            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(SensorsConfiguration));
                using (StreamWriter streamWriter = new StreamWriter(_configPath))
                {
                    xmlSerializer.Serialize(streamWriter, this);
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while saving configuration file: " + ex.Message);
            }

            return false;
        }

        public override string ToString()
        {
            StringBuilder configuration = new StringBuilder();

            foreach(var sensor in Sensors)
            {
                configuration.AppendLine(sensor.ToString());
            }

            return configuration.ToString();
        }
    }
}

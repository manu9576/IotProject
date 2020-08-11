using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Sensors.Configuration
{
    [Serializable]
    public class SensorsConfiguration
    {
        private const string DEFAULT_CONFIGURATION_FILE = "DefaultConfiguration.xml";
        private const string CONFIGURATION_FILE = "Configuration.xml";

        public List<SensorConfiguration> Sensors { get; set; }

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

                if (File.Exists(CONFIGURATION_FILE))
                {
                    using (StreamReader streamReader = new StreamReader(CONFIGURATION_FILE))
                    {
                        readedConfiguration = xmlSerializer.Deserialize(streamReader) as SensorsConfiguration;
                    }
                }
                else
                {
                    using (StreamReader streamReader = new StreamReader(DEFAULT_CONFIGURATION_FILE))
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

            return readedConfiguration;
        }

        public bool Save()
        {
            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(SensorsConfiguration));
                using (StreamWriter streamWriter = new StreamWriter(CONFIGURATION_FILE))
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

    }
}

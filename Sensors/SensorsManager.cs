using Sensors.Configuration;
using System.Collections.ObjectModel;

namespace Sensors
{
    public static class SensorsManager
    {
        public static ObservableCollection<ISensor> Sensors { get; }
        private static SensorsConfiguration _sensorsConfiguration;

        static SensorsManager()
        {
            Sensors = new ObservableCollection<ISensor>();
            ReloadConfiguration();
        }

        static public void ReloadConfiguration()
        {
            Sensors.Clear();
            _sensorsConfiguration = SensorsConfiguration.Load();

            foreach (var sensorConfiguration in _sensorsConfiguration.Sensors)
            {
                var sensor = SensorsBuilder.GetSensor(sensorConfiguration);

                Sensors.Add(sensor);
            }
        }
    }
}

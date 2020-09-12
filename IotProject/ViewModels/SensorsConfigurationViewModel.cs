using ReactiveUI;
using Sensors;
using Sensors.Configuration;
using Storage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;

namespace IotProject.ViewModels
{
    public class SensorsConfigurationViewModel : ViewModelBase
    {
        public ReactiveCommand<Unit, Unit> AddIotSensor { get; }
        public ReactiveCommand<Unit, Unit> AddWeatherSensor { get; }
        public ReactiveCommand<Unit, Unit> ApplyConfiguration { get; }
        public ReactiveCommand<string, Unit > RemoveSensor { get; }

        public SensorsConfiguration SensorsConfiguration { get; private set; }

        public ObservableCollection<SensorConfiguration> Sensors { get; private set; }

        public SensorsConfigurationViewModel()
        {
            SensorsConfiguration = SensorsConfiguration.Load();

            Sensors = new ObservableCollection<SensorConfiguration>( SensorsConfiguration.Sensors);

            AddIotSensor = ReactiveCommand.Create(AddNewIotSensor);
            AddWeatherSensor = ReactiveCommand.Create(AddNewWeatherSensor);
            ApplyConfiguration = ReactiveCommand.Create(ApplyNewConfiguration);

            RemoveSensor = ReactiveCommand.Create<string>(ApplyRemoveSensor);
        }

        private void AddNewIotSensor() => Sensors.Add(new GrovePiSensorConfiguration()); 

        private void AddNewWeatherSensor() => Sensors.Add(new OpenWeatherMapSensorConfiguration());

        private void ApplyNewConfiguration()
        {
            foreach(var sensor in Sensors)
            {
                if(sensor.SensorId == -1)
                {
                    sensor.SensorId = SensorsStorage.GetNewSensorId(sensor.Name);
                }
            }

            SensorsConfiguration.Sensors = new List<SensorConfiguration>(Sensors);

            SensorsConfiguration.Save();

            SensorsManager.ReloadConfiguration();
        }

        private void ApplyRemoveSensor(string sensorHashCode)
        {
            var sensorConfigurationToRemove = Sensors.FirstOrDefault(sen => sen.HashCode == sensorHashCode);

            if(sensorConfigurationToRemove != null)
            {
                Sensors.Remove(sensorConfigurationToRemove);
            }

        }
    }
}

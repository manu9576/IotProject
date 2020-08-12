using ReactiveUI;
using Sensors;
using Sensors.Configuration;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;

namespace IotProject.ViewModels
{
    public class SensorsConfigurationViewModel : ViewModelBase
    {
        public ReactiveCommand<Unit, Unit> AddIotSensor { get; }
        public ReactiveCommand<Unit, Unit> AddWeatherSensor { get; }
        public ReactiveCommand<Unit, Unit> ApplyConfiguration { get; }

        public SensorsConfiguration SensorsConfiguration { get; private set; }

        public ObservableCollection<SensorConfiguration> Sensors { get; private set; }

        public SensorsConfigurationViewModel()
        {
            SensorsConfiguration = SensorsConfiguration.Load();

            Sensors = new ObservableCollection<SensorConfiguration>( SensorsConfiguration.Sensors);

            AddIotSensor = ReactiveCommand.Create(AddNewIotSensor);
            AddWeatherSensor = ReactiveCommand.Create(AddNewWeatherSensor);

            ApplyConfiguration = ReactiveCommand.Create(ApplyNewConfiguration);
        }

        private void AddNewIotSensor() => Sensors.Add(new GrovePiSensorConfiguration()); 

        private void AddNewWeatherSensor() => Sensors.Add(new OpenWeatherMapSensorConfiguration());

        private void ApplyNewConfiguration()
        {
            SensorsConfiguration.Sensors = new List<SensorConfiguration>(Sensors);

            SensorsConfiguration.Save();

            SensorsManager.ReloadConfiguration();
        }


    }
}

using ReactiveUI;
using Sensors;
using Sensors.GrovePi;
using Sensors.Weather;
using System.Collections.ObjectModel;
using System.Reactive;

namespace IotProject.ViewModels
{
    public class SensorsConfigurationViewModel : ViewModelBase
    {
        public ReactiveCommand<Unit, Unit> AddIotSensor { get; }
        public ReactiveCommand<Unit, Unit> AddWeatherSensor { get; }

        public ObservableCollection<ISensor> Sensors { get; private set; }

        public SensorsConfigurationViewModel()
        {
            Sensors = SensorsManager.Sensors;

            AddIotSensor = ReactiveCommand.Create(AddNewIotSensor);
            AddWeatherSensor = ReactiveCommand.Create(AddNewWeatherSensor);
        }

        private void AddNewIotSensor()
        {
            Sensors.Add(GrovePiSensorBuilder.CreateSensor(SensorType.DhtHumiditySensor, Iot.Device.GrovePiDevice.Models.GrovePort.DigitalPin2, "Nouvelle voie"));
        }

        private void AddNewWeatherSensor()
        {
            Sensors.Add(WeatherSensorBuilder.GetSensor(SensorWeatherType.Temperature,"Température"));
        }

    }
}

using Iot.Device.GrovePiDevice;
using Iot.Device.GrovePiDevice.Models;
using Iot.Device.GrovePiDevice.Sensors;
using ReactiveUI;
using System;
using System.Device.I2c;
using System.Threading;
using System.Threading.Tasks;

namespace IotProject.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {

            I2cConnectionSettings i2CConnectionSettings = new I2cConnectionSettings(1, GrovePi.DefaultI2cAddress);
            _grovePi = new GrovePi(I2cDevice.Create(i2CConnectionSettings));

            _dhtSensor = new DhtSensor(_grovePi, GrovePort.DigitalPin7, DhtType.Dht11);

            Task.Run(() => UpdateValues());
        }

        private void UpdateValues()
        {
            while (true)
            {

                TimeOfDay = DateTime.Now.ToString("dd/MM/yy HH:mm:ss");
                _dhtSensor.Read();
                Sensors = $"Température = {_dhtSensor.LastTemperature} °C (Humidité = {_dhtSensor.LastRelativeHumidity} %)";
                Thread.Sleep(1000);
            }
        }

        private string _timeOfDay;
        private string _sensors;
        private GrovePi _grovePi;
        private DhtSensor _dhtSensor;

        public string TimeOfDay
        {
            get => _timeOfDay;
            set => this.RaiseAndSetIfChanged(ref _timeOfDay, value);
        }

        public string Sensors
        {
            get => _sensors;
            set => this.RaiseAndSetIfChanged(ref _sensors, value);
        }
    }
}

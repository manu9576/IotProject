using Iot.Device.GrovePiDevice;
using Iot.Device.GrovePiDevice.Models;
using Iot.Device.GrovePiDevice.Sensors;
using ReactiveUI;
using Sensors.GrovePi;
using System;
using System.Collections.Generic;
using System.Device.I2c;
using System.Threading;
using System.Threading.Tasks;

namespace IotProject.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            SensorsViewModel = new List<SensorViewModel>
            {
                new SensorViewModel(SensorType.DhtTemperatureSensor, GrovePort.DigitalPin7, "Température"),
                new SensorViewModel(SensorType.DhtHumiditySensor, GrovePort.DigitalPin7, "Humidité")
            };
            Task.Run(() => UpdateValues());
        }

        private void UpdateValues()
        {
            while (true)
            {
                TimeOfDay = DateTime.Now.ToString("dd/MM/yy HH:mm:ss");
                foreach (var vm in SensorsViewModel)
                {
                    vm.Refresh();
                }
                Thread.Sleep(1000);
            }
        }

        private string _timeOfDay;

        public List<SensorViewModel> SensorsViewModel
        {
            get;
            private set;
        }


        public string TimeOfDay
        {
            get => _timeOfDay;
            set => this.RaiseAndSetIfChanged(ref _timeOfDay, value);
        }

    }
}

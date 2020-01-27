using Iot.Device.GrovePiDevice.Models;
using ReactiveUI;
using Sensors;
using Sensors.GrovePi;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading;
using System.Threading.Tasks;

namespace IotProject.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly SensorsManager sensorManager;

        public MainWindowViewModel()
        {
            sensorManager = new SensorsManager();

            SensorsViewModel = new ObservableCollection<SensorViewModel>();

            foreach (var sensor in sensorManager.Sensors)
            {
                SensorsViewModel.Add(new SensorViewModel(sensor));
            }

            Task.Run(() => UpdateValues());

            Close = ReactiveCommand.Create(RunClose);
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

        public ObservableCollection<SensorViewModel> SensorsViewModel
        {
            get;
            private set;
        }

        public string TimeOfDay
        {
            get => _timeOfDay;
            set => this.RaiseAndSetIfChanged(ref _timeOfDay, value);
        }

        public ReactiveCommand<Unit, Unit> Close { get; }

        void RunClose()
        {
            Environment.Exit(0);
        }
    }

}

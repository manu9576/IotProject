using ReactiveUI;
using Sensors;
using Storage;
using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading;
using System.Threading.Tasks;

namespace IotProject.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly CancellationTokenSource cancellationTokenSource;
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

        public MainWindowViewModel()
        {
            cancellationTokenSource = new CancellationTokenSource();
            SensorsViewModel = new ObservableCollection<SensorViewModel>();

            foreach (var sensor in SensorsManager.Sensors)
            {
                SensorsViewModel.Add(new SensorViewModel(sensor));
            }

            Task.Run(() => UpdateValues(1000, cancellationTokenSource.Token));

            Close = ReactiveCommand.Create(RunClose);

            // TODO: Add a try catch for connection error to the DB
            sensorsStorage = SensorsStorage.GetInstance();
            sensorsStorage.Start(10);
        }

        private void UpdateValues(int intervalInMS, CancellationToken cancellationToken)
        {
            Task.Run(async () =>
            {
                while (true)
                {

                    TimeOfDay = DateTime.Now.ToString("dd/MM/yy HH:mm:ss");
                    foreach (var vm in SensorsViewModel)
                    {
                        vm.Refresh();
                    }

                    await Task.Delay(intervalInMS, cancellationToken);

                    if (cancellationToken.IsCancellationRequested)
                        break;
                }
            });
        }

        public ReactiveCommand<Unit, Unit> Close { get; }

        private SensorsStorage sensorsStorage;

        void RunClose()
        {
            sensorsStorage.Stop();
            cancellationTokenSource.Cancel();
            Environment.Exit(0);
        }
    }

}

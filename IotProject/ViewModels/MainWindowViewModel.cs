using Avalonia;
using Avalonia.Controls;
using IotProject.Views;
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
        private readonly SensorsStorage sensorsStorage;

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

        public ReactiveCommand<Window, Unit> DisplayConfig { get; }

        public MainWindowViewModel()
        {
            cancellationTokenSource = new CancellationTokenSource();
            SensorsViewModel = new ObservableCollection<SensorViewModel>();

            foreach (var sensor in SensorsManager.Sensors)
            {
                SensorsViewModel.Add(new SensorViewModel(sensor));
            }

            Task.Run(() => UpdateValues(900, cancellationTokenSource.Token));

            Close = ReactiveCommand.Create(RunClose);
            DisplayConfig = ReactiveCommand.Create<Window>(ShowConfig);

#if DEBUG
            sensorsStorage = SensorsStorage.GetInstance();
            sensorsStorage.Start(10);

#else
            sensorsStorage = SensorsStorage.GetInstance();
            sensorsStorage.Start(1800);

#endif
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

        private void RunClose()
        {
            sensorsStorage.Stop();
            cancellationTokenSource.Cancel();
            Environment.Exit(0);
        }

        private void ShowConfig(Window mainWindow)
        {
            var window = new ConfigurationWindow();
            window.ShowDialog(mainWindow);

        }
    }

}

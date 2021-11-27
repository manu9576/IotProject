using ReactiveUI;
using Storage;
using System;
using System.Reactive;
using System.Threading;
using System.Threading.Tasks;

namespace IotProject.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        ViewModelBase content;
        private readonly CancellationTokenSource cancellationTokenSource;
        private string _timeOfDay;
        private readonly ISensorsStorage sensorsStorage;
        public SensorsConfigurationViewModel SensorsConfigurationViewModel { get; }
        public SensorsMeasureViewModel SensorsMeasureViewModel {get;}
        public bool _isConfigMode;

        public ViewModelBase Content
        {
            get => content;
            private set => this.RaiseAndSetIfChanged(ref content, value);
        }

        public string TimeOfDay
        {
            get => _timeOfDay;
            set => this.RaiseAndSetIfChanged(ref _timeOfDay, value);
        }

        public bool IsConfigMode
        {
            get => _isConfigMode;
            set => this.RaiseAndSetIfChanged(ref _isConfigMode, value);
    }

    public ReactiveCommand<Unit, Unit> Close { get; }

        public ReactiveCommand<Unit, Unit> SwitchDisplay { get; }

        public MainWindowViewModel()
        {
            cancellationTokenSource = new CancellationTokenSource();

            Close = ReactiveCommand.Create(RunClose);
            SwitchDisplay = ReactiveCommand.Create(SwitchingDisplay);

            SensorsConfigurationViewModel = new SensorsConfigurationViewModel();
            SensorsMeasureViewModel = new SensorsMeasureViewModel();

            Content = SensorsMeasureViewModel;

            Task.Run(() => UpdateValues(900, cancellationTokenSource.Token));

            IsConfigMode = false;

#if DEBUG
            sensorsStorage = SensorsStorage.Instance;
            sensorsStorage.Start(10);

#else
            sensorsStorage = SensorsStorage.Instance;
            sensorsStorage.Start(1800);
#endif
        }

        private void SwitchingDisplay()
        {
            if (IsConfigMode)
            {
                Content = SensorsMeasureViewModel;
                IsConfigMode = false;
            }
            else
            {
                Content = SensorsConfigurationViewModel;
                IsConfigMode = true;
            }
        }


        private void RunClose()
        {
            sensorsStorage.Stop();
            cancellationTokenSource.Cancel();
            Environment.Exit(0);
        }


        private void UpdateValues(int intervalInMS, CancellationToken cancellationToken)
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    TimeOfDay = DateTime.Now.ToString("dd/MM/yy HH:mm:ss");

                    await Task.Delay(intervalInMS, cancellationToken);

                    if (cancellationToken.IsCancellationRequested)
                        break;
                }
            });
        }
    }

}

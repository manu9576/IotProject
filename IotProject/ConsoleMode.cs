using Sensors;
using Sensors.GrovePi;
using Storage;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace IotProject
{
    public class ConsoleMode
    {
        private readonly CancellationTokenSource cancellationTokenSource;
        private readonly ObservableCollection<ISensor> sensors;
        private SensorsStorage sensorsStorage;

        public ConsoleMode()
        {
            sensors = SensorsManager.Sensors;
            cancellationTokenSource = new CancellationTokenSource();
        }

        public bool IsRunning { get; private set; }

        public void Start()
        {
            IsRunning = true;
            PeriodicRefreshTask(2000, cancellationTokenSource.Token);
            sensorsStorage = SensorsStorage.GetInstance();
            sensorsStorage.Start(10);
        }

        public void Stop()
        {
            cancellationTokenSource.Cancel();
            IsRunning = false;
        }

        public void WriteLCD(string message)
        {
            var rgbDisplay = GrovePiRgbLcdDisplay.BuildRgbLcdDisplayImpl();
            rgbDisplay.SetText(message);
        }

        private void PeriodicRefreshTask(int intervalInMS, CancellationToken cancellationToken)
        {
            var rgbDisplay= GrovePiRgbLcdDisplay.BuildRgbLcdDisplayImpl();

            rgbDisplay.SetBacklightRgb(10, 10, 10);
            Task.Run(async () =>
            {
                while (true)
                {
                    foreach (var sensor in sensors)
                    {
                        Console.WriteLine(sensor.Name +": " + sensor.Value.ToString("0.0") + sensor.Unit);
                        rgbDisplay.SetText(sensor.Name + ": \n" + sensor.Value.ToString("0.0") + sensor.Unit);
                        await Task.Delay(intervalInMS, cancellationToken);
                    }

                    if (cancellationToken.IsCancellationRequested)
                        break;
                }
            });
        }
    }
}

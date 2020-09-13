using Sensors;
using Sensors.GrovePi;
using Storage;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace IotProject
{
    public class ServiceMode
    {
        private readonly CancellationTokenSource cancellationTokenSource;
        private readonly ObservableCollection<ISensor> sensors;
        private SensorsStorage sensorsStorage;

        public ServiceMode()
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
            sensorsStorage.Start(30 * 60);
        }

        public void Stop()
        {
            cancellationTokenSource.Cancel();
            IsRunning = false;
        }

        public static void WriteLCD(string message)
        {
            var rgbDisplay = GrovePiRgbLcdDisplay.BuildRgbLcdDisplayImpl();
            rgbDisplay.SetText(message);
        }

        private void PeriodicRefreshTask(int intervalInMS, CancellationToken cancellationToken)
        {
            var rgbDisplay = GrovePiRgbLcdDisplay.BuildRgbLcdDisplayImpl();

            rgbDisplay.SetBacklightRgb(10, 10, 10);
            Task.Run(async () =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        foreach (var sensor in sensors)
                        {
                            rgbDisplay.SetText(sensor.Name, sensor.Value.ToString("0.0") + " " + sensor.Unit);
                            await Task.Delay(intervalInMS, cancellationToken);
                        }

                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine("IotProject error: " + ex.Message + " - " + ex.StackTrace);
                    }
                }
            });
        }

    }
}

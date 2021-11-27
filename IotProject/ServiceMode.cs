using Sensors;
using Sensors.GrovePi;
using Storage;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IotProject
{
    public class ServiceMode
    {
        private readonly CancellationTokenSource cancellationTokenSource;
        private readonly ObservableCollection<ISensor> sensors;
        private ISensorsStorage sensorsStorage;

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
            sensorsStorage = SensorsStorage.Instance;
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
                        foreach (var sensor in sensors.Where(sen => sen.RgbDisplay))
                        {
                            rgbDisplay.SetText(sensor.Name, sensor.Value.ToString("0.0") + " " + sensor.Unit);
                            if (!cancellationToken.IsCancellationRequested)
                            {
                                await Task.Delay(intervalInMS, cancellationToken);
                            }
                        }

                    }
                    catch (TaskCanceledException)
                    {
                        Console.WriteLine("IotProject PeriodicRefreshTask has been cancelled");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("IotProject error: " + ex.Message + " - " + ex.StackTrace);
                    }
                }
            });
        }

    }
}

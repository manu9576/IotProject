using Sensors;
using Sensors.GrovePi;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace IotProject
{
    public class ConsoleMode
    {
        private CancellationTokenSource cancellationTokenSource;
        private ObservableCollection<ISensor> sensors;

        public ConsoleMode()
        {
            sensors = SensorsManager.Sensors;
            cancellationTokenSource = new CancellationTokenSource();
        }

        public void Start()
        {
            PeriodicRefreshTask(2000, cancellationTokenSource.Token);
            Console.Read();
        }


        private void PeriodicRefreshTask(int intervalInMS, CancellationToken cancellationToken)
        {
            var rgbDisplay= GrovePiRgbLcdDisplay.BuildRgbLcdDisplayImpl();

            rgbDisplay.SetBacklightRgb(20, 20, 20);
            Task.Run(async () =>
            {
                while (true)
                {
                    foreach (var sensor in sensors)
                    {
                        Console.WriteLine(sensor.Name +": " + sensor.Value + sensor.Unit);
                        rgbDisplay.SetText(sensor.Name + ": \n" + sensor.Value + sensor.Unit);
                        await Task.Delay(intervalInMS, cancellationToken);

                    }

                    if (cancellationToken.IsCancellationRequested)
                        break;
                }
            });
        }
    }
}

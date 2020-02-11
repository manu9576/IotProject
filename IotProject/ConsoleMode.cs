using GrovePiDevice.Sensors;
using Sensors;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IotProject
{
    public class ConsoleMode
    {
        private readonly CancellationTokenSource cancellationTokenSource;
        private readonly ObservableCollection<ISensor> sensors;

        public ConsoleMode()
        {
            sensors = SensorsManager.Sensors;
            cancellationTokenSource = new CancellationTokenSource();
        }

        public void Start()
        {
            PeriodicRefreshTask(1000, cancellationTokenSource.Token);
            Console.Read();
        }


        private void PeriodicRefreshTask(int intervalInMS, CancellationToken cancellationToken)
        {
            var rgbDisplay= RgbLcdDisplay.BuildRgbLcdDisplayImpl();

            rgbDisplay.SetBacklightRgb(255, 0, 0);
            Task.Run(async () =>
            {
                while (true)
                {
                    foreach (var sensor in sensors)
                    {
                        Console.WriteLine(sensor.Name +": " + sensor.Value + sensor.Unit);
                        rgbDisplay.SetText(sensor.Name + ": " + sensor.Value + sensor.Unit);
                        await Task.Delay(intervalInMS, cancellationToken);

                    }


                    if (cancellationToken.IsCancellationRequested)
                        break;
                }
            });
        }
    }
}

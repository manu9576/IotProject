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
        private CancellationTokenSource cancellationTokenSource;
        private ObservableCollection<ISensor> sensors;

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

            Task.Run(async () =>
            {
                while (true)
                {
                    foreach (var sensor in sensors)
                    {
                        Console.WriteLine(sensor.Name +": " + sensor.Value + sensor.Unit);
                        await Task.Delay(intervalInMS, cancellationToken);
                    }


                    if (cancellationToken.IsCancellationRequested)
                        break;
                }
            });
        }
    }
}

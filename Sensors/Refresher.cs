using Sensors.Weather;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Sensors
{
    internal class Refresher
    {
        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly List<IRefresher> sensors = new List<IRefresher>();

        private bool isRunning;

        public void AddSensor(IRefresher sensor)
        {
            if (!sensors.Contains(sensor))
            {
                sensors.Add(sensor);
            }
        }

        public void Start(int intervalInMs = 5000)
        {
            if (isRunning)
            {
                throw new Exception("The refresher is already running");
            }

            isRunning = true;
            PeriodicRefreshTask(intervalInMs, cancellationTokenSource.Token);
        }

        public void Stop()
        {
            isRunning = false;
            cancellationTokenSource.Cancel();
            cancellationTokenSource = new CancellationTokenSource();
        }

        private void PeriodicRefreshTask(int intervalInMS, CancellationToken cancellationToken)
        {

            Task.Run(async () =>
            {
                while (true)
                {
                    foreach (var sensor in sensors)
                    {
                        sensor.Refresh();
                    }

                    await Task.Delay(intervalInMS, cancellationToken);

                    if (cancellationToken.IsCancellationRequested)
                        break;
                }
            });
        }
    }
}

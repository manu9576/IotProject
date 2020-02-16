using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Sensors.GrovePi
{
    internal class Refresher
    {
        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly List<ISensor> sensors = new List<ISensor>();
        
        public bool IsRunning { get; private set ; }

        public void AddSensor(ISensor sensor)
        {
            if (!sensors.Contains(sensor))
            {
                sensors.Add(sensor);
            }
        }


        public void Start(int intervalInMs = 2000)
        {
            if (IsRunning)
            {
                throw new Exception("The refresher is already running");
            }

            IsRunning = true;
            PeriodicRefreshTask(intervalInMs, cancellationTokenSource.Token);
        }

        public void Stop()
        {
            IsRunning = false;
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

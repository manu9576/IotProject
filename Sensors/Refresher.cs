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
            else
            {
                Console.WriteLine("Refresher: Warning Adding already present sensor");
            }
        }

        public void Start(int intervalInMs = 5000)
        {
            if (isRunning)
            {
                throw new Exception("The refresher is already running");
            }

            PeriodicRefreshTask(intervalInMs, cancellationTokenSource.Token);
        }

        public void Stop()
        {
            cancellationTokenSource.Cancel();
            cancellationTokenSource = new CancellationTokenSource();
        }

        private async void PeriodicRefreshTask(int intervalInMS, CancellationToken cancellationToken)
        {
            isRunning = true;
            while (!cancellationToken.IsCancellationRequested)
            {
                foreach (var sensor in sensors)
                {
                    try
                    {
                        sensor.RefreshValues();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error while refreshing sensor " + ex.Message);
                    }
                }
                await Task.Delay(intervalInMS, cancellationToken);
            }
            isRunning = false;
        }
    }
}

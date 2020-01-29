using Iot.Device.GrovePiDevice.Models;
using Sensors.GrovePi;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace Sensors
{

    public static class SensorsManager
    {
        public static ObservableCollection<ISensor> Sensors { private set; get; }
       

        static SensorsManager()
        {
            Sensors = new ObservableCollection<ISensor>
            {
                GrovePiSensorBuilder.CreateSensor(SensorType.DhtTemperatureSensor, GrovePort.DigitalPin7, "Température"),
                GrovePiSensorBuilder.CreateSensor(SensorType.DhtHumiditySensor, GrovePort.DigitalPin7, "Humidité")
            };

            cancellationTokenSource = new CancellationTokenSource();

            PeriodicRefreshTask(1000, cancellationTokenSource.Token);
        }

        private static void PeriodicRefreshTask(int intervalInMS, CancellationToken cancellationToken)
        {

            Task.Run(async () =>
            {
                while (true)
                {

                    foreach (var sensor in Sensors)
                        sensor.Refresh();

                    await Task.Delay(intervalInMS, cancellationToken);

                    if (cancellationToken.IsCancellationRequested)
                        break;
                }
            });
        }
    }
}

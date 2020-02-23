using Iot.Device.GrovePiDevice.Models;
using Iot.Device.GrovePiDevice.Sensors;
using Sensors.Weather;

namespace Sensors.GrovePi
{
    internal class GrovePiDthHumiditySensor : GrovePiSensor, IRefresher
    {
        public GrovePiDthHumiditySensor(DhtSensor dhtSensor, string name, GrovePort port)
            : base(name, "%", SensorType.DhtHumiditySensor, port)
        {
            this.DhtSensor = dhtSensor;
        }

        public override double Value => DhtSensor.LastRelativeHumidity;

        public DhtSensor DhtSensor { get; }

        public override void Refresh()
        {
            System.Console.WriteLine("Refresh Humidity");
            DhtSensor.Read();
        }
    }
}
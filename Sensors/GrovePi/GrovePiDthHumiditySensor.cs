using Iot.Device.GrovePiDevice.Models;
using Iot.Device.GrovePiDevice.Sensors;

namespace Sensors.GrovePi
{
    internal class GrovePiDthHumiditySensor : GrovePiSensor
    {
        private readonly DhtSensor dhtSensor;
        private double value;

        public GrovePiDthHumiditySensor(DhtSensor dhtSensor, string name, GrovePort port)
            : base(name, "%", SensorType.DhtHumiditySensor, port)
        {
            this.dhtSensor = dhtSensor;
        }

        public override double Value => value;

        public DhtSensor DhtSensor => dhtSensor;

        public override void Refresh()
        {
            dhtSensor.Read();
            value = dhtSensor.LastRelativeHumidity;
        }
    }
}
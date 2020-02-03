using Iot.Device.GrovePiDevice.Models;
using Iot.Device.GrovePiDevice.Sensors;

namespace Sensors.GrovePi
{
    internal class GrovePiDthTemperatureSensor : GrovePiSensor
    {
        private readonly DhtSensor dhtSensor;
        public double value;

        public GrovePiDthTemperatureSensor(DhtSensor dhtSensor, string name, GrovePort port)
            : base(name, "°C", SensorType.DhtTemperatureSensor, port)
        {
            this.dhtSensor = dhtSensor;
        }


        public override double Value => value;

        public DhtSensor DhtSensor => dhtSensor;

        public override void Refresh()
        {
            DhtSensor.Read();
            value = DhtSensor.LastTemperature;
        }
    }
}
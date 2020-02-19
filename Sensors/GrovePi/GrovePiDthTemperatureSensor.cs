using Iot.Device.GrovePiDevice.Models;
using Iot.Device.GrovePiDevice.Sensors;

namespace Sensors.GrovePi
{
    internal class GrovePiDthTemperatureSensor : GrovePiSensor
    {
        public GrovePiDthTemperatureSensor(DhtSensor dhtSensor, string name, GrovePort port)
            : base(name, "°C", SensorType.DhtTemperatureSensor, port)
        {
            this.DhtSensor = dhtSensor;
        }

        public override double Value =>DhtSensor.LastTemperature;

        public DhtSensor DhtSensor { get; }

        public override void Refresh()
        {
            System.Console.WriteLine("Refresh Temperauture");

            DhtSensor.Read();
        }
    }
}
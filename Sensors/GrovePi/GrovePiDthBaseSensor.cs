using Iot.Device.GrovePiDevice.Sensors;

namespace Sensors.GrovePi
{
    internal class GrovePiDthBaseSensor
    {
        internal DhtSensor dhtSensor;

        public GrovePiDthBaseSensor(DhtSensor dhtSensor, string name)
        {
            this.dhtSensor = dhtSensor;
            Name = name;
        }

        public string Name { get; private set; }
    }
}
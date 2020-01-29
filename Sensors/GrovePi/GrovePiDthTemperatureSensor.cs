using Iot.Device.GrovePiDevice.Sensors;

namespace Sensors.GrovePi
{
    internal class GrovePiDthTemperatureSensor : GrovePiDthBaseSensor, ISensor
    {
        public double value;

        public GrovePiDthTemperatureSensor(DhtSensor dhtSensor, string name) : base(dhtSensor,name)
        {
        }

        public string Unit => "°C";

        public double Value
        {
            get
            {
                return value;
            }
        }

        public void Refresh()
        {
            dhtSensor.Read();
            value = dhtSensor.LastTemperature;
        }
    }
}
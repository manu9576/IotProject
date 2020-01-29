using Iot.Device.GrovePiDevice.Sensors;

namespace Sensors.GrovePi
{
    internal class GrovePiDthHumiditySensor : GrovePiDthBaseSensor, ISensor
    {
        private double value;

        public GrovePiDthHumiditySensor(DhtSensor dhtSensor, string name) : base(dhtSensor, name)
        {
        }

        public string Unit => "%";

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
            value = dhtSensor.LastRelativeHumidity;
        }
    }
}
using Iot.Device.GrovePiDevice.Sensors;

namespace Sensors.GrovePi
{
    internal class GrovePiDthHumiditySensor : GrovePiDthBaseSensor, ISensor
    {

        public GrovePiDthHumiditySensor(DhtSensor dhtSensor, string name) : base(dhtSensor, name)
        {
        }

        public string Unit => "%";

        public double Value
        {
            get
            {
                dhtSensor.Read();
                return dhtSensor.LastRelativeHumidity;
            }
        }
    }
}
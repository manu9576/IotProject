using Iot.Device.GrovePiDevice.Sensors;

namespace Sensors.GrovePi
{
    internal class GrovePiDthTemperatureSensor : GrovePiDthBaseSensor, ISensor
    {

        public GrovePiDthTemperatureSensor(DhtSensor dhtSensor, string name) : base(dhtSensor,name)
        {
        }

        public string Unit => "°C";

        public double Value
        {
            get
            {
                dhtSensor.Read();
                return dhtSensor.LastTemperature;
            }
        }
    }
}
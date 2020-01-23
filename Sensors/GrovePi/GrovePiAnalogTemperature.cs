using Iot.Device.GrovePiDevice.Sensors;

namespace Sensors.GrovePi
{
    internal class GrovePiAnalogTemperature : GrovePiAnalogSensor, IGrovePiSensor
    {

        internal GrovePiAnalogTemperature(GroveTemperatureSensor analogSensor, string name) :
            base(analogSensor, name)
        {
        }

        public override double Value
        {
            get
            {
                return (_analogSensor as GroveTemperatureSensor).Temperature;
            }
        }

        public override string Unit => "°C";
    }
}

using Iot.Device.GrovePiDevice.Sensors;

namespace Sensors.GrovePi
{
    internal class GrovePiAnalogTemperature : GrovePiAnalogSensor, ISensor
    {
        private double value;

        internal GrovePiAnalogTemperature(GroveTemperatureSensor analogSensor, string name) :
            base(analogSensor, name)
        {
        }

        public override double Value
        {
            get
            {
                return value;
            }
        }

        public override void Refresh()
        {
            value = (_analogSensor as GroveTemperatureSensor).Temperature; 
        }


        public override string Unit => "°C";
    }
}

using Iot.Device.GrovePiDevice.Sensors;

namespace Sensors.GrovePi
{
    internal class GrovePiAnalogPotentiometer : GrovePiAnalogSensor, ISensor
    {

        internal GrovePiAnalogPotentiometer(PotentiometerSensor analogSensor, string name) :
            base(analogSensor, name)
        {
        }

        public override double Value
        {
            get
            {
                return (_analogSensor as PotentiometerSensor).ValueAsPercent;
            }
        }

        public override string Unit => "%";
    }
}

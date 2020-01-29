using Iot.Device.GrovePiDevice.Sensors;

namespace Sensors.GrovePi
{
    internal class GrovePiAnalogPotentiometer : GrovePiAnalogSensor, ISensor
    {
        private double value;

        internal GrovePiAnalogPotentiometer(PotentiometerSensor analogSensor, string name) :
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
            value = (_analogSensor as PotentiometerSensor).ValueAsPercent;
        }

        public override string Unit => "%";
    }
}

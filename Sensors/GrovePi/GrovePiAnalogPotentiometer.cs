using Iot.Device.GrovePiDevice.Models;
using Iot.Device.GrovePiDevice.Sensors;

namespace Sensors.GrovePi
{
    internal class GrovePiAnalogPotentiometer : GrovePiAnalogSensor
    {
        internal GrovePiAnalogPotentiometer(PotentiometerSensor analogSensor, string name, GrovePort port)
            : base(analogSensor, name, port)
        {
        }

        public override void Refresh()
        {
            value = (_analogSensor as PotentiometerSensor).ValueAsPercent;
        }
    }
}

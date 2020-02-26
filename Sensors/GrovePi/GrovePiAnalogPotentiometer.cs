using Iot.Device.GrovePiDevice.Models;
using Iot.Device.GrovePiDevice.Sensors;
using Sensors.Weather;

namespace Sensors.GrovePi
{
    internal class GrovePiAnalogPotentiometer : GrovePiAnalogSensor, IRefresher
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

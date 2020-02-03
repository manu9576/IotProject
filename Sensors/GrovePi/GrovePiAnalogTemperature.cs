using Iot.Device.GrovePiDevice.Models;
using Iot.Device.GrovePiDevice.Sensors;

namespace Sensors.GrovePi
{
    internal class GrovePiAnalogTemperature : GrovePiAnalogSensor
    {

        internal GrovePiAnalogTemperature(GroveTemperatureSensor analogSensor, string name, GrovePort port) 
            : base(analogSensor, name, port)
        {
        }

        public override double Value => value;

        public override void Refresh()
        {
            value = (_analogSensor as GroveTemperatureSensor).Temperature; 
        }
    }
}

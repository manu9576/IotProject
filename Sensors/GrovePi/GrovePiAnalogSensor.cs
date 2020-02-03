using Iot.Device.GrovePiDevice.Models;
using Iot.Device.GrovePiDevice.Sensors;

namespace Sensors.GrovePi
{
    internal class GrovePiAnalogSensor : GrovePiSensor
    {
        protected AnalogSensor _analogSensor;
        protected double value;

        internal GrovePiAnalogSensor(AnalogSensor analogSensor, string name, GrovePort port):
            base(name,"V",SensorType.AnalogSensor, port)
        {
            _analogSensor = analogSensor;
        }

        public override double Value => value;
        public override void Refresh() => value = _analogSensor.Value;
       
    }
}

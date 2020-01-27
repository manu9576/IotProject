using Iot.Device.GrovePiDevice.Sensors;

namespace Sensors.GrovePi
{
    internal class GrovePiAnalogSensor : ISensor
    {
        protected AnalogSensor _analogSensor;

        internal GrovePiAnalogSensor(AnalogSensor analogSensor, string name)
        {
            Name = name;
            _analogSensor = analogSensor;
        }

        public string Name { get; private set; }

        public virtual double Value
        {
            get
            {
                return _analogSensor.Value;
            }
        }

        public virtual string Unit => "V";
    }
}

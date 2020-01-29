using Iot.Device.GrovePiDevice.Sensors;

namespace Sensors.GrovePi
{
    internal class GrovePiAnalogSensor : ISensor
    {
        protected AnalogSensor _analogSensor;
        private double value;

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
                return value;
            }
        }

        public virtual string Unit => "V";

        public virtual void Refresh()
        {
            value = _analogSensor.Value;
        }
    }
}

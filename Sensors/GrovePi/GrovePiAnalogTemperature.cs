using Iot.Device.GrovePiDevice.Models;
using Iot.Device.GrovePiDevice.Sensors;
using Sensors.Weather;
using System.ComponentModel;

namespace Sensors.GrovePi
{
    internal class GrovePiAnalogTemperature : GrovePiAnalogSensor, IRefresher, ISensor
    {

        internal GrovePiAnalogTemperature(GroveTemperatureSensor analogSensor, string name, int sensorId, GrovePort port, bool rgbDisplay)
            : base(analogSensor, name, sensorId, port, rgbDisplay)
        {
        }
        
        public override event PropertyChangedEventHandler PropertyChanged;

        public override double Value => value;

        public override void RefreshValues()
        {
            value = (_analogSensor as GroveTemperatureSensor).Temperature;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Value"));
        }
    }
}

using Iot.Device.GrovePiDevice.Models;
using Iot.Device.GrovePiDevice.Sensors;
using Sensors.Weather;
using System.ComponentModel;

namespace Sensors.GrovePi
{
    internal class GrovePiAnalogTemperature : GrovePiAnalogSensor, IRefresher, ISensor
    {

        internal GrovePiAnalogTemperature(GroveTemperatureSensor analogSensor, string name, GrovePort port) 
            : base(analogSensor, name, port)
        {
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public override double Value => value;

        public override void Refresh()
        {
            value = (_analogSensor as GroveTemperatureSensor).Temperature;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Value"));
        }
    }
}

using Iot.Device.GrovePiDevice.Models;
using Iot.Device.GrovePiDevice.Sensors;
using Sensors.Weather;
using System.ComponentModel;

namespace Sensors.GrovePi
{
    internal class GrovePiAnalogPotentiometer : GrovePiAnalogSensor, IRefresher, ISensor
    {
        internal GrovePiAnalogPotentiometer(PotentiometerSensor analogSensor, string name, int sensorId, GrovePort port, bool rgbDisplay)
            : base(analogSensor, name, sensorId, port, rgbDisplay)
        {
        }

        public override event PropertyChangedEventHandler PropertyChanged;

        public override void Refresh()
        {
            value = (_analogSensor as PotentiometerSensor).ValueAsPercent;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Value"));
        }
    }
}

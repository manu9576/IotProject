﻿using Iot.Device.GrovePiDevice.Models;
using Iot.Device.GrovePiDevice.Sensors;
using Sensors.Weather;
using System.ComponentModel;

namespace Sensors.GrovePi
{
    internal class GrovePiAnalogSensor : GrovePiSensor, IRefresher, ISensor
    {
        protected AnalogSensor _analogSensor;
        protected double value;

        internal GrovePiAnalogSensor(AnalogSensor analogSensor, string name, int sensorId, GrovePort port, bool rgbDisplay) :
            base(name, "V", sensorId, SensorType.AnalogSensor, port, rgbDisplay)
        {
            _analogSensor = analogSensor;
        }

        public override double Value => value;

        public virtual event PropertyChangedEventHandler PropertyChanged;

        public override void Refresh()
        {
            value = _analogSensor.Value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Value"));
        }

    }
}

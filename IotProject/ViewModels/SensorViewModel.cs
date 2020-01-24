using Iot.Device.GrovePiDevice.Models;
using ReactiveUI;
using Sensors.GrovePi;

namespace IotProject.ViewModels
{
    public class SensorViewModel : ViewModelBase
    {
        private readonly IGrovePiSensor sensor;
        private string text;

        public SensorViewModel(IGrovePiSensor sensor)
        {
            this.sensor = sensor;
        }

        public SensorViewModel(SensorType sensorType, GrovePort port, string name)
        {
            this.sensor = GrovePiSensorBuilder.CreateSensor(sensorType, port, name);
        }

        public string Text
        {
            get => text;
            set => this.RaiseAndSetIfChanged(ref text, value);
        }

        public void Refresh()
        {
            Text = sensor.Name + ": " + sensor.Value + " " + sensor.Unit;
        }
    }
}

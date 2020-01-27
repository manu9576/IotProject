using Iot.Device.GrovePiDevice.Models;
using ReactiveUI;
using Sensors;
using Sensors.GrovePi;

namespace IotProject.ViewModels
{
    public class SensorViewModel : ViewModelBase
    {
        private readonly ISensor sensor;
        private string text;

        // Fake constructor for designer
        public SensorViewModel()
        {
            this.sensor = GrovePiSensorBuilder.CreateSensor(SensorType.DhtTemperatureSensor, GrovePort.DigitalPin7, "Température");
        }

        public SensorViewModel(ISensor sensor)
        {
            this.sensor = sensor;
        }

        public string Text
        {
            get => text;
            set => this.RaiseAndSetIfChanged(ref text, value);
        }

        public void Refresh()
        {
            Text = sensor.Name + ": " + sensor.Value.ToString("0.0") + " " + sensor.Unit;
        }
    }
}

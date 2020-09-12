using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace IotProject.Views
{
    public class WeatherSensorConfigurationView : UserControl
    {
        public WeatherSensorConfigurationView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}

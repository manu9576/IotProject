using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace IotProject.Views
{
    public class SensorsConfigurationView : UserControl
    {
        public SensorsConfigurationView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}

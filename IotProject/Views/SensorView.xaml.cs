using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace IotProject.Views
{
    public class SensorView : UserControl
    {
        public SensorView()
        {
            this.InitializeComponent();

        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}

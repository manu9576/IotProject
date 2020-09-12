using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace IotProject.Views
{
    public class SensorsMeasureView : UserControl
    {
        public SensorsMeasureView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}

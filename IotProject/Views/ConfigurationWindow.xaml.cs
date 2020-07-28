using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace IotProject.Views
{
    public class ConfigurationWindow : Window
    {
        public ConfigurationWindow()
        {
            this.InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}

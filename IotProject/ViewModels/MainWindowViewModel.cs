using ReactiveUI;
using System;
using System.Reactive;
using System.Threading;
using System.Threading.Tasks;

namespace IotProject.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            Task.Run(() => UpdateValues());
        }

        private void UpdateValues()
        {
            while (true)
            {

                TimeOfDay = DateTime.Now.ToString("dd/MM/yy HH:mm:ss");

                Thread.Sleep(1000);
            }
        }

        private string _timeOfDay;

        public string TimeOfDay
        {
            get => _timeOfDay;
            set => this.RaiseAndSetIfChanged(ref _timeOfDay, value);
        }
    }
}

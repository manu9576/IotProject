using Sensors;
using System.Collections.ObjectModel;

namespace IotProject.ViewModels
{
    public class SensorsMeasureViewModel : ViewModelBase
    {
        public ObservableCollection<ISensor> Sensors { get; private set; }

        public SensorsMeasureViewModel()
        {
            Sensors = SensorsManager.Sensors;
        }

    }
}

using CommunityToolkit.Mvvm.ComponentModel;

namespace UserInterfaceTemplate.Infrastructure.BaseModels
{
    /// <summary>
    /// Can be usefull for binding against charts
    /// </summary>
    public class SimpleDataPoint : ObservableObject
    {

        public SimpleDataPoint(string category, double value)
        {
            CategoryLabel = category;
            PointValue = value;
        }

        private string _category;
        public string CategoryLabel
        {
            get => _category;
            set => SetProperty(ref _category, value);
        }

        private double _pointValue;
        public double PointValue
        {
            get => _pointValue;
            set => SetProperty(ref _pointValue, value);
        }
    }
}

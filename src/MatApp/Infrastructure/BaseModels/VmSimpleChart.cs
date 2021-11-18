using CommunityToolkit.Mvvm.ComponentModel;

namespace MatApp.Infrastructure.BaseModels
{
    public class VmSimpleChart : ObservableObject
    {
        public ObservableRangeCollection<SimpleDataPoint> DataPoints { get; }

        public VmSimpleChart()
        {
            DataPoints = new ObservableRangeCollection<SimpleDataPoint>();
        }


        private string _chartTitle;
        public string ChartTitle
        {
            get => _chartTitle;
            set => SetProperty(ref _chartTitle, value);
        }


        private string _yAxisTitle;
        public string YAxisTitle
        {
            get => _yAxisTitle;
            set => SetProperty(ref _yAxisTitle, value);
        }


        private string _xAxisTitle;
        public string XAxisTitle
        {
            get => _xAxisTitle;
            set => SetProperty(ref _xAxisTitle, value);
        }


    }
}

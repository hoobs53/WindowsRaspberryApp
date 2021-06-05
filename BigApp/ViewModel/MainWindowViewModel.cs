using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace BigApp.ViewModel
{
    using View;

    public class MainWindowViewModel : BaseViewModel
    {

        private BaseViewModel _contetnViewModel;
        public BaseViewModel ContentViewModel
        {
            get { return _contetnViewModel; }
            set
            {
                if (_contetnViewModel != value)
                {
                    _contetnViewModel = value;
                    OnPropertyChanged("ContentViewModel");
                }
            }
        }

        public ButtonCommand MenuCommandChart { get; set; }
        public ButtonCommand MenuCommandDisplay { get; set; }

        public ButtonCommand MenuCommandConfig { get; set; }

        public MainWindowViewModel()
        {
            MenuCommandChart = new ButtonCommand(MenuSetChart);
            MenuCommandDisplay = new ButtonCommand(MenuSetDisplay);
            MenuCommandConfig = new ButtonCommand(MenuSetConfig);
            ContentViewModel = new ChartViewModel(); // ChartViewModel.Instance
        }

        private void MenuSetChart()
        {
            ContentViewModel = new ChartViewModel(); // ChartViewModel.Instance
        }

        private void MenuSetDisplay()
        {
            ContentViewModel = new DisplayViewModel(); // DisplayViewModel.Instance

        }

        private void MenuSetConfig()
        {
            ContentViewModel = new ConfigViewModel(); // ConfigViewModel.Instance
        }
    }
}

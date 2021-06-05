using BigApp.Model;
using System;
using System.ComponentModel;
using System.Windows.Media;

namespace BigApp.ViewModel
{
    public class DisplayViewModel : BaseViewModel
    {
        public ButtonCommand ButtonCommand { get; set; }
        public string pixelsState;
        public LedDisplay ledDisplay;  //!< LED display model
        private Server server;       //!< IoT server model

        public ButtonCommandWithParameter CommonButtonCommand { get; set; }
        public ButtonCommand SendRequestCommand { get; set; }
        public ButtonCommand SendClearCommand { get; set; }

        private byte _r;
        public int R
        {
            get
            {
                return _r;
            }
            set
            {
                if (_r != (byte)value)
                {
                    _r = (byte)value;
                    ledDisplay.ActiveColorR = _r;
                    SelectedColor = new SolidColorBrush(ledDisplay.ActiveColor);
                    OnPropertyChanged("R");
                }
            }
        }

        private byte _g;
        public int G
        {
            get
            {
                return _g;
            }
            set
            {
                if (_g != (byte)value)
                {
                    _g = (byte)value;
                    ledDisplay.ActiveColorG = _g;
                    SelectedColor = new SolidColorBrush(ledDisplay.ActiveColor);
                    OnPropertyChanged("G");
                }
            }
        }

        private byte _b;
        public int B
        {
            get
            {
                return _b;
            }
            set
            {
                if (_b != (byte)value)
                {
                    _b = (byte)value;
                    ledDisplay.ActiveColorB = _b;
                    SelectedColor = new SolidColorBrush(ledDisplay.ActiveColor);
                    OnPropertyChanged("B");
                }
            }
        }

        private SolidColorBrush _selectedColor;
        public SolidColorBrush SelectedColor
        {
            get
            {
                return _selectedColor;
            }
            set
            {
                if (_selectedColor != value)
                {
                    _selectedColor = value;
                    OnPropertyChanged("SelectedColor");
                }
            }
        }

        private string _statusText;
        public string StatusText
        {
            get
            {
                return _statusText;
            }
            set
            {
                if (_statusText != value)
                {
                    _statusText = value;
                    OnPropertyChanged("StatusText");
                }
            }
        }

        public DisplayViewModel()
        {
            ledDisplay = new LedDisplay(0x00000000);
            SelectedColor = new SolidColorBrush(ledDisplay.ActiveColor);
            SendRequestCommand = new ButtonCommand(SendControlRequest);

            server = new Server("192.168.1.104");
            GetInitPixels();
        }

        // Returns index ij from LEDij tag
        public (int, int) LedTagToIndex(string name)
        {
            return (int.Parse(name.Substring(3, 1)), int.Parse(name.Substring(4, 1)));
        }

        // Loads pixels state from server and saving it to pixelsState variable
        public void GetInitPixels()
        {
            pixelsState = server.POSTgetPixels();
        }

        // Changes status text
        public void setStatus()
        {
            StatusText = ledDisplay.status;
        }

        // Sends current pixel state to server which sets physical leds
        private async void SendControlRequest()
        {
            await server.PostControlRequest(ledDisplay.getControlPostData());
            StatusText = "";
        }


    }
}
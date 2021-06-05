using System;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using System.Windows.Data;
using System.Diagnostics;
using System.Windows.Media;
using Newtonsoft.Json;
namespace BigApp.View
{
    using ViewModel;
    using Model;
    /// <summary>
    /// Interaction logic for DisplayView.xaml
    /// </summary>
    public partial class DisplayView : UserControl
    {
        private string responseText;
        private PixelsData pixels;
        public DisplayViewModel viewmodel { get { return DataContext as DisplayViewModel; } }
        public DisplayView()
        {
            this.DataContext = new DisplayViewModel();
            InitializeComponent();        
                // Button matrix grid definition 
                for (int i = 0; i < 8; i++)
                {
                    ButtonMatrixGrid.ColumnDefinitions.Add(new ColumnDefinition());
                    ButtonMatrixGrid.ColumnDefinitions[i].Width = new GridLength(1, GridUnitType.Star);
                }

                for (int i = 0; i < 8; i++)
                {
                    ButtonMatrixGrid.RowDefinitions.Add(new RowDefinition());
                    ButtonMatrixGrid.RowDefinitions[i].Height = new GridLength(1, GridUnitType.Star);
                }

                responseText = viewmodel.pixelsState;

                try
                {
                    pixels = JsonConvert.DeserializeObject<PixelsData>(responseText);

                    int cnt = 0;
                    byte r, g, b;
                    for (int i = 0; i < 8; i++)
                    {
                        for (int j = 0; j < 8; j++)
                        {
                            r = (byte)pixels.response[cnt][0];
                            g = (byte)pixels.response[cnt][1];
                            b = (byte)pixels.response[cnt][2];
                            // <Button
                            Button led = new Button()
                            {

                                // Name = "LEDij"
                                Name = "LED" + j.ToString() + i.ToString(),
                                // CommandParameter = "LEDij"
                                CommandParameter = "LED" + j.ToString() + i.ToString(),
                                // Style="{StaticResource LedButtonStyle}"
                                Style = (Style)FindResource("LedButtonStyle"),
                                // Bacground="{StaticResource ... }"

                                Background = new SolidColorBrush(Color.FromArgb(255, r, g, b)),

                                // BorderThicness="2"
                                BorderThickness = new Thickness(2),
                            };

                            // Click = "{Binding Path=changeColor}"
                            led.Click += new RoutedEventHandler(changeColor);
                            // Grid.Column="i" 
                            Grid.SetColumn(led, j);
                            // Grid.Row="j"
                            Grid.SetRow(led, i);
                            // />

                            ButtonMatrixGrid.Children.Add(led);
                            ButtonMatrixGrid.RegisterName(led.Name, led);
                            viewmodel.ledDisplay.UpdateModel(j, i, r, g, b);
                            cnt++;
                        }
                    }
                    viewmodel.ledDisplay.initDisplay();
                }
                catch (Exception e)
                {
                    Debug.WriteLine("JSON DATA ERROR");
                    Debug.WriteLine(responseText);
                    Debug.WriteLine(e);
                }
            }
        // Used when pixel is clicked, changes its color to current RGB values on sliders
        public void changeColor(object sender, RoutedEventArgs e)
        {
            byte r, g, b;
            Button button = sender as Button;

            object objR = Sliders.FindName("R");
            object objG = Sliders.FindName("G");
            object objB = Sliders.FindName("B");

            if (objR is Slider && objG is Slider && objB is Slider)
            {
                Slider rSlider = objR as Slider;
                Slider gSlider = objG as Slider;
                Slider bSlider = objB as Slider;
                r = (byte)rSlider.Value;
                g = (byte)gSlider.Value;
                b = (byte)bSlider.Value;
                rSlider.Style = (Style)FindResource("ColorSlider");
                SolidColorBrush color = new SolidColorBrush(Color.FromArgb(255, r, g, b));
                button.Background = color;
                (int x, int y) = viewmodel.LedTagToIndex(button.Name);
                viewmodel.ledDisplay.UpdateModel(x, y, r, g, b);
                viewmodel.setStatus();
            }
        }
    }
}

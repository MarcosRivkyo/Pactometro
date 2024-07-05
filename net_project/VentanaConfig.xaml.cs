using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Windows.Threading;

namespace Pactometro
{

    public class ColorSeleccionadoEventArgs : EventArgs
    {
        public Color NuevoColor { get; set; }

    }

    public partial class VentanaConfig : Window
    {

        public event EventHandler<ColorSeleccionadoEventArgs> ColorSeleccionado;
        public event PropertyChangedEventHandler PropertyChanged;

        DispatcherTimer timer = new DispatcherTimer();

        public VentanaConfig(EleccionesViewModel viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();

            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();

            comboBoxIdioma.SelectedItem = comboBoxIdioma.Items[0];

        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            textBlockReloj.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        public void InicializarSliders(byte red, byte green, byte blue)
        {
            Slider_Red.Value = red;
            Slider_Green.Value = green;
            Slider_Blue.Value = blue;

            Color col = Color.FromRgb(red, green, blue);

            Panel.Background = new SolidColorBrush(col);

        }


        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            byte red = (byte)Slider_Red.Value;
            byte green = (byte)Slider_Green.Value;
            byte blue = (byte)Slider_Blue.Value;

            Color nuevoColor = Color.FromRgb(red, green, blue);

            Panel.Background = new SolidColorBrush(nuevoColor);

            ColorSeleccionado?.Invoke(this, new ColorSeleccionadoEventArgs { NuevoColor = nuevoColor });
        }


        private void aceptar_Click(object sender, RoutedEventArgs e)
        {
            
            this.Close(); 

        }


        private void ComboBoxDimensiones_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            double alto, ancho;

            if (comboBoxDimensiones.SelectedItem != null)
            {
                string opcionSeleccionada = ((ComboBoxItem)comboBoxDimensiones.SelectedItem).Content.ToString();

                switch (opcionSeleccionada)
                {
 
                    case "Pequeño":
                        ancho = 800;
                        alto = 500;
                        ((MainWindow)this.Owner).Height= alto;
                        ((MainWindow)this.Owner).Width = ancho;

                        break;
                    case "Mediano":
                        ancho = 900;
                        alto = 600;
                        ((MainWindow)this.Owner).Height = alto;
                        ((MainWindow)this.Owner).Width = ancho; break;
                    case "Grande":
                        ancho = 1000;
                        alto = 700;
                        ((MainWindow)this.Owner).Height = alto;
                        ((MainWindow)this.Owner).Width = ancho; break;
                    case "Muy Grande":
                        ancho = 1200;
                        alto = 800;
                        ((MainWindow)this.Owner).Height = alto;
                        ((MainWindow)this.Owner).Width = ancho;
                        break;
                    case "Maximizar":
                        ((MainWindow)this.Owner).WindowState = WindowState.Maximized;
                        break;

                    case "Reiniciar":
                        ((MainWindow)this.Owner).WindowState = WindowState.Normal;
                        break;

                    default:
                        
                        break;
                }


            }
        }
            private void ComboBoxIdioma_Selected(object sender, SelectionChangedEventArgs e)
            {
            var viewModel = DataContext as EleccionesViewModel;

            if (comboBoxIdioma.SelectedItem != null)
            {
                string opcionSeleccionada = string.Empty;

                if (comboBoxIdioma.SelectedItem is ComboBoxItem selectedItem && selectedItem.Content is StackPanel stackPanel)
                {

                    if (stackPanel.Children.Count > 1 && stackPanel.Children[0] is TextBlock selectedTextBlock)
                    {
                        opcionSeleccionada = selectedTextBlock.Text;
                    }

                    if (stackPanel.Children.Count > 1 && stackPanel.Children[1] is TextBlock selectedTextBlock1)
                    {
                        opcionSeleccionada = selectedTextBlock1.Text;
                    }
                }


                if (comboBoxIdioma.SelectedItem != null)
                {

                    if (opcionSeleccionada.Equals("Español"))
                    {
                        MessageBox.Show($"Se ha cambiado el idioma al : {opcionSeleccionada}");
                        viewModel.Idioma = "es";

                        var itemToSelect = comboBoxIdioma.Items.Cast<ComboBoxItem>().FirstOrDefault(item => item.Content.ToString() == "Español");

                        if (itemToSelect != null)
                        {
                            comboBoxIdioma.SelectedItem = itemToSelect;
                        }
                    }

                    if (opcionSeleccionada.Equals("Inglés"))
                    {
                        MessageBox.Show($"Language changed to : {opcionSeleccionada}");
                        viewModel.Idioma = "en";
                        var itemToSelect = comboBoxIdioma.Items.Cast<ComboBoxItem>().FirstOrDefault(item => item.Content.ToString() == "Inglés");

                        if (itemToSelect != null)
                        {
                            comboBoxIdioma.SelectedItem = itemToSelect;
                        }

                    }


                }
            }




        }


        private void listBoxOpciones_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listBoxOpciones.SelectedItem != null)
            {
                var selectedListBoxItem = (ListBoxItem)listBoxOpciones.SelectedItem;
                var stackPanel = (StackPanel)selectedListBoxItem.Content;

   
                string opcionSeleccionado = ((TextBlock)stackPanel.Children[1]).Text;

                if (opcionSeleccionado.Equals("Oscuro"))
                {
                    MessageBox.Show("Cambiando al tema : oscuro");
                    ((MainWindow)this.Owner).Panel.Background = Brushes.DarkGray;
                    ((MainWindow)this.Owner).BotonOpciones.Background = Brushes.DarkSlateGray;
                    ((MainWindow)this.Owner).BotonOpciones.Foreground = Brushes.White;

                    ((MainWindow)this.Owner).Background = Brushes.Black;


                }
                else if (opcionSeleccionado.Equals("Claro"))
                {
                    MessageBox.Show("Cambiando al tema : claro");

                    ((MainWindow)this.Owner).Panel.Background = Brushes.White;
                    ((MainWindow)this.Owner).BotonOpciones.Background = Brushes.White;
                    ((MainWindow)this.Owner).BotonOpciones.Foreground = Brushes.Black;

                    ((MainWindow)this.Owner).Background = Brushes.LightGray;


                }
                else if (opcionSeleccionado.Equals("Tropical"))
                {
                    MessageBox.Show("Cambiando al tema : tropical");

                    ((MainWindow)this.Owner).Panel.Background = Brushes.LightYellow;
                    ((MainWindow)this.Owner).BotonOpciones.Background = Brushes.Orange;
                    ((MainWindow)this.Owner).BotonOpciones.Foreground = Brushes.DarkBlue;

                    ((MainWindow)this.Owner).Background = Brushes.Khaki;


                }
                else if(opcionSeleccionado.Equals("Por Defecto"))
                {
                    MessageBox.Show("Cambiando al tema : por defecto");

                    ((MainWindow)this.Owner).Panel.Background = Brushes.White;
                    ((MainWindow)this.Owner).BotonOpciones.Background = Brushes.LightGray;
                    ((MainWindow)this.Owner).BotonOpciones.Foreground = Brushes.Black;

                    ((MainWindow)this.Owner).Background = Brushes.LightBlue;



                }
            }
        
        
        }



    }
}

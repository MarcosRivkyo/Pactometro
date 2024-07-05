using System;
using System.Collections;
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

namespace Pactometro
{

    public partial class VentanaFecha : Window
    {
        private DatePicker nuevaFecha;
        private int procMod;
        public VentanaFecha(EleccionesViewModel viewModel, int procesoAMod)
        {
            InitializeComponent();
            DataContext = viewModel;
            this.procMod = procesoAMod;

            this.Loaded += VentanaFecha_Loaded;

            this.Background = new SolidColorBrush(Colors.LightBlue);

        }

        private void VentanaFecha_Loaded(object sender, RoutedEventArgs e)
        {
            txtFecha.Focus();
        }

        private void BotonAceptar_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as EleccionesViewModel;

            nuevaFecha = txtFecha;

            if (nuevaFecha != null)
            {
                viewModel.Procesos[procMod].Fecha = nuevaFecha.ToString();

                this.DialogResult = true;
            }


        }

        private void BotonCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }


        private void txtFecha_SelectedDateChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (txtFecha.Text.Length > 0)
            {
                BotonAceptar.IsEnabled = true;
                BotonCancelar.IsEnabled = true;

            }
            else
            {
                BotonAceptar.IsEnabled = false;
                BotonCancelar.IsEnabled = false;
            }

        }


    }
}

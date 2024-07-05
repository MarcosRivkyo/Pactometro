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

    public partial class VentanaEscaños : Window
    {
        private int txtEscaños;
        private int procMod;
        public VentanaEscaños(EleccionesViewModel viewModel, int procesoAMod)
        {
            InitializeComponent();

            DataContext = viewModel;
            this.procMod = procesoAMod;

            Loaded += MainWindow_Loaded;

            this.Background = new SolidColorBrush(Colors.LightBlue);


        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            txtEscaño.Focus();
        }


        private void BotonAceptar_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as EleccionesViewModel;

            int nuevoEscaño;
            int nuevaMayoria;

            nuevoEscaño = txtEscaños;
            nuevaMayoria = (txtEscaños / 2) + 1;


            if (nuevoEscaño >= 0)
            {
                viewModel.Procesos[procMod].Escaños = nuevoEscaño;
                viewModel.Procesos[procMod].MayoriaAbsoluta = nuevaMayoria;
                this.DialogResult = true;
 
            }





        }

        private void BotonCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void TxtEscaño_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(txtEscaño.Text, out int valorEntero))
            {
                txtEscaños = valorEntero;
                BotonAceptar.IsEnabled = true;
                BotonCancelar.IsEnabled = true;
            }
            else
            {
                MessageBox.Show("Por favor, ingrese un número entero válido.", "Error");
            }



        }
    }
}

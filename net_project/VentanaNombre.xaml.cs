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

namespace Pactometro
{
 
    public partial class VentanaNombre : Window
    {
        private string nuevoNombre = "";
        int procMod;

        public VentanaNombre(EleccionesViewModel viewModel, int procesoAMod)
        {
            InitializeComponent();
            DataContext = viewModel;
            this.procMod = procesoAMod;
            Loaded += MainWindow_Loaded;
            this.Background = new SolidColorBrush(Colors.LightBlue);


        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            txtProceso.Focus();
        }

        private void txtProceso_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(txtProceso.Text.Length > 0)
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

        private void BotonAceptar_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as EleccionesViewModel;

            nuevoNombre = txtProceso.Text;

            if (nuevoNombre != null)
            {

                viewModel.Procesos[procMod].Elecciones = nuevoNombre;
                this.DialogResult = true;


            }
        }

        private void BotonCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;

        }
    }
}

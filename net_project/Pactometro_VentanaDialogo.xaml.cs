using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Eventing.Reader;
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

    public partial class VentanaDialogo : Window
    {

        private string txtNombre { get;  set; }
        private string txtFechas { get;  set; }
        private int txtEscaños { get;  set; }
        private int txtEscaños2 { get;  set; }


        private ObservableCollection<Partido> PartidosDisponibles { get;  set; }
        private ObservableCollection<Partido> PartidosSeleccionados { get;  set; }




        public VentanaDialogo(EleccionesViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;

            this.Background = new SolidColorBrush(Colors.LightBlue);
        }




        private void Aceptar_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as EleccionesViewModel;
            Proceso procesoAgregar = new Proceso();
            string nombre = txtProceso.Text;
            string fecha = txtFecha.Text;
            

                if (int.TryParse(txtEscaño.Text, out int valorEntero))
                {

                    txtNombre = nombre;
                    txtFechas = fecha;
                    txtEscaños = valorEntero;
                    txtEscaños2 = (valorEntero / 2) + 1;


                procesoAgregar = new Proceso
                {
                    Elecciones = txtNombre,
                    Fecha = txtFechas,
                    Escaños = txtEscaños,
                    MayoriaAbsoluta = txtEscaños2,

                    ListaPartidos = PartidosSeleccionados,
                };

                viewModel.Procesos.Add(procesoAgregar);
                    
                    DialogResult = true; 
                }
                else
                {
                    MessageBox.Show("Por favor, ingrese un número entero válido.", "Error");
                }
    
        }

        private void AgregarPartidosSeleccionados_Click(object sender, RoutedEventArgs e)
        {

            var viewModel = DataContext as EleccionesViewModel;

            VentanaPartidos dialog = new VentanaPartidos(viewModel);
            dialog.ShowDialog();

            

                PartidosDisponibles = dialog.PartidosDisponiblesDesdeVentanaSecundaria();
                PartidosSeleccionados = dialog.PartidosSeleccionadosDesdeVentanaSecundaria();

            

                BotonAceptar.IsEnabled = true;
            


        }


        private void txtProceso_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtProceso.Text.Length > 0)
            {
                txtFecha.IsEnabled = true;

            }
            else
            {
                txtFecha.IsEnabled = false;

            }
        }

        private void txtFecha_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (txtFecha.Text.Length>0)
            {

                txtEscaño.IsEnabled = true;

            }
            else
            {
                txtEscaño.IsEnabled = false;

            }

        }


 

 
        private void TxtFecha_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (txtFecha.Text.Length > 0)
            {

                txtEscaño.IsEnabled = true;

            }
            else
            {
                txtEscaño.IsEnabled = false;

            }
        }

        private void activar(object sender, TextChangedEventArgs e)
        {
            if (txtFecha.Text.Length > 0)
            {

                BotonAgregarPartidos.IsEnabled = true;

            }
            else
            {
                BotonAgregarPartidos.IsEnabled = false;

            }
        }


    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

    public partial class VentanaComparador : Window
    {



        public VentanaComparador(EleccionesViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;

            CargarProcesos();

            this.Background = new SolidColorBrush(Colors.LightBlue);

        }
        private void CargarProcesos()
        {
            var viewModel = DataContext as EleccionesViewModel;

            foreach(var proceso in viewModel.Procesos)
            {
                ProcesosListBox.Items.Add(proceso);

            }

        }

        private void CargarPartidos()
        {
            var viewModel = DataContext as EleccionesViewModel;

            VentanaPartidos nueva = new VentanaPartidos(viewModel);

            ObservableCollection<Partido> partidosDisponibles = nueva.PartidosDisponiblesDesdeVentanaSecundaria();

            nueva.Close();

            PartidosListBox.ItemsSource = partidosDisponibles;
        }


        private void SeleccionarProcesos_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as EleccionesViewModel;

            viewModel.ProcesosSeleccionados.Clear();

            foreach (var procesos in ProcesosListBox.SelectedItems)
            {
                viewModel.ProcesosSeleccionados.Add(procesos as Proceso);
            }

            ProcesosListBox.IsEnabled = false;
            boton1.IsEnabled = false;

            boton.IsEnabled = true;

            CargarPartidos();

        }

        private void DibujarGraficaButton_Click (object sender, RoutedEventArgs e)
        {

            var viewModel = DataContext as EleccionesViewModel;

            viewModel.PartidosSeleccionados.Clear();


            foreach (Partido partido in PartidosListBox.SelectedItems)
            {
                viewModel.PartidosSeleccionados.Add(partido);
            }



            this.Close();

        }

    }
}

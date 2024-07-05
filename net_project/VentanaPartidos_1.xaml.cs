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


    public partial class VentanaPartidos_1 : Window
    {
        private int procMod;
        private ObservableCollection<Partido> ListaPartidosAntes;
        private ObservableCollection<Partido> ListaPartidosProceso;
        private ObservableCollection<Partido> ListaPartidosDisponibles;
        private ObservableCollection<Partido> ListaPartidosPosibles;


        public VentanaPartidos_1(EleccionesViewModel viewModel, int procesoAMod)
        {
            InitializeComponent();
            DataContext = viewModel;
            this.procMod = procesoAMod;

            ListaPartidosAntes = viewModel.Procesos[procMod].ListaPartidos;
            ListaPartidosProceso = viewModel.Procesos[procMod].ListaPartidos;
            ListaPartidosDisponibles = new ObservableCollection<Partido>();
            ListaPartidosPosibles = new ObservableCollection<Partido>();



            ListaPartidosDisponibles.Add(new Partido { Nombre = "PP", LogoPath = viewModel.RutasDeArchivos["PP"], Escaños = 0, Color = viewModel.ColoresPartidos["PP"] });
            ListaPartidosDisponibles.Add(new Partido { Nombre = "PSOE", LogoPath = viewModel.RutasDeArchivos["PSOE"], Escaños = 0, Color = viewModel.ColoresPartidos["PSOE"] });
            ListaPartidosDisponibles.Add(new Partido { Nombre = "VOX", LogoPath = viewModel.RutasDeArchivos["VOX"], Escaños = 0, Color = viewModel.ColoresPartidos["VOX"] });
            ListaPartidosDisponibles.Add(new Partido { Nombre = "SUMAR", LogoPath = viewModel.RutasDeArchivos["SUMAR"], Escaños = 0, Color = viewModel.ColoresPartidos["SUMAR"] });
            ListaPartidosDisponibles.Add(new Partido { Nombre = "ERC", LogoPath = viewModel.RutasDeArchivos["ERC"], Escaños = 0, Color = viewModel.ColoresPartidos["ERC"] });
            ListaPartidosDisponibles.Add(new Partido { Nombre = "JUNTS", LogoPath = viewModel.RutasDeArchivos["JUNTS"], Escaños = 0, Color = viewModel.ColoresPartidos["JUNTS"] });
            ListaPartidosDisponibles.Add(new Partido { Nombre = "EH_BILDU", LogoPath = viewModel.RutasDeArchivos["EH_BILDU"], Escaños = 0, Color = viewModel.ColoresPartidos["EH_BILDU"] });
            ListaPartidosDisponibles.Add(new Partido { Nombre = "EAJ_PNV", LogoPath = viewModel.RutasDeArchivos["EAJ_PNV"], Escaños = 0, Color = viewModel.ColoresPartidos["EAJ_PNV"] });
            ListaPartidosDisponibles.Add(new Partido { Nombre = "BNG", LogoPath = viewModel.RutasDeArchivos["BNG"], Escaños = 0, Color = viewModel.ColoresPartidos["BNG"] });
            ListaPartidosDisponibles.Add(new Partido { Nombre = "CCA", LogoPath = viewModel.RutasDeArchivos["CCA"], Escaños = 0, Color = viewModel.ColoresPartidos["CCA"] });
            ListaPartidosDisponibles.Add(new Partido { Nombre = "PODEMOS", LogoPath = viewModel.RutasDeArchivos["PODEMOS"], Escaños = 0, Color = viewModel.ColoresPartidos["PODEMOS"] });
            ListaPartidosDisponibles.Add(new Partido { Nombre = "CS", LogoPath = viewModel.RutasDeArchivos["CS"], Escaños = 0, Color = viewModel.ColoresPartidos["CS"] });
            ListaPartidosDisponibles.Add(new Partido { Nombre = "MASPAIS", LogoPath = viewModel.RutasDeArchivos["MASPAIS"], Escaños = 0, Color = viewModel.ColoresPartidos["MASPAIS"] });
            ListaPartidosDisponibles.Add(new Partido { Nombre = "CUP_PR", LogoPath = viewModel.RutasDeArchivos["CUP_PR"], Escaños = 0, Color = viewModel.ColoresPartidos["CUP_PR"] });
            ListaPartidosDisponibles.Add(new Partido { Nombre = "OTROS", LogoPath = viewModel.RutasDeArchivos["OTROS"], Escaños = 0, Color = viewModel.ColoresPartidos["OTROS"] });
            ListaPartidosDisponibles.Add(new Partido { Nombre = "UPL", LogoPath = viewModel.RutasDeArchivos["UPL"], Escaños = 0, Color = viewModel.ColoresPartidos["UPL"] });
            ListaPartidosDisponibles.Add(new Partido { Nombre = "SY", LogoPath = viewModel.RutasDeArchivos["SY"], Escaños = 0, Color = viewModel.ColoresPartidos["SY"] });
            ListaPartidosDisponibles.Add(new Partido { Nombre = "XAV", LogoPath = viewModel.RutasDeArchivos["XAV"], Escaños = 0, Color = viewModel.ColoresPartidos["XAV"] });

            var partidosNoEnProceso = ListaPartidosDisponibles.Where(p => !ListaPartidosProceso.Any(p2 => p2.Nombre == p.Nombre));

            foreach (var partido in partidosNoEnProceso)
            {
                ListaPartidosPosibles.Add(partido);
            }

            partidosProceso.ItemsSource = ListaPartidosProceso;
            partidosDisponibles.ItemsSource = ListaPartidosPosibles;

            this.Background = new SolidColorBrush(Colors.LightBlue);



        }



        private void partidosDisponibles_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            if (partidosProceso.SelectedItem is Partido partido)
            {
                ListaPartidosPosibles.Add(partido);
                ListaPartidosProceso.Remove(partido);
            }



        }

        private void partidosDisponibles_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {
            if (partidosDisponibles.SelectedItem is Partido partido1)
            {
                ListaPartidosProceso.Add(partido1);
                ListaPartidosPosibles.Remove(partido1);
            }
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as EleccionesViewModel;

            viewModel.Procesos[procMod].ListaPartidos.Equals(ListaPartidosProceso);


            this.DialogResult = true;

        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as EleccionesViewModel;

            viewModel.Procesos[procMod].ListaPartidos.Equals(ListaPartidosAntes);

            this.DialogResult = false;

        }




 
    }

    }


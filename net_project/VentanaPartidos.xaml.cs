using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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

    public partial class VentanaPartidos : Window
    {
        private ObservableCollection<Partido> ListaPartidosDisponibles;
        private ObservableCollection<Partido> ListaPartidosSeleccionados;

        public VentanaPartidos(EleccionesViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;

            viewModel.PartidosSeleccionados.Clear();


            ListaPartidosDisponibles = new ObservableCollection<Partido>();
            ListaPartidosSeleccionados = new ObservableCollection<Partido>();


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

            partidosDisponibles.ItemsSource = ListaPartidosDisponibles;
            partidosSeleccionados.ItemsSource = ListaPartidosSeleccionados;

            this.Background = new SolidColorBrush(Colors.LightBlue);

        }



        private void partidosDisponibles_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (partidosDisponibles.SelectedItem is Partido partido)
            {
                ListaPartidosSeleccionados.Add(partido);
                ListaPartidosDisponibles.Remove(partido);
            }
        }

        public ObservableCollection<Partido> PartidosDisponiblesDesdeVentanaSecundaria()
        {
              return ListaPartidosDisponibles;
        }


        public ObservableCollection<Partido> PartidosSeleccionadosDesdeVentanaSecundaria()
        {
             return ListaPartidosSeleccionados; 

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();


        }
    }

 

    }


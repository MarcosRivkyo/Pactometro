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
using System.Collections.ObjectModel;
using System.Windows.Media.Animation;
using System.Security.Cryptography;
using System.Diagnostics;
using System.Resources;
using System.ComponentModel;

namespace Pactometro
{

    public partial class CDTabla : Window
    {
 
        public delegate void DibujarGraficaEventHandler(object sender,RoutedEventArgs e);
        public event DibujarGraficaEventHandler DibujarGraficaRequested;

        public delegate void DibujarGraficaComparativaEventHandler(object sender, RoutedEventArgs e);
        public event DibujarGraficaComparativaEventHandler DibujarGraficaComparativaRequested;


        public delegate void PactometroHandler(object sender, RoutedEventArgs e);
        public event PactometroHandler PactometroRequested;

        public delegate void DibujarGraficaSectoresEventHandler(object sender, RoutedEventArgs e);
        public event DibujarGraficaSectoresEventHandler DibujarGraficaSectoresRequested;


        private ContextMenu contextMenu;

        private Proceso[] nuevoElemento = new Proceso[5];


        public CDTabla(EleccionesViewModel viewModel)
        {
            InitializeComponent();

            this.Background = new SolidColorBrush(Colors.LightBlue);

            DataContext = viewModel;

            comprobarIdioma();


            Lista.SelectionChanged += Lista_SelectionChanged;
            Lista.PreviewKeyDown += Lista_PreviewKeyDown;
            Lista.ItemsSource = viewModel.Procesos;

            CargarEleccionesPredefinidas();

            viewModel.IdiomaChanged += viewModel_IdiomaChanged;

            this.Closing += CDTabla_Closing;





        }



        private void CargarEleccionesPredefinidas()
        {
            var viewModel = DataContext as EleccionesViewModel;



            nuevoElemento[0] = new Proceso
            {

                Elecciones = "Elecciones Generales 23-7-2023",
                Fecha = "23/7/2023",
                Escaños = 350,
                MayoriaAbsoluta = 176,


                ListaPartidos = new ObservableCollection<Partido>()
                {
                    new Partido{Nombre = "PP", Escaños =136, LogoPath = viewModel.RutasDeArchivos["PP"], Color = viewModel.ColoresPartidos["PP"]},
                    new Partido{Nombre = "PSOE", Escaños = 122, LogoPath = viewModel.RutasDeArchivos["PSOE"], Color = viewModel.ColoresPartidos["PSOE"]},
                    new Partido{Nombre = "VOX", Escaños = 33, LogoPath = viewModel.RutasDeArchivos["VOX"], Color = viewModel.ColoresPartidos["VOX"]},
                    new Partido{Nombre = "SUMAR", Escaños = 31, LogoPath = viewModel.RutasDeArchivos["SUMAR"], Color = viewModel.ColoresPartidos["SUMAR"]},
                    new Partido{Nombre = "ERC", Escaños = 7, LogoPath = viewModel.RutasDeArchivos["ERC"], Color = viewModel.ColoresPartidos["ERC"]},
                    new Partido{Nombre = "JUNTS", Escaños = 7, LogoPath = viewModel.RutasDeArchivos["JUNTS"], Color = viewModel.ColoresPartidos["JUNTS"]},
                    new Partido{Nombre = "EH_BILDU", Escaños = 6, LogoPath = viewModel.RutasDeArchivos["EH_BILDU"], Color = viewModel.ColoresPartidos["EH_BILDU"]},
                    new Partido{Nombre = "EAJ_PNV", Escaños = 5, LogoPath = viewModel.RutasDeArchivos["EAJ_PNV"], Color = viewModel.ColoresPartidos["EAJ_PNV"]},
                    new Partido{Nombre = "BNG", Escaños = 1, LogoPath = viewModel.RutasDeArchivos["BNG"], Color = viewModel.ColoresPartidos["BNG"]},
                    new Partido{Nombre = "CCA", Escaños = 1, LogoPath = viewModel.RutasDeArchivos["CCA"], Color = viewModel.ColoresPartidos["CCA"]},
                    new Partido{Nombre = "UPN", Escaños = 1, LogoPath = viewModel.RutasDeArchivos["UPN"], Color = viewModel.ColoresPartidos["UPN"]}


                }
            };




            nuevoElemento[1] = new Proceso
            {

                Elecciones = "Elecciones Generales 10-11-2019",
                Fecha = "10/11/2019",
                Escaños = 350,
                MayoriaAbsoluta = 176,

                ListaPartidos = new ObservableCollection<Partido>()
                {
                    new Partido{Nombre = "PSOE", Escaños = 120, LogoPath = viewModel.RutasDeArchivos["PSOE"], Color = viewModel.ColoresPartidos["PSOE"]},
                    new Partido{Nombre = "PP", Escaños =89, LogoPath = viewModel.RutasDeArchivos["PP"], Color = viewModel.ColoresPartidos["PP"]},
                    new Partido{Nombre = "VOX", Escaños = 52, LogoPath = viewModel.RutasDeArchivos["VOX"], Color = viewModel.ColoresPartidos["VOX"] },
                    new Partido{Nombre = "PODEMOS", Escaños = 35, LogoPath = viewModel.RutasDeArchivos["PODEMOS"], Color = viewModel.ColoresPartidos["PODEMOS"] },
                    new Partido{Nombre = "ERC", Escaños = 13, LogoPath = viewModel.RutasDeArchivos["ERC"], Color = viewModel.ColoresPartidos["ERC"] },
                    new Partido{Nombre = "CS", Escaños = 10, LogoPath = viewModel.RutasDeArchivos["CS"], Color = viewModel.ColoresPartidos["CS"] },
                    new Partido{Nombre = "JUNTS", Escaños = 8, LogoPath = viewModel.RutasDeArchivos["JUNTS"], Color = viewModel.ColoresPartidos["JUNTS"] },
                    new Partido{Nombre = "EAJ_PNV", Escaños = 6, LogoPath = viewModel.RutasDeArchivos["EAJ_PNV"], Color = viewModel.ColoresPartidos["EAJ_PNV"] },
                    new Partido{Nombre = "EH_BILDU", Escaños = 5, LogoPath = viewModel.RutasDeArchivos["EH_BILDU"], Color = viewModel.ColoresPartidos["EH_BILDU"] },
                    new Partido{Nombre = "MASPAIS", Escaños = 3,LogoPath = viewModel.RutasDeArchivos["MASPAIS"], Color = viewModel.ColoresPartidos["MASPAIS"] },
                    new Partido{Nombre = "CUP_PR", Escaños = 2, LogoPath = viewModel.RutasDeArchivos["CUP_PR"], Color = viewModel.ColoresPartidos["CUP_PR"] },
                    new Partido{Nombre = "CCA", Escaños = 2, LogoPath = viewModel.RutasDeArchivos["CCA"], Color = viewModel.ColoresPartidos["CCA"] },
                    new Partido{Nombre = "BNG", Escaños = 1, LogoPath = viewModel.RutasDeArchivos["BNG"], Color = viewModel.ColoresPartidos["BNG"] },
                    new Partido{Nombre = "OTROS", Escaños = 4, LogoPath = viewModel.RutasDeArchivos["OTROS"], Color = viewModel.ColoresPartidos["OTROS"]},



                }
            };


            nuevoElemento[2] = new Proceso
            {

                Elecciones = "Autonómicas Comunidad de CASTILLA Y LEÓN 14-2-2022",
                Fecha = "14/2/2022",
                Escaños = 81,
                MayoriaAbsoluta = 41,


                ListaPartidos = new ObservableCollection<Partido>()
                {
                    new Partido{Nombre = "PP", Escaños =31, LogoPath = viewModel.RutasDeArchivos["PP"], Color = viewModel.ColoresPartidos["PP"]},
                    new Partido{Nombre = "PSOE", Escaños = 28, LogoPath = viewModel.RutasDeArchivos["PSOE"], Color = viewModel.ColoresPartidos["PSOE"]},
                    new Partido{Nombre = "VOX", Escaños = 13, LogoPath = viewModel.RutasDeArchivos["VOX"], Color = viewModel.ColoresPartidos["VOX"]},
                    new Partido{Nombre = "UPL", Escaños = 3, LogoPath = viewModel.RutasDeArchivos["UPL"], Color = viewModel.ColoresPartidos["UPL"]},
                    new Partido{Nombre = "SY", Escaños = 3, LogoPath = viewModel.RutasDeArchivos["SY"], Color = viewModel.ColoresPartidos["SY"]},
                    new Partido{Nombre = "PODEMOS", Escaños = 1, LogoPath = viewModel.RutasDeArchivos["PODEMOS"], Color = viewModel.ColoresPartidos["PODEMOS"]},
                    new Partido{Nombre = "CS", Escaños = 1, LogoPath = viewModel.RutasDeArchivos["CS"], Color = viewModel.ColoresPartidos["CS"]},
                    new Partido{Nombre = "XAV", Escaños = 1, LogoPath = viewModel.RutasDeArchivos["XAV"], Color = viewModel.ColoresPartidos["XAV"]}

                }
            };



            nuevoElemento[3] = new Proceso
            {

                Elecciones = "Autonómicas Comunidad de CASTILLA Y LEÓN 26-5-2019",
                Fecha = "26/5/2019",
                Escaños = 81,
                MayoriaAbsoluta = 41,

                ListaPartidos = new ObservableCollection<Partido>()
                {
                    new Partido{Nombre = "PSOE", Escaños =35,  LogoPath = viewModel.RutasDeArchivos["PSOE"], Color = viewModel.ColoresPartidos["PSOE"]},
                    new Partido{Nombre = "PP", Escaños = 29,  LogoPath = viewModel.RutasDeArchivos["PP"], Color = viewModel.ColoresPartidos["PP"]},
                    new Partido{Nombre = "CS", Escaños = 12, LogoPath = viewModel.RutasDeArchivos["CS"], Color = viewModel.ColoresPartidos["CS"]},
                    new Partido{Nombre = "PODEMOS", Escaños = 2,  LogoPath = viewModel.RutasDeArchivos["PODEMOS"], Color = viewModel.ColoresPartidos["PODEMOS"]},
                    new Partido{Nombre = "VOX", Escaños = 1, LogoPath = viewModel.RutasDeArchivos["VOX"], Color = viewModel.ColoresPartidos["VOX"]},
                    new Partido{Nombre = "UPL", Escaños = 1,  LogoPath = viewModel.RutasDeArchivos["UPL"], Color = viewModel.ColoresPartidos["UPL"] },
                    new Partido{Nombre = "XAV", Escaños = 1, LogoPath = viewModel.RutasDeArchivos["XAV"], Color = viewModel.ColoresPartidos["XAV"]}


                }
            };



            nuevoElemento[4] = new Proceso
            {
                Elecciones = "Elecciones Prueba 27-10-2003",
                Fecha = "27/10/2003",
                Escaños = 500,
                MayoriaAbsoluta = 251,


                ListaPartidos = new ObservableCollection<Partido>()
                {
                    new Partido{Nombre = "PP", Escaños = 290, LogoPath = viewModel.RutasDeArchivos["PP"], Color = viewModel.ColoresPartidos["PP"]},
                    new Partido{Nombre = "PSOE", Escaños =110, LogoPath = viewModel.RutasDeArchivos["PSOE"], Color = viewModel.ColoresPartidos["PSOE"] },
                    new Partido{Nombre = "CS", Escaños = 40, LogoPath = viewModel.RutasDeArchivos["CS"], Color = viewModel.ColoresPartidos["CS"]},
                    new Partido{Nombre = "PODEMOS", Escaños = 25, LogoPath = viewModel.RutasDeArchivos["PODEMOS"], Color = viewModel.ColoresPartidos["PODEMOS"]},
                    new Partido{Nombre = "VOX", Escaños = 10, LogoPath = viewModel.RutasDeArchivos["VOX"], Color = viewModel.ColoresPartidos["VOX"]},
                    new Partido{Nombre = "EAJ_PNV", Escaños = 6, LogoPath = viewModel.RutasDeArchivos["EAJ_PNV"], Color = viewModel.ColoresPartidos["EAJ_PNV"] },
                    new Partido{Nombre = "UPL", Escaños = 5, LogoPath = viewModel.RutasDeArchivos["UPL"], Color = viewModel.ColoresPartidos["UPL"]},
                    new Partido{Nombre = "EH_BILDU", Escaños = 5, LogoPath = viewModel.RutasDeArchivos["EH_BILDU"], Color = viewModel.ColoresPartidos["EH_BILDU"] },
                    new Partido{Nombre = "XAV", Escaños = 4, LogoPath = viewModel.RutasDeArchivos["XAV"], Color = viewModel.ColoresPartidos["XAV"]},
                    new Partido{Nombre = "SY", Escaños = 3, LogoPath = viewModel.RutasDeArchivos["SY"], Color = viewModel.ColoresPartidos["SY"]},
                    new Partido{Nombre = "BNG", Escaños = 1, LogoPath = viewModel.RutasDeArchivos["BNG"], Color = viewModel.ColoresPartidos["BNG"] },
                    new Partido{Nombre = "CCA", Escaños = 1, LogoPath = viewModel.RutasDeArchivos["CCA"], Color = viewModel.ColoresPartidos["CCA"]},




                }
            };

            foreach (Proceso proceso in nuevoElemento)
            {
                if (proceso != null)
                {
                    viewModel.Procesos.Add(proceso);
                }
            }
            
        }


        private void Lista_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var viewModel = DataContext as EleccionesViewModel;


            if (Lista.SelectionMode == SelectionMode.Single)
            {

                if (Lista.SelectedItem is Proceso selectedData)
                {


                    viewModel.ProcesoSeleccionado = selectedData;

                    if (viewModel.ProcesoSeleccionado != null)
                    {
                        HabilitarOpcionesDeMenu();

                    }
                    else
                    {
                        DeshabilitarOpcionesDeMenu();

                    }

                    foreach (Partido partidos in selectedData.ListaPartidos)
                    {
                        viewModel.Partidos.Add(partidos);

                    }
                    Lista2.ItemsSource = selectedData.ListaPartidos;
                    
                }
            }

        }



        //===============================
        // Gestión de Idioma
        //===============================

        private void comprobarIdioma()
        {
            var viewModel = DataContext as EleccionesViewModel;



                ventanaPrinicpal_CambiarIdioma();


        }
        private void viewModel_IdiomaChanged(object sender, EventArgs e)
        {
            ventanaPrinicpal_CambiarIdioma();
        }



        private void ventanaPrinicpal_CambiarIdioma()
        {

            var viewModel = DataContext as EleccionesViewModel;
            if (viewModel.Idioma.Equals("es"))
            {


                string nombreArchivoRecursos = "Pactometro.Properties.Resources_es";

                // Crea el ResourceManager
                ResourceManager resourceManager = new ResourceManager(nombreArchivoRecursos, typeof(MainWindow).Assembly);


                var ColumnaElecciones = Lista.FindName("ColumnaElecciones") as GridViewColumn;
                var ColumnaFecha = Lista.FindName("ColumnaFecha") as GridViewColumn;
                var ColumnaEscaños = Lista.FindName("ColumnaEscaños") as GridViewColumn;
                var ColumnaMayoria = Lista.FindName("ColumnaMayoria") as GridViewColumn;

                var columnaPartido = Lista2.FindName("ColumnaPartido") as GridViewColumn;
                var ColumnaEscañoPartido = Lista2.FindName("ColumnaEscañoPartido") as GridViewColumn;
                var ColumnaLogo = Lista2.FindName("ColumnaLogo") as GridViewColumn;



                if (ColumnaElecciones != null)
                {
                    ColumnaElecciones.Header = resourceManager.GetString("ElectionText");
                }
                if (ColumnaFecha != null)
                {
                    ColumnaFecha.Header = resourceManager.GetString("DateText");
                }
                if (ColumnaEscaños != null)
                {
                    ColumnaEscaños.Header = resourceManager.GetString("SeatsText");
                }
                if (ColumnaMayoria != null)
                {
                    ColumnaMayoria.Header = resourceManager.GetString("AbsoluteMajorityText");
                }
                if (columnaPartido != null)
                {
                    columnaPartido.Header = resourceManager.GetString("PartyText");
                }
                if (ColumnaEscañoPartido != null)
                {
                    ColumnaEscañoPartido.Header = resourceManager.GetString("SeatsText");
                }
                if (ColumnaLogo != null)
                {
                    ColumnaLogo.Header = resourceManager.GetString("LogoText");
                }


                MenuOpciones.Header = resourceManager.GetString("OptionsButtonText");
                OpcionAgregar.Header = resourceManager.GetString("AddProcessText");
                OpcionEliminar.Header = resourceManager.GetString("RemoveProcessText");
                OpcionModificar.Header = resourceManager.GetString("ModifyProcessText");
                OpcionNombre.Header = resourceManager.GetString("ModifyNameText");
                OpcionFecha.Header = resourceManager.GetString("ModifyDateText");
                OpcionEscaños.Header = resourceManager.GetString("ModifySeatText");
                OpcionPartidos.Header = resourceManager.GetString("ModifyPartText");
                OpcionSalir.Header = resourceManager.GetString("ExitCDText");
                OpcionDibujar.Header = resourceManager.GetString("DrawText");
                OpcionNormal.Header = resourceManager.GetString("DrawNormalText");
                OpcionComparativa.Header = resourceManager.GetString("DrawComparativeText");
                OpcionPactometro.Header = resourceManager.GetString("DrawPactometerText");
                OpcionCircular.Header = resourceManager.GetString("DrawCircularText");

            }


            else if (viewModel.Idioma.Equals("en"))
            {


                string nombreArchivoRecursos = "Pactometro.Properties.Resources_en";


                ResourceManager resourceManager = new ResourceManager(nombreArchivoRecursos, typeof(MainWindow).Assembly);


                var ColumnaElecciones = Lista.FindName("ColumnaElecciones") as GridViewColumn;
                var ColumnaFecha = Lista.FindName("ColumnaFecha") as GridViewColumn;
                var ColumnaEscaños = Lista.FindName("ColumnaEscaños") as GridViewColumn;
                var ColumnaMayoria = Lista.FindName("ColumnaMayoria") as GridViewColumn;

                var columnaPartido = Lista2.FindName("ColumnaPartido") as GridViewColumn;
                var ColumnaEscañoPartido = Lista2.FindName("ColumnaEscañoPartido") as GridViewColumn;
                var ColumnaLogo = Lista2.FindName("ColumnaLogo") as GridViewColumn;

                if (ColumnaElecciones != null)
                {
                    ColumnaElecciones.Header = resourceManager.GetString("ElectionText");
                }
                if (ColumnaFecha != null)
                {
                    ColumnaFecha.Header = resourceManager.GetString("DateText");
                }
                if (ColumnaEscaños != null)
                {
                    ColumnaEscaños.Header = resourceManager.GetString("SeatsText");
                }
                if (ColumnaMayoria != null)
                {
                    ColumnaMayoria.Header = resourceManager.GetString("AbsoluteMajorityText");
                }

                if (columnaPartido != null)
                {
                    columnaPartido.Header = resourceManager.GetString("PartyText");
                }
                if (ColumnaEscañoPartido != null)
                {
                    ColumnaEscañoPartido.Header = resourceManager.GetString("SeatsText");
                }
                if (ColumnaLogo != null)
                {
                    ColumnaLogo.Header = resourceManager.GetString("LogoText");
                }

                MenuOpciones.Header = resourceManager.GetString("OptionsButtonText");
                OpcionAgregar.Header = resourceManager.GetString("AddProcessText");
                OpcionEliminar.Header = resourceManager.GetString("RemoveProcessText");
                OpcionModificar.Header = resourceManager.GetString("ModifyProcessText");
                OpcionNombre.Header = resourceManager.GetString("ModifyNameText");
                OpcionFecha.Header = resourceManager.GetString("ModifyDateText");
                OpcionEscaños.Header = resourceManager.GetString("ModifySeatText");
                OpcionPartidos.Header = resourceManager.GetString("ModifyPartText");
                OpcionSalir.Header = resourceManager.GetString("ExitCDText");
                OpcionDibujar.Header = resourceManager.GetString("DrawText");
                OpcionNormal.Header = resourceManager.GetString("DrawNormalText");
                OpcionComparativa.Header = resourceManager.GetString("DrawComparativeText");
                OpcionPactometro.Header = resourceManager.GetString("DrawPactometerText");
                OpcionCircular.Header = resourceManager.GetString("DrawCircularText");

            }
        }





        //===============================
        //Botones
        //===============================






        private void Salir_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Desea guardar los procesos actuales para futuras ejecuciones? ", "PREGUNTA", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                MessageBox.Show("CerrandO ventana con guardado ... (Puede que algunas salgan repetidas)", "AVISO", MessageBoxButton.OK);
                this.Hide();
            }
            else
            {
                MessageBox.Show("Cerrando ventana sin guardado ...", "AVISO", MessageBoxButton.OK);
                this.Close();
            }
        }




        
        private void AgregarProceso_Click(object sender, RoutedEventArgs e)
        {


            var viewModel = DataContext as EleccionesViewModel;

            VentanaDialogo dialog = new VentanaDialogo(viewModel);

            dialog.ShowDialog();



        }

  

        private void EliminarProceso_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as EleccionesViewModel;

            if (viewModel.ProcesoSeleccionado is null)
            {
                MessageBox.Show("Tiene que elegir el proceso a eliminar");

            }
            else
            {
                viewModel.Procesos.Remove(viewModel.ProcesoSeleccionado);
            }

        }








        private void ModificarProceso_Click(object sender, RoutedEventArgs e)
        {
            EleccionesViewModel viewModel = DataContext as EleccionesViewModel;


            if (viewModel.ProcesoSeleccionado == null)
            {
                    MessageBox.Show("Tiene que elegir el proceso a modificar");

            }
            

        }

        private void ModificarNombre_Click(object sender, RoutedEventArgs e)
        {
            EleccionesViewModel viewModel = DataContext as EleccionesViewModel;
            int indice = viewModel.Procesos.IndexOf(viewModel.ProcesoSeleccionado);

            VentanaNombre dialog = new VentanaNombre(viewModel, indice);

            dialog.ShowDialog();
        }

        private void ModificarFecha_Click(object sender, RoutedEventArgs e)
        {
            EleccionesViewModel viewModel = DataContext as EleccionesViewModel;
            int indice = viewModel.Procesos.IndexOf(viewModel.ProcesoSeleccionado);

            VentanaFecha dialog = new VentanaFecha(viewModel, indice);

            dialog.ShowDialog();


        }



        private void ModificarEscaños_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as EleccionesViewModel;
            int indice = viewModel.Procesos.IndexOf(viewModel.ProcesoSeleccionado);


            VentanaEscaños dialog = new VentanaEscaños(viewModel, indice);

            dialog.ShowDialog();

            
        }



        private void ModificarPartidos_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as EleccionesViewModel;
            int indice = viewModel.Procesos.IndexOf(viewModel.ProcesoSeleccionado);


            VentanaPartidos_1 dialog = new VentanaPartidos_1(viewModel, indice);

            dialog.ShowDialog();




        }



        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            EleccionesViewModel viewModel = DataContext as EleccionesViewModel;

            if (viewModel.ProcesoSeleccionado == null)
            {
                MessageBox.Show("Seleccione el proceso que desea dibujar haciendo click sobre el: ", "AVISO", MessageBoxButton.OK);
                
            }
            else if(viewModel.ProcesoSeleccionado.Escaños>0)
            {
                DibujarGraficaRequested?.Invoke(this, new RoutedEventArgs());
            }


        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            EleccionesViewModel viewModel = DataContext as EleccionesViewModel;

            VentanaComparador dialog = new VentanaComparador(viewModel);

            dialog.ShowDialog();

            if(viewModel.PartidosSeleccionados == null)
            {
                MessageBox.Show("Seleccione los procesos que desea comparar haciendo click sobre el: ", "AVISO", MessageBoxButton.OK);

            }


            else
            {
                DibujarGraficaComparativaRequested?.Invoke(this, new RoutedEventArgs());

            }

        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            EleccionesViewModel viewModel = DataContext as EleccionesViewModel;



            if (viewModel.ProcesoSeleccionado == null)
            {
                MessageBox.Show("Seleccione el proceso que desea tratar con el pactómetro haciendo click sobre el: ", "AVISO", MessageBoxButton.OK);
            }
            else
            {
                PactometroRequested?.Invoke(this, new RoutedEventArgs());


            }

        }

        private void MenuItem_Click_4(object sender, RoutedEventArgs e)
        {
            EleccionesViewModel viewModel = DataContext as EleccionesViewModel;



            if (viewModel.ProcesoSeleccionado == null)
            {
                MessageBox.Show("Seleccione el proceso que desea dibujar haciendo click sobre el: ", "AVISO", MessageBoxButton.OK);
            }
            else
            {
                DibujarGraficaSectoresRequested?.Invoke(this, new RoutedEventArgs());


            }
        }


        private void HabilitarOpcionesDeMenu()
        {
            OpcionNombre.IsEnabled = true;
            OpcionFecha.IsEnabled = true;
            OpcionEscaños.IsEnabled = true;
            OpcionPartidos.IsEnabled = true;

            OpcionNombre.Visibility = Visibility.Visible;
            OpcionFecha.Visibility = Visibility.Visible;
            OpcionEscaños.Visibility = Visibility.Visible;
            OpcionPartidos.Visibility = Visibility.Visible;


        }
        private void DeshabilitarOpcionesDeMenu()
        {
            OpcionNombre.IsEnabled = false;
            OpcionFecha.IsEnabled = false;
            OpcionEscaños.IsEnabled = false;
            OpcionPartidos.IsEnabled = false;

            OpcionNombre.Visibility = Visibility.Hidden;
            OpcionFecha.Visibility = Visibility.Hidden;
            OpcionEscaños.Visibility = Visibility.Hidden;
            OpcionPartidos.Visibility = Visibility.Hidden;

        }


        private void CDTabla_Closing(object sender, CancelEventArgs e)
        {
            var viewModel = DataContext as EleccionesViewModel;

            MessageBoxResult result = MessageBox.Show("¿Guardar procesos para futuras ejecuciones?", "CONFIRMACIÓN", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                MessageBox.Show("Procesos guardados con éxito", "AVISO", MessageBoxButton.OK);
            }
            else
            {
                MessageBox.Show("Procesos eliminados con éxito", "AVISO", MessageBoxButton.OK);

                viewModel.Partidos.Clear();
                viewModel.PartidosSeleccionados.Clear();
                viewModel.ProcesosSeleccionados.Clear();
                viewModel.ProcesoSeleccionado = null;
                viewModel.Procesos.Clear();

            }
        }

        private void Lista_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var viewModel = DataContext as EleccionesViewModel;

            if (e.Key == Key.Delete)
            {
                Proceso procesoSeleccionado = (Proceso)Lista.SelectedItem;

                if (procesoSeleccionado != null)
                {
                    viewModel.Procesos.Remove(procesoSeleccionado);
                }
            }

            else if (e.Key == Key.Enter)
            {
                AgregarProceso_Click(sender, e);
            }
        }


    }
}
    










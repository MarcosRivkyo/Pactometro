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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using Microsoft.Win32;
using System.IO;
using static System.Net.WebRequestMethods;
using System.ComponentModel;
using System.Globalization;
using System.Threading;
using System.Resources;
using System.Windows.Media.Effects;


//Marcos Rivas Kyoguro PB1 
//70962760D

namespace Pactometro
{
    public class ColorChangedEventArgs : EventArgs
    {
        public Color NuevoColor { get; set; }
    
    }


    public partial class MainWindow : Window
    {



        public event EventHandler<ColorChangedEventArgs> ColorCambiado;

        private CDTabla nuevaVentana { get; set; }

        private VentanaConfig ventanaConfig { get; set; }


        private HashSet<Proceso> procesosRegistrados = new HashSet<Proceso>();


        private ObservableCollection<Partido> partidosIzq = new ObservableCollection<Partido>();
        private ObservableCollection<Partido> partidosDer = new ObservableCollection<Partido>();

        private Rectangle rectanguloAMover;



        private EleccionesViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();


            this.Background = new SolidColorBrush(Colors.LightBlue);

            Panel.Height= 400;
            Panel.Width= 800;
            viewModel = new EleccionesViewModel();
            viewModel.Idioma = "es";

            DataContext = viewModel;

            Closing += MainWindow_Closing;
            this.viewModel.IdiomaChanged += viewModel_IdiomaChanged;

        }


 

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            foreach (var ventana in viewModel.OtrasVentanas)
            {
                ventana.Close();
            }
        }

        //===============================
        //Gestión del botón de opciones
        //===============================

        private void MostrarContextMenu(object sender, RoutedEventArgs e)
        {
            ContextMenu contextMenu = (ContextMenu)Resources["ContextMenu"];
            contextMenu.PlacementTarget = (UIElement)sender;
            contextMenu.IsOpen = true;

        }


        //===============================
        //Gestión de las ventanas
        //===============================
        private void MostrarNuevaVentana(object sender, RoutedEventArgs e)
        {
            if (nuevaVentana == null || !nuevaVentana.IsVisible)
            {


                nuevaVentana = new CDTabla(viewModel);
                viewModel.OtrasVentanas.Add(nuevaVentana);
                nuevaVentana.Closing += VentanaSecundaria_Closing;

                nuevaVentana.DibujarGraficaRequested += DibujarGrafica;
                nuevaVentana.DibujarGraficaComparativaRequested += DibujarGraficaComparativa;
                nuevaVentana.PactometroRequested += Pactometro;
                nuevaVentana.DibujarGraficaSectoresRequested += DibujarGraficaSectores;

                nuevaVentana.Show();

            }
            else
            {
                MessageBox.Show("Ya existe la ventana CDTabla", "AVISO", MessageBoxButton.OK);
                
                nuevaVentana.Focus();

                
            }

        }

 
        private void VentanaSecundaria_Closing(object sender, CancelEventArgs e)
        {
            nuevaVentana = null;


        }

        private void CerrarNuevaVentana(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("¿Está seguro de que desea salir de la aplicación?", "PREGUNTA", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                MessageBox.Show("Saliendo de la aplicación", "CIERRE", MessageBoxButton.OK);
                this.Close();
            }
            else
                MessageBox.Show("Cierre cancelado", "CANCELADO", MessageBoxButton.OK);
        }

        private void CerrarNuevaVentanaCD(object sender, RoutedEventArgs e)
        {
            if (nuevaVentana == null || !nuevaVentana.IsVisible)
                MessageBox.Show("No se ha abierto la ventana CDTabla", "ERROR", MessageBoxButton.OK);
            else
                if (MessageBox.Show("Está seguro de que desea cerrar la ventana CDTabla?", "PREGUNTA", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                MessageBox.Show("Saliendo de la ventana CDTabla", "CIERRE", MessageBoxButton.OK);
                nuevaVentana.Close();
            }
            else
                MessageBox.Show("Cierre cancelado", "CANCELADO", MessageBoxButton.OK);




        }


        private void MostrarHistorial(object sender, RoutedEventArgs e)
        {
            popupProcesos.IsOpen = true; 
        }


        //===============================
        //Gestión de las gráficas
        //===============================

        private double lastPanelWidth = 0;
        private double lastPanelHeight = 0;


          private void DibujarGrafica(object sender, RoutedEventArgs e)
          {

              Proceso procesoSeleccionado = new Proceso();

              procesoSeleccionado = viewModel.ProcesoSeleccionado;


            if (procesoSeleccionado == null)
            {

                MessageBox.Show("Seleccione el proceso que desea tratar en la ventana secundaria", "AVISO", MessageBoxButton.OK);
                return;

            }


            if (procesosRegistrados.Add(procesoSeleccionado))
            {

                viewModel.HistorialProcesos.Add(procesoSeleccionado);

            }



            int numPartidos = procesoSeleccionado.ListaPartidos.Count();
            int[] escaños = new int[numPartidos];
            string[] nombres = new string[numPartidos];
            int mayoria = procesoSeleccionado.MayoriaAbsoluta;

            int intervaloMarcas = 20;
            int i = 0;

            double anchuraMaximoBarra = 20;
            double anchuraMínimoBarra = 10;
            double separacion = 20;

            double canvasHeight = Panel.Height;


            double espacioBarra = Panel.Width / numPartidos;
            double barWidth = Math.Max(Math.Max(anchuraMínimoBarra, espacioBarra - separacion), anchuraMaximoBarra);


            Panel.Visibility = Visibility.Visible;
            Panel_Pactometro.Visibility = Visibility.Hidden;

            CanvasBarrasIzq.Children.Clear();
            CanvasBarrasDer.Children.Clear();
            Panel.Children.Clear();


            titulo.Visibility = Visibility.Visible;
            titulo.Text = procesoSeleccionado.Elecciones;



            for (int j = 0; j < numPartidos; j++)
            {
                  escaños[j] = procesoSeleccionado.ListaPartidos[j].Escaños;
                  nombres[j] = procesoSeleccionado.ListaPartidos[j].Nombre;
            }

            int maxEscaños = escaños.Max();
            int referenciaAltura = Math.Max(maxEscaños, mayoria);
            int escañosTotal = escaños.Sum();


            if (escañosTotal != procesoSeleccionado.Escaños)
            {

                MessageBox.Show("La suma total de escaños de los partidos registrados no coincide con el total de escaños del proceso electoral. Por favor, ajuste los escaños de los partidos o modifique la lista de partidos para corregir esta discrepancia.", "Error de Coincidencia de Escaños", MessageBoxButton.OK);
                return;

            }

             Line xAxis = new Line
             {
                X1 = 20,
                X2 = Panel.Width - 10,
                Y1 = Panel.Height - 30,
                Y2 = Panel.Height - 30,
                Stroke = Brushes.Black,
             };

             Panel.Children.Add(xAxis);


            for (int es = 0; es <= referenciaAltura; es += intervaloMarcas)
            {
                double yCoord = Panel.Height - ((Panel.Height - 30) * es / referenciaAltura);

                Line marca = new Line
                {
                    X1 = 15,
                    X2 = 20,
                    Y1 = yCoord - 30,
                    Y2 = yCoord - 30,
                    Stroke = Brushes.Red,
                    StrokeThickness = 1,
                };

                Panel.Children.Add(marca);

                TextBlock escañosLabel = new TextBlock
                {
                    Text = es.ToString(),
                    TextAlignment = TextAlignment.Right,
                };

                escañosLabel.FontSize = 10;
                escañosLabel.Foreground = Brushes.Red;

                Canvas.SetLeft(escañosLabel, 1);
                Canvas.SetTop(escañosLabel, yCoord - 45);

                Panel.Children.Add(escañosLabel);
            }


            
            foreach (Partido partido in procesoSeleccionado.ListaPartidos)
            {


                double barHeight = (partido.Escaños / (double)referenciaAltura) * (canvasHeight - 30); ;

                Line bar = new Line
                {
                    X1 = i * (barWidth + separacion) + barWidth - 10,
                    X2 = i * (barWidth + separacion) + barWidth - 10,
                    Y1 = Panel.Height - 30,
                    Y2 = Panel.Height - barHeight - 30,
                    Stroke = new SolidColorBrush(partido.color),
                    StrokeThickness = 10
                };


                    ToolTip tooltip = new ToolTip
                    {
                        Content = "Escaños: " + escaños[i].ToString()
                    };
                    bar.ToolTip = tooltip;


                TextBlock partidoTextBlock = new TextBlock
                {
                    Text = nombres[i],
                    TextAlignment = TextAlignment.Right,
                };

                partidoTextBlock.FontSize = 10;

                Canvas.SetLeft(partidoTextBlock, i * (barWidth + separacion) + barWidth - 15);
                Canvas.SetBottom(partidoTextBlock, 5);

                Panel.Children.Add(bar);
                Panel.Children.Add(partidoTextBlock);

                i++;
            }
            
            

          }
      


        private void DibujarGraficaSectores(object sender, RoutedEventArgs e)
        {


            Proceso procesoSeleccionado = viewModel.ProcesoSeleccionado;

            if (procesoSeleccionado == null)
            {
                    MessageBox.Show("Seleccione el proceso que desea tratar en la ventana secundaria", "AVISO", MessageBoxButton.OK);

                     return;
            }


            if (procesosRegistrados.Add(procesoSeleccionado))
            {

                viewModel.HistorialProcesos.Add(procesoSeleccionado);

            }

            int numPartidos = procesoSeleccionado.ListaPartidos.Count();
            int[] escaños = new int[numPartidos];
            string[] nombres = new string[numPartidos];
            int totalEscaños = viewModel.ProcesoSeleccionado.Escaños;


            double radio = Math.Min(Panel.Width / 2, Panel.Height / 2);
            Point centro = new Point(Panel.Width / 2, Panel.Height / 2);

            double inicioAngulo = 0;


            Panel.Visibility = Visibility.Visible;
            Panel_Pactometro.Visibility = Visibility.Hidden;

            CanvasBarrasIzq.Children.Clear();
            CanvasBarrasDer.Children.Clear();
            Panel.Children.Clear();

            titulo.Visibility = Visibility.Visible;
            titulo.Text = procesoSeleccionado.Elecciones;

            for (int j = 0; j < numPartidos; j++)
            {
                escaños[j] = procesoSeleccionado.ListaPartidos[j].Escaños;
                nombres[j] = procesoSeleccionado.ListaPartidos[j].Nombre;
            }

            int escañosTotal = escaños.Sum();
            if (escañosTotal != procesoSeleccionado.Escaños)
            {

                MessageBox.Show("La suma total de escaños de los partidos registrados no coincide con el total de escaños del proceso electoral. Por favor, ajuste los escaños de los partidos o modifique la lista de partidos para corregir esta discrepancia.", "Error de Coincidencia de Escaños", MessageBoxButton.OK);
                return;

            }

            int i = 0;
    
            foreach(Partido partido in procesoSeleccionado.ListaPartidos)
            {

            double porcentaje = (partido.Escaños / (double)totalEscaños) * 360;

            SolidColorBrush colorBrush = new SolidColorBrush(partido.color);

            System.Windows.Shapes.Path sector = CrearSector(centro, radio, inicioAngulo, porcentaje, colorBrush);

            ToolTip tooltip = new ToolTip
            {
                    Content = "Partido: " + partido.Nombre + "\nEscaños: " + partido.Escaños
            };
            sector.ToolTip = tooltip; 


            Panel.Children.Add(sector);

            TextBlock partidoTextBlock = new TextBlock
            {
                Text = partido.Nombre,
                TextAlignment = TextAlignment.Center,
                FontSize = 10

            };

            Point labelPosition = CalcularPuntoEnCircunferencia(centro, radio + 20, inicioAngulo + porcentaje / 2);
            Canvas.SetLeft(partidoTextBlock, labelPosition.X - partidoTextBlock.ActualWidth / 2);
            Canvas.SetTop(partidoTextBlock, labelPosition.Y - partidoTextBlock.ActualHeight / 2);

            Panel.Children.Add(partidoTextBlock);

            inicioAngulo += porcentaje;
                
            i++;

        }   

        }

        private System.Windows.Shapes.Path CrearSector(Point centro, double radio, double inicioAngulo, double angulo, SolidColorBrush color)
        {
            System.Windows.Shapes.Path sector = new System.Windows.Shapes.Path();
            sector.Fill = color;

            PathGeometry geometria = new PathGeometry();
            PathFigure figura = new PathFigure();
            figura.StartPoint = centro;
            figura.Segments.Add(new LineSegment(CalcularPuntoEnCircunferencia(centro, radio, inicioAngulo), true));
            figura.Segments.Add(new ArcSegment(CalcularPuntoEnCircunferencia(centro, radio, inicioAngulo + angulo), new Size(radio, radio), 0, angulo > 180, SweepDirection.Clockwise, true));
            figura.Segments.Add(new LineSegment(centro, true));

            geometria.Figures.Add(figura);
            sector.Data = geometria;

            return sector;
        }

        private Point CalcularPuntoEnCircunferencia(Point centro, double radio, double angulo)
        {
            double radianes = angulo * (Math.PI / 180.0);
            double x = centro.X + radio * Math.Cos(radianes);
            double y = centro.Y + radio * Math.Sin(radianes);
            
            return new Point(x, y);
 
        }
      
        private void DibujarGraficaComparativa(object sender, RoutedEventArgs e)
        {
              var viewModel = DataContext as EleccionesViewModel;


              if(viewModel.ProcesosSeleccionados.Count <= 0)
              {
                  MessageBox.Show("Seleccione los proceso que desea tratar en la ventana secundaria", "AVISO", MessageBoxButton.OK);
                  
                  return;

              }


              Panel.Visibility = Visibility.Visible;
              Panel_Pactometro.Visibility = Visibility.Hidden;

              CanvasBarrasIzq.Children.Clear();
              CanvasBarrasDer.Children.Clear();
              Panel.Children.Clear();



              double posX = 30;
              double canvasHeight = Panel.Height;
              double canvasWidth = Panel.Width;
              int numProcesos = viewModel.ProcesosSeleccionados.Count();
              int numPartidos = viewModel.PartidosSeleccionados.Count();


            double separacionMaxima = 50;
            double separacionMinima = 10;

            int numTotalBarras = numPartidos * numProcesos;
            double espacioTotalSeparaciones = (numPartidos - 1) * separacionMaxima + (numTotalBarras - numPartidos) * separacionMinima;
            double anchoDisponibleParaBarras = canvasWidth - espacioTotalSeparaciones;


            double anchoBarra = anchoDisponibleParaBarras / numTotalBarras;


            int mayoria = 0;

              foreach (Proceso proceso in viewModel.ProcesosSeleccionados)
              {
                  if (proceso.MayoriaAbsoluta > mayoria)
                      mayoria = proceso.MayoriaAbsoluta;

              }



              double barHeight;

              Panel.Children.Clear();

              if (viewModel.PartidosSeleccionados.Count < 0)
              {
                  MessageBox.Show("No ha seleccionado ningún proceso");
              }
              else
              {
                  titulo.Visibility = Visibility.Visible;
                  titulo.Text = "Gráfica comparativa";

               int maxEscaños = 0;

                foreach (var proceso in viewModel.ProcesosSeleccionados)
                {
                    foreach (var partido in proceso.ListaPartidos)
                    {
                        if (viewModel.PartidosSeleccionados.Any(p => p.Nombre == partido.Nombre))
                        {
                            if (partido.Escaños > maxEscaños)
                            {
                                maxEscaños = partido.Escaños;
                            }
                        }
                    }
                }


                int referenciaAltura = Math.Max(maxEscaños, mayoria);


                Line xAxis = new Line
                  {
                      X1 = 20,
                      X2 = Panel.Width - 10,
                      Y1 = Panel.Height - 30,
                      Y2 = Panel.Height - 30,
                      Stroke = Brushes.Black,
                  };

                  Panel.Children.Add(xAxis);



                int intervaloMarcas = 20;

                for (int es = 0; es <= referenciaAltura; es += intervaloMarcas)
                {
                    double yCoord = Panel.Height - ((Panel.Height - 30) * es / referenciaAltura);

                    Line marca = new Line
                    {
                        X1 = 15,
                        X2 = 20,
                        Y1 = yCoord - 30,
                        Y2 = yCoord - 30,
                        Stroke = Brushes.Red,
                        StrokeThickness = 1,
                    };

                    Panel.Children.Add(marca);

                    TextBlock escañosLabel = new TextBlock
                    {
                        Text = es.ToString(),
                        TextAlignment = TextAlignment.Right,
                    };

                    escañosLabel.FontSize = 10;
                    escañosLabel.Foreground = Brushes.Red;

                    Canvas.SetLeft(escañosLabel, 1);
                    Canvas.SetTop(escañosLabel, yCoord - 45);

                    Panel.Children.Add(escañosLabel);
                }


                  int opacidadInicial = 255;

                  foreach (var proceso in viewModel.ProcesosSeleccionados)
                  {
                      proceso.Opacidad = opacidadInicial;


                      opacidadInicial -= 50;

                      if (opacidadInicial <= 0)
                      {
                          opacidadInicial = 255;
                      }


                  }




                foreach (var partido in viewModel.PartidosSeleccionados)
                  {

                    string nombrePartido = partido.Nombre;


                    TextBlock partidoTextBlock = new TextBlock
                    {
                        Text = nombrePartido,
                        TextAlignment = TextAlignment.Left,
                    };

                    partidoTextBlock.FontSize = 10;

                    Canvas.SetLeft(partidoTextBlock, posX-8);
                    Canvas.SetBottom(partidoTextBlock, 5);

                    Panel.Children.Add(partidoTextBlock);


                    foreach (var proceso in viewModel.ProcesosSeleccionados)
                      {
                          int escaño = proceso.ListaPartidos.FirstOrDefault(p => p.Nombre == partido.Nombre)?.Escaños ?? 0;

                          barHeight = (escaño / (double)referenciaAltura) * (canvasHeight-30);


                          Color colorBase = partido.color;
                          Color colorAjustado = Color.FromArgb((byte)proceso.Opacidad, colorBase.R, colorBase.G, colorBase.B);



                          Line bar = new Line
                          {

                              X1 = posX,
                              X2 = posX,
                              Y1 = Panel.Height - 30, 
                              Y2 = Panel.Height - barHeight - 30,
                              Stroke = new SolidColorBrush(colorAjustado),
                              StrokeThickness = 10
                          };

                          ToolTip tooltip = new ToolTip
                          {
                              Content = "Escaños: " + escaño.ToString()
                          };

                          bar.ToolTip = tooltip;



                        Panel.Children.Add(bar);

                        posX += anchoBarra + separacionMinima;

                    }
                    posX += separacionMaxima - separacionMinima; 


                }

                double alturaLeyenda = viewModel.ProcesosSeleccionados.Count * 20 + 10;

                Rectangle recta = new Rectangle
                {
                    Width = 130,
                    Height = alturaLeyenda,
                    Fill = Brushes.LightGray,
                };

                double yPosition = 10; 
                double rectSize = 15;

                foreach (var proceso in viewModel.ProcesosSeleccionados)
                {
                    Color colorConOpacidad = Color.FromArgb((byte)proceso.Opacidad, 0, 0, 0); 

                    Rectangle colorRect = new Rectangle
                    {
                        Width = 30,
                        Height = rectSize,
                        Fill = new SolidColorBrush(colorConOpacidad), 
                        Stroke = Brushes.Black
                    };


                    Canvas.SetLeft(colorRect, 680);
                    Canvas.SetTop(colorRect, yPosition);
                    Panel.Children.Add(colorRect);

                    TextBlock textoDatos = new TextBlock
                    {
                        Text = proceso.Fecha, 
                        Foreground = Brushes.Black
                    };

                    Canvas.SetLeft(textoDatos, 730);
                    Canvas.SetTop(textoDatos, yPosition);
                    Panel.Children.Add(textoDatos);

                    yPosition += 20; 
                }

                Canvas.SetRight(recta, 0);
                Canvas.SetTop(recta, 0);
                Panel.Children.Add(recta);

                Panel.Children.Remove(recta);
                Panel.Children.Insert(0, recta);


            }
        }
          


        private void Pactometro(object sender, RoutedEventArgs e)
        {
            Proceso procesoSeleccionado = viewModel.ProcesoSeleccionado;

            if(procesoSeleccionado == null)
            {

                MessageBox.Show("Seleccione el proceso que desea tratar en la ventana secundaria", "AVISO", MessageBoxButton.OK);
                return;

            }


            if (procesosRegistrados.Add(procesoSeleccionado))
            {

                viewModel.HistorialProcesos.Add(procesoSeleccionado);

            }


            escañosIzq = 0;
            escañosDer = 0;
            partidosIzq.Clear();
            partidosDer.Clear();

 
            foreach (var partido in procesoSeleccionado.ListaPartidos)
                {
                partidosIzq.Add(partido);

            }


            titulo.Visibility = Visibility.Visible;
            titulo.Text = procesoSeleccionado.Elecciones;


            Panel_Pactometro.Visibility = Visibility.Visible;
            Panel.Visibility = Visibility.Hidden;


            Panel.Children.Clear();


            CanvasBarrasIzq.Visibility = Visibility.Visible;
            CanvasBarrasDer.Visibility = Visibility.Visible;
            ListaGanadores.Visibility = Visibility.Visible;
            ListaPerdedores.Visibility = Visibility.Visible;

            Ganador.Visibility = Visibility.Visible;
            TextGanador.Visibility = Visibility.Visible;
            TextPerdedor.Visibility = Visibility.Visible;

            titulo.Text = procesoSeleccionado.Elecciones;

            DibujarBarras();


        }

        int escañosIzq = 0;
        int escañosDer = 0;

        private void DibujarBarras()
        {
          

            CanvasBarrasIzq.Children.Clear();
            CanvasBarrasDer.Children.Clear();

            double canvasWidth = CanvasBarrasIzq.Width;
            double canvasHeight = CanvasBarrasIzq.Height;
            double y = 0;
            double barHeight;
            int numEscaños = viewModel.ProcesoSeleccionado.Escaños;
            int mayoriaAbsoluta = viewModel.ProcesoSeleccionado.MayoriaAbsoluta;


            int numPartidos = partidosIzq.Count();
            int[] escaños = new int[numPartidos];





                for (int i = 0; i < numPartidos; i++)
                {
                    escaños[i] = partidosIzq[i].Escaños;

                    if (escaños[i] > mayoriaAbsoluta)
                    {
                        MessageBox.Show("Un partido ha sobrepasado la mayoría absoluta, luego será el ganador de las elecciones.", "AVISO", MessageBoxButton.OK);
                    }

                }

            int escañosTotal = escaños.Sum();
            if (escañosTotal != numEscaños)
            {

                MessageBox.Show("La suma total de escaños de los partidos registrados no coincide con el total de escaños del proceso electoral. Por favor, ajuste los escaños de los partidos o modifique la lista de partidos para corregir esta discrepancia.", "Error de Coincidencia de Escaños", MessageBoxButton.OK);
                return;

            }

            foreach (var partido in partidosIzq)
                {
                    Color col = partido.color;

                    barHeight = ((partido.Escaños / (double)numEscaños) * canvasHeight);

                    Rectangle barra = new Rectangle
                    {
                        Width = canvasWidth,
                        Height = barHeight,
                        Fill = new SolidColorBrush(col),
                        Tag = partido.Nombre,
                    };

                    TextBlock etiqueta = new TextBlock
                    {

                        Height = barHeight,
                        Foreground = Brushes.Black,
                        FontSize = 8,
                        Tag = partido.Nombre,


                    };

                    ToolTip tooltip = new ToolTip
                    {
                        Content = "Partido: " + partido.Nombre.ToString() + "\nEscaños: " + partido.Escaños.ToString()
                    };

                    barra.ToolTip = tooltip;

                    etiqueta.Text = partido.Nombre + " - " + partido.Escaños.ToString();


                    Canvas.SetLeft(barra, 0);
                    Canvas.SetBottom(barra, y);

                    Canvas.SetLeft(etiqueta, canvasWidth + 8);
                    Canvas.SetBottom(etiqueta, y);



                    escañosIzq += partido.Escaños;

                    viewModel.EscañosTotalIzq = escañosIzq.ToString();

                    escañosDer = 0;

                    viewModel.EscañosTotalDer = escañosDer.ToString();

                    CanvasBarrasIzq.Children.Add(barra);
                    CanvasBarrasIzq.Children.Add(etiqueta);


                Line xAxis = new Line
                {
                    X1 = 10,
                    X2 = Panel_Pactometro.Width-40,

                    Stroke = Brushes.Black,
                };

                Canvas.SetBottom(xAxis, (mayoriaAbsoluta / (double)numEscaños) * Panel_Pactometro.Height);

                Panel_Pactometro.Children.Add(xAxis);

                y += barra.Height;




                    barra.MouseDown += (sender, e) =>
                                    {


                                        if (sender is Rectangle rect)
                                        {

                                            if (CanvasBarrasIzq.Children.Contains(barra))
                                            {

                                                MoverBarraYEtiquetaEntreCanvas(barra, etiqueta, CanvasBarrasIzq, CanvasBarrasDer);

                                            }
                                            else if (CanvasBarrasDer.Children.Contains(barra))
                                            {
                                                MoverBarraYEtiquetaEntreCanvas(barra, etiqueta, CanvasBarrasDer, CanvasBarrasIzq);

                                            }

                                            double alturaAcumulativaIzq = 0;
                                            double alturaAcumulativaDer = 0;


                                            foreach (var barraIzquierda in CanvasBarrasIzq.Children)
                                            {
                                                if (barraIzquierda is Rectangle rectIzquierda)
                                                {
                                                    alturaAcumulativaIzq += rectIzquierda.Height;
                                                }
                                            }

                                            foreach (var barraDerecha in CanvasBarrasDer.Children)
                                            {
                                                if (barraDerecha is Rectangle rectDerecha)
                                                {
                                                    alturaAcumulativaDer += rectDerecha.Height;
                                                }
                                            }

                                            bool ganaIzq = alturaAcumulativaIzq > alturaAcumulativaDer;
                                            bool ganaDer = alturaAcumulativaDer > alturaAcumulativaIzq;

                                            if (ganaIzq)
                                            {
                                                ListaGanadores.ItemsSource = partidosIzq;
                                                ListaPerdedores.ItemsSource = partidosDer;
                                            }

                                            if (ganaDer)
                                            {
                                                ListaGanadores.ItemsSource = partidosDer;
                                                ListaPerdedores.ItemsSource = partidosIzq;


                                            }

                                        }

                                    };

                }
                                    
        }



        private void MoverBarraYEtiquetaEntreCanvas(Rectangle barra, TextBlock etiqueta, Canvas canvasOrigen, Canvas canvasDestino)
        {
            Partido partido = null;

            TextBlock etiquetaNueva = null;
            TextBlock etiquetaNuevaDer = null;

            if (canvasOrigen == CanvasBarrasIzq)
            {

                partido = partidosIzq.FirstOrDefault(p => p.Nombre == barra.Tag.ToString());

                if (partido != null)
                {
                    

                    etiquetaNueva = CanvasBarrasIzq.Children.OfType<TextBlock>().FirstOrDefault(tb => tb.Tag?.ToString() == barra.Tag?.ToString());

                    if (etiquetaNueva != null)
                    {
                        CanvasBarrasIzq.Children.Remove(barra);

                        CanvasBarrasIzq.Children.Remove(etiquetaNueva);

                        partidosIzq.Remove(partido);
                        partidosDer.Add(partido);

                        escañosIzq -= partido.Escaños;
                        escañosDer += partido.Escaños;

                        viewModel.EscañosTotalIzq = escañosIzq.ToString();
                        viewModel.EscañosTotalDer = escañosDer.ToString();

                        etiquetaNueva = new TextBlock
                        {
                            Foreground = etiqueta.Foreground,
                            FontSize = etiqueta.FontSize,
                            Text = partido.Nombre + " - " + partido.Escaños.ToString(),
                            Tag = partido.Nombre,
                        };


                        CanvasBarrasDer.Children.Add(etiquetaNueva);
                        CanvasBarrasDer.Children.Add(barra);

                        Canvas.SetLeft(barra, 0);
                        Canvas.SetBottom(barra, 0);

                        Canvas.SetLeft(etiquetaNueva, 0);
                        Canvas.SetBottom(etiquetaNueva, 0);

                        double yDerecho = 0;
                        double yIzquierdo = 0;

                        foreach (var barraIzquierda in CanvasBarrasIzq.Children)
                        {
                            if (barraIzquierda is Rectangle rectIzquierda)
                            {

                                Canvas.SetBottom(rectIzquierda, yIzquierdo);

                                TextBlock etiquetaBarraIzquierda = CanvasBarrasIzq.Children.OfType<TextBlock>().FirstOrDefault(tb => tb.Tag?.ToString() == rectIzquierda.Tag?.ToString());

                                if (etiquetaBarraIzquierda != null)
                                {
                                    Canvas.SetLeft(etiquetaBarraIzquierda, Canvas.GetLeft(rectIzquierda) + rectIzquierda.Width + 8);
                                    Canvas.SetBottom(etiquetaBarraIzquierda, yIzquierdo);
                                }

                                yIzquierdo += rectIzquierda.Height;

                            }
                        }


                        foreach (var barraDerecha in CanvasBarrasDer.Children)
                        {

                            if (barraDerecha is Rectangle rectDerecha)
                            {
                                Canvas.SetBottom(rectDerecha, yDerecho);

                                TextBlock etiquetaBarraDerecha = CanvasBarrasDer.Children.OfType<TextBlock>().FirstOrDefault(tb => tb.Tag?.ToString() == rectDerecha.Tag?.ToString());

                                if (etiquetaBarraDerecha != null)
                                {
                                    Canvas.SetLeft(etiquetaBarraDerecha, Canvas.GetLeft(rectDerecha) + rectDerecha.Width + 8);
                                    Canvas.SetBottom(etiquetaBarraDerecha, yDerecho);
                                }
                                yDerecho += rectDerecha.Height;
                            }


                        }

                    }
                    else
                    {
                        MessageBox.Show("No se encontró la etiqueta");
                    }

                }
                else
                {
                    MessageBox.Show("No se encontró el partido");
                }

            }
            if (canvasOrigen == CanvasBarrasDer)
            {

                partido = partidosDer.FirstOrDefault(p => p.Nombre == barra.Tag.ToString());

                if (partido != null)
                {



                    etiquetaNuevaDer = CanvasBarrasDer.Children.OfType<TextBlock>().FirstOrDefault(tb => tb.Tag?.ToString() == barra.Tag?.ToString());

                    if (etiquetaNuevaDer != null)
                    {
                        CanvasBarrasDer.Children.Remove(barra);

                        CanvasBarrasDer.Children.Remove(etiquetaNuevaDer);

                        partidosDer.Remove(partido);
                        partidosIzq.Add(partido);

                        escañosIzq += partido.Escaños;
                        escañosDer -= partido.Escaños;

                        viewModel.EscañosTotalIzq = escañosIzq.ToString();
                        viewModel.EscañosTotalDer = escañosDer.ToString();

                        etiquetaNuevaDer = new TextBlock
                        {
                            Foreground = etiqueta.Foreground,
                            FontSize = etiqueta.FontSize,
                            Text = partido.Nombre + " - " + partido.Escaños.ToString(),
                            Tag = partido.Nombre,

                        };



                        CanvasBarrasIzq.Children.Add(etiquetaNuevaDer);
                        CanvasBarrasIzq.Children.Add(barra);

                        Canvas.SetLeft(barra, 0);
                        Canvas.SetBottom(barra, 0);

                        double yDerecho = 0;
                        double yIzquierdo = 0;

                        foreach (var barraIzquierda in CanvasBarrasIzq.Children)
                        {
                            if (barraIzquierda is Rectangle rectIzquierda)
                            {

                                  
                                TextBlock etiquetaBarraIzquierda = CanvasBarrasIzq.Children.OfType<TextBlock>().FirstOrDefault(tb => tb.Tag?.ToString() == rectIzquierda.Tag?.ToString());

                                if (etiquetaBarraIzquierda != null)
                                {
                                    Canvas.SetBottom(rectIzquierda, yIzquierdo);

                                    Canvas.SetLeft(etiquetaBarraIzquierda, Canvas.GetLeft(barra) + barra.Width + 8);
                                    Canvas.SetBottom(etiquetaBarraIzquierda, yIzquierdo);
                                }

                                yIzquierdo += rectIzquierda.Height;


                            }
                        }


                        foreach (var barraDerecha in CanvasBarrasDer.Children)
                        {

                            if (barraDerecha is Rectangle rectDerecha)
                            {

                                TextBlock etiquetaBarraDerecha = CanvasBarrasDer.Children.OfType<TextBlock>().FirstOrDefault(tb => tb.Tag?.ToString() == rectDerecha.Tag?.ToString());

                                if (etiquetaBarraDerecha != null)
                                {
                                    Canvas.SetBottom(rectDerecha, yDerecho);

                                    Canvas.SetLeft(etiquetaBarraDerecha, Canvas.GetLeft(barra) + barra.Width + 8);
                                    Canvas.SetBottom(etiquetaBarraDerecha, yDerecho);
                                }
                                yDerecho += rectDerecha.Height;
                            }


                        }
                    }
                    else
                    {
                        MessageBox.Show("No se encontró la etiqueta");
                    }
                }
                else{
                                        
                    
                        MessageBox.Show("No se encontró el partido");
                    
                }
            }


        }




        //===============================
        //Botones
        //===============================


        private void Empezar_Click(object sender, EventArgs e)
        {
            BotonOpciones.Visibility = Visibility.Visible;
            ImagenInicio.Visibility= Visibility.Hidden;
            SPanel.Visibility = Visibility.Hidden;
            Panel.Visibility = Visibility.Visible;
            Vista.Visibility = Visibility.Visible;
            Vista_Pactometro.Visibility = Visibility.Visible;
            ToolbarMain.Visibility = Visibility.Visible;
            ToolbarGraficas.Visibility = Visibility.Visible;

        }

        private void Salir_Click(object sender, EventArgs e)
        {

            this.Close();

        }

        protected virtual void OnColorCambiado(ColorChangedEventArgs e)
        {
            ColorCambiado?.Invoke(this, e);
            this.Background = new SolidColorBrush(e.NuevoColor);

        }


        private void CambiarIdioma(object sender, EventArgs e)
        {

            if (viewModel.Idioma.Equals("es"))
            {



                string nombreArchivoRecursos = "Pactometro.Properties.Resources_es";

                ResourceManager resourceManager = new ResourceManager(nombreArchivoRecursos, typeof(MainWindow).Assembly);

                BotonInicio.Content = resourceManager.GetString("InitiateText");
                BotonAyuda.Content = resourceManager.GetString("AidText");
                BotonConfig.Content = resourceManager.GetString("ConfigText");
                BotonSalir.Content = resourceManager.GetString("ExitText");
                BotonOpciones.Content = resourceManager.GetString("OptionsButtonText");

                ToolSalir.Content = resourceManager.GetString("ExitAppText");
                ToolConfig.Content = resourceManager.GetString("OpenConfigText");
                ToolLimpiar.Content = resourceManager.GetString("ClearCanvasText");
                ToolAyuda.Content = resourceManager.GetString("OpenAidText");
                ToolNormal.Content = resourceManager.GetString("DrawNormalText");
                ToolCompa.Content = resourceManager.GetString("DrawComparativeText");
                ToolPacto.Content = resourceManager.GetString("DrawPactometerText");
                ToolCircul.Content = resourceManager.GetString("DrawCircularText");
                ToolHistorial.Content = resourceManager.GetString("LanguageText1");



                MenuItem headExport = BotonOpciones.ContextMenu.Items[0] as MenuItem;
                MenuItem headOpenCD = BotonOpciones.ContextMenu.Items[1] as MenuItem;
                MenuItem headExitCD = BotonOpciones.ContextMenu.Items[2] as MenuItem;
                MenuItem headExit = BotonOpciones.ContextMenu.Items[3] as MenuItem;


                if (headExport != null)
                {
                    headExport.Header = resourceManager.GetString("ExportText");
                }

                if (headOpenCD != null)
                {
                    headOpenCD.Header = resourceManager.GetString("OpenCDText");
                }

                if (headExitCD != null)
                {
                    headExitCD.Header = resourceManager.GetString("ExitCDText");
                }

                if (headExit != null)
                {
                    headExit.Header = resourceManager.GetString("ExitAppText");
                }

            }


            else if (viewModel.Idioma.Equals("en"))
            {




                string nombreArchivoRecursos = "Pactometro.Properties.Resources_en";

                ResourceManager resourceManager = new ResourceManager(nombreArchivoRecursos, typeof(MainWindow).Assembly);

                BotonInicio.Content = resourceManager.GetString("InitiateText");
                BotonAyuda.Content = resourceManager.GetString("AidText");
                BotonConfig.Content = resourceManager.GetString("ConfigText");
                BotonSalir.Content = resourceManager.GetString("ExitText");
                BotonOpciones.Content = resourceManager.GetString("OptionsButtonText");

                ToolSalir.Content = resourceManager.GetString("ExitAppText");
                ToolConfig.Content = resourceManager.GetString("OpenConfigText");
                ToolLimpiar.Content = resourceManager.GetString("ClearCanvasText");
                ToolAyuda.Content = resourceManager.GetString("OpenAidText");
                ToolNormal.Content = resourceManager.GetString("DrawNormalText");
                ToolCompa.Content = resourceManager.GetString("DrawComparativeText");
                ToolPacto.Content = resourceManager.GetString("DrawPactometerText");
                ToolCircul.Content = resourceManager.GetString("DrawCircularText");
                ToolHistorial.Content = resourceManager.GetString("LanguageText1");


                MenuItem headExport = BotonOpciones.ContextMenu.Items[0] as MenuItem;
                MenuItem headOpenCD = BotonOpciones.ContextMenu.Items[1] as MenuItem;
                MenuItem headExitCD = BotonOpciones.ContextMenu.Items[2] as MenuItem;
                MenuItem headExit = BotonOpciones.ContextMenu.Items[3] as MenuItem;


                if (headExport != null)
                {
                    headExport.Header = resourceManager.GetString("ExportText");
                }

                if (headOpenCD != null)
                {
                    headOpenCD.Header = resourceManager.GetString("OpenCDText");
                }

                if (headExitCD != null)
                {
                    headExitCD.Header = resourceManager.GetString("ExitCDText");
                }

                if (headExit != null)
                {
                    headExit.Header = resourceManager.GetString("ExitAppText");
                }

            }
        }

        private void Config_Click(object sender, RoutedEventArgs e)
        {


            if (ventanaConfig == null || !ventanaConfig.IsVisible)
            {
                ventanaConfig = new VentanaConfig(this.viewModel);
                viewModel.OtrasVentanas.Add(ventanaConfig);

                Color colorActual = ((SolidColorBrush)this.Background).Color;

                ventanaConfig.InicializarSliders(colorActual.R, colorActual.G, colorActual.B);

                ventanaConfig.ColorSeleccionado += (s, args) =>
                {
                    OnColorCambiado(new ColorChangedEventArgs { NuevoColor = args.NuevoColor });
                };

                ventanaConfig.Owner = this;

                ventanaConfig.Show();

            }
            else
            {
                MessageBox.Show("Ya existe la ventana de configuración", "AVISO", MessageBoxButton.OK);

                ventanaConfig.Focus();


            }
        }


        private void LimpiarGrafica_Click(object sender, RoutedEventArgs e)
        {
            Panel.Children.Clear();
            
            CanvasBarrasDer.Children.Clear();
            CanvasBarrasIzq.Children.Clear();

            Panel.Visibility = Visibility.Hidden;
            Panel_Pactometro.Visibility = Visibility.Hidden;

            Panel.Visibility = Visibility.Hidden;


        }

        private void Ayuda_Click(object sender, RoutedEventArgs e)
        {
 
            String resourceName = "Manual.pdf";


            var saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                FileName = resourceName,
                Filter = "Archivos pdf | *.pdf",
            };
           
            if(saveFileDialog.ShowDialog() == true)
            {

                String destino = saveFileDialog.FileName;
                String rutaArchivo = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, resourceName);

                if (System.IO.File.Exists(rutaArchivo))
                {
                    System.IO.File.Copy(rutaArchivo, destino, true);
                    MessageBox.Show("Manual descargado con exito");
                }
                else
                {
                    MessageBox.Show("Manual no encontrado");
                }



            }

        }

        private void ExportarGraficaPNG_Click(object sender, RoutedEventArgs e)
        {
            if (Panel.Visibility == Visibility.Visible)
            {

                Panel.UpdateLayout();

                Size size = new Size(Panel.DesiredSize.Width, Panel.DesiredSize.Height);
                Panel.Measure(size);
                Panel.Arrange(new Rect(size));

                RenderTargetBitmap renderTarget = new RenderTargetBitmap(
                    (int)size.Width,
                    (int)size.Height,
                    96d,
                    96d,
                    PixelFormats.Pbgra32);
                renderTarget.Render(Panel); 


                BitmapEncoder encoder = new PngBitmapEncoder();

                encoder.Frames.Add(BitmapFrame.Create(renderTarget));

                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Imágenes PNG|*.png";

                if (saveFileDialog.ShowDialog() == true)
                {
                    using (FileStream stream = new FileStream(saveFileDialog.FileName, FileMode.Create))
                    {
                        encoder.Save(stream);
                    }
                }

            }
            else if(Panel.Visibility == Visibility.Hidden)
            {

                Panel_Pactometro.UpdateLayout();

                Size size = new Size(Panel_Pactometro.DesiredSize.Width, Panel_Pactometro.DesiredSize.Height);
                Panel_Pactometro.Measure(size);
                Panel_Pactometro.Arrange(new Rect(size));

                RenderTargetBitmap renderTarget = new RenderTargetBitmap(
                    (int)size.Width,
                    (int)size.Height,
                    96d,
                    96d,
                    PixelFormats.Pbgra32);
                renderTarget.Render(Panel_Pactometro); 

                BitmapEncoder encoder = new PngBitmapEncoder();

                encoder.Frames.Add(BitmapFrame.Create(renderTarget));

                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Imágenes PNG|*.png";

                if (saveFileDialog.ShowDialog() == true)
                {
                    using (FileStream stream = new FileStream(saveFileDialog.FileName, FileMode.Create))
                    {
                        encoder.Save(stream);
                    }
                }

            }

        }

        private void ExportarGraficaJPG_Click(object sender, RoutedEventArgs e)
        {
            if (Panel.Visibility == Visibility.Visible)
            {
                Panel.UpdateLayout();

                Size size = new Size(Panel.DesiredSize.Width, Panel.DesiredSize.Height);
                Panel.Measure(size);
                Panel.Arrange(new Rect(size));

                RenderTargetBitmap renderTarget = new RenderTargetBitmap(
                    (int)size.Width,
                    (int)size.Height,
                    96d,
                    96d,
                    PixelFormats.Pbgra32);
                renderTarget.Render(Panel);

                BitmapEncoder encoder = new PngBitmapEncoder();

                encoder.Frames.Add(BitmapFrame.Create(renderTarget));

                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Imágenes PNG|*.png";

                if (saveFileDialog.ShowDialog() == true)
                {
                    using (FileStream stream = new FileStream(saveFileDialog.FileName, FileMode.Create))
                    {
                        encoder.Save(stream);
                    }
                }

            }
            else if (Panel.Visibility == Visibility.Hidden)
            {

                Panel_Pactometro.UpdateLayout();

                Size size = new Size(Panel_Pactometro.DesiredSize.Width, Panel_Pactometro.DesiredSize.Height);
                Panel_Pactometro.Measure(size);
                Panel_Pactometro.Arrange(new Rect(size));

                RenderTargetBitmap renderTarget = new RenderTargetBitmap(
                    (int)size.Width,
                    (int)size.Height,
                    96d,
                    96d,
                    PixelFormats.Pbgra32);
                renderTarget.Render(Panel_Pactometro); 

                BitmapEncoder encoder = new PngBitmapEncoder();

                encoder.Frames.Add(BitmapFrame.Create(renderTarget));

                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Imágenes PNG|*.png";

                if (saveFileDialog.ShowDialog() == true)
                {
                    using (FileStream stream = new FileStream(saveFileDialog.FileName, FileMode.Create))
                    {
                        encoder.Save(stream);
                    }
                }

            }

        }


        private void listBoxProcesos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var procesoSeleccionado = listBoxProcesos.SelectedItem as Proceso;

            if (procesoSeleccionado != null)
            {

                viewModel.ProcesoSeleccionado = procesoSeleccionado;

            }

        }


        private void viewModel_IdiomaChanged(object sender, EventArgs e)
        {

            CambiarIdioma(sender, e);

        }


    }
}




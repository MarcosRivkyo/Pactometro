using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Pactometro
{
    public class EleccionesViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler IdiomaChanged;


        private ObservableCollection<Proceso> procesos = new ObservableCollection<Proceso>();
        private ObservableCollection<Proceso> procesosSeleccionados = new ObservableCollection<Proceso>();
        private ObservableCollection<Partido> partidosSeleccionados = new ObservableCollection<Partido>();
        private Proceso procesoSeleccionado = new Proceso();
        private ObservableCollection<Proceso> historialProcesos = new ObservableCollection<Proceso>();
        private ObservableCollection<Partido> partidos = new ObservableCollection<Partido>();
        private List<Window> otrasVentanas = new List<Window>();

        public Dictionary<string, string> RutasDeArchivos {  get; private set; }

        public Dictionary<string, Color> ColoresPartidos { get; private set; }


        private string escañosTotalIzq;

        private string escañosTotalDer;

        private string idioma;


        public EleccionesViewModel()
        {

                RutasDeArchivos = new Dictionary<string, string>();

                        RutasDeArchivos.Add("PP", "Fotos/1200px-PP_icono_2019.svg_.png");
                        RutasDeArchivos.Add("PSOE", "Fotos/1200px-Logotipo_del_PSOE.svg_.png");
                        RutasDeArchivos.Add("VOX", "Fotos/VOX_LOGO_TV_1.080_PX_FONDO_VERDE-1-e1583152027270-1024x580.png");
                        RutasDeArchivos.Add("SUMAR", "Fotos/6485efeb00590.jpeg");
                        RutasDeArchivos.Add("ERC", "Fotos/erc.png");
                        RutasDeArchivos.Add("JUNTS", "Fotos/1200_1510956898Logo_JxCat-18154610.jpg");
                        RutasDeArchivos.Add("EH_BILDU", "Fotos/ECDIMA20140906_0001_1.jpg");
                        RutasDeArchivos.Add("EAJ_PNV", "Fotos/unnamed.jpg");
                        RutasDeArchivos.Add("BNG", "Fotos/R.png");
                        RutasDeArchivos.Add("CCA", "Fotos/6a877328-f255-42c2-920d-262c7e0bb978_16-9-aspect-ratio_default_0.jpg");
                        RutasDeArchivos.Add("UPN", "Fotos/upn.png");
                        RutasDeArchivos.Add("PODEMOS", "Fotos/fotonoticia_20200525114746_1200.jpg");
                        RutasDeArchivos.Add("CS", "Fotos/descarga.png");
                        RutasDeArchivos.Add("MASPAIS", "Fotos/descarga (1).png");
                        RutasDeArchivos.Add("CUP_PR", "Fotos/2345.png");
                        RutasDeArchivos.Add("OTROS", "Fotos/kisspng-computer-icons-handshake-handshake-5b37211108e9e4.1956331815303396010365.jpg");
                        RutasDeArchivos.Add("UPL", "Fotos/descarga (2).png");
                        RutasDeArchivos.Add("SY", "Fotos/kisspng-computer-icons-handshake-handshake-5b37211108e9e4.1956331815303396010365.jpg");
                        RutasDeArchivos.Add("XAV", "Fotos/12.jpg");

                ColoresPartidos = new Dictionary<string, Color>();

                        ColoresPartidos.Add("PP", Colors.Blue);
                        ColoresPartidos.Add("PSOE", Colors.Red);
                        ColoresPartidos.Add("VOX", Colors.Green);
                        ColoresPartidos.Add("PODEMOS", Colors.Purple);
                        ColoresPartidos.Add("SUMAR", Colors.Purple);
                        ColoresPartidos.Add("ERC", Colors.Yellow);
                        ColoresPartidos.Add("JUNTS", Colors.YellowGreen);
                        ColoresPartidos.Add("EH_BILDU", Colors.Wheat);
                        ColoresPartidos.Add("EAJ_PNV", Colors.Turquoise);
                        ColoresPartidos.Add("BNG", Colors.Teal);
                        ColoresPartidos.Add("CCA", Colors.SpringGreen);
                        ColoresPartidos.Add("UPN", Colors.Snow);
                        ColoresPartidos.Add("CS", Colors.Orange);
                        ColoresPartidos.Add("MASPAIS", Colors.Pink);
                        ColoresPartidos.Add("CUD_PR", Colors.Gold);
                        ColoresPartidos.Add("CUP_PR", Colors.Aquamarine);
                        ColoresPartidos.Add("OTROS", Colors.Brown);
                        ColoresPartidos.Add("SY", Colors.LightPink);
                        ColoresPartidos.Add("XAV", Colors.LightSeaGreen);
                        ColoresPartidos.Add("UPL", Colors.Lime);
                        ColoresPartidos.Add("T1", Colors.IndianRed);

        }
        public ObservableCollection<Proceso> Procesos
        {
            get { return procesos; }

            set 
            { 
                if(procesos != value)
                {
                    procesos = value;
                    OnPropertyChanged(nameof(Procesos));
                }

            }
        }

        public ObservableCollection<Proceso> ProcesosSeleccionados
        {
            get { return procesosSeleccionados; }

            set
            {
                if(procesosSeleccionados != value)
                {
                    procesosSeleccionados = value;
                    OnPropertyChanged(nameof(ProcesoSeleccionado));
                }

            }
        }

        public ObservableCollection<Partido> PartidosSeleccionados
        {
            get { return  partidosSeleccionados;}

            set 
            {

                if (partidosSeleccionados != value)
                {
                    partidosSeleccionados = value;
                    OnPropertyChanged(nameof(PartidosSeleccionados));
                }
            }

        }
        public ObservableCollection<Partido> Partidos
        {
            get { return partidos; }

            set
            {

                if (partidos != value)
                {
                    partidos = value;
                    OnPropertyChanged(nameof(Partidos));
                }
            }

        }

        public Proceso ProcesoSeleccionado
        {
            get { return procesoSeleccionado; }

            set
            {

                if (procesoSeleccionado != value)
                {
                    procesoSeleccionado = value;
                    OnPropertyChanged(nameof(ProcesoSeleccionado));
                }
            }

        }
        public ObservableCollection<Proceso> HistorialProcesos
        {
            get { return historialProcesos; }

            set
            {

                if (historialProcesos != value)
                {
                    historialProcesos = value;
                    OnPropertyChanged(nameof(HistorialProcesos));
                }
            }

        }

        public List<Window> OtrasVentanas
        {
            get { return otrasVentanas; }

            set
            {

                if (otrasVentanas != value)
                {
                    otrasVentanas = value;
                    OnPropertyChanged(nameof(OtrasVentanas));
                }

            }

        }



        public string EscañosTotalIzq
        {
            get { return escañosTotalIzq; }
            set
            {
                if (escañosTotalIzq != value)
                {
                    escañosTotalIzq = value;
                    OnPropertyChanged(nameof(EscañosTotalIzq));
                }
            }
        }

        public string EscañosTotalDer
        {
            get { return escañosTotalDer; }
            set
            {
                if (escañosTotalDer != value)
                {
                    escañosTotalDer = value;
                    OnPropertyChanged(nameof(EscañosTotalDer));
                }
            }
        }

        public string Idioma
        {
            get { return idioma; }
            set
            {

                if(idioma != value)
                {
                    idioma = value;
                    OnPropertyChanged(nameof(Idioma));
                    OnIdiomaChanged();
                }
            }

        }





        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnIdiomaChanged()
        {
            IdiomaChanged?.Invoke(this, EventArgs.Empty);
        }

    }
}

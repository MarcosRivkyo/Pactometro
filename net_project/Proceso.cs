using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;



public class Proceso : INotifyPropertyChanged
{

    private string elecciones;
    private string fecha;
    private int escaños;
    private int mayoriaAbsoluta;
    private int opacidad;

    private ObservableCollection<Partido> listaPartidos;


    public string Elecciones
    {
        get { return elecciones; }

        set
        {
            elecciones = value;
            OnPropertyChanged(nameof(Elecciones));

        }
    }


    public string Fecha
    {

        get { return fecha; }
        set 
        { 
              fecha = value; 
              OnPropertyChanged(nameof(Fecha));
        }
    }


    public int Escaños
    {

        get { return escaños; }
        set
        {
            escaños = value;
            OnPropertyChanged(nameof(Escaños));
        }
    }

    public int MayoriaAbsoluta 
    {

        get { return mayoriaAbsoluta; }
        set
        {
            mayoriaAbsoluta = value;
            OnPropertyChanged(nameof(MayoriaAbsoluta));
        }
    }

    public int Opacidad
    {

        get { return opacidad; }
        set
        {
            opacidad = value;
            OnPropertyChanged(nameof(Opacidad));
        }
    }

    public ObservableCollection<Partido> ListaPartidos
    {
        get { return listaPartidos; }
        set
        {
            listaPartidos = value;
            OnPropertyChanged(nameof(ListaPartidos));
        }

    }



    public event PropertyChangedEventHandler PropertyChanged;


    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }


}

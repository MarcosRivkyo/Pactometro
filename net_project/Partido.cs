using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Media;
using System.Windows.Media.Imaging;

public class Partido : INotifyPropertyChanged
{

    private string nombre;
    private string logoPath;
    private int escaños;
    public Color color;



    public string Nombre
    {

        get { return nombre; }
        set
        {
            nombre = value;
            OnPropertyChanged(nameof(Nombre));
        }
    }

    public string LogoPath
    {

        get { return logoPath; }
        set
        {
            logoPath = value;
            OnPropertyChanged(nameof(LogoPath));
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

    public Color Color
    {

        get { return color; }
        set
        {
            color = value;
            OnPropertyChanged(nameof(Color));
        }
    }



    public event PropertyChangedEventHandler PropertyChanged;



    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

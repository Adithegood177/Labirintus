using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Labirintus;

public partial class MainWindow : Window
{
    private const int CellaMeret = 40;

    private readonly int[][,] Palyak =
    {
        new int[,]
        {
            {1,1,1,1,1,1,1,1,1,1,1,1}, //A pályák aival generáltak
            {1,0,0,0,1,0,0,0,0,0,0,1},
            {1,0,1,0,1,0,1,1,1,1,0,1},
            {1,0,1,0,0,0,0,0,0,1,0,1},
            {1,0,1,1,1,1,0,1,0,1,0,1},
            {1,0,0,0,0,1,0,1,0,0,0,1},
            {1,1,1,1,0,1,0,1,1,1,0,1},
            {1,0,0,1,0,0,0,0,0,1,0,1},
            {1,0,1,0,0,1,1,1,0,1,0,1},
            {1,0,0,0,0,0,0,1,0,0,0,1},
            {1,1,1,1,1,1,0,1,1,1,2,1},
            {1,1,1,1,1,1,1,1,1,1,1,1},
        },
        new int[,]
        {
            {1,1,1,1,1,1,1,1,1,1,1,1},
            {1,0,0,0,0,0,1,0,0,0,0,1},
            {1,0,1,1,1,0,1,0,1,1,0,1},
            {1,0,1,0,0,0,1,0,1,0,0,1},
            {1,0,1,0,1,1,1,0,1,0,1,1},
            {1,0,0,0,0,0,0,0,1,0,0,1},
            {1,1,1,1,1,1,0,1,1,1,0,1},
            {1,0,0,0,0,0,0,1,0,0,0,1},
            {1,0,1,1,1,1,0,1,0,1,1,1},
            {1,0,0,0,0,0,0,1,0,0,0,1},
            {1,1,1,1,1,1,0,1,1,1,2,1},
            {1,1,1,1,1,1,1,1,1,1,1,1},
        },
        new int[,]
        {
            {1,1,1,1,1,1,1,1,1,1,1,1},
            {1,0,0,0,0,1,0,0,0,0,0,1},
            {1,0,1,1,0,1,0,1,1,1,0,1},
            {1,0,1,0,0,0,0,1,0,0,0,1},
            {1,0,1,0,1,1,0,1,0,1,1,1},
            {1,0,0,0,1,0,0,1,0,0,0,1},
            {1,1,1,0,1,0,1,1,1,1,0,1},
            {1,0,0,0,0,0,0,0,0,1,0,1},
            {1,0,1,1,1,1,1,1,0,1,0,1},
            {1,0,0,0,0,0,0,0,0,0,0,1},
            {1,1,1,1,1,1,0,1,1,1,2,1},
            {1,1,1,1,1,1,1,1,1,1,1,1},
        }
    };

    private readonly Random Veletlen = new();
    private int[,] AktualisPalya = new int[0, 0];//Ai
    private Point JatekosCella = new(1, 1);//Ai
    private Point KezdoCella = new(1, 1); //Ai
    private Point KijaratCella = new(10, 10);//Ai
    private Ellipse? JatekosAlakzat; //Ai
    private Rectangle? KijaratAlakzat;

    public MainWindow()
    {
        InitializeComponent();
    }

    private void AblakBetoltve(object sender, RoutedEventArgs e)
    {
        InditasVagyReset();
    }

    private void InditasVagyReset()
    {
        AktualisPalya = Palyak[Veletlen.Next(Palyak.Length)];
        JatekosCella = KezdoCella;
        PalyaEpites();
        JatekosRajzolas();
        AllapotFrissites("Találd meg a kijáratot!");
        JatekVaszon.Focus();
    }

    private void PalyaEpites()
    {
        JatekVaszon.Children.Clear();
        JatekosAlakzat = null;
        KijaratAlakzat = null;

        var sorok = AktualisPalya.GetLength(0);
        var oszlopok = AktualisPalya.GetLength(1);
        JatekVaszon.Width = oszlopok * CellaMeret;
        JatekVaszon.Height = sorok * CellaMeret;

        for (var r = 0; r < sorok; r++)
        {
            for (var c = 0; c < oszlopok; c++)
            {
                switch (AktualisPalya[r, c])
                {
                    case 1:
                        FalRajzolas(r, c);
                        break;
                    case 2:
                        KijaratCella = new Point(c, r);
                        KijaratRajzolas(r, c);
                        break;
                }
            }
        }
    }

    private void FalRajzolas(int sor, int oszlop) //Ebbe is segitett az AI
    {
        var fal = new Rectangle
        {
            Width = CellaMeret,
            Height = CellaMeret,
            Fill = new SolidColorBrush(Color.FromRgb(150, 150, 150)),
            Stroke = new SolidColorBrush(Color.FromRgb(90, 90, 90)),
            StrokeThickness = 1
        };

        Canvas.SetLeft(fal, oszlop * CellaMeret);
        Canvas.SetTop(fal, sor * CellaMeret);
        JatekVaszon.Children.Add(fal);
    }

    private void KijaratRajzolas(int sor, int oszlop)
    {
        KijaratAlakzat = new Rectangle
        {
            Width = CellaMeret,
            Height = CellaMeret,
            RadiusX = 4,
            RadiusY = 4,
            Fill = new SolidColorBrush(Color.FromRgb(120, 180, 120)),
            Stroke = new SolidColorBrush(Color.FromRgb(80, 130, 80)),
            StrokeThickness = 2
        };

        Canvas.SetLeft(KijaratAlakzat, oszlop * CellaMeret);
        Canvas.SetTop(KijaratAlakzat, sor * CellaMeret);
        JatekVaszon.Children.Add(KijaratAlakzat);
    }

    private void JatekosRajzolas()
    {
        if (JatekosAlakzat == null)
        {
            JatekosAlakzat = new Ellipse
            {
                Width = CellaMeret * 0.6,
                Height = CellaMeret * 0.6,
                Fill = new SolidColorBrush(Color.FromRgb(200, 180, 80)),
                Stroke = new SolidColorBrush(Color.FromRgb(120, 100, 40)),
                StrokeThickness = 2
            };
            JatekVaszon.Children.Add(JatekosAlakzat);
        }

        var left = JatekosCella.X * CellaMeret + (CellaMeret - JatekosAlakzat.Width) / 2;
        var top = JatekosCella.Y * CellaMeret + (CellaMeret - JatekosAlakzat.Height) / 2;
        Canvas.SetLeft(JatekosAlakzat, left);
        Canvas.SetTop(JatekosAlakzat, top);
    }

    private void BillentyuLenyomva(object sender, KeyEventArgs e) //AI segitett
    {
        if (JatekosAlakzat == null)
        {
            return;
        }

        var irany = e.Key switch
        {
            Key.Up or Key.W => new Point(0, -1),
            Key.Down or Key.S => new Point(0, 1),
            Key.Left or Key.A => new Point(-1, 0),
            Key.Right or Key.D => new Point(1, 0),
            _ => new Point(0, 0)
        };

        if (irany == new Point(0, 0))
        {
            return;
        }

        JatekosMozgatas(irany);
    }

    private void JatekosMozgatas(Point irany)
    {
        var cel = new Point(JatekosCella.X + irany.X, JatekosCella.Y + irany.Y);

        if (FalE(cel))
        {
            JatekosCella = KezdoCella;
            JatekosRajzolas();
            AllapotFrissites("Fal! Visszakerültél a startpontra.");
            return;
        }

        if (!HataronBelul(cel))
        {
            AllapotFrissites("A labirintus határán vagy.");
            return;
        }

        JatekosCella = cel;
        JatekosRajzolas();

        if (NyertE())
        {
            AllapotFrissites("Gratulálok! Megtaláltad a kijáratot.");
        }
        else
        {
            AllapotFrissites("Haladj a zöld kijárat felé!");
        }
    }

    private bool HataronBelul(Point cella)
    {
        var sorok = AktualisPalya.GetLength(0);
        var oszlopok = AktualisPalya.GetLength(1);
        return cella.Y >= 0 && cella.Y < sorok && cella.X >= 0 && cella.X < oszlopok;
    }

    private bool FalE(Point cella)
    {
        if (!HataronBelul(cella))
        {
            return true;
        }

        return AktualisPalya[(int)cella.Y, (int)cella.X] == 1;
    }

    private bool NyertE() //AI segitett
    {
        return Math.Abs(JatekosCella.X - KijaratCella.X) < double.Epsilon &&
               Math.Abs(JatekosCella.Y - KijaratCella.Y) < double.Epsilon;
    }

    private void AllapotFrissites(string uzenet)
    {
        AllapotSzoveg.Text = uzenet;
    }

    private void UjrakezdesKattintas(object sender, RoutedEventArgs e)
    {
        InditasVagyReset();
    }
}
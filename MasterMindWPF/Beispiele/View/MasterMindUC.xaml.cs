namespace MasterMindWPF.Beispiele
{
    using System.Globalization;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Shapes;

    /// <summary>
    /// Interaktionslogik für MasterMindUC.xaml
    /// </summary>
    public partial class MasterMindUC : UserControlBase
    {
        private readonly List<PlayerColor> _playerColors = new();
        private readonly List<string> _availableColors = new()
        {
            "Red",
            "Blue",
            "Green",
            "Yellow",
            "Orange",
            "Purple"
        };

        public MasterMindUC()
        {
            this.InitializeComponent();
            WeakEventManager<UserControlBase, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);

            this.SelectColorCommand = new CommandBase(args => this.OnSelectColor(args), () => true);
            this.PlayerColorCommand = new CommandBase(args => this.OnPlayerColor(args), () => true);
            this.CheckCommand = new CommandBase(args => this.OnCheck(args), () => true);

            this.DataContext = this;
        }

        public CommandBase SelectColorCommand { get; private set; }
        public CommandBase PlayerColorCommand { get; private set; }
        public CommandBase CheckCommand { get; private set; }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.SelectR0C1.FillColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString(_availableColors[0]));
            this.SelectR0C2.FillColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString(_availableColors[1]));
            this.SelectR0C3.FillColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString(_availableColors[2]));
            this.SelectR0C4.FillColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString(_availableColors[3]));
            this.SelectR0C5.FillColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString(_availableColors[4]));
            this.SelectR0C6.FillColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString(_availableColors[5]));

            this.DisableAllPlayerColors();
        }

        private void OnSelectColor(object args)
        {
            string tag = ((Ellipse)args).Tag.ToString();
            Brush color = ((Ellipse)args).Fill;

            string[] selectEllipse = tag.Split(':');

            if (this._playerColors != null)
            {
                int row = int.Parse(new string(selectEllipse[1].Where(char.IsDigit).ToArray()), CultureInfo.CurrentCulture);
                int col = int.Parse(new string(selectEllipse[2].Where(char.IsDigit).ToArray()), CultureInfo.CurrentCulture);

                PlayerColor playerColor = new PlayerColor()
                {
                    Color = color,
                    Row = row,
                    Col = col,
                    PlayerWin = false,
                    LineChecked = false
                };

                if (_playerColors.Any(pc => pc.Row == row && pc.Col == col && pc.Color == color) == false)
                {
                    _playerColors.Add(playerColor);
                }
            }

        }

        private void OnPlayerColor(object args)
        {
            if (this._playerColors != null)
            {

            }
        }

        private void OnCheck(object args)
        {
            if (this._playerColors != null && this._playerColors.Count == 4)
            {
            }
        }

        private void DisableAllPlayerColors()
        {
            this.PlayerR1C1.FillColor = Brushes.LightGray;
            this.PlayerR1C1.IsEnabled = false;
            this.PlayerR1C2.FillColor = Brushes.LightGray;
            this.PlayerR1C2.IsEnabled = false;
            this.PlayerR1C3.FillColor = Brushes.LightGray;
            this.PlayerR1C3.IsEnabled = false;
            this.PlayerR1C4.FillColor = Brushes.LightGray;
            this.PlayerR1C4.IsEnabled = false;

            this.PlayerR2C1.FillColor = Brushes.LightGray;
            this.PlayerR2C1.IsEnabled = false;
            this.PlayerR2C2.FillColor = Brushes.LightGray;
            this.PlayerR2C2.IsEnabled = false;
            this.PlayerR2C3.FillColor = Brushes.LightGray;
            this.PlayerR2C3.IsEnabled = false;
            this.PlayerR2C4.FillColor = Brushes.LightGray;
            this.PlayerR2C4.IsEnabled = false;

            this.PlayerR3C1.FillColor = Brushes.LightGray;
            this.PlayerR3C1.IsEnabled = false;
            this.PlayerR3C2.FillColor = Brushes.LightGray;
            this.PlayerR3C2.IsEnabled = false;
            this.PlayerR3C3.FillColor = Brushes.LightGray;
            this.PlayerR3C3.IsEnabled = false;
            this.PlayerR3C4.FillColor = Brushes.LightGray;
            this.PlayerR3C4.IsEnabled = false;

            this.PlayerR4C1.FillColor = Brushes.LightGray;
            this.PlayerR4C1.IsEnabled = false;
            this.PlayerR4C2.FillColor = Brushes.LightGray;
            this.PlayerR4C2.IsEnabled = false;
            this.PlayerR4C3.FillColor = Brushes.LightGray;
            this.PlayerR4C3.IsEnabled = false;
            this.PlayerR4C4.FillColor = Brushes.LightGray;
            this.PlayerR4C4.IsEnabled = false;
        }

        private sealed class PlayerColor
        {
            public Brush Color { get; set; }

            public int Row { get; set; }

            public int Col { get; set; }

            public bool LineChecked { get; set; }

            public bool PlayerWin { get; set; }
        }
    }
}

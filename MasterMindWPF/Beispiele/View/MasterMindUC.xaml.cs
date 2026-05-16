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

            this.NewPlayCommand = new CommandBase(args => this.OnNewPlay(args), () => true);
            this.ShowColorsCommand = new CommandBase(args => this.OnShowColors(args), () => true);
            this.SelectColorCommand = new CommandBase(args => this.OnSelectColor(args), () => true);
            this.PlayerColorCommand = new CommandBase(args => this.OnPlayerColor(args), () => true);
            this.CheckCommand = new CommandBase(args => this.OnCheck(args), () => true);

            this.DataContext = this;
        }

        public CommandBase NewPlayCommand { get; private set; }
        public CommandBase ShowColorsCommand { get; private set; }
        public CommandBase SelectColorCommand { get; private set; }
        public CommandBase PlayerColorCommand { get; private set; }
        public CommandBase CheckCommand { get; private set; }

        public Brush CurrentSelectedColor { get; set; }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.SelectR0C1.FillColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString(_availableColors[0]));
            this.SelectR0C2.FillColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString(_availableColors[1]));
            this.SelectR0C3.FillColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString(_availableColors[2]));
            this.SelectR0C4.FillColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString(_availableColors[3]));
            this.SelectR0C5.FillColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString(_availableColors[4]));
            this.SelectR0C6.FillColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString(_availableColors[5]));

            this.DisableAllPlayerColors();
            this.RandomStackPanel.Visibility = Visibility.Hidden;
        }

        private void OnNewPlay(object args)
        {
            List<Brush> randomColors = new BrushProvider().GetRandomBrushes();
            this.RandomStackPanel.Visibility = Visibility.Hidden;
            this.RandomR1C1.Fill = randomColors[0];
            this.RandomR1C2.Fill = randomColors[1];
            this.RandomR1C3.Fill = randomColors[2];
            this.RandomR1C4.Fill = randomColors[3];

            if (this._playerColors != null)
            {
                this._playerColors.Clear();

                for (int i = 0; i < randomColors.Count; i++)
                {
                    PlayerColor rndColor = new PlayerColor()
                    {
                        Color = randomColors[0],
                        Row = -1,
                        Col = -1,
                        PlayerWin = false,
                        LineChecked = false,
                        Modus = PlayerModus.RandomColor
                    };

                    this._playerColors.Add(rndColor);
                }

                this.EnablelPlayerColors(1);
            }
        }

        private void OnShowColors(object args)
        {
            this.RandomStackPanel.Visibility = Visibility.Visible;


        }

        private void OnSelectColor(object args)
        {
            string tag = ((Ellipse)args).Tag.ToString();
            Brush color = ((Ellipse)args).Fill;
            string[] selectEllipse = tag.Split(':');

            this.CurrentSelectedColor = color;
        }

        private void OnPlayerColor(object args)
        {
            string tag = ((Ellipse)args).Tag.ToString();
            Brush color = ((Ellipse)args).Fill;
            string[] selectEllipse = tag.Split(':');

            if (this._playerColors != null && this.CurrentSelectedColor != Brushes.Transparent)
            {
                int row = int.Parse(new string(selectEllipse[1].Where(char.IsDigit).ToArray()), CultureInfo.CurrentCulture);
                int col = int.Parse(new string(selectEllipse[2].Where(char.IsDigit).ToArray()), CultureInfo.CurrentCulture);

                if (_playerColors.Any(r => r.Row == row && r.Color == this.CurrentSelectedColor) == false)
                {
                    PlayerColor playerColor = new PlayerColor()
                    {
                        Color = this.CurrentSelectedColor,
                        Row = row,
                        Col = col,
                        PlayerWin = false,
                        LineChecked = false,
                        Modus = PlayerModus.PlayerColor
                    };

                    if (this._playerColors.Any(pc => pc.Row == row && pc.Col == col && pc.Color == color) == false)
                    {
                        this._playerColors.Add(playerColor);
                    }

                    if (tag.Replace(":", string.Empty) == "PlayerR1C1")
                    {
                        this.PlayerR1C1.FillColor = this.CurrentSelectedColor;
                    }
                    else if (tag.Replace(":", string.Empty) == "PlayerR1C2")
                    {
                        this.PlayerR1C2.FillColor = this.CurrentSelectedColor;
                    }
                    else if (tag.Replace(":", string.Empty) == "PlayerR1C3")
                    {
                        this.PlayerR1C3.FillColor = this.CurrentSelectedColor;
                    }
                    else if (tag.Replace(":", string.Empty) == "PlayerR1C4")
                    {
                        this.PlayerR1C4.FillColor = this.CurrentSelectedColor;
                    }

                    this.CurrentSelectedColor = Brushes.Transparent;
                }
            }
        }

        private void OnCheck(object args)
        {
            if (this._playerColors != null && this._playerColors.Count == 4)
            {
            }
        }

        private void EnablelPlayerColors(int row)
        {
            if (row == 1)
            {
                this.PlayerR1C1.FillColor = Brushes.Transparent;
                this.PlayerR1C1.IsEnabled = true;
                this.PlayerR1C2.FillColor = Brushes.Transparent;
                this.PlayerR1C2.IsEnabled = true;
                this.PlayerR1C3.FillColor = Brushes.Transparent;
                this.PlayerR1C3.IsEnabled = true;
                this.PlayerR1C4.FillColor = Brushes.Transparent;
                this.PlayerR1C4.IsEnabled = true;
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

            this.PlayerR5C1.FillColor = Brushes.LightGray;
            this.PlayerR5C1.IsEnabled = false;
            this.PlayerR5C2.FillColor = Brushes.LightGray;
            this.PlayerR5C2.IsEnabled = false;
            this.PlayerR5C3.FillColor = Brushes.LightGray;
            this.PlayerR5C3.IsEnabled = false;
            this.PlayerR5C4.FillColor = Brushes.LightGray;
            this.PlayerR5C4.IsEnabled = false;

            this.PlayerR6C1.FillColor = Brushes.LightGray;
            this.PlayerR6C1.IsEnabled = false;
            this.PlayerR6C2.FillColor = Brushes.LightGray;
            this.PlayerR6C2.IsEnabled = false;
            this.PlayerR6C3.FillColor = Brushes.LightGray;
            this.PlayerR6C3.IsEnabled = false;
            this.PlayerR6C4.FillColor = Brushes.LightGray;
            this.PlayerR6C4.IsEnabled = false;
        }

        private enum PlayerModus
        {
            RandomColor,
            PlayerColor,
        }

        private sealed class PlayerColor
        {
            public Brush Color { get; set; }

            public int Row { get; set; }

            public int Col { get; set; }

            public bool LineChecked { get; set; }

            public bool PlayerWin { get; set; }

            public PlayerModus Modus { get; set; }
        }
    }
}

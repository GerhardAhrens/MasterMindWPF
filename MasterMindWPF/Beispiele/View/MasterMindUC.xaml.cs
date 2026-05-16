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
            this.DisableAllResults();
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
                        Color = randomColors[i],
                        Row = -1,
                        Col = i+1,
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

                        this.gridPlayer.Children.OfType<EllipseButton>().Where(f => f.Name == tag.Replace(":", string.Empty)).ToList().ForEach(plItem =>
                        {
                            plItem.FillColor = this.CurrentSelectedColor;
                        });
                    }

                    this.CurrentSelectedColor = Brushes.Transparent;
                }
            }
        }

        private void OnCheck(object args)
        {
            if (this._playerColors != null && this._playerColors.Count >= 4)
            {
                List<PlayerColor> randomColors = this._playerColors.Where(p => p.Modus == PlayerModus.RandomColor).ToList();
                if (randomColors != null)
                {
                    List<PlayerColor> playerColors = this._playerColors.Where(p => p.Modus == PlayerModus.PlayerColor).ToList();
                    foreach (PlayerColor playerColor in playerColors)
                    {
                        if (randomColors.Any(rc => rc.Color.ToString(CultureInfo.CurrentCulture) == playerColor.Color.ToString(CultureInfo.CurrentCulture) && rc.Col == playerColor.Col))
                        {
                            /* Spieler hat die richtige Farbe an der richtigen Position gewählt */
                            this.SetResults(playerColor.Row, playerColor.Col, ResultModus.PosAndColor);
                        }
                        else if (randomColors.Any(rc => rc.Color.ToString(CultureInfo.CurrentCulture) == playerColor.Color.ToString(CultureInfo.CurrentCulture)))
                        {
                            /* Spieler hat die richtige Farbe gewählt, aber an der falschen Position */
                            this.SetResults(playerColor.Row, playerColor.Col, ResultModus.ColorOnly);
                        }
                        else
                        {
                            this.SetResults(playerColor.Row, playerColor.Col, ResultModus.None);
                        }
                    }
                }
            }
        }

        private void EnablelPlayerColors(int currentRow)
        {
            this.DisableAllPlayerColors();

            this.gridPlayer.Children.OfType<EllipseButton>().ToList().ForEach(plItem =>
            {
                string name = plItem.TagContent.ToString();
                string[] selectEllipse = name.Split(':');
                int row = int.Parse(new string(selectEllipse[1].Where(char.IsDigit).ToArray()), CultureInfo.CurrentCulture);

                if (row == currentRow)
                {
                    {
                        plItem.FillColor = Brushes.Transparent;
                        plItem.IsEnabled = true;
                    }
                }
            });
        }

        private void DisableAllPlayerColors()
        {

            this.gridPlayer.Children.OfType<EllipseButton>().ToList().ForEach(plItem =>
            {
                plItem.FillColor = Brushes.LightGray;
                plItem.IsEnabled = false;
            });

        }

        private void DisableAllResults()
        {

            this.gridResults.Children.OfType<Ellipse>().ToList().ForEach(plItem =>
            {
                plItem.Fill = Brushes.LightGray;
            });

        }

        private void SetResults(int row, int col, ResultModus result)
        {

            this.gridResults.Children.OfType<Ellipse>().ToList().ForEach(plItem =>
            {
                string name = plItem.Name.Replace("Result",string.Empty).Substring(0,3);
                string[] selectEllipse = name.Split(':');
                int currentRow = int.Parse(new string(selectEllipse[0].Where(char.IsDigit).ToArray()), CultureInfo.CurrentCulture);

                if (row == currentRow)
                {
                    if (result == ResultModus.ColorOnly)
                    {
                        plItem.Fill = Brushes.White;
                    }
                    else if (result == ResultModus.PosAndColor)
                    {
                        plItem.Fill = Brushes.Black;
                    }
                }
            });

        }

        private enum ResultModus
        {
            None,
            ColorOnly,
            PosAndColor,
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

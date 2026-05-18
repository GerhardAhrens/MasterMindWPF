namespace MasterMindWPF.Beispiele
{
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Text.Json;
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

            this.JSONOption = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
                IndentSize = 2,
                WriteIndented = true
            };

            this.NewPlayCommand = new CommandBase(args => this.OnNewPlay(args), () => true);
            this.ShowColorsCommand = new CommandBase(args => this.OnShowColors(args), () => true);
            this.SelectColorCommand = new CommandBase(args => this.OnSelectColor(args), () => true);
            this.PlayerColorCommand = new CommandBase(args => this.OnPlayerColor(args), () => true);
            this.CheckCommand = new CommandBase(args => this.OnCheck(args), () => true);
            this.ShowHistoryCommand = new CommandBase(args => this.OnShowHistory(args), () => true);
            this.SpielTitel = LocalizationValue.Get("SpielTitel");
            this.DataContext = this;
        }

        public CommandBase NewPlayCommand { get; private set; }
        public CommandBase ShowColorsCommand { get; private set; }
        public CommandBase SelectColorCommand { get; private set; }
        public CommandBase PlayerColorCommand { get; private set; }
        public CommandBase CheckCommand { get; private set; }
        public CommandBase ShowHistoryCommand { get; private set; }

        public Brush CurrentSelectedColor { get; set; }

        public string SpielTitel
        {
            get => base.GetValue<string>();
            set => base.SetValue(value);
        }

        private MessageBase Message { get; } = new MessageBase();

        private JsonSerializerOptions JSONOption { get; set; }

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
                        Modus = PlayerModus.RandomColor,
                        TryPlaying = 0,
                        PlayTime = DateTime.Now
                    };

                    this._playerColors.Add(rndColor);
                }

                this.EnablelPlayerColors(1);
                this.DisableAllResults(1);
            }
        }

        private void OnShowColors(object args)
        {
            MessageBoxResult msgYN = this.Message.EndTheGameYN();
            if (msgYN == MessageBoxResult.Yes)
            {
                if (this.RandomStackPanel.Visibility != Visibility.Visible)
                {
                    this.RandomStackPanel.Visibility = Visibility.Visible;
                }
                else
                {
                    this.RandomStackPanel.Visibility = Visibility.Hidden;
                }
            }
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
                        Modus = PlayerModus.PlayerColor,
                        TryPlaying = 0,
                        PlayTime = DateTime.Now
                    };

                    if (this._playerColors.Any(pc => pc.Row == row && pc.Col == col && pc.Color == color) == false)
                    {
                        PlayerColor playerColorCurrent  = this._playerColors.FirstOrDefault(pc => pc.Row == row && pc.Col == col && pc.Color != Brushes.Transparent);
                        this._playerColors.Add(playerColor);

                        this.gridPlayer.Children.OfType<EllipseButton>().Where(f => f.Name == tag.Replace(":", string.Empty)).ToList().ForEach(plItem =>
                        {
                            plItem.FillColor = this.CurrentSelectedColor;
                        });
                    }
                    else
                    {
                        PlayerColor playerColorCurrent = this._playerColors.FirstOrDefault(pc => pc.Row == row && pc.Col == col && pc.Color != Brushes.Transparent);
                        if (playerColorCurrent != null)
                        {
                            this._playerColors.Remove(playerColorCurrent);
                        }

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
                    int lastRow = this._playerColors.LastOrDefault(l => l.Modus == PlayerModus.PlayerColor).Row;
                    List<PlayerColor> playerColors = this._playerColors.Where(p => p.Row == lastRow && p.Modus == PlayerModus.PlayerColor).ToList();
                    if (playerColors.Count == 4)
                    {
                        foreach (PlayerColor playerColor in playerColors)
                        {
                            int index = playerColors.IndexOf(playerColor);

                            if (randomColors.Any(rc => rc.Color.ToString(CultureInfo.CurrentCulture) == playerColor.Color.ToString(CultureInfo.CurrentCulture) && rc.Col == playerColor.Col))
                            {
                                int versuche = playerColors.DistinctBy(d => d.Row).Count(c => c.Modus == PlayerModus.PlayerColor);

                                /* Spieler hat die richtige Farbe an der richtigen Position gewählt */
                                playerColor.LineChecked = true;
                                playerColor.PlayerWin = true;
                                playerColor.TryPlaying = versuche;
                                playerColor.PlayTime = DateTime.Now;
                                this.SetResults(playerColor.Row, playerColor.Col, ResultModus.PosAndColor);
                            }
                            else if (randomColors.Any(rc => rc.Color.ToString(CultureInfo.CurrentCulture) == playerColor.Color.ToString(CultureInfo.CurrentCulture)))
                            {
                                /* Spieler hat die richtige Farbe gewählt, aber an der falschen Position */
                                playerColor.LineChecked = true;
                                this.SetResults(playerColor.Row, playerColor.Col, ResultModus.ColorOnly);
                            }
                            else
                            {
                                this.SetResults(playerColor.Row, playerColor.Col, ResultModus.None);
                            }
                        }

                        if (playerColors.Count(c => c.PlayerWin == true && c.Row < 10) == 4)
                        {
                            int versuche = playerColors.Count(c => c.Modus == PlayerModus.PlayerColor);
                            this.gridPlayer.Children.OfType<EllipseButton>().ToList().ForEach(plItem =>
                            {
                                plItem.IsEnabled = false;
                            });

                            this.CreateHistoryEntry(true, versuche);
                            this.Message.PlayerWinGame(versuche);
                        }
                        else if (playerColors.Any(c => c.PlayerWin == false && c.Row > 9) == true)
                        {
                            int versuche = playerColors.DistinctBy(d => d.Row).Count(c => c.Modus == PlayerModus.PlayerColor);
                            this.gridPlayer.Children.OfType<EllipseButton>().ToList().ForEach(plItem =>
                            {
                                plItem.IsEnabled = false;
                            });

                            this.Message.PlayerNotWinGame(versuche);
                        }
                    }
                }
            }
        }

        private void EnablelPlayerColors(int currentRow)
        {
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

        private void DisableAllPlayerColors(int currentRow = 1)
        {

            this.gridPlayer.Children.OfType<EllipseButton>().ToList().ForEach(plItem =>
            {
                int count = this._playerColors.Count(l => l.Row == currentRow && l.Modus == PlayerModus.PlayerColor);
                if (count == 0)
                {
                    plItem.FillColor = Brushes.LightGray;
                    plItem.IsEnabled = false;
                }
                else
                {
                    int lastRow = this._playerColors.LastOrDefault(l => l.Row == currentRow && l.Modus == PlayerModus.PlayerColor).Row;

                    if (lastRow >= currentRow)
                    {
                        {
                            plItem.FillColor = Brushes.LightGray;
                            plItem.IsEnabled = false;
                        }
                    }
                }
            });

        }

        private void DisableAllResults(int currentRow = 1)
        {

            this.gridResults.Children.OfType<Ellipse>().ToList().ForEach(plItem =>
            {
                string xrow = plItem.Name.Replace("Result", string.Empty).Substring(0, 3);
                string xcol = plItem.Name.Replace("Result", string.Empty).Substring(2);
                int row = int.Parse(new string(xrow.Where(char.IsDigit).ToArray()), CultureInfo.CurrentCulture);
                int col = int.Parse(new string(xcol.Where(char.IsDigit).ToArray()), CultureInfo.CurrentCulture);

                if (row >= currentRow && col > 0)
                {
                    plItem.Fill = Brushes.LightGray;
                }
            });

        }

        private void SetResults(int row, int col, ResultModus result)
        {

            this.gridResults.Children.OfType<Ellipse>().ToList().ForEach(plItem =>
            {
                string xrow = plItem.Name.Replace("Result",string.Empty).Substring(0,3);
                string xcol = plItem.Name.Replace("Result", string.Empty).Substring(2);
                int currentRow = int.Parse(new string(xrow.Where(char.IsDigit).ToArray()), CultureInfo.CurrentCulture);
                int currentCol = int.Parse(new string(xcol.Where(char.IsDigit).ToArray()), CultureInfo.CurrentCulture);

                if (row == currentRow && col == currentCol)
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

            this.EnablelPlayerColors(row + 1);
        }

        private void OnShowHistory(object args)
        {
            if (_playerColors != null && _playerColors.Count >= 8)
            {
            }
        }

        private void CreateHistoryEntry(bool isGameWin, int tryPlaying)
        {
            List<PlayerHistory> playerHistorySource = new();

            if (_playerColors != null && _playerColors.Count >= 8)
            {
                string rootPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
                string historyFile = $"{rootPath}\\{AppDomain.CurrentDomain.FriendlyName}\\Settings";
                if (Directory.Exists(historyFile) == false)
                {
                    Directory.CreateDirectory(historyFile);
                }

                historyFile = $"{historyFile}\\PlayerHistory.json";

                if (File.Exists(historyFile) == true)
                {
                    string jsonString = File.ReadAllText(historyFile);

                    playerHistorySource = JsonSerializer.Deserialize<List<PlayerHistory>>(jsonString, this.JSONOption);
                }

                PlayerColor firstRow = this._playerColors.LastOrDefault(l => l.Modus == PlayerModus.RandomColor);
                PlayerColor lastRow = this._playerColors.LastOrDefault(l => l.Modus == PlayerModus.PlayerColor);
                if (firstRow != null && lastRow != null)
                {
                    TimeSpan timeDiff = lastRow.PlayTime - firstRow.PlayTime;
                    double totalMinuten = timeDiff.TotalMinutes;
                    int minuten = (int)Math.Floor(timeDiff.TotalMinutes);
                    int sekunden = timeDiff.Seconds; // verbleibende Sekunden (0 bis 59)

                    PlayerHistory historyEntry = new();
                    historyEntry.PlayerName = Environment.UserName;
                    historyEntry.TryPlaying = tryPlaying;
                    historyEntry.TimeTequired = $"Spielzeit: {minuten} Minuten und {sekunden} Sekunden";
                    historyEntry.GameWin = isGameWin;
                    playerHistorySource.Add(historyEntry);

                    if (playerHistorySource.Count > 0)
                    {
                        string jsonString = JsonSerializer.Serialize(playerHistorySource, this.JSONOption);
                        File.WriteAllText(historyFile, jsonString);
                    }
                }

            }
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

        [DebuggerDisplay("Row={this.Row};Co=l{this.Col};Color={this.Color}")]
        private sealed class PlayerColor
        {
            public Brush Color { get; set; }

            public int Row { get; set; }

            public int Col { get; set; }

            public bool LineChecked { get; set; }

            public bool PlayerWin { get; set; }

            public PlayerModus Modus { get; set; }

            public int TryPlaying { get; set; }

            public DateTime PlayTime { get; set; }
        }

        [DebuggerDisplay("Versuche={this.TryPlaying};Spielzeit=l{this.TimeTequired};Gewonnen={this.GameWin}")]
        private sealed class PlayerHistory
        {
            public string PlayerName { get; set; }
            public int TryPlaying { get; set; }
            public string TimeTequired { get; set; }

            public bool GameWin { get; set; }
        }
    }
}

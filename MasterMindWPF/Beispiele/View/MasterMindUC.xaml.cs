namespace MasterMindWPF.Beispiele
{
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Shapes;

    /// <summary>
    /// Interaktionslogik für MasterMindUC.xaml
    /// </summary>
    public partial class MasterMindUC : UserControlBase
    {
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

            this.DataContext = this;
        }

        public CommandBase SelectColorCommand { get; private set; }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.SelectR1C1.FillColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString(_availableColors[0]));
            this.SelectR1C2.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString(_availableColors[1]));
            this.SelectR1C3.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString(_availableColors[2]));
            this.SelectR1C4.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString(_availableColors[3]));
            this.SelectR1C5.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString(_availableColors[4]));
            this.SelectR1C6.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString(_availableColors[5]));
        }

        private void OnSelectColor(object args)
        {
            string tag = ((Ellipse)args).Tag.ToString();
            Brush color = ((Ellipse)args).Fill;
        }
    }
}

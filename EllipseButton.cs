namespace MasterMindWPF.Beispiele
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Shapes;

    public class EllipseButton : Control
    {
        static EllipseButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(EllipseButton),
                new FrameworkPropertyMetadata(typeof(EllipseButton)));
        }

        #region Text

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(
                nameof(Text),
                typeof(string),
                typeof(EllipseButton),
                new PropertyMetadata(string.Empty));

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        #endregion

        #region FillColor

        public static readonly DependencyProperty FillColorProperty =
            Shape.FillProperty.AddOwner(typeof(EllipseButton));

        public Brush FillColor
        {
            get => (Brush)GetValue(FillColorProperty);
            set => SetValue(FillColorProperty, value);
        }

        #endregion

        #region BorderColor

        public static readonly DependencyProperty BorderColorProperty =
            Shape.StrokeProperty.AddOwner(typeof(EllipseButton));

        public Brush BorderColor
        {
            get => (Brush)GetValue(BorderColorProperty);
            set => SetValue(BorderColorProperty, value);
        }

        #endregion

        #region BorderWidth

        public static readonly DependencyProperty BorderWidthProperty =
            Shape.StrokeThicknessProperty.AddOwner(typeof(EllipseButton));

        public double BorderWidth
        {
            get => (double)GetValue(BorderWidthProperty);
            set => SetValue(BorderWidthProperty, value);
        }

        #endregion

        #region Command

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register(
                nameof(Command),
                typeof(ICommand),
                typeof(EllipseButton));

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        #endregion

        #region CommandParameter

        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register(
                nameof(CommandParameter),
                typeof(object),
                typeof(EllipseButton));

        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        #endregion

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (GetTemplateChild("PART_Ellipse") is Ellipse ellipse)
            {
                ellipse.MouseLeftButtonUp += (_, _) =>
                {
                    object parameter = CommandParameter ?? ellipse;

                    if (Command?.CanExecute(parameter) == true)
                    {
                        Command.Execute(parameter);
                    }
                };
            }
        }
    }
}
namespace MasterMindWPF.Beispiele
{
    using System.Drawing;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Shapes;

    public class EllipseButton : Control
    {
        static EllipseButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EllipseButton), new FrameworkPropertyMetadata(typeof(EllipseButton)));
        }

        #region FillColor

        public static readonly DependencyProperty FillColorProperty =
            DependencyProperty.Register(
                nameof(FillColor),
                typeof(System.Windows.Media.Brush),
                typeof(EllipseButton),
                new PropertyMetadata(System.Windows.Media.Brushes.DodgerBlue));

        public System.Windows.Media.Brush FillColor
        {
            get => (System.Windows.Media.Brush)GetValue(FillColorProperty);
            set => SetValue(FillColorProperty, value);
        }

        #endregion

        #region BorderColor

        public static readonly DependencyProperty BorderColorProperty =
            DependencyProperty.Register(
                nameof(BorderColor),
                typeof(System.Windows.Media.Brush),
                typeof(EllipseButton),
                new PropertyMetadata(System.Windows.Media.Brushes.Gray));

        public System.Windows.Media.Brush BorderColor
        {
            get => (System.Windows.Media.Brush)GetValue(BorderColorProperty);
            set => SetValue(BorderColorProperty, value);
        }

        #endregion

        #region BorderWidth

        public static readonly DependencyProperty BorderWidthProperty =
            DependencyProperty.Register(
                nameof(BorderWidth),
                typeof(double),
                typeof(EllipseButton),
                new PropertyMetadata(1.0));

        public double BorderWidth
        {
            get => (double)GetValue(BorderWidthProperty);
            set => SetValue(BorderWidthProperty, value);
        }

        #endregion

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

        #region TagContent

        public static readonly DependencyProperty TagContentProperty =
            DependencyProperty.Register(
                nameof(TagContent),
                typeof(object),
                typeof(EllipseButton),
                new PropertyMetadata());

        public object TagContent
        {
            get => (object)GetValue(TagContentProperty);
            set => SetValue(TagContentProperty, value);
        }

        #endregion

        #region Command

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register(
                nameof(Command),
                typeof(ICommand),
                typeof(EllipseButton),
                new PropertyMetadata(null));

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
                typeof(EllipseButton),
                new PropertyMetadata(null));

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

namespace MasterMindWPF.Beispiele
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Media;

    public class BrushProvider
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

        private readonly Random _random = new();

        /// <summary>
        /// Gibt 4 zufällige Brushes zurück.
        /// </summary>
        public List<Brush> GetRandomBrushes()
        {
            return _availableColors
                .OrderBy(x => _random.Next())
                .Take(4)
                .Select(colorName =>
                {
                    var property = typeof(Colors).GetProperty(colorName);

                    if (property != null)
                    {
                        var color = (Color)property.GetValue(null)!;
                        return (Brush)new SolidColorBrush(color);
                    }

                    return Brushes.Transparent;
                }).ToList();
        }
    }
}

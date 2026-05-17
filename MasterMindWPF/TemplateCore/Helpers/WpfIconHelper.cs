//-----------------------------------------------------------------------
// <copyright file="MainWindow.cs" company="Lifeprojects.de">
//     Class: MainWindow
//     Copyright © Lifeprojects.de 2026
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>05.03.2026 18:21:36</date>
//
// <summary>
// WPF Template mit Minimalfunktionen
// </summary>
//-----------------------------------------------------------------------

namespace System.Windows
{
    using System.IO;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    public static class WpfIconHelper
    {
        public static ImageSource CreateIcon(DrawingImage drawingImage, int size = 32, double dpi = 96)
        {
            if (size.In(32, 48, 64) == false)
            {
                throw new ArgumentOutOfRangeException(nameof(size), "Der Wert für die Icon Größe muß 32 oder 64 Pixel sein.");
            }

            if (dpi == 96)
            {
                size = 64;
            }
            else if (dpi == 144)
            {
                size = 48;
            }
            else if (dpi == 192)
            {
                size = 64;
            }

            DrawingVisual visual = new DrawingVisual();
            using (var dc = visual.RenderOpen())
            {
                dc.DrawImage(drawingImage, new Rect(0, 0, size, size));
            }

            RenderTargetBitmap bitmap = new RenderTargetBitmap(size, size, dpi, dpi, PixelFormats.Pbgra32);
            bitmap.Render(visual);
            bitmap.Freeze(); // Performance + Thread-Safety

            return bitmap;
        }

        public static void ApplyIcon(Window window, DrawingImage drawingImage, int size = 32)
        {
            window.Icon = CreateIcon(drawingImage, size);
        }

        /// <summary>
        /// Speichnern eines DrawingImage Object (aus Resources) als PNG Bilddatei
        /// </summary>
        /// <param name="resourceKey">ResourceKey</param>
        /// <param name="filePath">Pfad und Dateiname</param>
        /// <param name="width">Breite in Pixel</param>
        /// <param name="height">Höhe in Pixel</param>
        /// <exception cref="ArgumentException"></exception>
        /// <remarks>
        /// WpfIconHelper.SaveImageAsPng("MasterMindIcon","c:\\temp\\test.ico");
        /// </remarks>
        public static void SaveImageAsPng(string resourceKey, string filePath, int width = 256, int height = 265)
        {
            DrawingImage drawing = (DrawingImage)Application.Current.TryFindResource(resourceKey);
            if (drawing != null)
            {
                SaveImageAsPng(drawing, filePath, width, height);
            }
            else
            {
                throw new ArgumentException($"Die Ressource mit dem Schlüssel '{resourceKey}' wurde nicht gefunden oder ist kein DrawingImage.", nameof(resourceKey));
            }

        }

        /// <summary>
        /// Speichnern eines DrawingImage Object als PNG Bilddatei
        /// </summary>
        /// <param name="drawingImage">DrawingImage Vektorgrafik</param>
        /// <param name="filePath">Pfad und Dateiname</param>
        /// <param name="width">Breite in Pixel</param>
        /// <param name="height">Höhe in Pixel</param>
        /// <exception cref="ArgumentException"></exception>
        /// <remarks>
        /// WpfIconHelper.SaveImageAsPng(drawingImageObjekt,"c:\\temp\\test.ico");
        /// </remarks>
        public static void SaveImageAsPng(DrawingImage drawingImage, string filePath, int width = 256, int height = 265)
        {
            // DrawingVisual erzeugen
            DrawingVisual visual = new DrawingVisual();

            using (DrawingContext dc = visual.RenderOpen())
            {
                dc.DrawImage(drawingImage, new Rect(0, 0, width, height));
            }

            // Bitmap rendern
            RenderTargetBitmap bitmap = new RenderTargetBitmap(
                width,
                height,
                96, // DPI X
                96, // DPI Y
                PixelFormats.Pbgra32);

            bitmap.Render(visual);

            // PNG speichern
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmap));

            using FileStream stream = new FileStream(filePath, FileMode.Create);
            encoder.Save(stream);
        }

        /// <summary>
        /// Speichnern eines DrawingImage (aus Resources) Object als Icon (ico)
        /// </summary>
        /// <param name="resourceKey">ResourceKey</param>
        /// <param name="filePath">Pfad und Dateiname</param>
        /// <param name="sizes">Ohne Angabe werden folgende Größen erstellt: 16, 24, 32, 48, 64, 128, 256</param>
        /// <exception cref="ArgumentException"></exception>
        /// <remarks>
        /// WpfIconHelper.SaveDrawingImageAsIcon("MasterMindIcon","c:\\temp\\test.ico");
        /// </remarks>
        public static void SaveDrawingImageAsIcon(string resourceKey, string filePath, params int[] sizes)
        {
            DrawingImage drawing = (DrawingImage)Application.Current.TryFindResource(resourceKey);
            if (drawing != null)
            {
                SaveDrawingImageAsIcon(drawing, filePath, sizes);
            }
            else
            {
                throw new ArgumentException($"Die Ressource mit dem Schlüssel '{resourceKey}' wurde nicht gefunden oder ist kein DrawingImage.", nameof(resourceKey));
            }
        }

        /// <summary>
        /// Speichnern eines DrawingImage Object als Icon (ico)
        /// </summary>
        /// <param name="resourceKey">DrawingImage Vektorgrafik</param>
        /// <param name="filePath">Pfad und Dateiname</param>
        /// <param name="sizes">Ohne Angabe werden folgende Größen erstellt: 16, 24, 32, 48, 64, 128, 256</param>
        /// <exception cref="ArgumentException"></exception>
        /// <remarks>
        /// WpfIconHelper.SaveDrawingImageAsIcon(drawingImageObjekt,"c:\\temp\\test.ico");
        /// </remarks>
        public static void SaveDrawingImageAsIcon(DrawingImage drawingImage, string filePath, params int[] sizes)
        {
            if (sizes == null || sizes.Length == 0)
            {
                sizes = new[] { 16, 24, 32, 48, 64, 128, 256 };
            }

            // PNG-Daten aller Größen vorbereiten
            List<IconImage> images = new();

            foreach (int size in sizes)
            {
                byte[] pngData = RenderDrawingToPng(drawingImage, size);

                images.Add(new IconImage
                {
                    Size = size,
                    Data = pngData
                });
            }

            using FileStream fs = new FileStream(filePath, FileMode.Create);
            using BinaryWriter writer = new BinaryWriter(fs);

            //---------------------------------
            // ICONDIR
            //---------------------------------

            writer.Write((ushort)0);                  // Reserved
            writer.Write((ushort)1);                  // Type = Icon
            writer.Write((ushort)images.Count);       // Image count

            //---------------------------------
            // ICONDIRENTRY TABLE
            //---------------------------------

            int imageOffset = 6 + (16 * images.Count);

            foreach (IconImage image in images)
            {
                int size = image.Size;

                writer.Write((byte)(size >= 256 ? 0 : size)); // Width
                writer.Write((byte)(size >= 256 ? 0 : size)); // Height

                writer.Write((byte)0); // Color count
                writer.Write((byte)0); // Reserved

                writer.Write((ushort)1);   // Planes
                writer.Write((ushort)32);  // BitCount

                writer.Write((uint)image.Data.Length); // Bytes in resource
                writer.Write((uint)imageOffset);       // Offset

                imageOffset += image.Data.Length;
            }

            //---------------------------------
            // IMAGE DATA
            //---------------------------------

            foreach (IconImage image in images)
            {
                writer.Write(image.Data);
            }
        }

        private static byte[] RenderDrawingToPng(
            DrawingImage drawingImage,
            int size)
        {
            DrawingVisual visual = new DrawingVisual();

            using (DrawingContext dc = visual.RenderOpen())
            {
                dc.DrawImage(drawingImage, new Rect(0, 0, size, size));
            }

            RenderTargetBitmap bitmap = new RenderTargetBitmap(
                size,
                size,
                96,
                96,
                PixelFormats.Pbgra32);

            bitmap.Render(visual);

            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmap));

            using MemoryStream ms = new MemoryStream();

            encoder.Save(ms);

            return ms.ToArray();
        }

        private sealed class IconImage
        {
            public int Size { get; set; }

            public byte[] Data { get; set; }
        }
    }
}
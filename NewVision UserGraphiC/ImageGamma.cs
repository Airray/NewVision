using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace NewVision_UserGraphiC
{
    class ImageGamma
    {
        public static Image _currentBitmap = null;

        public ImageGamma()
        {
        }

        public static void SetGamma2(double Gamma, double Contrast, double S_bright, double red, double green, double blue)
        {
            Bitmap temp = (Bitmap)_currentBitmap;
            Bitmap bmap = (Bitmap)temp.Clone();
            Color c;
            byte[] redGamma = CreateGammaArray(Gamma, Contrast, S_bright, red);
            byte[] greenGamma = CreateGammaArray(Gamma, Contrast, S_bright, green);
            byte[] blueGamma = CreateGammaArray(Gamma, Contrast, S_bright, blue);
            for (int i = 0; i < bmap.Width; i++)
            {
                for (int j = 0; j < bmap.Height; j++)
                {
                    c = bmap.GetPixel(i, j);
                    bmap.SetPixel(i, j, Color.FromArgb(redGamma[c.R], greenGamma[c.G], blueGamma[c.B]));
                }
            }
            _currentBitmap = (Bitmap)bmap.Clone();
        }

        private static byte[] CreateGammaArray(double Gamma, double Contrast, double S_brights, double color)
        {
            byte[] gammaArray = new byte[256];
            for (int i = 1; i < 256; ++i)
            {
                gammaArray[i] = (byte)Math.Min(255, Math.Max(0, (int)(((Contrast/257) * Math.Pow((i + 1) / 255.0, Gamma * 0.1))) + (i * (S_brights - 50) * 2)/255) + (i * color));
            }
            return gammaArray;
        }
    }
}

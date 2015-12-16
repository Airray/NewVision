using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Screen_team
{
    class CalculationRGB
    {
        private Bitmap bimage = null;
        public CalculationRGB(Bitmap b)
        {
            this.bimage = b;
        }

        public int[] getRGBData()
        {
            int Height = bimage.Height;
            int Width = bimage.Width;
            int[] rgbData = new int[3];
            int x, y;

            for (y = 0; y < Height; y++)
            {
                for (x = 0; x < Width; x++)
                {
                    Color color = bimage.GetPixel(x, y);
                    rgbData[0] += color.R;
                    rgbData[1] += color.G;
                    rgbData[2] += color.B;
                }
            }
            rgbData[0] = rgbData[0] / (Height * Width);
            rgbData[1] = rgbData[1] / (Height * Width);
            rgbData[2] = rgbData[2] / (Height * Width);
            return rgbData;
        }

        public double[][] get_ab_data(double[] s)
        {
            int Height = bimage.Height;
            int Width = bimage.Width;
            int[] rgbData = new int[3];
            double[] labValue = new double[3];
            int x, y, z;
            double[][] lll = new double[2][];
            lll[0] = new double[Height * Width];
            lll[1] = new double[Height * Width];
            int count = 0;
            for (y = 0; y < Height; y++)
            {
                for (x = 0; x < Width; x++)
                {
                    Color color = bimage.GetPixel(x, y);
                    rgbData[0] = color.R;
                    rgbData[1] = color.G;
                    rgbData[2] = color.B;
                    RGB_to_XYZ rgbtoxyz = new RGB_to_XYZ();
                    double[] xyzValue = new double[3];
                    xyzValue = rgbtoxyz.RGBtoXYZ(rgbData[0], rgbData[1], rgbData[2]);
                    XYZ_to_LAB xyztolab = new XYZ_to_LAB();
                    labValue = xyztolab.XYZtoLAB(s, xyzValue);
                   
                    for (z = 1; z < 3; z++)
                    {
                        lll[z - 1][count] = labValue[z];
                    }
                    count++;
                }
            }
            return lll;
        }


        public int[] get_big_RGBData()
        {
            int Height = bimage.Height;
            int Width = bimage.Width;
            int[] rgbData = new int[3];
            int x, y;
            int rmax = 0;
            int gmax=0;
            int bmax=0;
            for (y = 0; y < Height; y++)
            {
                for (x = 0; x < Width; x++)
                {
                    Color color = bimage.GetPixel(x, y);
                    if (color.R > rmax)
                    {
                        rmax = color.R;
                    }
                    if (color.G > gmax)
                    {
                        gmax = color.G;
                    }
                    if (color.B > bmax)
                    {
                        bmax = color.B;
                    }
                }
            }
            rgbData[0] = rmax;
            rgbData[1] = gmax;
            rgbData[2] = bmax;
            return rgbData;
        }

    }

}









using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewVision_UserGraphiC
{
    class Look_up_table
    {
        public static int[] D50_table(int bright)
        {
            int[] GrayRGB = new int[3];
            if (bright > 50)
            {
                if (bright > 75)
                {
                    if (bright > 80)
                    {
                        GrayRGB[0] = 215; GrayRGB[1] = 220; GrayRGB[2] = 218;
                        return GrayRGB;
                    }
                    else
                    {
                        GrayRGB[0] = 190; GrayRGB[1] = 194; GrayRGB[2] = 192;
                        return GrayRGB;
                    }
                }
                else
                {
                    GrayRGB[0] = 148; GrayRGB[1] = 152; GrayRGB[2] = 151;
                    return GrayRGB;
                }
            }
            else
            {
                if (bright > 25)
                {
                    if (bright > 30)
                    {
                        GrayRGB[0] = 78; GrayRGB[1] = 83; GrayRGB[2] = 81;
                        return GrayRGB;
                    }
                    else
                    {
                        GrayRGB[0] = 52; GrayRGB[1] = 57; GrayRGB[2] = 55;
                        return GrayRGB;
                    }
                }
                else
                {
                    GrayRGB[0] = 20; GrayRGB[1] = 24; GrayRGB[2] = 21;
                    return GrayRGB;
                }
            }
        }

        public static int[] A_table(int bright)
        {
            int[] GrayRGB = new int[3];
            if (bright > 50)
            {
                if (bright > 75)
                {
                    if (bright > 90)
                    {
                        GrayRGB[0] = 254; GrayRGB[1] = 221; GrayRGB[2] = 170;
                        return GrayRGB;
                    }
                    else
                    {
                        GrayRGB[0] = 253; GrayRGB[1] = 193; GrayRGB[2] = 132;
                        return GrayRGB;
                    }
                }
                else
                {
                    if (bright > 70)
                    {
                        GrayRGB[0] = 218; GrayRGB[1] = 159; GrayRGB[2] = 97;
                        return GrayRGB;
                    }
                    else
                    {
                        GrayRGB[0] = 168; GrayRGB[1] = 118; GrayRGB[2] = 64;
                        return GrayRGB;
                    }
                }
            }
            else
            {
                if (bright > 25)
                {
                  
                     GrayRGB[0] = 111; GrayRGB[1] = 74; GrayRGB[2] = 36;
                     return GrayRGB;
                }
                else
                {
                    GrayRGB[0] = 61; GrayRGB[1] = 38; GrayRGB[2] = 13;
                    return GrayRGB;
                }
            }
        }

        public static int[] CWF_table(int bright)
        {
            int[] GrayRGB = new int[3];
            if (bright > 50)
            {
                if (bright > 75)
                {
                    if (bright > 90)
                    {
                        GrayRGB[0] = 242; GrayRGB[1] = 239; GrayRGB[2] = 225;
                        return GrayRGB;
                    }
                    else
                    {
                        GrayRGB[0] = 218; GrayRGB[1] = 212; GrayRGB[2] = 195;
                        return GrayRGB;
                    }
                }
                else
                {
                    GrayRGB[0] = 173; GrayRGB[1] = 169; GrayRGB[2] = 151;
                    return GrayRGB;
                }
            }
            else
            {
                if (bright > 25)
                {
                    if (bright > 40)
                    {
                        GrayRGB[0] = 98; GrayRGB[1] = 95; GrayRGB[2] = 83;
                        return GrayRGB;
                    }
                    else
                    {
                        GrayRGB[0] = 69; GrayRGB[1] = 65; GrayRGB[2] = 57;
                        return GrayRGB;
                    }
                }
                else
                {
                    GrayRGB[0] = 25; GrayRGB[1] = 17; GrayRGB[2] = 16;
                    return GrayRGB;
                }
            }
        }

        public static int[] D65_table(int bright)
        {
            int[] GrayRGB = new int[3];
            if (bright > 50)
            {
                if (bright > 80)
                {
                    if (bright > 90)
                    {
                        GrayRGB[0] = 241; GrayRGB[1] = 243; GrayRGB[2] = 253;
                        return GrayRGB;
                    }
                    else
                    {
                        GrayRGB[0] = 215; GrayRGB[1] = 221; GrayRGB[2] = 239;
                        return GrayRGB;
                    }
                }
                else
                {
                    GrayRGB[0] = 171; GrayRGB[1] = 178; GrayRGB[2] = 189;
                    return GrayRGB;
                }
            }
            else
            {
                if (bright > 25)
                {
                    if (bright > 40)
                    {
                        GrayRGB[0] = 94; GrayRGB[1] = 102; GrayRGB[2] = 118;
                        return GrayRGB;
                    }
                    else
                    {
                        GrayRGB[0] = 64; GrayRGB[1] = 70; GrayRGB[2] = 86;
                        return GrayRGB;
                    }
                }
                else
                {
                    GrayRGB[0] = 18; GrayRGB[1] = 21; GrayRGB[2] = 39;
                    return GrayRGB;
                }
            }
        }

        public static int[] Gap(int[] a, int[] b)
        {
            int[] result = new int[3];
            if (a[0] > b[0])
            {
                result[0]=-(a[0]-b[0]);
            }
            else
            {
                result[0] =(b[0] - a[0]);
            }
            if (a[1] > b[1])
            {
                result[1] =-(a[1] - b[1]);
            }
            else
            {
                result[1] =(b[1] - a[1]);
            }
            if (a[2] > b[2])
            {
                result[2] = -(a[2] - b[2]);
            }
            else
            {
                result[2] = (b[2] - a[2]);
            }
            return result;
        }

    }
}

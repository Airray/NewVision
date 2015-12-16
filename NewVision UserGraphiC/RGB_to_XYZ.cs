using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Screen_team
{
    class RGB_to_XYZ
    {
        public RGB_to_XYZ()
        {

        }
        public double[] RGBtoXYZ(int red, int green, int blue)
        {
            double rLinear = (double)red / 255.0;
            double gLinear = (double)green / 255.0;
            double bLinear = (double)blue / 255.0;
            double[] rgbData = new double[3];

            double r = (rLinear > 0.04045) ? Math.Pow((rLinear + 0.055) / (
                1 + 0.055), 2.2) : (rLinear / 12.92);
            double g = (gLinear > 0.04045) ? Math.Pow((gLinear + 0.055) / (
                1 + 0.055), 2.2) : (gLinear / 12.92);
            double b = (bLinear > 0.04045) ? Math.Pow((bLinear + 0.055) / (
                1 + 0.055), 2.2) : (bLinear / 12.92);


            rgbData[0] = (r * 0.4124 + g * 0.3576 + b * 0.1805)*100;
            rgbData[1] = (r * 0.2126 + g * 0.7152 + b * 0.0722)*100;
            rgbData[2] = (r * 0.0193 + g * 0.1192 + b * 0.9505)*100;
            
            return rgbData;
        }

    }
}

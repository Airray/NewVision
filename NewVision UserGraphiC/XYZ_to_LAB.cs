using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Screen_team
{
    class XYZ_to_LAB
    {
       //private double[] LAB;
       public XYZ_to_LAB()
       {

       }
       public double[] XYZtoLAB(double[] whitepoint, double[] XYZ)
       {


           double Xr, Yr, Zr;
           double fx, fy, fz;
           double e, k;
           int count = XYZ.Length / 3;
           double[] LAB = new double[3];
           e = 0.008856;
           k = 903.3;

           Xr = XYZ[0] / whitepoint[0];
           Yr = XYZ[1] / whitepoint[1];
           Zr = XYZ[2]/ whitepoint[2];

           if (Xr > e)
           { fx = Math.Pow(Xr,0.33333333333); }
           else
           { fx = ((k * Xr) + 16) / 116; }

           if (Yr > e)
           { fy = Math.Pow(Yr, 0.33333333333); }
           else
           { fy = ((k * Yr) + 16) / 116; }

           if (Zr > e)
           { fz = Math.Pow(Zr, 0.33333333333); }
           else
           { fz = ((k * Zr) + 16) / 116; }

           LAB[0] = ((116 * fy) - 16);
           LAB[1] = (500 * (fx - fy));
           LAB[2] = (200 * (fy - fz));

           return LAB;
       }


    }
    
}
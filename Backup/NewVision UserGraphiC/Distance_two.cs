using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
namespace Screen_team
{
    class Distance_two
    {
       
        private int[] avRGB;
        
        private Bitmap bimage = null;
        public Distance_two(Bitmap b)
        {
            this.bimage = b;
        }
        public int Gray(double[] WhitePointRGB){
            avRGB =new CalculationRGB(bimage).getRGBData();
            double w = avRGB[0] + avRGB[1] + avRGB[2];
            double r = ((avRGB[0]*100/w));
            double g = ((avRGB[1]*100/w));
            double b = ((avRGB[2]*100/w));

            int wpw = (int)(WhitePointRGB[0] + WhitePointRGB[1] + WhitePointRGB[2]);
            int wpR = (int)((WhitePointRGB[0] / wpw)*100);
            int wpG = (int)((WhitePointRGB[1] / wpw)*100);
            int wpB = (int)((WhitePointRGB[2] / wpw)*100);

            int a = (int)Math.Sqrt((r - wpR) * (r - wpR) + (g - wpG) * (g - wpG) + (b - wpB) * (b - wpB));
            return a;

        }
    }
}

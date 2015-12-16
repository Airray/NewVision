using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewVision_UserGraphiC
{
   public  class Situation
    {
        private int Gamma;
        private int Contrast;
        private int Brightness;
        private int Rcolor;
        private int Gcolor;
        private int Bcolor;
        public Situation(int Gamma, int Contrast, int Brightness, int Rcolor, int Gcolor, int Bcolor)
        {
            this.Gamma = Gamma;
            this.Contrast = Contrast;
            this.Brightness = Brightness;
            this.Rcolor = Rcolor;
            this.Gcolor = Gcolor;
            this.Bcolor = Bcolor;
        }
        public int GatGamma()
        {
            return Gamma;
        }
        public int GatContrast()
        {
            return Contrast;
        }
        public int GatBrightness()
        {
            return Brightness; 
        }
        public int GatRcolor()
        {
            return Rcolor;
        }
        public int GatGcolor()
        {
            return Gcolor;
        }
        public int GatBcolor()
        {
            return Bcolor;
        }
    }
}

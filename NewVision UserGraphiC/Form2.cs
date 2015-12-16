using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NewVision_UserGraphiC;
using Screen_team;

namespace NewVision_UserGraphiC
{
    public partial class Form2 : Form
    {
        Form Main_form;
        Form1.cVideo video;

        public static int[] LastValue = new int[6];

        public static int[] rgb = new int[3];

        public Image capImage= null;
        private double[] min = new double[4];
        private int[] avRGB= new int[3];
        private double[] xyz= new double[3];
        private static  double[] lab= new double[3];
        public Form2(Form Main_form1)
        {
            InitializeComponent();
            Main_form = Main_form1;
        
        }
        public Form2()
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            textBox1.Text = " ";
            video = new Form1.cVideo(pictureBox1.Handle, 0, 0);
            DialogResult result1 = MessageBox.Show("將轉換為A光源，確定嗎？", "A光源轉換確認", MessageBoxButtons.YesNo);

            if (result1 == DialogResult.Yes)
            {    
                video.StartWebCam();
                video.GrabImage();
                IDataObject obj1 = Clipboard.GetDataObject();
               
                if (obj1.GetDataPresent(typeof(Bitmap)))
                {
                    capImage = (Image)obj1.GetData(typeof(Bitmap));   
                }
                Bitmap bitmap = (Bitmap)capImage;

                avRGB = new CalculationRGB(bitmap).getRGBData();         
                xyz = new RGB_to_XYZ().RGBtoXYZ(avRGB[0], avRGB[1], avRGB[2]);
                lab = new XYZ_to_LAB().XYZtoLAB(Form1.D65, xyz);

                if (Math.Sqrt((lab[1] * lab[1]) + lab[2] * lab[2]) > 3)    //色域法
                {
                    for (int i = 0; i < 4; i++)
                    {
                        double[] type_a = null, type_b = null;
                        switch (i)
                        {
                            case 0:
                                Form1.s = Form1.D65;
                                type_a = Form1.D65_light_A;
                                type_b = Form1.D65_light_B;
                                break;
                            case 1:
                                Form1.s = Form1.D50;
                                type_a = Form1.D50_light_A;
                                type_b = Form1.D50_light_B;
                                break;
                            case 2:
                                Form1.s = Form1.CWF;
                                type_a = Form1.CWF_light_A;
                                type_b = Form1.CWF_light_B;
                                break;
                            case 3:
                                Form1.s = Form1.A;
                                type_a = Form1.A_light_A;
                                type_b = Form1.A_light_B;
                                break;
                            default: break;
                        }
                        double[][] abValue = new CalculationRGB(bitmap).get_ab_data(Form1.s);
                        double[] a_value = new double[bitmap.Width * bitmap.Height];
                        double[] b_value = new double[bitmap.Width * bitmap.Height];
                        for (int d = 0; d < abValue[0].Length; d++)
                        {
                            a_value[d] = abValue[0][d];
                            b_value[d] = abValue[1][d];

                        }
                        min[i] = Distance.get_line_ab(a_value, b_value, type_a, type_b);

                    }
                    double Min = 9999999;
                    for (int i = 0; i < 4; i++)
                    {
                        if (min[i] < Min)
                            Min = min[i];
                    }
                    if (Min == min[3])
                    {

                        textBox1.Text += "A";
                   
                        Form1.SetGamma(10, 65535, (int)lab[0], 39, 1, -48);
                        LastValue[0] = 10;
                        LastValue[1] = 65535;
                        LastValue[2] = (int)lab[0];
                        LastValue[3] = 39;
                        LastValue[4] = 1;
                        LastValue[5] = -48;
                    }
                    else if (Min == min[1])
                    {
                        textBox1.Text += "D50";
                        int[] D50_RGB = new int[3];
                        D50_RGB = Look_up_table.D50_table((int)lab[0]);
                        int[] A_RGB = new int[3];
                        A_RGB = Look_up_table.A_table((int)lab[0]);
                        Form2.rgb = Look_up_table.Gap(D50_RGB, A_RGB);
                        Form2.rgb[0] = 39 + Form2.rgb[0];
                        Form2.rgb[1] = 1 + Form2.rgb[1];
                        Form2.rgb[2] = -48 + Form2.rgb[2];
                        Form1.SetGamma(10, 65535, (int)lab[0], Form2.rgb[0], Form2.rgb[1], Form2.rgb[2]);
                        LastValue[0] = 10;
                        LastValue[1] = 65535;
                        LastValue[2] = (int)lab[0];
                        LastValue[3] = Form2.rgb[0];
                        LastValue[4] = Form2.rgb[1];
                        LastValue[5] = Form2.rgb[2];
                    }
                    else if (Min == min[0])
                    {
                        textBox1.Text += "D65";
                        int[] D65_RGB = new int[3];
                        D65_RGB = Look_up_table.D50_table((int)lab[0]);
                        int[] A_RGB = new int[3];
                        A_RGB = Look_up_table.A_table((int)lab[0]);
                        Form2.rgb = Look_up_table.Gap(D65_RGB, A_RGB);
                        Form2.rgb[0] = 39 + Form2.rgb[0];
                        Form2.rgb[1] = 1 + Form2.rgb[1];
                        Form2.rgb[2] = -48 + Form2.rgb[2];
                        Form1.SetGamma(10, 65535, (int)lab[0], Form2.rgb[0], Form2.rgb[1], Form2.rgb[2]);
                        LastValue[0] = 10;
                        LastValue[1] = 65535;
                        LastValue[2] = (int)lab[0];
                        LastValue[3] = Form2.rgb[0];
                        LastValue[4] = Form2.rgb[1];
                        LastValue[5] = Form2.rgb[2];
                    }
                    else if (Min == min[2])
                    {
                        textBox1.Text += "CWF";
                        int[] CWF_RGB = new int[3];
                        CWF_RGB = Look_up_table.D50_table((int)lab[0]);
                        int[] A_RGB = new int[3];
                        A_RGB = Look_up_table.A_table((int)lab[0]);
                        Form2.rgb = Look_up_table.Gap(CWF_RGB, A_RGB);
                        Form2.rgb[0] = 39 + Form2.rgb[0];
                        Form2.rgb[1] = 1 + Form2.rgb[1];
                        Form2.rgb[2] = -48 + Form2.rgb[2];
                        Form1.SetGamma(10, 65535, (int)lab[0], Form2.rgb[0], Form2.rgb[1], Form2.rgb[2]);
                        LastValue[0] = 10;
                        LastValue[1] = 65535;
                        LastValue[2] = (int)lab[0];
                        LastValue[3] = Form2.rgb[0];
                        LastValue[4] = Form2.rgb[1];
                        LastValue[5] = Form2.rgb[2];
                    }

                    video.CloseWebcam();
                }
                else  //灰階法
                {
                    for (int i = 0; i < 4; i++)
                    {
                        switch (i)
                        {
                            case 0:
                                Form1.factor = Form1.D65;
                                break;
                            case 1:
                                Form1.factor = Form1.D50;
                                break;
                            case 2:
                                Form1.factor = Form1.CWF;
                                break;
                            case 3:
                                Form1.factor = Form1.A;
                                break;
                            default: break;
                        }
                        min[i] = new Distance_two(bitmap).Gray(Form1.factor);
                    }
                    double Min = 9999999;
                    for (int i = 0; i < 4; i++)
                    {
                        if (min[i] < Min)
                            Min = min[i];
                    }
                    if (Min == min[3])
                    {

                        textBox1.Text += "A";
                        Form1.SetGamma(10, 65535, (int)lab[0], 39, 1, -48);
                        LastValue[0] = 10;
                        LastValue[1] = 65535;
                        LastValue[2] = (int)lab[0];
                        LastValue[3] = 39;
                        LastValue[4] = 1;
                        LastValue[5] = -48;
                    }
                    else if (Min == min[1])
                    {
                        textBox1.Text += "D50";
                        int[] D50_RGB = new int[3];
                        D50_RGB = Look_up_table.D50_table((int)lab[0]);
                        int[] A_RGB = new int[3];
                        A_RGB = Look_up_table.A_table((int)lab[0]);
                        Form2.rgb = Look_up_table.Gap(D50_RGB, A_RGB);
                        Form2.rgb[0] = 39 + Form2.rgb[0];
                        Form2.rgb[1] = 1 + Form2.rgb[1];
                        Form2.rgb[2] = -48 + Form2.rgb[2];
                        Form1.SetGamma(10, 65535, (int)lab[0], Form2.rgb[0], Form2.rgb[1], Form2.rgb[2]);
                        LastValue[0] = 10;
                        LastValue[1] = 65535;
                        LastValue[2] = (int)lab[0];
                        LastValue[3] = Form2.rgb[0];
                        LastValue[4] = Form2.rgb[1];
                        LastValue[5] = Form2.rgb[2];
                    }
                    else if (Min == min[0])
                    {
                        textBox1.Text += "D65";
                        int[] D65_RGB = new int[3];
                        D65_RGB = Look_up_table.D50_table((int)lab[0]);
                        int[] A_RGB = new int[3];
                        A_RGB = Look_up_table.A_table((int)lab[0]);
                        Form2.rgb = Look_up_table.Gap(D65_RGB, A_RGB);
                        Form2.rgb[0] = 39 + Form2.rgb[0];
                        Form2.rgb[1] = 1 + Form2.rgb[1];
                        Form2.rgb[2] = -48 + Form2.rgb[2];
                        Form1.SetGamma(10, 65535, (int)lab[0], Form2.rgb[0], Form2.rgb[1], Form2.rgb[2]);
                        LastValue[0] = 10;
                        LastValue[1] = 65535;
                        LastValue[2] = (int)lab[0];
                        LastValue[3] = Form2.rgb[0];
                        LastValue[4] = Form2.rgb[1];
                        LastValue[5] = Form2.rgb[2];
                    }
                    else if (Min == min[2])
                    {
                        textBox1.Text += "CWF";
                        int[] CWF_RGB = new int[3];
                        CWF_RGB = Look_up_table.D50_table((int)lab[0]);
                        int[] A_RGB = new int[3];
                        A_RGB = Look_up_table.A_table((int)lab[0]);
                        Form2.rgb = Look_up_table.Gap(CWF_RGB, A_RGB);
                        Form2.rgb[0] = 39 + Form2.rgb[0];
                        Form2.rgb[1] = 1 + Form2.rgb[1];
                        Form2.rgb[2] = -48 + Form2.rgb[2];
                        Form1.SetGamma(10, 65535, (int)lab[0], Form2.rgb[0], Form2.rgb[1], Form2.rgb[2]);
                        LastValue[0] = 10;
                        LastValue[1] = 65535;
                        LastValue[2] = (int)lab[0];
                        LastValue[3] = Form2.rgb[0];
                        LastValue[4] = Form2.rgb[1];
                        LastValue[5] = Form2.rgb[2];
                    }

                    video.CloseWebcam();
                }
              
                MessageBox.Show("轉換成功", "通知");
            }
            if (result1 == DialogResult.No)
            {
                MessageBox.Show("取消轉換", "通知");
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            textBox1.Text = " ";
            video = new Form1.cVideo(pictureBox2.Handle, 0, 0);
            DialogResult result2 = MessageBox.Show("將轉換為CWF光源，確定嗎？", "CWF光源轉換確認", MessageBoxButtons.YesNo);

            if (result2 == DialogResult.Yes)
            {
                video.StartWebCam();
                video.GrabImage();
                IDataObject obj1 = Clipboard.GetDataObject();

                if (obj1.GetDataPresent(typeof(Bitmap)))
                {
                    capImage = (Image)obj1.GetData(typeof(Bitmap));
                }
                Bitmap bitmap = (Bitmap)capImage;

                avRGB = new CalculationRGB(bitmap).getRGBData();
                xyz = new RGB_to_XYZ().RGBtoXYZ(avRGB[0], avRGB[1], avRGB[2]);
                lab = new XYZ_to_LAB().XYZtoLAB(Form1.D65, xyz);

                if (Math.Sqrt((lab[1] * lab[1]) + lab[2] * lab[2]) > 3)    //色域法
                {
                    for (int i = 0; i < 4; i++)
                    {
                        double[] type_a = null, type_b = null;
                        switch (i)
                        {
                            case 0:
                                Form1.s = Form1.D65;
                                type_a = Form1.D65_light_A;
                                type_b = Form1.D65_light_B;
                                break;
                            case 1:
                                Form1.s = Form1.D50;
                                type_a = Form1.D50_light_A;
                                type_b = Form1.D50_light_B;
                                break;
                            case 2:
                                Form1.s = Form1.CWF;
                                type_a = Form1.CWF_light_A;
                                type_b = Form1.CWF_light_B;
                                break;
                            case 3:
                                Form1.s = Form1.A;
                                type_a = Form1.A_light_A;
                                type_b = Form1.A_light_B;
                                break;
                            default: break;
                        }
                        double[][] abValue = new CalculationRGB(bitmap).get_ab_data(Form1.s);
                        double[] a_value = new double[bitmap.Width * bitmap.Height];
                        double[] b_value = new double[bitmap.Width * bitmap.Height];
                        for (int d = 0; d < abValue[0].Length; d++)
                        {
                            a_value[d] = abValue[0][d];
                            b_value[d] = abValue[1][d];

                        }
                        min[i] = Distance.get_line_ab(a_value, b_value, type_a, type_b);

                    }
                    double Min = 9999999;
                    for (int i = 0; i < 4; i++)
                    {
                        if (min[i] < Min)
                            Min = min[i];
                    }
                    if (Min == min[3])
                    {
                        textBox1.Text += "A";
                        int[] A_RGB = new int[3];
                        A_RGB = Look_up_table.A_table((int)lab[0]);
                        int[] CWF_RGB = new int[3];
                        CWF_RGB = Look_up_table.CWF_table((int)lab[0]);
                        Form2.rgb = Look_up_table.Gap(A_RGB, CWF_RGB);
                        Form2.rgb[0] = 19+ Form2.rgb[0];
                        Form2.rgb[1] = 10 + Form2.rgb[1];
                        Form2.rgb[2] = -6 + Form2.rgb[2];
                        Form1.SetGamma(10, 65535, (int)lab[0], Form2.rgb[0], Form2.rgb[1], Form2.rgb[2]);
                        LastValue[0] = 10;
                        LastValue[1] = 65535;
                        LastValue[2] = (int)lab[0];
                        LastValue[3] = Form2.rgb[0];
                        LastValue[4] = Form2.rgb[1];
                        LastValue[5] = Form2.rgb[2];
                    }
                    else if (Min == min[1])
                    {
                        textBox1.Text += "D50";
                        int[] D50_RGB = new int[3];
                        D50_RGB = Look_up_table.D50_table((int)lab[0]);
                        int[] CWF_RGB = new int[3];
                        CWF_RGB = Look_up_table.CWF_table((int)lab[0]);
                        Form2.rgb = Look_up_table.Gap(D50_RGB, CWF_RGB);
                        Form2.rgb[0] = 19 + Form2.rgb[0];
                        Form2.rgb[1] = 10 + Form2.rgb[1];
                        Form2.rgb[2] = -6 + Form2.rgb[2];
                        Form1.SetGamma(10, 65535, (int)lab[0], Form2.rgb[0], Form2.rgb[1], Form2.rgb[2]);
                        LastValue[0] = 10;
                        LastValue[1] = 65535;
                        LastValue[2] = (int)lab[0];
                        LastValue[3] = Form2.rgb[0];
                        LastValue[4] = Form2.rgb[1];
                        LastValue[5] = Form2.rgb[2];
                    }
                    else if (Min == min[0])
                    {
                        textBox1.Text += "D65";
                        int[] D65_RGB = new int[3];
                        D65_RGB = Look_up_table.D65_table((int)lab[0]);
                        int[] CWF_RGB = new int[3];
                        CWF_RGB = Look_up_table.CWF_table((int)lab[0]);
                        Form2.rgb = Look_up_table.Gap(D65_RGB, CWF_RGB);
                        Form2.rgb[0] = 19 + Form2.rgb[0];
                        Form2.rgb[1] = 10 + Form2.rgb[1];
                        Form2.rgb[2] = -6 + Form2.rgb[2];
                        Form1.SetGamma(10, 65535, (int)lab[0], Form2.rgb[0], Form2.rgb[1], Form2.rgb[2]);
                        LastValue[0] = 10;
                        LastValue[1] = 65535;
                        LastValue[2] = (int)lab[0];
                        LastValue[3] = Form2.rgb[0];
                        LastValue[4] = Form2.rgb[1];
                        LastValue[5] = Form2.rgb[2];
                    }
                    else if (Min == min[2])
                    {
                        textBox1.Text += "CWF";
                        Form1.SetGamma(10, 65535, (int)lab[0], 19, 10, -6);
                        LastValue[0] = 10;
                        LastValue[1] = 65535;
                        LastValue[2] = (int)lab[0];
                        LastValue[3] = 19;
                        LastValue[4] = 10;
                        LastValue[5] = -6;
                    }
                    video.CloseWebcam();
                }
                else  //灰階法
                {
                    for (int i = 0; i < 4; i++)
                    {
                        switch (i)
                        {
                            case 0:
                                Form1.factor = Form1.D65;
                                break;
                            case 1:
                                Form1.factor = Form1.D50;
                                break;
                            case 2:
                                Form1.factor = Form1.CWF;
                                break;
                            case 3:
                                Form1.factor = Form1.A;
                                break;
                            default: break;
                        }
                        min[i] = new Distance_two(bitmap).Gray(Form1.factor);
                    }
                    double Min = 9999999;
                    for (int i = 0; i < 4; i++)
                    {
                        if (min[i] < Min)
                            Min = min[i];
                    }
                    if (Min == min[3])
                    {

                        textBox1.Text += "A";
                        int[] A_RGB = new int[3];
                        A_RGB = Look_up_table.A_table((int)lab[0]);
                        int[] CWF_RGB = new int[3];
                        CWF_RGB = Look_up_table.CWF_table((int)lab[0]);
                        Form2.rgb = Look_up_table.Gap(A_RGB, CWF_RGB);
                        Form2.rgb[0] = 19 + Form2.rgb[0];
                        Form2.rgb[1] = 10 + Form2.rgb[1];
                        Form2.rgb[2] = -6 + Form2.rgb[2];
                        Form1.SetGamma(10, 65535, (int)lab[0], Form2.rgb[0], Form2.rgb[1], Form2.rgb[2]);
                        LastValue[0] = 10;
                        LastValue[1] = 65535;
                        LastValue[2] = (int)lab[0];
                        LastValue[3] = Form2.rgb[0];
                        LastValue[4] = Form2.rgb[1];
                        LastValue[5] = Form2.rgb[2];
                    }
                    else if (Min == min[1])
                    {
                        textBox1.Text += "D50";
                        int[] D50_RGB = new int[3];
                        D50_RGB = Look_up_table.D50_table((int)lab[0]);
                        int[] CWF_RGB = new int[3];
                        CWF_RGB = Look_up_table.CWF_table((int)lab[0]);
                        Form2.rgb = Look_up_table.Gap(D50_RGB, CWF_RGB);
                        Form2.rgb[0] = 19 + Form2.rgb[0];
                        Form2.rgb[1] = 10 + Form2.rgb[1];
                        Form2.rgb[2] = -6 + Form2.rgb[2];
                        Form1.SetGamma(10, 65535, (int)lab[0], Form2.rgb[0], Form2.rgb[1], Form2.rgb[2]);
                        LastValue[0] = 10;
                        LastValue[1] = 65535;
                        LastValue[2] = (int)lab[0];
                        LastValue[3] = Form2.rgb[0];
                        LastValue[4] = Form2.rgb[1];
                        LastValue[5] = Form2.rgb[2];
                    }
                    else if (Min == min[0])
                    {
                        textBox1.Text += "D65";
                        int[] D65_RGB = new int[3];
                        D65_RGB = Look_up_table.D65_table((int)lab[0]);
                        int[] CWF_RGB = new int[3];
                        CWF_RGB = Look_up_table.CWF_table((int)lab[0]);
                        Form2.rgb = Look_up_table.Gap(D65_RGB, CWF_RGB);
                        Form2.rgb[0] = 19 + Form2.rgb[0];
                        Form2.rgb[1] = 10 + Form2.rgb[1];
                        Form2.rgb[2] = -6 + Form2.rgb[2];
                        Form1.SetGamma(10, 65535, (int)lab[0], Form2.rgb[0], Form2.rgb[1], Form2.rgb[2]);
                        LastValue[0] = 10;
                        LastValue[1] = 65535;
                        LastValue[2] = (int)lab[0];
                        LastValue[3] = Form2.rgb[0];
                        LastValue[4] = Form2.rgb[1];
                        LastValue[5] = Form2.rgb[2];
                    }
                    else if (Min == min[2])
                    {
                        textBox1.Text += "CWF";
                        Form1.SetGamma(10, 65535, (int)lab[0], 19, 10, -6);
                        LastValue[0] = 10;
                        LastValue[1] = 65535;
                        LastValue[2] = (int)lab[0];
                        LastValue[3] = 19;
                        LastValue[4] = 10;
                        LastValue[5] = -6;
                    }

                    video.CloseWebcam();
                }

                MessageBox.Show("轉換成功", "通知");
            }

            if (result2 == DialogResult.No)
            {
                MessageBox.Show("取消轉換", "通知");
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            textBox1.Text = " ";
            video = new Form1.cVideo(pictureBox3.Handle, 0, 0);
            DialogResult result3 = MessageBox.Show("將轉換為D50光源，確定嗎？", "D50光源轉換確認", MessageBoxButtons.YesNo);

            if (result3 == DialogResult.Yes)
            {
                video.StartWebCam();
                video.GrabImage();
                IDataObject obj1 = Clipboard.GetDataObject();

                if (obj1.GetDataPresent(typeof(Bitmap)))
                {
                    capImage = (Image)obj1.GetData(typeof(Bitmap));
                }
                Bitmap bitmap = (Bitmap)capImage;

                avRGB = new CalculationRGB(bitmap).getRGBData();
                xyz = new RGB_to_XYZ().RGBtoXYZ(avRGB[0], avRGB[1], avRGB[2]);
                lab = new XYZ_to_LAB().XYZtoLAB(Form1.D65, xyz);

                if (Math.Sqrt((lab[1] * lab[1]) + lab[2] * lab[2]) > 3)    //色域法
                {
                    for (int i = 0; i < 4; i++)
                    {
                        double[] type_a = null, type_b = null;
                        switch (i)
                        {
                            case 0:
                                Form1.s = Form1.D65;
                                type_a = Form1.D65_light_A;
                                type_b = Form1.D65_light_B;
                                break;
                            case 1:
                                Form1.s = Form1.D50;
                                type_a = Form1.D50_light_A;
                                type_b = Form1.D50_light_B;
                                break;
                            case 2:
                                Form1.s = Form1.CWF;
                                type_a = Form1.CWF_light_A;
                                type_b = Form1.CWF_light_B;
                                break;
                            case 3:
                                Form1.s = Form1.A;
                                type_a = Form1.A_light_A;
                                type_b = Form1.A_light_B;
                                break;
                            default: break;
                        }
                        double[][] abValue = new CalculationRGB(bitmap).get_ab_data(Form1.s);
                        double[] a_value = new double[bitmap.Width * bitmap.Height];
                        double[] b_value = new double[bitmap.Width * bitmap.Height];
                        for (int d = 0; d < abValue[0].Length; d++)
                        {
                            a_value[d] = abValue[0][d];
                            b_value[d] = abValue[1][d];

                        }
                        min[i] = Distance.get_line_ab(a_value, b_value, type_a, type_b);

                    }
                    double Min = 9999999;
                    for (int i = 0; i < 4; i++)
                    {
                        if (min[i] < Min)
                            Min = min[i];
                    }
                    if (Min == min[3])
                    {

                        textBox1.Text += "A";
                        int[] A_RGB = new int[3];
                        A_RGB = Look_up_table.A_table((int)lab[0]);
                        int[] D50_RGB = new int[3];
                        D50_RGB = Look_up_table.D50_table((int)lab[0]);
                        Form2.rgb = Look_up_table.Gap(A_RGB, D50_RGB);
                        Form2.rgb[0] = 0 + Form2.rgb[0];
                        Form2.rgb[1] = 0 + Form2.rgb[1];
                        Form2.rgb[2] = 0 + Form2.rgb[2];
                        Form1.SetGamma(10, 65535, (int)lab[0], Form2.rgb[0], Form2.rgb[1], Form2.rgb[2]);
                        LastValue[0] = 10;
                        LastValue[1] = 65535;
                        LastValue[2] = (int)lab[0];
                        LastValue[3] = Form2.rgb[0];
                        LastValue[4] = Form2.rgb[1];
                        LastValue[5] = Form2.rgb[2];
                    }
                    else if (Min == min[1])
                    {
                        textBox1.Text += "D50";
                        Form1.SetGamma(10, 65535, (int)lab[0], 0, 0, 0);
                        LastValue[0] = 10;
                        LastValue[1] = 65535;
                        LastValue[2] = (int)lab[0];
                        LastValue[3] =0;
                        LastValue[4] = 0;
                        LastValue[5] = 0;
                    }
                    else if (Min == min[0])
                    {
                        textBox1.Text += "D65";
                        int[] D65_RGB = new int[3];
                        D65_RGB = Look_up_table.D65_table((int)lab[0]);
                        int[] D50_RGB = new int[3];
                        D50_RGB = Look_up_table.D50_table((int)lab[0]);
                        Form2.rgb = Look_up_table.Gap(D65_RGB, D50_RGB);
                        Form2.rgb[0] = 0 + Form2.rgb[0];
                        Form2.rgb[1] = 0 + Form2.rgb[1];
                        Form2.rgb[2] = 0 + Form2.rgb[2];
                        Form1.SetGamma(10, 65535, (int)lab[0], Form2.rgb[0], Form2.rgb[1], Form2.rgb[2]);
                        LastValue[0] = 10;
                        LastValue[1] = 65535;
                        LastValue[2] = (int)lab[0];
                        LastValue[3] = Form2.rgb[0];
                        LastValue[4] = Form2.rgb[1];
                        LastValue[5] = Form2.rgb[2];
                    }
                    else if (Min == min[2])
                    {
                        textBox1.Text += "CWF";
                        int[] CWF_RGB = new int[3];
                        CWF_RGB = Look_up_table.CWF_table((int)lab[0]);
                        int[] D50_RGB = new int[3];
                        D50_RGB = Look_up_table.D50_table((int)lab[0]);
                        Form2.rgb = Look_up_table.Gap(CWF_RGB, D50_RGB);
                        Form2.rgb[0] = 0 + Form2.rgb[0];
                        Form2.rgb[1] = 0 + Form2.rgb[1];
                        Form2.rgb[2] = 0 + Form2.rgb[2];
                        Form1.SetGamma(10, 65535, (int)lab[0], Form2.rgb[0], Form2.rgb[1], Form2.rgb[2]);
                        LastValue[0] = 10;
                        LastValue[1] = 65535;
                        LastValue[2] = (int)lab[0];
                        LastValue[3] = Form2.rgb[0];
                        LastValue[4] = Form2.rgb[1];
                        LastValue[5] = Form2.rgb[2];
                    }

                    video.CloseWebcam();
                }
                else  //灰階法
                {
                    for (int i = 0; i < 4; i++)
                    {
                        switch (i)
                        {
                            case 0:
                                Form1.factor = Form1.D65;
                                break;
                            case 1:
                                Form1.factor = Form1.D50;
                                break;
                            case 2:
                                Form1.factor = Form1.CWF;
                                break;
                            case 3:
                                Form1.factor = Form1.A;
                                break;
                            default: break;
                        }
                        min[i] = new Distance_two(bitmap).Gray(Form1.factor);
                    }
                    double Min = 9999999;
                    for (int i = 0; i < 4; i++)
                    {
                        if (min[i] < Min)
                            Min = min[i];
                    }
                    if (Min == min[3])
                    {

                        textBox1.Text += "A";
                        int[] A_RGB = new int[3];
                        A_RGB = Look_up_table.A_table((int)lab[0]);
                        int[] D50_RGB = new int[3];
                        D50_RGB = Look_up_table.D50_table((int)lab[0]);
                        Form2.rgb = Look_up_table.Gap(A_RGB, D50_RGB);
                        Form2.rgb[0] = 0 + Form2.rgb[0];
                        Form2.rgb[1] = 0 + Form2.rgb[1];
                        Form2.rgb[2] = 0 + Form2.rgb[2];
                        Form1.SetGamma(10, 65535, (int)lab[0], Form2.rgb[0], Form2.rgb[1], Form2.rgb[2]);
                        LastValue[0] = 10;
                        LastValue[1] = 65535;
                        LastValue[2] = (int)lab[0];
                        LastValue[3] = Form2.rgb[0];
                        LastValue[4] = Form2.rgb[1];
                        LastValue[5] = Form2.rgb[2];
                    }
                    else if (Min == min[1])
                    {
                        textBox1.Text += "D50";
                        Form1.SetGamma(10, 65535, (int)lab[0], 0, 0, 0);
                        LastValue[0] = 10;
                        LastValue[1] = 65535;
                        LastValue[2] = (int)lab[0];
                        LastValue[3] = 0;
                        LastValue[4] = 0;
                        LastValue[5] = 0;
                    }
                    else if (Min == min[0])
                    {
                        textBox1.Text += "D65";
                        int[] D65_RGB = new int[3];
                        D65_RGB = Look_up_table.D65_table((int)lab[0]);
                        int[] D50_RGB = new int[3];
                        D50_RGB = Look_up_table.D50_table((int)lab[0]);
                        Form2.rgb = Look_up_table.Gap(D65_RGB, D50_RGB);
                        Form2.rgb[0] = 0 + Form2.rgb[0];
                        Form2.rgb[1] = 0 + Form2.rgb[1];
                        Form2.rgb[2] = 0 + Form2.rgb[2];
                        Form1.SetGamma(10, 65535, (int)lab[0], Form2.rgb[0], Form2.rgb[1], Form2.rgb[2]);
                        LastValue[0] = 10;
                        LastValue[1] = 65535;
                        LastValue[2] = (int)lab[0];
                        LastValue[3] = Form2.rgb[0];
                        LastValue[4] = Form2.rgb[1];
                        LastValue[5] = Form2.rgb[2];
                    }
                    else if (Min == min[2])
                    {
                        textBox1.Text += "CWF";
                        int[] CWF_RGB = new int[3];
                        CWF_RGB = Look_up_table.CWF_table((int)lab[0]);
                        int[] D50_RGB = new int[3];
                        D50_RGB = Look_up_table.D50_table((int)lab[0]);
                        Form2.rgb = Look_up_table.Gap(CWF_RGB, D50_RGB);
                        Form2.rgb[0] = 0 + Form2.rgb[0];
                        Form2.rgb[1] = 0 + Form2.rgb[1];
                        Form2.rgb[2] = 0 + Form2.rgb[2];
                        Form1.SetGamma(10, 65535, (int)lab[0], Form2.rgb[0], Form2.rgb[1], Form2.rgb[2]);
                        LastValue[0] = 10;
                        LastValue[1] = 65535;
                        LastValue[2] = (int)lab[0];
                        LastValue[3] = Form2.rgb[0];
                        LastValue[4] = Form2.rgb[1];
                        LastValue[5] = Form2.rgb[2];
                    }

                    video.CloseWebcam();
                }

                MessageBox.Show("轉換成功", "通知");
            }

            if (result3 == DialogResult.No)
            {
                MessageBox.Show("取消轉換", "通知");
            }
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            textBox1.Text = " ";
            video = new Form1.cVideo(pictureBox4.Handle, 0, 0);
            DialogResult result4 = MessageBox.Show("將轉換為D65光源，確定嗎？", "D65光源轉換確認", MessageBoxButtons.YesNo);

            if (result4 == DialogResult.Yes)
            {
                video.StartWebCam();
                video.GrabImage();
                IDataObject obj1 = Clipboard.GetDataObject();

                if (obj1.GetDataPresent(typeof(Bitmap)))
                {
                    capImage = (Image)obj1.GetData(typeof(Bitmap));
                }
                Bitmap bitmap = (Bitmap)capImage;

                avRGB = new CalculationRGB(bitmap).getRGBData();
                xyz = new RGB_to_XYZ().RGBtoXYZ(avRGB[0], avRGB[1], avRGB[2]);
                lab = new XYZ_to_LAB().XYZtoLAB(Form1.D65, xyz);

                if (Math.Sqrt((lab[1] * lab[1]) + lab[2] * lab[2]) > 3)    //色域法
                {
                    for (int i = 0; i < 4; i++)
                    {
                        double[] type_a = null, type_b = null;
                        switch (i)
                        {
                            case 0:
                                Form1.s = Form1.D65;
                                type_a = Form1.D65_light_A;
                                type_b = Form1.D65_light_B;
                                break;
                            case 1:
                                Form1.s = Form1.D50;
                                type_a = Form1.D50_light_A;
                                type_b = Form1.D50_light_B;
                                break;
                            case 2:
                                Form1.s = Form1.CWF;
                                type_a = Form1.CWF_light_A;
                                type_b = Form1.CWF_light_B;
                                break;
                            case 3:
                                Form1.s = Form1.A;
                                type_a = Form1.A_light_A;
                                type_b = Form1.A_light_B;
                                break;
                            default: break;
                        }
                        double[][] abValue = new CalculationRGB(bitmap).get_ab_data(Form1.s);
                        double[] a_value = new double[bitmap.Width * bitmap.Height];
                        double[] b_value = new double[bitmap.Width * bitmap.Height];
                        for (int d = 0; d < abValue[0].Length; d++)
                        {
                            a_value[d] = abValue[0][d];
                            b_value[d] = abValue[1][d];

                        }
                        min[i] = Distance.get_line_ab(a_value, b_value, type_a, type_b);

                    }
                    double Min = 9999999;
                    for (int i = 0; i < 4; i++)
                    {
                        if (min[i] < Min)
                            Min = min[i];
                    }
                    if (Min == min[3])
                    {

                        textBox1.Text += "A";
                        int[] A_RGB = new int[3];
                        A_RGB = Look_up_table.A_table((int)lab[0]);
                        int[] D65_RGB = new int[3];
                        D65_RGB = Look_up_table.D65_table((int)lab[0]);
                        Form2.rgb = Look_up_table.Gap(A_RGB, D65_RGB);
                        Form2.rgb[0] = 17 + Form2.rgb[0];
                        Form2.rgb[1] = 17 + Form2.rgb[1];
                        Form2.rgb[2] = 34 + Form2.rgb[2];
                        Form1.SetGamma(10, 65535, (int)lab[0], Form2.rgb[0], Form2.rgb[1], Form2.rgb[2]);
                        LastValue[0] = 10;
                        LastValue[1] = 65535;
                        LastValue[2] = (int)lab[0];
                        LastValue[3] = Form2.rgb[0];
                        LastValue[4] = Form2.rgb[1];
                        LastValue[5] = Form2.rgb[2];
                    }
                    else if (Min == min[1])
                    {
                        textBox1.Text += "D50";
                        int[] D50_RGB = new int[3];
                        D50_RGB = Look_up_table.D50_table((int)lab[0]);
                        int[] D65_RGB = new int[3];
                        D65_RGB = Look_up_table.D65_table((int)lab[0]);
                        Form2.rgb = Look_up_table.Gap(D50_RGB, D65_RGB);
                        Form2.rgb[0] = 17 + Form2.rgb[0];
                        Form2.rgb[1] = 17 + Form2.rgb[1];
                        Form2.rgb[2] = 34 + Form2.rgb[2];
                        Form1.SetGamma(10, 65535, (int)lab[0], Form2.rgb[0], Form2.rgb[1], Form2.rgb[2]);
                        LastValue[0] = 10;
                        LastValue[1] = 65535;
                        LastValue[2] = (int)lab[0];
                        LastValue[3] = Form2.rgb[0];
                        LastValue[4] = Form2.rgb[1];
                        LastValue[5] = Form2.rgb[2];
                    }
                    else if (Min == min[0])
                    {
                        textBox1.Text += "D65";
               
                        Form1.SetGamma(10, 65535, (int)lab[0],0, 1, 25);
                        LastValue[0] = 10;
                        LastValue[1] = 65535;
                        LastValue[2] = (int)lab[0];
                        LastValue[3] = 0;
                        LastValue[4] = 1;
                        LastValue[5] = 25;
                    }
                    else if (Min == min[2])
                    {
                        textBox1.Text += "CWF";
                        int[] CWF_RGB = new int[3];
                        CWF_RGB = Look_up_table.CWF_table((int)lab[0]);
                        int[] D65_RGB = new int[3];
                        D65_RGB = Look_up_table.D65_table((int)lab[0]);
                        Form2.rgb = Look_up_table.Gap(CWF_RGB, D65_RGB);
                        Form2.rgb[0] = 17 + Form2.rgb[0];
                        Form2.rgb[1] = 17 + Form2.rgb[1];
                        Form2.rgb[2] = 34 + Form2.rgb[2];
                        Form1.SetGamma(10, 65535, (int)lab[0], Form2.rgb[0], Form2.rgb[1], Form2.rgb[2]);
                        LastValue[0] = 10;
                        LastValue[1] = 65535;
                        LastValue[2] = (int)lab[0];
                        LastValue[3] = Form2.rgb[0];
                        LastValue[4] = Form2.rgb[1];
                        LastValue[5] = Form2.rgb[2];
                    }

                    video.CloseWebcam();
                }
                else  //灰階法
                {
                    for (int i = 0; i < 4; i++)
                    {
                        switch (i)
                        {
                            case 0:
                                Form1.factor = Form1.D65;
                                break;
                            case 1:
                                Form1.factor = Form1.D50;
                                break;
                            case 2:
                                Form1.factor = Form1.CWF;
                                break;
                            case 3:
                                Form1.factor = Form1.A;
                                break;
                            default: break;
                        }
                        min[i] = new Distance_two(bitmap).Gray(Form1.factor);
                    }
                    double Min = 9999999;
                    for (int i = 0; i < 4; i++)
                    {
                        if (min[i] < Min)
                            Min = min[i];
                    }
                    if (Min == min[3])
                    {

                        textBox1.Text += "A";
                        int[] A_RGB = new int[3];
                        A_RGB = Look_up_table.A_table((int)lab[0]);
                        int[] D65_RGB = new int[3];
                        D65_RGB = Look_up_table.D65_table((int)lab[0]);
                        Form2.rgb = Look_up_table.Gap(A_RGB, D65_RGB);
                        Form2.rgb[0] = 17 + Form2.rgb[0];
                        Form2.rgb[1] = 17 + Form2.rgb[1];
                        Form2.rgb[2] = 34 + Form2.rgb[2];
                        Form1.SetGamma(10, 65535, (int)lab[0], Form2.rgb[0], Form2.rgb[1], Form2.rgb[2]);
                        LastValue[0] = 10;
                        LastValue[1] = 65535;
                        LastValue[2] = (int)lab[0];
                        LastValue[3] = Form2.rgb[0];
                        LastValue[4] = Form2.rgb[1];
                        LastValue[5] = Form2.rgb[2];
                    }
                    else if (Min == min[1])
                    {
                        textBox1.Text += "D50";
                        int[] D50_RGB = new int[3];
                        D50_RGB = Look_up_table.D50_table((int)lab[0]);
                        int[] D65_RGB = new int[3];
                        D65_RGB = Look_up_table.D65_table((int)lab[0]);
                        Form2.rgb = Look_up_table.Gap(D50_RGB, D65_RGB);
                        Form2.rgb[0] = 17 + Form2.rgb[0];
                        Form2.rgb[1] = 17 + Form2.rgb[1];
                        Form2.rgb[2] = 34 + Form2.rgb[2];
                        Form1.SetGamma(10, 65535, (int)lab[0], Form2.rgb[0], Form2.rgb[1], Form2.rgb[2]);
                        LastValue[0] = 10;
                        LastValue[1] = 65535;
                        LastValue[2] = (int)lab[0];
                        LastValue[3] = Form2.rgb[0];
                        LastValue[4] = Form2.rgb[1];
                        LastValue[5] = Form2.rgb[2];
                    }
                    else if (Min == min[0])
                    {
                        textBox1.Text += "D65";

                        Form1.SetGamma(10, 65535, (int)lab[0], 17, 17, 34);
                        LastValue[0] = 10;
                        LastValue[1] = 65535;
                        LastValue[2] = (int)lab[0];
                        LastValue[3] = 17;
                        LastValue[4] = 17;
                        LastValue[5] = 34;
                    }
                    else if (Min == min[2])
                    {
                        textBox1.Text += "CWF";
                        int[] CWF_RGB = new int[3];
                        CWF_RGB = Look_up_table.CWF_table((int)lab[0]);
                       int[] D65_RGB = new int[3];
                        D65_RGB = Look_up_table.D65_table((int)lab[0]);
                        Form2.rgb = Look_up_table.Gap(CWF_RGB, D65_RGB);
                        Form2.rgb[0] = 17 + Form2.rgb[0];
                        Form2.rgb[1] = 17 + Form2.rgb[1];
                        Form2.rgb[2] = 34 + Form2.rgb[2];
                        Form1.SetGamma(10, 65535, (int)lab[0], Form2.rgb[0], Form2.rgb[1], Form2.rgb[2]);
                        LastValue[0] = 10;
                        LastValue[1] = 65535;
                        LastValue[2] = (int)lab[0];
                        LastValue[3] = Form2.rgb[0];
                        LastValue[4] = Form2.rgb[1];
                        LastValue[5] = Form2.rgb[2];
                    }

                    video.CloseWebcam();
                }

                MessageBox.Show("轉換成功", "通知");
            }

            if (result4 == DialogResult.No)
            {
                MessageBox.Show("取消轉換", "通知");
            }
        }
        public int GetBrightness2()
        {
            return (int)lab[0];
        }
        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            Main_form.Show();
        }

        
    }
}

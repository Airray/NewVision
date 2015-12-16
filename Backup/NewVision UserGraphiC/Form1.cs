using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Screen_team;
using Microsoft.Win32;
using System.IO;

namespace NewVision_UserGraphiC
{
    public partial class Form1 : Form
    {

        #region 視頻API
        public class VideoAPI  //視頻API類
        {
            //  視頻ＡＰＩ呼叫
            [DllImport("avicap32.dll")]
            public static extern IntPtr capCreateCaptureWindowA(byte[] lpszWindowName, int dwStyle, int x, int y, int nWidth, int nHeight, IntPtr hWndParent, int nID);
            [DllImport("avicap32.dll")]
            public static extern bool capGetDriverDescriptionA(short wDriver, byte[] lpszName, int cbName, byte[] lpszVer, int cbVer);
            [DllImport("User32.dll")]
            public static extern bool SendMessage(IntPtr hWnd, int wMsg, bool wParam, int lParam);
            [DllImport("User32.dll")]
            public static extern bool SendMessage(IntPtr hWnd, int wMsg, short wParam, int lParam);

            //  常數
            public const int WM_USER = 0x400;
            public const int WS_CHILD = 0x40000000;
            public const int WS_VISIBLE = 0x10000000;
            public const int SWP_NOMOVE = 0x2;
            public const int SWP_NOZORDER = 0x4;
            public const int WM_CAP_DRIVER_CONNECT = WM_USER + 10;
            public const int WM_CAP_DRIVER_DISCONNECT = WM_USER + 11;
            public const int WM_CAP_SET_CALLBACK_FRAME = WM_USER + 5;
            public const int WM_CAP_SET_PREVIEW = WM_USER + 50;
            public const int WM_CAP_SET_PREVIEWRATE = WM_USER + 52;
            public const int WM_CAP_SET_VIDEOFORMAT = WM_USER + 45;
            public const int WM_CAP_START = WM_USER;
            public const int WM_CAP_SAVEDIB = WM_CAP_START + 25;
        }

        public class cVideo     //視頻類
        {
            private IntPtr lwndC;       //保存無符號控制碼
            private IntPtr mControlPtr; //保存管理指示器
            private int mWidth;
            private int mHeight;

            public cVideo(IntPtr handle, int width, int height)
            {
                mControlPtr = handle; //顯示視頻控制項的控制碼
                mWidth = width;      //視頻寬度
                mHeight = height;    //視頻高度
            }
            /// <summary>
            /// 打開視頻設備
            /// </summary>
            public void StartWebCam()
            {
                byte[] lpszName = new byte[100];
                byte[] lpszVer = new byte[100];

                VideoAPI.capGetDriverDescriptionA(0, lpszName, 100, lpszVer, 100);
                this.lwndC = VideoAPI.capCreateCaptureWindowA(lpszName, VideoAPI.WS_CHILD | VideoAPI.WS_VISIBLE, 0, 0, mWidth, mHeight, mControlPtr, 0);

                if (VideoAPI.SendMessage(lwndC, VideoAPI.WM_CAP_DRIVER_CONNECT, 0, 0))
                {
                    VideoAPI.SendMessage(lwndC, VideoAPI.WM_CAP_SET_PREVIEWRATE, 100, 0);
                    VideoAPI.SendMessage(lwndC, VideoAPI.WM_CAP_SET_PREVIEW, true, 0);
                }
            }
            /// <summary>
            /// 關閉視頻設備
            /// </summary>
            public void CloseWebcam()
            {
                VideoAPI.SendMessage(lwndC, VideoAPI.WM_CAP_DRIVER_DISCONNECT, 0, 0);
            }

            ///   <summary>   
            ///   拍照 
            ///   </summary>   
            ///   <param   name="path">要保存bmp文件的路徑</param>   
            //public void GrabImage(IntPtr hWndC, string path)
            //{
            //    IntPtr hBmp = Marshal.StringToHGlobalAnsi(path);

            //    VideoAPI.SendMessage(lwndC, VideoAPI.WM_CAP_SAVEDIB, 0, hBmp.ToInt32());
            //}
            public void GrabImage()
            {
                // IntPtr hBmp = Marshal.StringToHGlobalAnsi(path);

                VideoAPI.SendMessage(lwndC, 0x41e, 0, 0);
            }

        }
        #endregion
       
        #region 光源資料

        public static double[] D65_RGB = { 254, 255, 254 };
        public static double[] A_RGB = { 333, 234, 132 };
        public static double[] D50_RGB = { 273, 252, 220 };
        public static double[] CWF_RGB = { 260, 252, 265 };
        public static double[] factor = new double[3];
        public  static double[] A = new double[3] { 109.83, 100.0, 35.55 };
        public  static double[] CWF = new double[3] { 98.04, 100.0, 118.1 };
        public static double[] D65 = new double[3] { 95.02, 100.0, 108.81 };
        public  static double[] D50 = new double[3] { 96.4296, 100.0, 82.5105 };
        public static double[] s = new double[3];
        public static double[] v = new double[3];

        public static double[] A_light_A = { -47, -42, -21, -9, 43, 36, 29, -25, -48 };
        public static double[] A_light_B = { -3, 20, 34, 39, 23, 4, -12, -84, -28 };

        public static double[] D65_light_A = { -37, -20, -7, 38, 59, 55, 39, -5, -23, -36, -45, -58, -61 };
        public static double[] D65_light_B = { 64, 77, 73, 60, 37, -39, -61, -44, -30, -14, 4, 32, 39 };

        public static double[] D50_light_A = { -37, -24, -9, 41, 54, 39, 16, -4, -21, -31, -39, -54 };
        public static double[] D50_light_B = { 54, 60, 64, 51, 28, -42, -54, -60, -40, -27, -8, 32 };

        public static double[] CWF_light_A = { -32, -16, -5, 35, 58, 54, 46, 26, -34, -44, -58 };
        public static double[] CWF_light_B = { 67, 79, 78, 65, 52, 21, -22, -46, -15, 3, 44 };

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //static double[] D65_light_A = { -14, 74, 112, 142, 147, 98, 10, -12, -28, -89, -68, -58 };
        //static double[] D65_light_B = { -100, -62, -22, 34, 87, 113, 137, 142, 135, 65, -35, -55 };

        //static double[] D50_light_A = { -31, 2, 61, 111, 140, 115, 16, -5, -9, -49, -73, -52, -43 };
        //static double[] D50_light_B = { -59, -58, -23, 16, 86, 94, 119, 121, 121, 110, 70, -10, -36 };

        //static double[] CWF_light_A = { 4, 59, 97, 119, 124, -6, -24, -40, -59, -65, -59, -52, -42, -27 };
        //static double[] CWF_light_B = { -48, -18, 27, 83, 99, 146, 152, 136, 109, 94, 55, 16, -13, -33 };

        //static double[] A_light_A = { -35, 11, 132, 139, 136, -25, -43, -66, -66, -51 };
        //static double[] A_light_B = { -73, -51, 22, 60, 62, 86, 77, 53, 52, -55 };
        #endregion

        #region gamma
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        private struct RAMP
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public UInt16[] Red;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public UInt16[] Green;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public UInt16[] Blue;
        }
        [DllImport("gdi32.dll")]
        private static extern bool GetDeviceGammaRamp(IntPtr hdc, ref RAMP lpRamp);
        [DllImport("gdi32.dll")]
        private static extern bool SetDeviceGammaRamp(IntPtr hDC, ref RAMP lpRamp);
        [DllImport("user32.dll")]
        static extern IntPtr GetDC(IntPtr hWnd);
        private static RAMP s_ramp = new RAMP();
        public static void SetGamma(double Gamma, double Contrast, double S_brights, double R_brights, double G_brights, double B_brights)
        {
            s_ramp.Red = new ushort[256];
            s_ramp.Green = new ushort[256];
            s_ramp.Blue = new ushort[256];
            for (int i = 1; i < 256; i++)
            {
                // gamma is a value between 3 and 44
                //s_ramp.Red[i] = s_ramp.Green[i] = s_ramp.Blue[i] = (ushort)(Math.Min(65535, Math.Max(0, Math.Pow((i + 1) / 256.0, 30 * 0.1) * 65535 + 0.5)));
                //s_ramp.Red[i] = (ushort)(Math.Min(65535, Math.Max(0, Math.Pow((i + 1) / 256.0, gamma1 * 0.1) * (65535) + (i * (brights - 50) * 2))));
                //s_ramp.Green[i] = (ushort)(Math.Min(65535, Math.Max(0, Math.Pow((i + 1) / 256.0, gamma2 * 0.1) * (65535) + (i * (brights - 50) * 2))));
                //s_ramp.Blue[i] = (ushort)(Math.Min(65535, Math.Max(0, Math.Pow((i + 1) / 256.0, gamma3 * 0.1) * (65535) + (i * (brights - 50) * 2))));

                s_ramp.Red[i] = (ushort)(Math.Min(65535, Math.Max(0, Math.Pow((i + 1) / 256.0, Gamma * 0.1) * (Contrast) + (i * (S_brights - 50) * 2) + (i * R_brights))));
                s_ramp.Green[i] = (ushort)(Math.Min(65535, Math.Max(0, Math.Pow((i + 1) / 256.0, Gamma * 0.1) * (Contrast) + (i * (S_brights - 50) * 2) + (i * G_brights))));
                s_ramp.Blue[i] = (ushort)(Math.Min(65535, Math.Max(0, Math.Pow((i + 1) / 256.0, Gamma * 0.1) * (Contrast) + (i * (S_brights - 50) * 2) + (i * B_brights))));
   
            }
            // Now set the value.
            SetDeviceGammaRamp(GetDC(IntPtr.Zero), ref s_ramp);
        }
        #endregion
        
        cVideo video;
        NotifyIcon notifyIcon;
        public Image image1=null;
        string FileName;
        private double[] min = new double[4];
        private double[] small = new double[4];
        private int[] avRGB= new int[3];
        private double[] xyz= new double[3];
        private static double[] lab= new double[3];
        public static Situation[] SSa  = new Situation[20];
        DirectoryInfo dir = new DirectoryInfo(@"C:\MyMonitor");
        public Form1()
        {
            InitializeComponent();
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            string checkresult=null;
            if (!dir.Exists)
            {
                dir.Create();
                dir.Refresh();
                FileInfo file = new FileInfo(@"C:\MyMonitor\Situation.txt");
                FileStream fs = file.Create();
                fs.Close();
                FileInfo file2 = new FileInfo(@"C:\MyMonitor\Name.txt");
                FileStream fs2 = file2.Create();
                fs2.Close();
                FileInfo file3 = new FileInfo(@"C:\MyMonitor\LastRecord.txt");
                FileStream fs3 = file3.Create();
                fs3.Close();
            }



            StreamReader checksr = null;
            try
            {
                checksr = new StreamReader(@"C:\MyMonitor\LastRecord.txt", System.Text.Encoding.Default);
            }
            catch (Exception ex)
            {
            }
            try
            {
              checkresult = checksr.ReadLine();  //讀取一列 
              checksr.Close();
            }
            catch (NullReferenceException ex)
            {

            }


            try
            {
                if (checkresult.Equals("1"))
                {
                    StreamReader sr = null;
                    try
                    {
                        sr = new StreamReader(@"C:\MyMonitor\LastRecord.txt", System.Text.Encoding.Default);
                    }
                    catch (Exception ex)
                    {
                    }
                    try
                    {
                        while (sr.Peek() != -1) //判斷是否已經讀取最後
                        {
                            string currentRow = sr.ReadLine();  //讀取一列
                            if (currentRow.Equals("1"))
                            {
                                continue;
                            }
                            string[] tempword = currentRow.Trim().Split(' '); // 將字串以空白字元分解
                            Form2.LastValue[0] = Convert.ToInt32(tempword[0]);
                            Form2.LastValue[1] = Convert.ToInt32(tempword[1]);
                            Form2.LastValue[2] = Convert.ToInt32(tempword[2]);
                            Form2.LastValue[3] = Convert.ToInt32(tempword[3]);
                            Form2.LastValue[4] = Convert.ToInt32(tempword[4]);
                            Form2.LastValue[5] = Convert.ToInt32(tempword[5]);
                        }
                        sr.Close();
                    }
                    catch (NullReferenceException ex)
                    {

                    }

                    Form1.SetGamma(Form2.LastValue[0], Form2.LastValue[1], Form2.LastValue[2], Form2.LastValue[3], Form2.LastValue[4], Form2.LastValue[5]);
                    StreamWriter sss=  File.CreateText(@"C:\MyMonitor\LastRecord.txt");
                    sss.Close();
                }
                else
                {
                    StreamWriter sss = File.CreateText(@"C:\MyMonitor\LastRecord.txt");
                    sss.Close();                 
                }
            }
            catch (Exception ex)
            {
            }
            notifyIconShow();
            video = new cVideo(button1.Handle, 0, 0);   
        }
        void notifyIconShow()
        {
            this.notifyIcon = new NotifyIcon();
            this.notifyIcon.BalloonTipText = "New Vision";
            this.notifyIcon.Text = "New Vision";
            this.notifyIcon.Icon = new System.Drawing.Icon("64.ico");
            this.notifyIcon.ShowBalloonTip(1000);
            notifyIcon.DoubleClick += new EventHandler(notifyIcon_DoubleClick);
            //TODO:初始化 notifyIcon菜單
            ContextMenu notifyIconMenu = new ContextMenu();
            MenuItem notifyIconMenuItem = new MenuItem();
            notifyIconMenuItem.Index = 0;
            notifyIconMenuItem.Text = "Open";
            notifyIconMenuItem.Click += new EventHandler(notifyIconMenuItem1_Click);
            notifyIconMenu.MenuItems.Add(notifyIconMenuItem);
            
            MenuItem notifyIconMenuItem2 = new MenuItem();
            notifyIconMenuItem2.Index = 1;
            notifyIconMenuItem2.Text = "Exit";
            notifyIconMenuItem2.Click += new EventHandler(notifyIconMenuItem2_Click);
            notifyIconMenu.MenuItems.Add(notifyIconMenuItem2);

            notifyIcon.ContextMenu = notifyIconMenu;
            this.notifyIcon.Visible = false;     //設定剛開啟不會顯示ico於系統通知列
         
        }

        void notifyIconMenuItem1_Click(object sender, EventArgs e)
        {
            this.Show();
            WindowState = FormWindowState.Normal;
            notifyIcon.Visible = false;
        }

        void notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            this.Show();
            WindowState = FormWindowState.Normal;
            notifyIcon.Visible = false;
        }

        void notifyIconMenuItem2_Click(object sender, EventArgs e)
        {
            notifyIcon.Visible = false;
            this.notifyIcon = new NotifyIcon();
            Application.Exit();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
                notifyIcon.Visible = true;
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            notifyIcon.Visible = false;
            Application.Exit();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                string check = "1" + "\r\n";
                char[] checkCharAry = check.ToCharArray(); // 將 string 轉為 char
                byte[] checkByteAry = new byte[checkCharAry.Length]; // 宣告 byte[] 陣列，長度為 CharAry.Length
                for (int i = 0; i < checkCharAry.Length; i++)
                    checkByteAry[i] = (byte)checkCharAry[i]; // 將 char 轉為 byte
                FileStream fss = new FileStream(@"C:\MyMonitor\LastRecord.txt", FileMode.Append);
                fss.Write(checkByteAry, 0, checkByteAry.Length); // 以 fs.Write() 寫入檔案
                fss.Close();
            }
            else
            {
                string check2 = "0" + "\r\n";
                char[] checkCharAry2 = check2.ToCharArray(); // 將 string 轉為 char
                byte[] checkByteAry2 = new byte[checkCharAry2.Length]; // 宣告 byte[] 陣列，長度為 CharAry.Length
                for (int i = 0; i < checkCharAry2.Length; i++)
                    checkByteAry2[i] = (byte)checkCharAry2[i]; // 將 char 轉為 byte
                FileStream fsss = new FileStream(@"C:\MyMonitor\LastRecord.txt", FileMode.Append);
                fsss.Write(checkByteAry2, 0, checkByteAry2.Length); // 以 fs.Write() 寫入檔案
                fsss.Close();
            }

            string data = Form2.LastValue[0].ToString()
                  + " " + Form2.LastValue[1].ToString()
                  + " " + Form2.LastValue[2].ToString()
                   + " " + Form2.LastValue[3].ToString()
                   + " " + Form2.LastValue[4].ToString()
                    + " " + Form2.LastValue[5].ToString() + "\r\n";
            char[] CharAry = data.ToCharArray(); // 將 string 轉為 char
            byte[] ByteAry = new byte[CharAry.Length]; // 宣告 byte[] 陣列，長度為 CharAry.Length
            for (int i = 0; i < CharAry.Length; i++)
                ByteAry[i] = (byte)CharAry[i]; // 將 char 轉為 byte
            FileStream fs = new FileStream(@"C:\MyMonitor\LastRecord.txt", FileMode.Append);
            fs.Write(ByteAry, 0, ByteAry.Length); // 以 fs.Write() 寫入檔案
            fs.Close();
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Webcam--->Choose your webcam by ComboBox. \nManual--->Choose the most comfortable to you. \nExit--->Quit \n ");
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("iexplore.exe", "http://www.google.com.tw"); //打開超連結
        }

        private void button1_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = null;
            textBox1.Text = " ";
            video.StartWebCam();
            video.GrabImage();
            IDataObject obj1 = Clipboard.GetDataObject();
            //Clipboard 方法，將資料放置於系統剪貼簿上，然後從系統剪貼簿加以擷取。
            // SetDataObject，以取得文字方塊中所選取的文字，並將它放置於系統的剪貼簿中。
            //GetDataObject，從系統剪貼簿中擷取資料
            //該程式碼會使用 IDataObject 和 DataFormats，擷取傳回的資料
            if (obj1.GetDataPresent(typeof(Bitmap)))//決定儲存於這個執行個體中的資料是否與指定的格式相關，或是可以轉換成指定的格式。
            {
                image1 = (Image)obj1.GetData(typeof(Bitmap));   //從系統剪貼簿撈回影像數據資料
                pictureBox1.Image = image1;
            }
            Bitmap bitmap = (Bitmap)image1;

            avRGB = new CalculationRGB(bitmap).getRGBData();          //判斷彩度
            xyz = new RGB_to_XYZ().RGBtoXYZ(avRGB[0], avRGB[1], avRGB[2]);
            lab = new XYZ_to_LAB().XYZtoLAB(D65, xyz);

            //textBox1.Text = " " + lab[0].ToString();

            if (Math.Sqrt((lab[1] * lab[1]) + lab[2] * lab[2]) > 3)    //色域法
                {
                    for (int i = 0; i < 4; i++)
                    {
                        double[] type_a = null, type_b = null;
                        switch (i)
                        {
                            case 0:
                                s = D65;
                                type_a = D65_light_A;
                                type_b = D65_light_B;
                                break;
                            case 1:
                                s = D50;
                                type_a = D50_light_A;
                                type_b = D50_light_B;
                                break;
                            case 2:
                                s = CWF;
                                type_a = CWF_light_A;
                                type_b = CWF_light_B;
                                break;
                            case 3:
                                s = A;
                                type_a = A_light_A;
                                type_b = A_light_B;
                                break;
                            default: break;
                        }
                        double[][] abValue = new CalculationRGB(bitmap).get_ab_data(s);
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
                        int[] D50_RGB = new int[3];
                        D50_RGB = Look_up_table.D50_table((int)lab[0]);
                        int[] A_RGB = new int[3];
                        A_RGB = Look_up_table.A_table((int)lab[0]);
                        Form2.rgb = Look_up_table.Gap(D50_RGB, A_RGB);
                        SetGamma(10, 65535, (int)lab[0], Form2.rgb[0], Form2.rgb[1], Form2.rgb[2]);
                        Form2.LastValue[0] = 10;
                        Form2.LastValue[1] = 65535;
                        Form2.LastValue[2] = (int)lab[0];
                        Form2.LastValue[3] = Form2.rgb[0];
                        Form2.LastValue[4] = Form2.rgb[1];
                        Form2.LastValue[5] = Form2.rgb[2];
                       
                        
                    }
                    else if (Min == min[1])
                    {
                        textBox1.Text += "D50";
                        Form2.rgb[0] = 0;
                        Form2.rgb[1] = 0;
                        Form2.rgb[2] = 0;
                        SetGamma(10, 65535, (int)lab[0], Form2.rgb[0], Form2.rgb[1], Form2.rgb[2]);
                        Form2.LastValue[0] = 10;
                        Form2.LastValue[1] = 65535;
                        Form2.LastValue[2] = (int)lab[0];
                        Form2.LastValue[3] = Form2.rgb[0];
                        Form2.LastValue[4] = Form2.rgb[1];
                        Form2.LastValue[5] = Form2.rgb[2];
                    }
                    else if (Min == min[0])
                    {
                        textBox1.Text += "D65";
                        int[] D50_RGB = new int[3];
                        D50_RGB = Look_up_table.D50_table((int)lab[0]);
                        int[] D65_RGB = new int[3];
                        D65_RGB = Look_up_table.D65_table((int)lab[0]);
                        Form2.rgb = Look_up_table.Gap(D50_RGB, D65_RGB);
                        SetGamma(10, 65535, (int)lab[0], Form2.rgb[0], Form2.rgb[1], Form2.rgb[2]);
                        Form2.LastValue[0] = 10;
                        Form2.LastValue[1] = 65535;
                        Form2.LastValue[2] = (int)lab[0];
                        Form2.LastValue[3] = Form2.rgb[0];
                        Form2.LastValue[4] = Form2.rgb[1];
                        Form2.LastValue[5] = Form2.rgb[2];
                    }
                    else if (Min == min[2])
                    {
                        textBox1.Text += "CWF";
                        int[] D50_RGB = new int[3];
                        D50_RGB = Look_up_table.D50_table((int)lab[0]);
                        int[] CWF_RGB = new int[3];
                        CWF_RGB = Look_up_table.CWF_table((int)lab[0]);
                        Form2.rgb = Look_up_table.Gap(D50_RGB, CWF_RGB);
                        SetGamma(10, 65535, (int)lab[0], Form2.rgb[0], Form2.rgb[1], Form2.rgb[2]);
                        Form2.LastValue[0] = 10;
                        Form2.LastValue[1] = 65535;
                        Form2.LastValue[2] = (int)lab[0];
                        Form2.LastValue[3] = Form2.rgb[0];
                        Form2.LastValue[4] = Form2.rgb[1];
                        Form2.LastValue[5] = Form2.rgb[2];
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
                                factor = D65;
                                break;
                            case 1:
                                factor = D50;
                                break;
                            case 2:
                                factor = CWF;
                                break;
                            case 3:
                                factor = A;
                                break;
                            default: break;
                        }
                        min[i] = new Distance_two(bitmap).Gray(factor);
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
                        int[] D50_RGB = new int[3];
                        D50_RGB = Look_up_table.D50_table((int)lab[0]);
                        int[] A_RGB = new int[3];
                        A_RGB = Look_up_table.A_table((int)lab[0]);
                        Form2.rgb = Look_up_table.Gap(D50_RGB, A_RGB);
                        SetGamma(10, 65535, (int)lab[0], Form2.rgb[0], Form2.rgb[1], Form2.rgb[2]);
                        Form2.LastValue[0] = 10;
                        Form2.LastValue[1] = 65535;
                        Form2.LastValue[2] = (int)lab[0];
                        Form2.LastValue[3] = Form2.rgb[0];
                        Form2.LastValue[4] = Form2.rgb[1];
                        Form2.LastValue[5] = Form2.rgb[2];


                    }
                    else if (Min == min[1])
                    {
                        textBox1.Text += "D50";
                        Form2.rgb[0] = 0;
                        Form2.rgb[1] = 0;
                        Form2.rgb[2] = 0;
                        SetGamma(10, 65535, (int)lab[0], Form2.rgb[0], Form2.rgb[1], Form2.rgb[2]);
                        Form2.LastValue[0] = 10;
                        Form2.LastValue[1] = 65535;
                        Form2.LastValue[2] = (int)lab[0];
                        Form2.LastValue[3] = Form2.rgb[0];
                        Form2.LastValue[4] = Form2.rgb[1];
                        Form2.LastValue[5] = Form2.rgb[2];
                    }
                    else if (Min == min[0])
                    {
                        textBox1.Text += "D65";
                        int[] D50_RGB = new int[3];
                        D50_RGB = Look_up_table.D50_table((int)lab[0]);
                        int[] D65_RGB = new int[3];
                        D65_RGB = Look_up_table.D65_table((int)lab[0]);
                        Form2.rgb = Look_up_table.Gap(D50_RGB, D65_RGB);
                        SetGamma(10, 65535, (int)lab[0], Form2.rgb[0], Form2.rgb[1], Form2.rgb[2]);
                        Form2.LastValue[0] = 10;
                        Form2.LastValue[1] = 65535;
                        Form2.LastValue[2] = (int)lab[0];
                        Form2.LastValue[3] = Form2.rgb[0];
                        Form2.LastValue[4] = Form2.rgb[1];
                        Form2.LastValue[5] = Form2.rgb[2];
                    }
                    else if (Min == min[2])
                    {
                        textBox1.Text += "CWF";
                        int[] D50_RGB = new int[3];
                        D50_RGB = Look_up_table.D50_table((int)lab[0]);
                        int[] CWF_RGB = new int[3];
                        CWF_RGB = Look_up_table.CWF_table((int)lab[0]);
                        Form2.rgb = Look_up_table.Gap(D50_RGB, CWF_RGB);
                        SetGamma(10, 65535, (int)lab[0], Form2.rgb[0], Form2.rgb[1], Form2.rgb[2]);
                        Form2.LastValue[0] = 10;
                        Form2.LastValue[1] = 65535;
                        Form2.LastValue[2] = (int)lab[0];
                        Form2.LastValue[3] = Form2.rgb[0];
                        Form2.LastValue[4] = Form2.rgb[1];
                        Form2.LastValue[5] = Form2.rgb[2];
                    }

                    video.CloseWebcam();
                   
                }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            new Form2(this).Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new Form3(this).Show();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

            ControlPaint.DrawBorder(e.Graphics,
                                  this.panel1.ClientRectangle,
                                  Color.Chocolate,//7f9db9
                                  1,
                                  ButtonBorderStyle.Solid,
                                  Color.Chocolate,
                                  1,
                                  ButtonBorderStyle.Solid,
                                  Color.Chocolate,
                                  1,
                                  ButtonBorderStyle.Solid,
                                  Color.Chocolate,
                                  1,
                                  ButtonBorderStyle.Solid);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                RegistryKey hklm = Registry.LocalMachine;
                RegistryKey run = hklm.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run");

                try
                {
                    run.SetValue("NewVision UserGraphiC.exe",
                    @"C:\Users\Screen\Desktop\XD\NewVision UserGraphiC\bin\Debug\NewVision UserGraphiC.exe");
                    MessageBox.Show("開機啟動ON", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    hklm.Close();
                }
                catch (Exception)
                {
                }
            }
            else
            {
                RegistryKey hklm = Registry.LocalMachine;
                RegistryKey run = hklm.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run");
                try
                {
                    run.DeleteValue("NewVision UserGraphiC.exe"); //这儿是关键的区别，删除hello.exe这个启动项键值
                    MessageBox.Show("開機啟動OFF", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    hklm.Close();
                }

                catch (Exception my)
                {
                    MessageBox.Show(my.Message.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                pictureBox1.Image = null;
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    FileName = openFileDialog1.FileName;
                    pictureBox1.Image = Image.FromFile(FileName);
                }
            }
            catch
            {
                MessageBox.Show("讀取錯誤");
            }
            textBox1.Text = " ";
            Bitmap bitmap = (Bitmap)pictureBox1.Image;
            avRGB = new CalculationRGB(bitmap).getRGBData();          //判斷彩度
            xyz = new RGB_to_XYZ().RGBtoXYZ(avRGB[0], avRGB[1], avRGB[2]);
            lab = new XYZ_to_LAB().XYZtoLAB(D65, xyz);
        
            if (Math.Sqrt((lab[1]*lab[1])+lab[2]*lab[2])>3)    //色域法
            {
                for (int i = 0; i < 4; i++)
                {
                    double[] type_a = null, type_b = null;
                    switch (i)
                    {
                        case 0:
                            s = D65;
                            type_a = D65_light_A;
                            type_b = D65_light_B;
                            break;
                        case 1:
                            s = D50;
                            type_a = D50_light_A;
                            type_b = D50_light_B;
                            break;
                        case 2:
                            s = CWF;
                            type_a = CWF_light_A;
                            type_b = CWF_light_B;
                            break;
                        case 3:
                            s = A;
                            type_a = A_light_A;
                            type_b = A_light_B;
                            break;
                        default: break;
                    }
                    double[][] abValue = new CalculationRGB(bitmap).get_ab_data(s);
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
                    int[] D50_RGB = new int[3];
                    D50_RGB = Look_up_table.D50_table((int)lab[0]);
                    int[] A_RGB = new int[3];
                    A_RGB = Look_up_table.A_table((int)lab[0]);
                    Form2.rgb = Look_up_table.Gap(D50_RGB, A_RGB);
                    SetGamma(10, 65535, (int)lab[0], Form2.rgb[0], Form2.rgb[1], Form2.rgb[2]);
                }
                else if (Min == min[1])
                {
                    textBox1.Text += "D50";
                    Form2.rgb[0] = 0;
                    Form2.rgb[1] = 0;
                    Form2.rgb[2] = 0;
                    SetGamma(10, 65535, (int)lab[0], Form2.rgb[0], Form2.rgb[1], Form2.rgb[2]);
                }
                else if (Min == min[0])
                {
                    textBox1.Text += "D65";
                    int[] D50_RGB = new int[3];
                    D50_RGB = Look_up_table.D50_table((int)lab[0]);
                    int[] D65_RGB = new int[3];
                    D65_RGB = Look_up_table.D65_table((int)lab[0]);
                    Form2.rgb = Look_up_table.Gap(D50_RGB, D65_RGB);
                    SetGamma(10, 65535, (int)lab[0], Form2.rgb[0], Form2.rgb[1], Form2.rgb[2]);
                }
                else if (Min == min[2])
                {
                    textBox1.Text += "CWF";
                    int[] D50_RGB = new int[3];
                    D50_RGB = Look_up_table.D50_table((int)lab[0]);
                    int[] CWF_RGB = new int[3];
                    CWF_RGB = Look_up_table.CWF_table((int)lab[0]);
                    Form2.rgb = Look_up_table.Gap(D50_RGB, CWF_RGB);
                    SetGamma(10, 65535, (int)lab[0], Form2.rgb[0], Form2.rgb[1], Form2.rgb[2]);
                }

            }
            else  //灰階法
            {
                for (int i = 0; i < 4; i++)
                {
                    switch (i)
                    {
                        case 0:
                            factor = D65;
                            break;
                        case 1:
                            factor = D50;
                            break;
                        case 2:
                            factor = CWF;
                            break;
                        case 3:
                            factor = A;
                            break;
                        default: break;
                    }
                    min[i] = new Distance_two(bitmap).Gray(factor);
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
                    int[] D50_RGB = new int[3];
                    D50_RGB = Look_up_table.D50_table((int)lab[0]);
                    int[] A_RGB = new int[3];
                    A_RGB = Look_up_table.A_table((int)lab[0]);
                    Form2.rgb = Look_up_table.Gap(D50_RGB, A_RGB);
                    SetGamma(10, 65535, (int)lab[0], Form2.rgb[0], Form2.rgb[1], Form2.rgb[2]);


                }
                else if (Min == min[1])
                {
                    textBox1.Text += "D50";
                    Form2.rgb[0] = 0;
                    Form2.rgb[1] = 0;
                    Form2.rgb[2] = 0;
                    SetGamma(10, 65535, (int)lab[0], Form2.rgb[0], Form2.rgb[1], Form2.rgb[2]);
                }
                else if (Min == min[0])
                {
                    textBox1.Text += "D65";
                    int[] D50_RGB = new int[3];
                    D50_RGB = Look_up_table.D50_table((int)lab[0]);
                    int[] D65_RGB = new int[3];
                    D65_RGB = Look_up_table.D65_table((int)lab[0]);
                    Form2.rgb = Look_up_table.Gap(D50_RGB, D65_RGB);
                    SetGamma(10, 65535, (int)lab[0], Form2.rgb[0], Form2.rgb[1], Form2.rgb[2]);
                }
                else if (Min == min[2])
                {
                    textBox1.Text += "CWF";
                    int[] D50_RGB = new int[3];
                    D50_RGB = Look_up_table.D50_table((int)lab[0]);
                    int[] CWF_RGB = new int[3];
                    CWF_RGB = Look_up_table.CWF_table((int)lab[0]);
                    Form2.rgb = Look_up_table.Gap(D50_RGB, CWF_RGB);
                    SetGamma(10, 65535, (int)lab[0], Form2.rgb[0], Form2.rgb[1], Form2.rgb[2]);
                }

            }
        }
        public int GetBrightness1()
        {
            return (int)lab[0];
        }
        private void button7_Click(object sender, EventArgs e)
        {
            textBox2.Text = "  ";
            textBox2.Text += "亮度=  " + lab[0].ToString() + Environment.NewLine;
            for (int i = 1; i < 256; i++)
            {
                textBox2.Text += "Grayd[" + i.ToString() + "]=" + "Red:" + s_ramp.Red[i].ToString() + "        Green:" + s_ramp.Green[i].ToString() + "        Bule:" + s_ramp.Blue[i].ToString();
                textBox2.Text += Environment.NewLine;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                string check = "1" + "\r\n";
                char[] checkCharAry = check.ToCharArray(); // 將 string 轉為 char
                byte[] checkByteAry = new byte[checkCharAry.Length]; // 宣告 byte[] 陣列，長度為 CharAry.Length
                for (int i = 0; i < checkCharAry.Length; i++)
                    checkByteAry[i] = (byte)checkCharAry[i]; // 將 char 轉為 byte
                FileStream fss = new FileStream(@"C:\MyMonitor\LastRecord.txt", FileMode.Append);
                fss.Write(checkByteAry, 0, checkByteAry.Length); // 以 fs.Write() 寫入檔案
                fss.Close();
            }
            else
            {
                string check2 = "0" + "\r\n";
                char[] checkCharAry2 = check2.ToCharArray(); // 將 string 轉為 char
                byte[] checkByteAry2 = new byte[checkCharAry2.Length]; // 宣告 byte[] 陣列，長度為 CharAry.Length
                for (int i = 0; i < checkCharAry2.Length; i++)
                    checkByteAry2[i] = (byte)checkCharAry2[i]; // 將 char 轉為 byte
                FileStream fsss = new FileStream(@"C:\MyMonitor\LastRecord.txt", FileMode.Append);
                fsss.Write(checkByteAry2, 0, checkByteAry2.Length); // 以 fs.Write() 寫入檔案
                fsss.Close();
            }

            string data = Form2.LastValue[0].ToString()
                  + " " + Form2.LastValue[1].ToString()
                  + " " + Form2.LastValue[2].ToString()
                   + " " + Form2.LastValue[3].ToString()
                   + " " + Form2.LastValue[4].ToString()
                    + " " + Form2.LastValue[5].ToString() + "\r\n";
            char[] CharAry = data.ToCharArray(); // 將 string 轉為 char
            byte[] ByteAry = new byte[CharAry.Length]; // 宣告 byte[] 陣列，長度為 CharAry.Length
            for (int i = 0; i < CharAry.Length; i++)
                ByteAry[i] = (byte)CharAry[i]; // 將 char 轉為 byte
            FileStream fs = new FileStream(@"C:\MyMonitor\LastRecord.txt", FileMode.Append);
            fs.Write(ByteAry, 0, ByteAry.Length); // 以 fs.Write() 寫入檔案
            fs.Close();

            
        }   
    }
}
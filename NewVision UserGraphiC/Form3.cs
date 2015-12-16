using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
//using System.Drawing;
namespace NewVision_UserGraphiC
{
    public partial class Form3 : Form
    {
        Form MainForm;
        //public  static Situation[] Sa = new Situation[20];
        //int i = 0;
        double[] aa = new double[130];
        int n = 0;
        public Form3(Form mainform)
        {
            InitializeComponent();
            MainForm = mainform; 
        }

        private void GammaBar_ValueChanged(object sender, EventArgs e)
        {
            Form1.SetGamma(trackBar2.Value,trackBar3.Value, trackBar1.Value, Form2.rgb[0], Form2.rgb[1], Form2.rgb[2]);
            EnterLastvalue(trackBar2.Value, trackBar3.Value, trackBar1.Value, Form2.rgb[0], Form2.rgb[1], Form2.rgb[2]);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (new Form1().GetBrightness1() > 0)
            {
                trackBar1.Value = new Form1().GetBrightness1();
            }
            else if (new Form2().GetBrightness2() > 0)
            {
                trackBar1.Value = new Form2().GetBrightness2();
            }
            else
            {
                trackBar1.Value = 50;
            }

            trackBar2.Value = 10;
            trackBar3.Value = 65535;
         
            Form1.SetGamma(trackBar2.Value, trackBar3.Value, trackBar1.Value,Form2.rgb[0], Form2.rgb[1], Form2.rgb[2]);
            EnterLastvalue(trackBar2.Value, trackBar3.Value, trackBar1.Value, Form2.rgb[0], Form2.rgb[1], Form2.rgb[2]);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            pictureBox5.Image = Image.FromFile("擷取.png");
            trackBar1.Value = 50;
            trackBar2.Value = 10;
            trackBar3.Value = 65535;
            Form1.SetGamma(trackBar2.Value, trackBar3.Value, trackBar1.Value, 0, 0, 0);
            Form2.rgb[0] = 0;
            Form2.rgb[1] = 0;
            Form2.rgb[2] = 0;
        }

        public void button3_Click(object sender, EventArgs e)
        {
            string Name = "Situation";
            if (InputBox.InputBox1("Situation Mode", "New Situation Name:", ref Name) == DialogResult.OK)
            {
                FileStream fs2 = new FileStream(@"C:\MyMonitor\Name.txt", FileMode.Append);
                StreamWriter sw = new StreamWriter(fs2, Encoding.Default);
                comboBox1.Items.Add(Name);
                sw.Write(Name);
                sw.Write("\r\n");
                sw.Close();
                string data = trackBar2.Value.ToString()
                  + " " + trackBar3.Value.ToString()
                  + " " + trackBar1.Value.ToString()
                   + " " + Form2.rgb[0].ToString()
                   + " " + Form2.rgb[1].ToString()
                    + " " + Form2.rgb[2].ToString() + "\r\n";
                char[] CharAry = data.ToCharArray(); // 將 string 轉為 char
                byte[] ByteAry = new byte[CharAry.Length]; // 宣告 byte[] 陣列，長度為 CharAry.Length
                for (int i = 0; i < CharAry.Length; i++)
                    ByteAry[i] = (byte)CharAry[i]; // 將 char 轉為 byte
                FileStream fs = new FileStream(@"C:\MyMonitor\Situation.txt", FileMode.Append);
                fs.Write(ByteAry, 0, ByteAry.Length); // 以 fs.Write() 寫入檔案
                fs.Close();
                aa[(n - 1) * 6] = trackBar2.Value;
                aa[(n - 1) * 6 + 1] = trackBar3.Value;
                aa[(n - 1) * 6 + 2] = trackBar1.Value;
                aa[(n - 1) * 6 + 3] = Form2.rgb[0];
                aa[(n - 1) * 6 + 4] = Form2.rgb[1];
                aa[(n - 1) * 6 + 5] = Form2.rgb[2];
                n++;

            }
            else
            {
                
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox1.SelectedItem.ToString();

            if (comboBox1.Items.IndexOf(comboBox1.Text) == 0)
            {
                ImageGamma._currentBitmap = Image.FromFile("擷取.png");
                ImageGamma.SetGamma2(aa[0], aa[1], aa[2], (aa[3]/255), (aa[4]/255), (aa[5]/255));
                label2.Text = (aa[0]*0.22).ToString();
                label3.Text = (aa[1]/257).ToString();
                label4.Text = aa[2].ToString();
                label5.Text = aa[3].ToString();
                label6.Text = aa[4].ToString();
                label7.Text = aa[5].ToString();
                pictureBox5.Image = ImageGamma._currentBitmap;
                DialogResult result1 = MessageBox.Show("將轉換為預覽圖樣，確定嗎？", "預覽轉換確認", MessageBoxButtons.YesNo);
                if (result1 == DialogResult.Yes)
                {
                    Form1.SetGamma(aa[0],aa[1],aa[2],aa[3],aa[4],aa[5]);
                   pictureBox5.Image = Image.FromFile("擷取.png");
                   EnterLastvalue((int)aa[0], (int)aa[1], (int)aa[2], (int)aa[3], (int)aa[4], (int)aa[5]);
                }
                else
                {
                    label2.Text = "";
                    label3.Text = "";
                    label4.Text = "";
                    label5.Text = "";
                    label6.Text = "";
                    label7.Text = "";
                    pictureBox5.Image = Image.FromFile("擷取.png");
                }
            }
            else if (comboBox1.Items.IndexOf(comboBox1.Text) == 1)
            {
                ImageGamma._currentBitmap = Image.FromFile("擷取.png");
                ImageGamma.SetGamma2(aa[6], aa[7], aa[8], (aa[9] / 255), (aa[10] / 255), (aa[11] / 255));
                label2.Text = (aa[6] * 0.22).ToString();
                label3.Text = (aa[7] / 257).ToString();
                label4.Text = aa[8].ToString();
                label5.Text = aa[9].ToString();
                label6.Text = aa[10].ToString();
                label7.Text = aa[11].ToString();
                pictureBox5.Image = ImageGamma._currentBitmap;
                DialogResult result1 = MessageBox.Show("將轉換為預覽圖樣，確定嗎？", "預覽轉換確認", MessageBoxButtons.YesNo);
                if (result1 == DialogResult.Yes)
                {
                    Form1.SetGamma(aa[6], aa[7], aa[8], aa[9], aa[10], aa[11]);
                    pictureBox5.Image = Image.FromFile("擷取.png");
                    EnterLastvalue((int)aa[6], (int)aa[7], (int)aa[8], (int)aa[9], (int)aa[10], (int)aa[11]);
                }
                else
                {
                    label2.Text = "";
                    label3.Text = "";
                    label4.Text = "";
                    label5.Text = "";
                    label6.Text = "";
                    label7.Text = "";
                    pictureBox5.Image = Image.FromFile("擷取.png");
                }
                
            }
            else if (comboBox1.Items.IndexOf(comboBox1.Text) == 2)
            {
                ImageGamma._currentBitmap = Image.FromFile("擷取.png");
                ImageGamma.SetGamma2(aa[12], aa[13], aa[14], (aa[15] / 255), (aa[16] / 255), (aa[17] / 255));
                label2.Text = (aa[12] * 0.22).ToString();
                label3.Text = (aa[13] / 257).ToString();
                label4.Text = aa[14].ToString();
                label5.Text = aa[15].ToString();
                label6.Text = aa[16].ToString();
                label7.Text = aa[17].ToString();
                pictureBox5.Image = ImageGamma._currentBitmap;
                DialogResult result1 = MessageBox.Show("將轉換為預覽圖樣，確定嗎？", "預覽轉換確認", MessageBoxButtons.YesNo);
                if (result1 == DialogResult.Yes)
                {
                    Form1.SetGamma(aa[12], aa[13], aa[14], aa[15], aa[16], aa[17]);
                    pictureBox5.Image = Image.FromFile("擷取.png");
                    EnterLastvalue((int)aa[12], (int)aa[13], (int)aa[14], (int)aa[15], (int)aa[16], (int)aa[17]);
                }
                else
                {
                    label2.Text = "";
                    label3.Text = "";
                    label4.Text = "";
                    label5.Text = "";
                    label6.Text = "";
                    label7.Text = "";
                    pictureBox5.Image = Image.FromFile("擷取.png");
                }
         
            }
            else if (comboBox1.Items.IndexOf(comboBox1.Text) == 3)
            {
                ImageGamma._currentBitmap = Image.FromFile("擷取.png");
                ImageGamma.SetGamma2(aa[18], aa[19], aa[20], (aa[21] / 255), (aa[22] / 255), (aa[23] / 255));
                label2.Text = (aa[18] * 0.22).ToString();
                label3.Text = (aa[19] / 257).ToString();
                label4.Text = aa[20].ToString();
                label5.Text = aa[21].ToString();
                label6.Text = aa[22].ToString();
                label7.Text = aa[23].ToString();
                pictureBox5.Image = ImageGamma._currentBitmap;
                DialogResult result1 = MessageBox.Show("將轉換為預覽圖樣，確定嗎？", "預覽轉換確認", MessageBoxButtons.YesNo);
                if (result1 == DialogResult.Yes)
                {
                    Form1.SetGamma(aa[18], aa[19], aa[20], aa[21], aa[22], aa[23]);
                    pictureBox5.Image = Image.FromFile("擷取.png");
                    EnterLastvalue((int)aa[18], (int)aa[19], (int)aa[20], (int)aa[21], (int)aa[22], (int)aa[23]);
                }
                else
                {
                    label2.Text = "";
                    label3.Text = "";
                    label4.Text = "";
                    label5.Text = "";
                    label6.Text = "";
                    label7.Text = "";
                    pictureBox5.Image = Image.FromFile("擷取.png");
                } 
            }
            else if (comboBox1.Items.IndexOf(comboBox1.Text) == 4)
            {
                ImageGamma._currentBitmap = Image.FromFile("擷取.png");
                ImageGamma.SetGamma2(aa[24], aa[25], aa[26], (aa[27] / 255), (aa[28] / 255), (aa[29] / 255));
                label2.Text = (aa[24] * 0.22).ToString();
                label3.Text = (aa[25] / 257).ToString();
                label4.Text = aa[26].ToString();
                label5.Text = aa[27].ToString();
                label6.Text = aa[28].ToString();
                label7.Text = aa[29].ToString();
                pictureBox5.Image = ImageGamma._currentBitmap;
                DialogResult result1 = MessageBox.Show("將轉換為預覽圖樣，確定嗎？", "預覽轉換確認", MessageBoxButtons.YesNo);
                if (result1 == DialogResult.Yes)
                {
                    Form1.SetGamma(aa[24], aa[25], aa[26], aa[27], aa[28], aa[29]);
                    pictureBox5.Image = Image.FromFile("擷取.png");
                    EnterLastvalue((int)aa[24], (int)aa[25], (int)aa[26], (int)aa[27], (int)aa[28], (int)aa[29]);
                }
                else
                {
                    label2.Text = "";
                    label3.Text = "";
                    label4.Text = "";
                    label5.Text = "";
                    label6.Text = "";
                    label7.Text = "";
                    pictureBox5.Image = Image.FromFile("擷取.png");
                } 
              
            }
            else if (comboBox1.Items.IndexOf(comboBox1.Text) == 5)
            {
                ImageGamma._currentBitmap = Image.FromFile("擷取.png");
                ImageGamma.SetGamma2(aa[30], aa[31], aa[32], (aa[33] / 255), (aa[34] / 255), (aa[35] / 255));
                label2.Text = (aa[30] * 0.22).ToString();
                label3.Text = (aa[31] / 257).ToString();
                label4.Text = aa[32].ToString();
                label5.Text = aa[33].ToString();
                label6.Text = aa[34].ToString();
                label7.Text = aa[35].ToString();
                pictureBox5.Image = ImageGamma._currentBitmap;
                DialogResult result1 = MessageBox.Show("將轉換為預覽圖樣，確定嗎？", "預覽轉換確認", MessageBoxButtons.YesNo);
                if (result1 == DialogResult.Yes)
                {
                    Form1.SetGamma(aa[30], aa[31], aa[32], aa[33], aa[34], aa[35]);
                    pictureBox5.Image = Image.FromFile("擷取.png");
                    EnterLastvalue((int)aa[30], (int)aa[31], (int)aa[32], (int)aa[33], (int)aa[34], (int)aa[35]);
                }
                else
                {
                    label2.Text = "";
                    label3.Text = "";
                    label4.Text = "";
                    label5.Text = "";
                    label6.Text = "";
                    label7.Text = "";
                    pictureBox5.Image = Image.FromFile("擷取.png");
                } 
            }
            else if (comboBox1.Items.IndexOf(comboBox1.Text) == 6)
            {
                ImageGamma._currentBitmap = Image.FromFile("擷取.png");
                ImageGamma.SetGamma2(aa[36], aa[37], aa[38], (aa[39] / 255), (aa[40] / 255), (aa[41] / 255));
                label2.Text = (aa[36] * 0.22).ToString();
                label3.Text = (aa[37] / 257).ToString();
                label4.Text = aa[38].ToString();
                label5.Text = aa[39].ToString();
                label6.Text = aa[40].ToString();
                label7.Text = aa[41].ToString();
                pictureBox5.Image = ImageGamma._currentBitmap;
                DialogResult result1 = MessageBox.Show("將轉換為預覽圖樣，確定嗎？", "預覽轉換確認", MessageBoxButtons.YesNo);
                if (result1 == DialogResult.Yes)
                {
                    Form1.SetGamma(aa[36], aa[37], aa[38], aa[39], aa[40], aa[41]);
                    pictureBox5.Image = Image.FromFile("擷取.png");
                    EnterLastvalue((int)aa[36], (int)aa[37], (int)aa[38], (int)aa[39], (int)aa[40], (int)aa[41]);
                }
                else
                {
                    label2.Text = "";
                    label3.Text = "";
                    label4.Text = "";
                    label5.Text = "";
                    label6.Text = "";
                    label7.Text = "";
                    pictureBox5.Image = Image.FromFile("擷取.png");
                } 
            }
            else if (comboBox1.Items.IndexOf(comboBox1.Text) == 7)
            {
                ImageGamma._currentBitmap = Image.FromFile("擷取.png");
                ImageGamma.SetGamma2(aa[42], aa[43], aa[44], (aa[45] / 255), (aa[46] / 255), (aa[47] / 255));
                label2.Text = (aa[42] * 0.22).ToString();
                label3.Text = (aa[43] / 257).ToString();
                label4.Text = aa[44].ToString();
                label5.Text = aa[45].ToString();
                label6.Text = aa[46].ToString();
                label7.Text = aa[47].ToString();
                pictureBox5.Image = ImageGamma._currentBitmap;
                DialogResult result1 = MessageBox.Show("將轉換為預覽圖樣，確定嗎？", "預覽轉換確認", MessageBoxButtons.YesNo);
                if (result1 == DialogResult.Yes)
                {
                    Form1.SetGamma(aa[42], aa[43], aa[44], aa[45], aa[46], aa[47]);
                    pictureBox5.Image = Image.FromFile("擷取.png");
                    EnterLastvalue((int)aa[42], (int)aa[43], (int)aa[44], (int)aa[45], (int)aa[46], (int)aa[47]);
                }
                else
                {
                    label2.Text = "";
                    label3.Text = "";
                    label4.Text = "";
                    label5.Text = "";
                    label6.Text = "";
                    label7.Text = "";
                    pictureBox5.Image = Image.FromFile("擷取.png");
                }
            }
            else if (comboBox1.Items.IndexOf(comboBox1.Text) == 8)
            {
                ImageGamma._currentBitmap = Image.FromFile("擷取.png");
                ImageGamma.SetGamma2(aa[48], aa[49], aa[50], (aa[51] / 255), (aa[52] / 255), (aa[53] / 255));
                label2.Text = (aa[48] * 0.22).ToString();
                label3.Text = (aa[49] / 257).ToString();
                label4.Text = aa[50].ToString();
                label5.Text = aa[51].ToString();
                label6.Text = aa[52].ToString();
                label7.Text = aa[53].ToString();
                pictureBox5.Image = ImageGamma._currentBitmap;
                DialogResult result1 = MessageBox.Show("將轉換為預覽圖樣，確定嗎？", "預覽轉換確認", MessageBoxButtons.YesNo);
                if (result1 == DialogResult.Yes)
                {
                    Form1.SetGamma(aa[48], aa[49], aa[50], aa[51], aa[52], aa[53]);
                    pictureBox5.Image = Image.FromFile("擷取.png");
                    EnterLastvalue((int)aa[48], (int)aa[49], (int)aa[50], (int)aa[51], (int)aa[52], (int)aa[53]);
                }
                else
                {
                    label2.Text = "";
                    label3.Text = "";
                    label4.Text = "";
                    label5.Text = "";
                    label6.Text = "";
                    label7.Text = "";
                    pictureBox5.Image = Image.FromFile("擷取.png");
                }
                
            }
            else if (comboBox1.Items.IndexOf(comboBox1.Text) == 9)
            {
                ImageGamma._currentBitmap = Image.FromFile("擷取.png");
                ImageGamma.SetGamma2(aa[54], aa[55], aa[56], (aa[57] / 255), (aa[58] / 255), (aa[59] / 255));
                label2.Text = (aa[54] * 0.22).ToString();
                label3.Text = (aa[55] / 257).ToString();
                label4.Text = aa[56].ToString();
                label5.Text = aa[57].ToString();
                label6.Text = aa[58].ToString();
                label7.Text = aa[59].ToString();
                pictureBox5.Image = ImageGamma._currentBitmap;
                DialogResult result1 = MessageBox.Show("將轉換為預覽圖樣，確定嗎？", "預覽轉換確認", MessageBoxButtons.YesNo);
                if (result1 == DialogResult.Yes)
                {
                    Form1.SetGamma(aa[54], aa[55], aa[56], aa[57], aa[58], aa[59]);
                    pictureBox5.Image = Image.FromFile("擷取.png");
                    EnterLastvalue((int)aa[54], (int)aa[55], (int)aa[56], (int)aa[57], (int)aa[58], (int)aa[59]);
                }
                else
                {
                    label2.Text = "";
                    label3.Text = "";
                    label4.Text = "";
                    label5.Text = "";
                    label6.Text = "";
                    label7.Text = "";
                    pictureBox5.Image = Image.FromFile("擷取.png");
                }  
            }
            else if (comboBox1.Items.IndexOf(comboBox1.Text) == 10)
            {
                ImageGamma._currentBitmap = Image.FromFile("擷取.png");
                ImageGamma.SetGamma2(aa[60], aa[61], aa[62], (aa[63] / 255), (aa[64] / 255), (aa[65] / 255));
                label2.Text = (aa[60] * 0.22).ToString();
                label3.Text = (aa[61] / 257).ToString();
                label4.Text = aa[62].ToString();
                label5.Text = aa[63].ToString();
                label6.Text = aa[64].ToString();
                label7.Text = aa[65].ToString();
                pictureBox5.Image = ImageGamma._currentBitmap;
                DialogResult result1 = MessageBox.Show("將轉換為預覽圖樣，確定嗎？", "預覽轉換確認", MessageBoxButtons.YesNo);
                if (result1 == DialogResult.Yes)
                {
                    Form1.SetGamma(aa[60], aa[61], aa[62], aa[63], aa[64], aa[65]);
                    pictureBox5.Image = Image.FromFile("擷取.png");
                    EnterLastvalue((int)aa[60], (int)aa[61], (int)aa[62], (int)aa[63], (int)aa[64], (int)aa[65]);
                }
                else
                {
                    label2.Text = "";
                    label3.Text = "";
                    label4.Text = "";
                    label5.Text = "";
                    label6.Text = "";
                    label7.Text = "";
                    pictureBox5.Image = Image.FromFile("擷取.png");
                }          
            }
            else if (comboBox1.Items.IndexOf(comboBox1.Text) == 11)
            {
                ImageGamma._currentBitmap = Image.FromFile("擷取.png");
                ImageGamma.SetGamma2(aa[66], aa[67], aa[68], (aa[69] / 255), (aa[70] / 255), (aa[71] / 255));
                label2.Text = (aa[66] * 0.22).ToString();
                label3.Text = (aa[67] / 257).ToString();
                label4.Text = aa[68].ToString();
                label5.Text = aa[69].ToString();
                label6.Text = aa[70].ToString();
                label7.Text = aa[71].ToString();
                pictureBox5.Image = ImageGamma._currentBitmap;
                DialogResult result1 = MessageBox.Show("將轉換為預覽圖樣，確定嗎？", "預覽轉換確認", MessageBoxButtons.YesNo);
                if (result1 == DialogResult.Yes)
                {
                    Form1.SetGamma(aa[66], aa[67], aa[68], aa[69], aa[70], aa[71]);
                    pictureBox5.Image = Image.FromFile("擷取.png");
                    EnterLastvalue((int)aa[66], (int)aa[67], (int)aa[68], (int)aa[69], (int)aa[70], (int)aa[71]);
                }
                else
                {
                    label2.Text = "";
                    label3.Text = "";
                    label4.Text = "";
                    label5.Text = "";
                    label6.Text = "";
                    label7.Text = "";
                    pictureBox5.Image = Image.FromFile("擷取.png");
                }                
            }
            else if (comboBox1.Items.IndexOf(comboBox1.Text) == 12)
            {
                ImageGamma._currentBitmap = Image.FromFile("擷取.png");
                ImageGamma.SetGamma2(aa[72], aa[73], aa[74], (aa[75] / 255), (aa[76] / 255), (aa[77] / 255));
                label2.Text = (aa[72] * 0.22).ToString();
                label3.Text = (aa[73] / 257).ToString();
                label4.Text = aa[74].ToString();
                label5.Text = aa[75].ToString();
                label6.Text = aa[76].ToString();
                label7.Text = aa[77].ToString();
                pictureBox5.Image = ImageGamma._currentBitmap;
                DialogResult result1 = MessageBox.Show("將轉換為預覽圖樣，確定嗎？", "預覽轉換確認", MessageBoxButtons.YesNo);
                if (result1 == DialogResult.Yes)
                {
                    Form1.SetGamma(aa[72], aa[73], aa[74], aa[75], aa[76], aa[77]);
                    pictureBox5.Image = Image.FromFile("擷取.png");
                    EnterLastvalue((int)aa[72], (int)aa[73], (int)aa[74], (int)aa[75], (int)aa[76], (int)aa[77]);
                }
                else
                {
                    label2.Text = "";
                    label3.Text = "";
                    label4.Text = "";
                    label5.Text = "";
                    label6.Text = "";
                    label7.Text = "";
                    pictureBox5.Image = Image.FromFile("擷取.png");
                }             
            }
            else if (comboBox1.Items.IndexOf(comboBox1.Text) == 13)
            {
                ImageGamma._currentBitmap = Image.FromFile("擷取.png");
                ImageGamma.SetGamma2(aa[78], aa[79], aa[80], (aa[81] / 255), (aa[82] / 255), (aa[83] / 255));
                label2.Text = (aa[78] * 0.22).ToString();
                label3.Text = (aa[79] / 257).ToString();
                label4.Text = aa[80].ToString();
                label5.Text = aa[81].ToString();
                label6.Text = aa[82].ToString();
                label7.Text = aa[83].ToString();
                pictureBox5.Image = ImageGamma._currentBitmap;
                DialogResult result1 = MessageBox.Show("將轉換為預覽圖樣，確定嗎？", "預覽轉換確認", MessageBoxButtons.YesNo);
                if (result1 == DialogResult.Yes)
                {
                    Form1.SetGamma(aa[78], aa[79], aa[80], aa[81], aa[82], aa[83]);
                    pictureBox5.Image = Image.FromFile("擷取.png");
                    EnterLastvalue((int)aa[78], (int)aa[79], (int)aa[80], (int)aa[81], (int)aa[82], (int)aa[83]);
                }
                else
                {
                    label2.Text = "";
                    label3.Text = "";
                    label4.Text = "";
                    label5.Text = "";
                    label6.Text = "";
                    label7.Text = "";
                    pictureBox5.Image = Image.FromFile("擷取.png");
                }          
            }
            else if (comboBox1.Items.IndexOf(comboBox1.Text) == 14)
            {
                ImageGamma._currentBitmap = Image.FromFile("擷取.png");
                ImageGamma.SetGamma2(aa[84], aa[85], aa[86], (aa[87] / 255), (aa[88] / 255), (aa[89] / 255));
                label2.Text = (aa[84] * 0.22).ToString();
                label3.Text = (aa[85] / 257).ToString();
                label4.Text = aa[86].ToString();
                label5.Text = aa[87].ToString();
                label6.Text = aa[88].ToString();
                label7.Text = aa[89].ToString();
                pictureBox5.Image = ImageGamma._currentBitmap;
                DialogResult result1 = MessageBox.Show("將轉換為預覽圖樣，確定嗎？", "預覽轉換確認", MessageBoxButtons.YesNo);
                if (result1 == DialogResult.Yes)
                {
                    Form1.SetGamma(aa[84], aa[85], aa[86], aa[87], aa[88], aa[89]);
                    pictureBox5.Image = Image.FromFile("擷取.png");
                    EnterLastvalue((int)aa[0], (int)aa[85], (int)aa[86], (int)aa[87], (int)aa[88], (int)aa[89]);
                }
                else
                {
                    label2.Text = "";
                    label3.Text = "";
                    label4.Text = "";
                    label5.Text = "";
                    label6.Text = "";
                    label7.Text = "";
                    pictureBox5.Image = Image.FromFile("擷取.png");
                }         
            }
            else if (comboBox1.Items.IndexOf(comboBox1.Text) == 15)
            {
                ImageGamma._currentBitmap = Image.FromFile("擷取.png");
                ImageGamma.SetGamma2(aa[90], aa[91], aa[92], (aa[93] / 255), (aa[94] / 255), (aa[95] / 255));
                label2.Text = (aa[90] * 0.22).ToString();
                label3.Text = (aa[91] / 257).ToString();
                label4.Text = aa[92].ToString();
                label5.Text = aa[93].ToString();
                label6.Text = aa[94].ToString();
                label7.Text = aa[95].ToString();
                pictureBox5.Image = ImageGamma._currentBitmap;
                DialogResult result1 = MessageBox.Show("將轉換為預覽圖樣，確定嗎？", "預覽轉換確認", MessageBoxButtons.YesNo);
                if (result1 == DialogResult.Yes)
                {
                    Form1.SetGamma(aa[90], aa[91], aa[92], aa[93], aa[94], aa[95]);
                    pictureBox5.Image = Image.FromFile("擷取.png");
                    EnterLastvalue((int)aa[90], (int)aa[91], (int)aa[92], (int)aa[93], (int)aa[94], (int)aa[95]);
                }
                else
                {
                    label2.Text = "";
                    label3.Text = "";
                    label4.Text = "";
                    label5.Text = "";
                    label6.Text = "";
                    label7.Text = "";
                    pictureBox5.Image = Image.FromFile("擷取.png");
                }           
            }
            else if (comboBox1.Items.IndexOf(comboBox1.Text) == 16)
            {
                ImageGamma._currentBitmap = Image.FromFile("擷取.png");
                ImageGamma.SetGamma2(aa[96], aa[97], aa[98], (aa[99] / 255), (aa[100] / 255), (aa[101] / 255));
                label2.Text = (aa[96] * 0.22).ToString();
                label3.Text = (aa[97] / 257).ToString();
                label4.Text = aa[98].ToString();
                label5.Text = aa[99].ToString();
                label6.Text = aa[100].ToString();
                label7.Text = aa[101].ToString();
                pictureBox5.Image = ImageGamma._currentBitmap;
                DialogResult result1 = MessageBox.Show("將轉換為預覽圖樣，確定嗎？", "預覽轉換確認", MessageBoxButtons.YesNo);
                if (result1 == DialogResult.Yes)
                {
                    Form1.SetGamma(aa[96], aa[97], aa[98], aa[99], aa[100], aa[101]);
                    pictureBox5.Image = Image.FromFile("擷取.png");
                    EnterLastvalue((int)aa[96], (int)aa[97], (int)aa[98], (int)aa[99], (int)aa[100], (int)aa[101]);
                }
                else
                {
                    label2.Text = "";
                    label3.Text = "";
                    label4.Text = "";
                    label5.Text = "";
                    label6.Text = "";
                    label7.Text = "";
                    pictureBox5.Image = Image.FromFile("擷取.png");
                }               
            }
            else if (comboBox1.Items.IndexOf(comboBox1.Text) == 17)
            {
                ImageGamma._currentBitmap = Image.FromFile("擷取.png");
                ImageGamma.SetGamma2(aa[102], aa[103], aa[104], (aa[105] / 255), (aa[106] / 255), (aa[107] / 255));
                label2.Text = (aa[102] * 0.22).ToString();
                label3.Text = (aa[103] / 257).ToString();
                label4.Text = aa[104].ToString();
                label5.Text = aa[105].ToString();
                label6.Text = aa[106].ToString();
                label7.Text = aa[107].ToString();
                pictureBox5.Image = ImageGamma._currentBitmap;
                DialogResult result1 = MessageBox.Show("將轉換為預覽圖樣，確定嗎？", "預覽轉換確認", MessageBoxButtons.YesNo);
                if (result1 == DialogResult.Yes)
                {
                    Form1.SetGamma(aa[102], aa[103], aa[104], aa[105], aa[106], aa[107]);
                    pictureBox5.Image = Image.FromFile("擷取.png");
                    EnterLastvalue((int)aa[0], (int)aa[103], (int)aa[104], (int)aa[105], (int)aa[106], (int)aa[107]);
                }
                else
                {
                    label2.Text = "";
                    label3.Text = "";
                    label4.Text = "";
                    label5.Text = "";
                    label6.Text = "";
                    label7.Text = "";
                    pictureBox5.Image = Image.FromFile("擷取.png");
                }        
            }
            else if (comboBox1.Items.IndexOf(comboBox1.Text) == 18)
            {
                ImageGamma._currentBitmap = Image.FromFile("擷取.png");
                ImageGamma.SetGamma2(aa[108], aa[109], aa[110], (aa[111] / 255), (aa[112] / 255), (aa[113] / 255));
                label2.Text = (aa[108] * 0.22).ToString();
                label3.Text = (aa[109] / 257).ToString();
                label4.Text = aa[110].ToString();
                label5.Text = aa[111].ToString();
                label6.Text = aa[112].ToString();
                label7.Text = aa[113].ToString();
                pictureBox5.Image = ImageGamma._currentBitmap;
                DialogResult result1 = MessageBox.Show("將轉換為預覽圖樣，確定嗎？", "預覽轉換確認", MessageBoxButtons.YesNo);
                if (result1 == DialogResult.Yes)
                {
                    Form1.SetGamma(aa[108], aa[109], aa[110], aa[111], aa[112], aa[113]);
                    pictureBox5.Image = Image.FromFile("擷取.png");
                    EnterLastvalue((int)aa[108], (int)aa[109], (int)aa[110], (int)aa[111], (int)aa[112], (int)aa[113]);
                }
                else
                {
                    label2.Text = "";
                    label3.Text = "";
                    label4.Text = "";
                    label5.Text = "";
                    label6.Text = "";
                    label7.Text = "";
                    pictureBox5.Image = Image.FromFile("擷取.png");
                }  
            }
            else if (comboBox1.Items.IndexOf(comboBox1.Text) == 19)
            {
                ImageGamma._currentBitmap = Image.FromFile("擷取.png");
                ImageGamma.SetGamma2(aa[114], aa[115], aa[116], (aa[117] / 255), (aa[118] / 255), (aa[119] / 255));
                label2.Text = (aa[114] * 0.22).ToString();
                label3.Text = (aa[115] / 257).ToString();
                label4.Text = aa[116].ToString();
                label5.Text = aa[117].ToString();
                label6.Text = aa[118].ToString();
                label7.Text = aa[119].ToString();
                pictureBox5.Image = ImageGamma._currentBitmap;
                DialogResult result1 = MessageBox.Show("將轉換為預覽圖樣，確定嗎？", "預覽轉換確認", MessageBoxButtons.YesNo);
                if (result1 == DialogResult.Yes)
                {
                    Form1.SetGamma(aa[114], aa[115], aa[116], aa[117], aa[118], aa[119]);
                    pictureBox5.Image = Image.FromFile("擷取.png");
                    EnterLastvalue((int)aa[114], (int)aa[115], (int)aa[116], (int)aa[117], (int)aa[118], (int)aa[119]);
                }
                else
                {
                    label2.Text = "";
                    label3.Text = "";
                    label4.Text = "";
                    label5.Text = "";
                    label6.Text = "";
                    label7.Text = "";
                    pictureBox5.Image = Image.FromFile("擷取.png");
                }        
            }
            else
            {
                Form1.SetGamma(10, 65535, 50, 0, 0, 0);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            StreamReader sr = new StreamReader(@"C:\MyMonitor\Situation.txt", System.Text.Encoding.Default);
            //int n = 0; // 記錄共有多少個字串
            //string[] word = new string[100]; // 字串陣列
            textBox1.Text = "";      // 顯示原始文字內容
            while (sr.Peek() != -1) //判斷是否已經讀取最後
            {
                string currentRow = sr.ReadLine();  //讀取一列 
                string[] tempword = currentRow.Trim().Split(' '); // 將字串以空白字元分解
                textBox2.Text=tempword.Length.ToString();
                textBox1.Text += currentRow + "\r\n"; // 顯示原始文字內容
            }
            sr.Close();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            situation();
        }
        private void situation()
        {
            StreamReader sr2 = null;
            try
            {
                sr2 = new StreamReader(@"C:\MyMonitor\Name.txt", System.Text.Encoding.Default);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Open file error!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            try
            {
                while (sr2.Peek() != -1) //判斷是否已經讀取最後
                {
                    string currentRow2 = sr2.ReadLine();  //讀取一列 

                    comboBox1.Items.Add(currentRow2);
                }
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show(ex.Message, "End of file", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            StreamReader sr = null;
            try
            {
                sr = new StreamReader(@"C:\MyMonitor\Situation.txt", System.Text.Encoding.Default);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Open file error!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            int count = 0;
            textBox1.Text = "";      // 顯示原始文字內容
            try
            {
                while (sr.Peek() != -1) //判斷是否已經讀取最後
                {

                    string currentRow = sr.ReadLine();  //讀取一列 
                    string[] tempword = currentRow.Trim().Split(' '); // 將字串以空白字元分解
                    //comboBox1.Items.Add("情境" + "(" + (n + 1) + ")");
                    aa[count] = Convert.ToInt32(tempword[0]);
                    aa[count + 1] = Convert.ToInt32(tempword[1]);
                    aa[count + 2] = Convert.ToInt32(tempword[2]);
                    aa[count + 3] = Convert.ToInt32(tempword[3]);
                    aa[count + 4] = Convert.ToInt32(tempword[4]);
                    aa[count + 5] = Convert.ToInt32(tempword[5]);
                    textBox1.Text += currentRow + "\r\n"; // 顯示原始文字內容
                    n++;
                    count = count + 6;


                }
                textBox2.Text = n.ToString();
                sr.Close();
                sr2.Close();
                n = n + 1;
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show(ex.Message, "End of file", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            
            
        }
        private static void EnterLastvalue(int a,int b,int c,int d,int e,int f)
        {
            Form2.LastValue[0] = a;
            Form2.LastValue[1] = b;
            Form2.LastValue[2] = c;
            Form2.LastValue[3] = d;
            Form2.LastValue[4] = e;
            Form2.LastValue[5] = f;
        }
        private void Form3_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        
    }
    

}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace MorphologicalImageProcessing
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        int[,] maskIrosion;
        private void allButtonNotEnabled()
        {
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;
            button6.Enabled = false;
        }
        private void allButtonEnabled()
        {
            Invoke(new Action(() =>
            {
                button1.Enabled = true;
                button2.Enabled = true;
                button3.Enabled = true;
                button4.Enabled = true;
                button5.Enabled = true;
                button6.Enabled = true;
            }));
        }
        private void button1_Click(object sender, EventArgs e)
        {
            allButtonNotEnabled();
            Thread t = new Thread(Do_Erosion);
            t.Start();
        }
        private void Do_Erosion()
        {
            intiMask();
            picIrosionResolt.Image = erosion(picIrosionOrig.Image as Bitmap).Clone() as Bitmap;
            MessageBox.Show("Erosion Done!");
            allButtonEnabled();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            allButtonNotEnabled();
            Thread t = new Thread(Do_Delation);
            t.Start();
        }
        private void Do_Delation()
        {
            intiMask();
            picIrosionResolt.Image = delation(picIrosionOrig.Image as Bitmap).Clone() as Bitmap;
            MessageBox.Show("Delation Done!");
            allButtonEnabled();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            allButtonNotEnabled();
            Thread t = new Thread(DO_Opening);
            t.Start();
        }
        private void DO_Opening()
        {
            intiMask();
            Bitmap b1 = erosion(picIrosionOrig.Image as Bitmap);
            picIrosionResolt.Image = delation(b1);
            MessageBox.Show("Opening Done!");
            allButtonEnabled();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            allButtonNotEnabled();
            Thread t = new Thread(DO_Closing);
            t.Start();
        }
        private void DO_Closing()
        {
            intiMask();
            Bitmap b1 = delation(picIrosionOrig.Image as Bitmap);
            picIrosionResolt.Image = erosion(b1);
            MessageBox.Show("Closing Done!");
            allButtonEnabled();
        }
        private void button6_Click(object sender, EventArgs e)
        {
            //allButtonNotEnabled();
            //Thread t = new Thread(Do_Thinning);
            //t.Start();
            Do_Thinning();
        }
        private void Do_Thinning()
        {
            picIrosionResolt.Image = thinning(picIrosionOrig.Image as Bitmap);
            MessageBox.Show("Thinning Done!");
            allButtonEnabled();
        }

        private void intiMask()
        {
            picIrosionResolt.Image = new Bitmap(picIrosionOrig.Image.Width, picIrosionOrig.Image.Height);
            if (rdMask1.Checked)
                maskIrosion = new int[3, 3] { { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 } };
            else if (rdMask2.Checked)
                maskIrosion = new int[2, 2] { { 0, 0 }, { 0, 0 } };
            else if (rdMask3.Checked)
                maskIrosion = new int[3, 1] { { 0 }, { 0 }, { 0 } };
            else if (rdMask4.Checked)
                maskIrosion = new int[1, 3] { { 0, 0, 0 } };
        }

        private Bitmap fillWhiteColor(Bitmap b)
        {
            for (int i = 0; i < b.Width; i++)
            {
                for (int j = 0; j < b.Height; j++)
                {
                    b.SetPixel(i, j, Color.White);
                }
            }
            return b;
        }

        private Bitmap erosion(Bitmap b)
        {
            Bitmap im = b;
            Bitmap imResolt = fillWhiteColor(new Bitmap(b.Width, b.Height)); //picIrosionOrig.Image.Clone() as Bitmap;
            if (chkDraw.Checked)
                picIrosionResolt.Image = imResolt;
            for (int i = 0; i < im.Width - maskIrosion.GetLength(1); i++)
            {
                for (int j = 0; j < im.Height - maskIrosion.GetLength(0); j++)
                {
                    int sum = 0;
                    for (int m = 0; m < maskIrosion.GetLength(0); m++)
                    {
                        for (int n = 0; n < maskIrosion.GetLength(1); n++)
                        {

                            int x = im.GetPixel(i + n, j + m).R;
                            int y = maskIrosion[m, n];
                            if (x == y)
                                sum++;
                            if (sum == maskIrosion.GetLength(0) * maskIrosion.GetLength(1))
                            {
                                imResolt.SetPixel(i + n, j + m, Color.Black);

                                Invoke(new Action(() =>
                                {
                                    if (chkDraw.Checked)
                                        picIrosionResolt.Refresh();
                                }));
                            }
                        }
                    }
                }
            }
            return imResolt;
        }

        private Bitmap delation(Bitmap b)
        {
            //MessageBox.Show(maskIrosion.GetLength(0).ToString());
            // MessageBox.Show(maskIrosion.GetLength(1).ToString());
            Bitmap im = b;
            Bitmap imResolt = fillWhiteColor(new Bitmap(b.Width, b.Height)); //picIrosionOrig.Image.Clone() as Bitmap;
            if (chkDraw.Checked)
                picIrosionResolt.Image = imResolt;
            for (int i = 0; i < im.Width - maskIrosion.GetLength(1); i++)
            {
                for (int j = 0; j < im.Height - maskIrosion.GetLength(0); j++)
                {
                    int sum = 0;
                    for (int m = 0; m < maskIrosion.GetLength(0); m++)
                    {
                        for (int n = 0; n < maskIrosion.GetLength(1); n++)
                        {

                            int x = im.GetPixel(i + n, j + m).R;
                            int y = maskIrosion[m, n];
                            if (x == y)
                                sum++;
                            if (sum > 0)
                            {
                                imResolt.SetPixel(i + n, j + m, Color.Black);
                                Invoke(new Action(() =>
                                {
                                    if (chkDraw.Checked)
                                        picIrosionResolt.Refresh();
                                }));
                            }

                        }
                    }

                    //else
                    //    imResolt.SetPixel(i + 1, j + 1, Color.White);
                }
            }
            return imResolt;
        }

        private Bitmap thinning(Bitmap b)
        {
            List<int[,]> mask = new List<int[,]>();
            int[,] x0 = { { 255, 255, 255 }, { -1, 0, -1 }, { 0, 0, 0 } };
            mask.Add(x0);
            int[,] x1 = { { -1, 255, 255 }, { 0, 0, 255 }, { 0, 0, -1 } };
            mask.Add(x1);
            int[,] x2 = { { 0, -1, 255 }, { 0, 0, 255 }, { 0, -1, 255 } };
            mask.Add(x2);
            int[,] x3 = { { 0, 0, -1 }, { 0, 0, 255 }, { -1, 255, 255 } };
            mask.Add(x3);
            int[,] x4 = { { 0, 0, 0 }, { -1, 0, -1 }, { 255, 255, 255 } };
            mask.Add(x4);
            int[,] x5 = { { -1, 0, 0 }, { 255, 0, 0 }, { 255, 255, -1 } };
            mask.Add(x5);
            int[,] x6 = { { 255, -1, 0 }, { 255, 0, 0 }, { 255, -1, 0 } };
            mask.Add(x6);
            int[,] x7 = { { 255, 255, -1 }, { 255, 0, 0 }, { -1, 0, 0 } };
            mask.Add(x7);


            Bitmap im = b;
            Bitmap imResolt = im.Clone() as Bitmap; //new Bitmap(im.Width, im.Height);
            if (chkDraw.Checked)
                picIrosionResolt.Image = imResolt;
            bool isChange = true;
            while (isChange)
            {
                isChange = false;
                for (int k = 0; k < mask.Count; k++)
                {

                    for (int j = 0; j < im.Height - 3; j++)
                    {
                        for (int i = 0; i < im.Width - 3; i++)
                        {
                            int sum = 0;
                            for (int m = 0; m < mask[k].GetLength(0); m++)
                            {
                                for (int n = 0; n < mask[k].GetLength(1); n++)
                                {
                                    int x = im.GetPixel(i + n, j + m).R;
                                    int y = mask[k][m, n];
                                    if (y == -1)
                                        sum++;
                                    else if (x == y)
                                        sum++;
                                    if (sum == 9)
                                    {
                                        imResolt.SetPixel(i + 1, j + 1, Color.White);
                                        isChange = true;
                                        //Invoke(new Action(() =>
                                        //{
                                        if (chkDraw.Checked)
                                            picIrosionResolt.Refresh();
                                        //}));
                                    }
                                }
                            }
                        }
                    }
                    //Invoke(new Action(() =>
                    //{
                    picIrosionResolt.Refresh();
                    //}));
                    im = imResolt.Clone() as Bitmap;

                }
            }
            //imResolt.Save(@"./IP/result.bmp");
            return imResolt;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Bitmap files (*.bmp)|*.bmp|PNG files (*.png)|*.png|TIFF files (*.tif)|*tif|JPEG files (*.jpg)|*.jpg |All files (*.*)|*.*";
            ofd.FilterIndex = 5;
            ofd.RestoreDirectory = true;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    picIrosionOrig.Image = (Bitmap)Bitmap.FromFile(ofd.FileName);
                }
                catch (ApplicationException ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }

    }
}

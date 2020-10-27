using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace MyNeiroWeb
{
    public partial class Form1 : Form
    {
        const string F = "letters.txt";
        private NeiroWeb neiro;
        private Point p;
        private int[,] arr;
          
        public Form1()
        {
            InitializeComponent();
            radioButton1.Checked = true;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            NeiroGraphUtils.ClearImage(pictureBox1);
            neiro = new NeiroWeb();
            textBox1.Text = "";
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            neiro.SaveJson();
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point endP = new Point(e.X, e.Y);
                Bitmap image = (Bitmap)pictureBox1.Image;
                using (Graphics g = Graphics.FromImage(image))
                {
                    g.DrawLine(new Pen(Color.Red), p, endP);
                }
                pictureBox1.Image = image;
                p = endP;
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            p = new Point(e.X, e.Y);
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            string ls = "";
            foreach (var c in neiro.GetLettersFromList()) ls += c + " ";
            MessageBox.Show(ls);
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            ClearAll();
        }

        private void RadioButton2_CheckedChanged(object sender, EventArgs e)
        {
            button2.Visible = true;
            textBox2.Visible = true;
        }

        private void RadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            button2.Visible = false;
            textBox2.Visible = false;
            label2.Visible = false;
        }

        private void TextBox2_Click(object sender, EventArgs e)
        {
            textBox2.Text = "";
            textBox2.ForeColor = Color.Black;
        }
        private void TextBox2_Leave(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
            {
                textBox2.Text = "Введіть ваш символ...";
                textBox2.ForeColor = Color.Silver;
            }
        }
        private void Button2_Click(object sender, EventArgs e)
        {
            Learn();
        }
        private void Learn()
        {
            int[,] clipArr = NeiroGraphUtils.CutImageToArray((Bitmap)pictureBox1.Image, new Point(pictureBox1.Width, pictureBox1.Height));
            if (clipArr == null) return;
            arr = NeiroGraphUtils.LeadArray(clipArr, new int[NeiroWeb.neironInArrayWidth, NeiroWeb.neironInArrayHeight]);
            pictureBox2.Image = new Bitmap(NeiroGraphUtils.GetBitmapFromArr(clipArr), new Size(pictureBox2.Width, pictureBox2.Height)); ;
            pictureBox3.Image = new Bitmap(NeiroGraphUtils.GetBitmapFromArr(arr), new Size(pictureBox3.Width, pictureBox3.Height));
            if (textBox2.Text == " " || textBox2.Text == "Введіть ваш символ...")
            {
                label2.Text = "Введіть коррекний символ";
                label2.Visible = true;
                return;
            }
            else
            {
                label2.Text = "Символ " + "'" + textBox2.Text + "'" + " збережено!";
                label2.Visible = true;
                neiro.SetTraining(textBox2.Text, arr);
                NeiroGraphUtils.ClearImage(pictureBox1);
            }
        }
        private void PictureBox1_Leave(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                int[,] clipArr = NeiroGraphUtils.CutImageToArray((Bitmap)pictureBox1.Image, new Point(pictureBox1.Width, pictureBox1.Height));
                if (clipArr == null) return;
                arr = NeiroGraphUtils.LeadArray(clipArr, new int[NeiroWeb.neironInArrayWidth, NeiroWeb.neironInArrayHeight]);
                pictureBox2.Image = new Bitmap(NeiroGraphUtils.GetBitmapFromArr(clipArr), new Size(pictureBox2.Width, pictureBox2.Height)); ;
                pictureBox3.Image = new Bitmap(NeiroGraphUtils.GetBitmapFromArr(arr), new Size(pictureBox3.Width, pictureBox3.Height));
                string s = neiro.CheckLetter(arr);
                NeiroGraphUtils.ClearImage(pictureBox1);
                textBox1.Text += s;
            }
        }
        private void Button4_Click(object sender, EventArgs e)
        {
        string[] ls = File.ReadAllLines(F, Encoding.GetEncoding(1251));
        if (ls.Length == 0) return;
        var newItems = new List<string>();
        foreach (var i in ls) if (!newItems.Contains(i.ToString())) newItems.Add(i.ToString());
            foreach (var i in newItems)
            {
                NeiroGraphUtils.DrawLitera(pictureBox1.Image, i);
                textBox2.Text = i.ToString();
                Learn();
                ClearAll();
            }
            ClearAll();
            MessageBox.Show("Виконано");
        }
        private void ClearAll()
        {
            NeiroGraphUtils.ClearImage(pictureBox1);
            NeiroGraphUtils.ClearImage(pictureBox2);
            NeiroGraphUtils.ClearImage(pictureBox3);
            label2.Text = "Введіть коррекний символ";
        }
    } 
}

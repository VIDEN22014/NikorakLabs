using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        static PictureBox mainPictureBox = new PictureBox();
        static Bitmap bmp = new Bitmap(10000, 10000);
        static Graphics graph = Graphics.FromImage(bmp);
        static Pen pen = new Pen(Color.Black);
        static SerpinskyCarpetGroup group1 = new SerpinskyCarpetGroup();
        static TextBox mainTextBox = new TextBox();
        static int iter;
        public Form1()
        {
            InitializeComponent();
            mainPictureBox = pictureBox;
            mainTextBox = textBox1;
        }
        int TextBoxToInt(TextBox textBox)
        {
            if (textBox.Text == "") { return 0; }
            return Convert.ToInt32(textBox.Text);
        }
        void SerpinskyFractal(int iteration)
        {
            if (iteration == iter) { return; }
            for (int i = 0; i < Math.Pow(8, iteration); i++)
            {
                AddCarpet(group1.group[0]);
            }
            foreach (var i in group1.group)
            {
                i.DrawSquare();
            }
            SerpinskyFractal(iteration + 1);
        }
        void AddCarpet(SerpinskyCarpet carpet)
        {
            SerpinskyCarpet temp1 = new SerpinskyCarpet(carpet.x-2/3.0f*carpet.size, carpet.y-2/3.0f*carpet.size, carpet.size / 3.0f);
            SerpinskyCarpet temp2 = new SerpinskyCarpet(temp1.x+carpet.size, temp1.y, temp1.size);
            SerpinskyCarpet temp3 = new SerpinskyCarpet(temp2.x+carpet.size,temp1.y,temp1.size);
            SerpinskyCarpet temp4 = new SerpinskyCarpet(temp1.x,temp1.y+carpet.size,temp1.size);
            SerpinskyCarpet temp5 = new SerpinskyCarpet(temp3.x,temp4.y,temp1.size);
            SerpinskyCarpet temp6 = new SerpinskyCarpet(temp1.x,temp4.y+carpet.size,temp1.size);
            SerpinskyCarpet temp7 = new SerpinskyCarpet(temp2.x,temp6.y,temp1.size);
            SerpinskyCarpet temp8 = new SerpinskyCarpet(temp3.x,temp6.y,temp1.size);
            group1 += temp1;
            group1 += temp2;
            group1 += temp3;
            group1 += temp4;
            group1 += temp5;
            group1 += temp6;
            group1 += temp7;
            group1 += temp8;
            group1 -= carpet;
        }
        class SerpinskyCarpet
        {
            public float x { get; } = 0;
            public float y { get; } = 0;
            public float size { get; } = 0;
            public SerpinskyCarpet(float x, float y, float size)
            {
                this.x = x;
                this.y = y;
                this.size = size;
            }
            public void DrawSquare()
            {
                graph.DrawRectangle(pen, x, y, size, size);
                mainPictureBox.Image = bmp;
            }
        }
        class SerpinskyCarpetGroup
        {
            public SerpinskyCarpet[] group;
            public SerpinskyCarpetGroup()
            {
                group = new SerpinskyCarpet[0];
            }
            public static SerpinskyCarpetGroup operator +(SerpinskyCarpetGroup gr, SerpinskyCarpet rect)
            {
                Array.Resize(ref gr.group, gr.group.Length + 1);
                gr.group[gr.group.Length - 1] = rect;
                return gr;
            }
            public static SerpinskyCarpetGroup operator -(SerpinskyCarpetGroup gr, SerpinskyCarpet rect)
            {
                bool isFind = false;
                for (int i = 0; i < gr.group.Length; i++)
                {
                    if (isFind)
                    {
                        SerpinskyCarpet temp = gr.group[i];
                        gr.group[i] = gr.group[i - 1];
                        gr.group[i - 1] = temp;
                    }
                    if (gr.group[i] == rect)
                    {
                        isFind = true;
                    }
                }
                if (isFind)
                {
                    Array.Resize(ref gr.group, gr.group.Length - 1);
                }
                return gr;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            graph.Clear(Color.Transparent);
            SerpinskyCarpet carpet1 = new SerpinskyCarpet(10, 40, 700);
            carpet1.DrawSquare();
            SerpinskyCarpet carpet2 = new SerpinskyCarpet(carpet1.x+carpet1.size/3.0f, carpet1.y+carpet1.size / 3.0f, carpet1.size/3.0f);
            carpet2.DrawSquare();
            iter = TextBoxToInt(mainTextBox);
            foreach (SerpinskyCarpet i in group1.group)
            {
                group1 -= i;
            }
            foreach (SerpinskyCarpet i in group1.group)
            {
                group1 -= i;
            }
            group1 += carpet2;
            group1.group[0].DrawSquare();
            SerpinskyFractal(0);
        }
    }
}

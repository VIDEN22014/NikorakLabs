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
        static SerpinskyTriangleGroup group1 = new SerpinskyTriangleGroup();
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
            for (int i = 0; i < Math.Pow(3, iteration); i++)
            {
                AddRectangle(group1.group[0]);
            }
            foreach (var i in group1.group)
            {
                i.DrawTriangle();
            }
            SerpinskyFractal(iteration + 1);
        }
        void AddRectangle(SerpinskyTriangle triangle)
        {
            SerpinskyTriangle temp1 = new SerpinskyTriangle(triangle.x, triangle.y, triangle.size / 2.0f);
            SerpinskyTriangle temp2 = new SerpinskyTriangle(triangle.x + triangle.size/2.0f, triangle.y, triangle.size / 2.0f);
            SerpinskyTriangle temp3 = new SerpinskyTriangle(triangle.x + triangle.size / 4.0f, (float)(triangle.y - triangle.size / 4.0f*Math.Sqrt(3)), triangle.size / 2.0f);
            group1 += temp1;
            group1 += temp2;
            group1 += temp3;
            group1 -= triangle;
        }
        class SerpinskyTriangle
        {
            public float x { get; } = 0;
            public float y { get; } = 0;
            public float size { get; } = 0;
            public SerpinskyTriangle(float x, float y, float size)
            {
                this.x = x;
                this.y = y;
                this.size = size;
            }
            public void DrawTriangle()
            {
                graph.DrawLine(pen, x, y, x+size, y);
                graph.DrawLine(pen, x, y, (2*x+size)/2.0f, (float)(y -size/2*Math.Sqrt(3)));
                graph.DrawLine(pen, (2 * x + size) / 2.0f, (float)(y - size / 2 * Math.Sqrt(3)), x+size, y);
                mainPictureBox.Image = bmp;
            }
        }
        class SerpinskyTriangleGroup
        {
            public SerpinskyTriangle[] group;
            public SerpinskyTriangleGroup()
            {
                group = new SerpinskyTriangle[0];
            }
            public static SerpinskyTriangleGroup operator +(SerpinskyTriangleGroup gr, SerpinskyTriangle rect)
            {
                Array.Resize(ref gr.group, gr.group.Length + 1);
                gr.group[gr.group.Length - 1] = rect;
                return gr;
            }
            public static SerpinskyTriangleGroup operator -(SerpinskyTriangleGroup gr, SerpinskyTriangle rect)
            {
                bool isFind = false;
                for (int i = 0; i < gr.group.Length; i++)
                {
                    if (isFind)
                    {
                        SerpinskyTriangle temp = gr.group[i];
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
            SerpinskyTriangle rect1 = new SerpinskyTriangle(10, 650, 700);
            rect1.DrawTriangle();
            iter = TextBoxToInt(mainTextBox);
            foreach (SerpinskyTriangle i in group1.group)
            {
                group1 -= i;
            }
            foreach (SerpinskyTriangle i in group1.group)
            {
                group1 -= i;
            }
            group1 += rect1;
            group1.group[0].DrawTriangle();
            SerpinskyFractal(0);
        }
    }
}

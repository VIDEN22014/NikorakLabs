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
        static CantorRectangleGroup group1 = new CantorRectangleGroup();
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
        void CantorFractal(int iteration)
        {
            if (iteration == iter) { return; }
            for (int i = 0; i < Math.Pow(2, iteration); i++)
            {
                AddRectangle(group1.group[0]);
            }
            CantorFractal(iteration + 1);
        }
        void AddRectangle(CantorRectangle rect)
        {
            CantorRectangle temp1 = new CantorRectangle(rect.x, rect.y + 20, rect.width / 3.0f);
            CantorRectangle temp2 = new CantorRectangle(rect.x + 2 / 3.0f * rect.width, rect.y + 20, rect.width / 3.0f);
            group1 += temp1;
            group1 += temp2;
            group1 -= rect;
            group1.group[group1.group.Length - 1].DrawRectangle();
            group1.group[group1.group.Length - 2].DrawRectangle();
        }
        class CantorRectangle
        {
            public float x { get; } = 0;
            public float y { get; } = 0;
            public float width { get; } = 0;
            public CantorRectangle(float x, float y, float width)
            {
                this.x = x;
                this.y = y;
                this.width = width;
            }
            public void DrawRectangle()
            {
                graph.DrawRectangle(pen, x + 10, y + 10, width, 10);
                mainPictureBox.Image = bmp;
            }
        }
        class CantorRectangleGroup
        {
            public CantorRectangle[] group;
            public CantorRectangleGroup()
            {
                group = new CantorRectangle[0];
            }
            public static CantorRectangleGroup operator +(CantorRectangleGroup gr, CantorRectangle rect)
            {
                Array.Resize(ref gr.group, gr.group.Length + 1);
                gr.group[gr.group.Length - 1] = rect;
                return gr;
            }
            public static CantorRectangleGroup operator -(CantorRectangleGroup gr, CantorRectangle rect)
            {
                bool isFind = false;
                for (int i = 0; i < gr.group.Length; i++)
                {
                    if (isFind)
                    {
                        CantorRectangle temp = gr.group[i];
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
            CantorRectangle rect1 = new CantorRectangle(0, 30, 1200);
            iter = TextBoxToInt(mainTextBox);
            foreach (CantorRectangle i in group1.group)
            {
                group1 -= i;
            }
            foreach (CantorRectangle i in group1.group)
            {
                group1 -= i;
            }
            group1 += rect1;
            group1.group[0].DrawRectangle();
            CantorFractal(0);
        }
    }
}

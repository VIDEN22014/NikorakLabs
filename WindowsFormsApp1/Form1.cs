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
        static PifagorLineGroup group1 = new PifagorLineGroup();
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
        void PifagorFractal(int iteration)
        {
            if (iteration == iter) { return; }
            for (int i = 0; i < Math.Pow(2, iteration); i++)
            {
                AddLine(group1.group[0]);
            }
            foreach (var i in group1.group)
            {
                i.Draw();
            }
            PifagorFractal(iteration + 1);
        }
        void AddLine(PifagorLine line)
        {
            PifagorLine temp1 = new PifagorLine((float)(line.x + (line.lenght * Math.Cos(line.angle))), (float)(line.y - (line.lenght * Math.Sin(line.angle))), line.lenght*0.7f, (float)(line.angle+Math.PI/4.0f));
            PifagorLine temp2 = new PifagorLine((float)(line.x + (line.lenght * Math.Cos(line.angle))), (float)(line.y - (line.lenght * Math.Sin(line.angle))), line.lenght*0.7f, (float)(line.angle-Math.PI/4.0f));
            group1 += temp1;
            group1 += temp2;
            group1 -= line;
        }
        class PifagorLine
        {
            public float x { get; } = 0;
            public float y { get; } = 0;
            public float lenght { get; } = 0;
            public float angle { get; } = 0;
            public PifagorLine(float x, float y, float lenght, float angle)
            {
                this.x = x;
                this.y = y;
                this.lenght = lenght;
                this.angle = angle;
            }
            public void Draw()
            {
                graph.DrawLine(pen, x, y, (float)(x + lenght*Math.Cos(angle)), (float)(y - lenght * Math.Sin(angle)));
                mainPictureBox.Image = bmp;
            }
        }
        class PifagorLineGroup
        {
            public PifagorLine[] group;
            public PifagorLineGroup()
            {
                group = new PifagorLine[0];
            }
            public static PifagorLineGroup operator +(PifagorLineGroup gr, PifagorLine rect)
            {
                Array.Resize(ref gr.group, gr.group.Length + 1);
                gr.group[gr.group.Length - 1] = rect;
                return gr;
            }
            public static PifagorLineGroup operator -(PifagorLineGroup gr, PifagorLine rect)
            {
                bool isFind = false;
                for (int i = 0; i < gr.group.Length; i++)
                {
                    if (isFind)
                    {
                        PifagorLine temp = gr.group[i];
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
            PifagorLine line1 = new PifagorLine(510, 700, 200, (float)(Math.PI/2.0f));
            line1.Draw();
            iter = TextBoxToInt(mainTextBox);
            foreach (PifagorLine i in group1.group)
            {
                group1 -= i;
            }
            foreach (PifagorLine i in group1.group)
            {
                group1 -= i;
            }
            group1 += line1;
            PifagorFractal(0);
        }
    }
}

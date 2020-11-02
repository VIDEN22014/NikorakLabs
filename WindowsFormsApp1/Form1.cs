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
        static KohLineGroup group1 = new KohLineGroup();
        static KohLineGroup group2 = new KohLineGroup();
        static KohLineGroup group3 = new KohLineGroup();
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
        void KohFractal(int iteration)
        {
            if (iteration == iter) {
                foreach (var i in group1.group)
                {
                    i.Draw();
                }
                foreach (var i in group2.group)
                {
                    i.Draw();
                }
                foreach (var i in group3.group)
                {
                    i.Draw();
                }
                return; }
            for (int i = 0; i < Math.Pow(4, iteration); i++)
            {
                AddLine(group1.group[0],1);
                AddLine(group2.group[0],2);
                AddLine(group3.group[0],3);
            }
            KohFractal(iteration + 1);
        }
        void AddLine(KohLine line,int a)
        {
            float alpha = (float)Math.Atan2(line.by - line.ay, line.bx - line.ax);
            float r = (float)Math.Sqrt((line.bx - line.ax) * (line.bx - line.ax) + (line.by - line.ay) * (line.by - line.ay));
            KohLine temp1 = new KohLine(line.ax,line.ay, (float)(line.ax + r * Math.Cos(alpha) / 3.0f), (float)(line.ay + r * Math.Sin(alpha) / 3.0f));
            KohLine temp2 = new KohLine(temp1.bx , temp1.by, (float)(temp1.bx + r * Math.Cos(alpha - Math.PI / 3.0f) / 3.0f), (float)(temp1.by + r * Math.Sin(alpha - Math.PI / 3.0f) / 3.0f));
            KohLine temp3 = new KohLine(temp2.bx, temp2.by, (float)(line.ax + 2*r * Math.Cos(alpha) / 3.0f), (float)(line.ay + 2* r * Math.Sin(alpha) / 3.0f));
            KohLine temp4 = new KohLine(temp3.bx, temp3.by,line.bx,line.by);
            if (a == 1)
            {
                group1 += temp1;
                group1 += temp2;
                group1 += temp3;
                group1 += temp4;
                group1 -= line;
            }
            if (a == 2)
            {
                group2 += temp1;
                group2 += temp2;
                group2 += temp3;
                group2 += temp4;
                group2 -= line;
            }
            if (a == 3)
            {
                group3 += temp1;
                group3 += temp2;
                group3 += temp3;
                group3 += temp4;
                group3 -= line;
            }
        }
        class KohLine
        {
            public float ax { get; } = 0;
            public float ay { get; } = 0;
            public float bx { get; } = 0;
            public float by { get; } = 0;
            public KohLine(float ax, float ay, float bx,float by)
            {
                this.ax = ax;
                this.ay = ay;
                this.bx = bx;
                this.by = by;
            }
            public void Draw()
            {
                graph.DrawLine(pen, ax, ay, bx, by);
                mainPictureBox.Image = bmp;
            }
        }
        class KohLineGroup
        {
            public KohLine[] group;
            public KohLineGroup()
            {
                group = new KohLine[0];
            }
            public static KohLineGroup operator +(KohLineGroup gr, KohLine rect)
            {
                Array.Resize(ref gr.group, gr.group.Length + 1);
                gr.group[gr.group.Length - 1] = rect;
                return gr;
            }
            public static KohLineGroup operator -(KohLineGroup gr, KohLine rect)
            {
                bool isFind = false;
                for (int i = 0; i < gr.group.Length; i++)
                {
                    if (isFind)
                    {
                        KohLine temp = gr.group[i];
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
            KohLine line1 = new KohLine(510, 500, 10,500);
            KohLine line2 = new KohLine(10, 500, 260, (float)(500 - 250*Math.Sqrt(3)));
            KohLine line3 = new KohLine(260, (float)(500 - 250 * Math.Sqrt(3)), 510, 500);
            iter = TextBoxToInt(mainTextBox);
            foreach (KohLine i in group1.group)
            {
                group1 -= i;
            }
            foreach (KohLine i in group1.group)
            {
                group1 -= i;
            }
            //
            foreach (KohLine i in group2.group)
            {
                group2 -= i;
            }
            foreach (KohLine i in group2.group)
            {
                group2 -= i;
            }
            //
            foreach (KohLine i in group3.group)
            {
                group3 -= i;
            }
            foreach (KohLine i in group3.group)
            {
                group3 -= i;
            }
            //
            group1 += line1;
            group2 += line2;
            group3 += line3;
            KohFractal(0);
        }
    }
}

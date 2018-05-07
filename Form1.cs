using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace Threads_CalculatorDemonstrator
{
    public partial class Form1 : Form
    {
        Calculator calculator;
        Demonstrator demonstrator;

        private int radius { get; set; }
        private int mxx { get; set; }
        private int mxy { get; set; }
        private int hit = 0, miss = 0;
        public bool f = false;

        public Form1()
        {
            InitializeComponent();
        }

        delegate void SetText(TextBox t, string text, bool flag);
        private void AddText(TextBox t, string text, bool flag)
        {
            if (t.InvokeRequired)
            {
                SetText d = new SetText(AddText);
                //this.Invoke(d, new object[] { t, text, f }); //???
                //is.Invoke(d, t, text); //???
                this.Invoke(d, t, text, f); //???
            }
            else
            {
                if (flag)
                    t.AppendText(text + Environment.NewLine);
                else
                {
                    textBox7.ReadOnly = false;
                    textBox8.ReadOnly = false;
                    t.Clear();
                    t.AppendText(text);
                }
            }
        }

        public void CalcPrint(object sender, CalcEventArgs e)
        {
            String str = "Количество полупростых чисел на промежутке [ " + e.A.ToString() + ";" + e.B.ToString() + " ] : " + e.Amount.ToString();
            AddText(textBox2, str, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (f)
            {
                Rectangle r = panel1.ClientRectangle;
                Graphics g = panel1.CreateGraphics();
                DrawTarget(r, g);
                DrawPoint(r, g);
            }
        }

        private void DrawTarget(Rectangle r, Graphics g)
        {
            g.Clear(Color.White);
            int W = r.Width;
            int  H = r.Height;
            Point LU = new Point(0, 0);
            Rectangle rect = new Rectangle(LU.X + W / 2 - radius, LU.Y + H / 2 - radius, 2 * radius, 2 * radius);

            Pen pen = new Pen(Color.FromArgb(0, 0, 255));
            SolidBrush brush = new SolidBrush(Color.FromArgb(0, 0, 255));
            float startAngle = -90f;
            float sweepAngle = 45f;
            g.DrawPie(pen, rect, startAngle, sweepAngle);
            g.FillPie(brush, rect, startAngle, sweepAngle);

            startAngle = 90f;
            g.DrawPie(pen, rect, startAngle, sweepAngle);
            g.FillPie(brush, rect, startAngle, sweepAngle);

            pen.Dispose();
            brush.Dispose();
        }

        delegate void SetMissHit();
        public void DrawPoint(Rectangle r, Graphics g)
        {
            Random rnd = new Random();
            int x = rnd.Next(-mxx, mxx + 1),
                y = rnd.Next(-mxy, mxy + 1);
            Pen pen = new Pen(Color.DarkRed);
            //int x = e.CoordX, y = e.CoordY;
            int W = r.Width, H = r.Height;

            g.DrawEllipse(pen, W / 2 + x - 3, H / 2 - y - 3, 6, 6);
            SolidBrush brush = new SolidBrush(Color.DarkRed);
            g.FillEllipse(brush, W / 2 + x - 3, H / 2 - y - 3, 6, 6);
            pen.Dispose();
            brush.Dispose();

            if (isHit(x, y))
                AddText(textBox8, hit++.ToString(), false);
            else AddText(textBox7, miss++.ToString(), false);
        }

        bool isHit(int x, int y)
        {
            if (x > 0 && x < Math.Sqrt(2) / 2 * radius && y > x && y < radius)
                return true;
            if (x < 0 && Math.Abs(x) < Math.Sqrt(2) / 2 * radius && y < x && Math.Abs(y) < radius)
                return true;
            return false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                calculator.StopThread();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                demonstrator.GetCalculator(calculator);
                demonstrator.StopThread();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }


        private void Form1_FormClosing(object sender, FormClosingEventArgs e) // ??? 
        {
            //if (calculator.thr != null)
            //    calculator.StopThread();
            //if (demonstrator.thr != null)
            //    demonstrator.StopThread();
            if (calculator != null && calculator.thr != null)
                calculator.StopThread();
            if (demonstrator != null && demonstrator.thr != null)
                demonstrator.StopThread();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            button2.Enabled = false;
            button3.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
                int time1 = 0, time2 = 0, rad = 0, maxx = 0, maxy = 0; //???
                try
                {
                    time1 = Int32.Parse(textBox1.Text);
                    time2 = Int32.Parse(textBox6.Text);
                    rad = Int32.Parse(textBox3.Text);
                    radius = rad;
                    maxx = Int32.Parse(textBox4.Text);
                    maxy = Int32.Parse(textBox5.Text);
                    mxx = maxx;
                    mxy = maxy;
                    //if (time1 <= 0 || time1 > 10000 || time2 <= 0 || time2 > 10000 ||
                    //    rad <= 0 || rad >= panel1.Height / 2 ||
                    //    Math.Abs(maxx) > rad || Math.Abs(maxy) > rad)
                    //{
                    //    MessageBox.Show("Неверный ввод!");
                    //    return; 
                    //}
                }
                catch (Exception)
                {
                    MessageBox.Show("Неверный ввод!");
                    return; 
                }


            if (time1 <= 0 || time1 > 10000 || time2 <= 0 || time2 > 10000 ||
                    rad <= 0 || rad >= /*panel1.*/Height / 2 ||
                    Math.Abs(maxx) > rad || Math.Abs(maxy) > rad)
            {
                MessageBox.Show("Неверный ввод!");
                return;
            }


            try
                {
                if (calculator != null && calculator.thr != null)
                    calculator.StopThread();
                if (demonstrator != null && demonstrator.thr != null)
                    demonstrator.StopThread();
                f = true;
                    calculator = new Calculator(time1);
                    demonstrator = new Demonstrator(time2, rad, maxx, maxy, Invalidate);

                    calculator.EventFinishCalc += demonstrator.Answer;
                    demonstrator.EventFinishCalcDem += new CalcEventHandlerDem(CalcPrint);
                    calculator.StartThread();

                    demonstrator.StartThread();

                button2.Enabled = true;
                button3.Enabled = true;
            }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
        }
    }
}

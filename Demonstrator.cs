using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Threads_CalculatorDemonstrator
{
    //public class DemEventArgs
    //{
    //    public int CoordX { get; set; }
    //    public int CoordY { get; set; }

    //    public DemEventArgs(int x, int y)
    //    {
    //        CoordX = x;
    //        CoordY = y;
    //    }
    //}


    public delegate void CalcEventHandlerDem(object sender, CalcEventArgs e);
    //public delegate void ShootEventHandler(Rectangle r, Graphics g, object sender, DemEventArgs e);

    class Demonstrator
    {
        public Thread thr;

        public event CalcEventHandlerDem EventFinishCalcDem;
        //public static event ShootEventHandler EventShoot;

        public int Time { get; set; }
        public int Radius { get; set; }
        public int MaxX { get; set; }
        public int MaxY { get; set; }

        private Calculator calculator;

        public delegate void delegatedraw();
        private delegatedraw draw;

        public Demonstrator(int time, int rad, int maxx, int maxy, delegatedraw f) 
        {
            Time = time; 
            Radius = rad;
            MaxX = maxx;
            MaxY = maxy;
            draw = f;
        }

        public void GetCalculator(Calculator _calculator)
        {
            calculator = _calculator;
        }

        public void StartThread()
        {
            thr = new Thread(Shooting); 
            thr.Start();
        }

        public void StopThread()
        {
            if (calculator != null)
            {
                if (calculator.thr != null && calculator.thr.IsAlive)
                    throw new Exception("Вычислитель не остановлен!");

                else
                {
                    if (thr != null)
                        thr.Abort();
                }
            }
        }

        public void Shooting()
        {
            while (true)
            {
                draw();
                Thread.Sleep(Time);
            }
        }

        public void Answer(object sender, CalcEventArgs e)
        {
            try
            {
                if (EventFinishCalcDem != null)
                    EventFinishCalcDem(this , e); // тот ли объект??

                Thread.Sleep(e.Time);

                Calculator calculate = new Calculator(e.Time);
                calculate.EventFinishCalc += this.Answer;
                calculate.Calc();
            }
            catch (Exception) { }
        }
    }
}

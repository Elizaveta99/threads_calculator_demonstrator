using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Threads_CalculatorDemonstrator
{
    public class CalcEventArgs
    {
        public int A { get; set; }
        public int B { get; set; }
        public int Amount { get; set; }
        public int Time { get; set; }

        public CalcEventArgs(int a, int b, int amount, int time)
        {
            A = a;
            B = b;
            Amount = amount;
            Time = time;
        }
    }

    public delegate void CalcEventHandler(object sender, CalcEventArgs e);


    class Calculator
    {
        public Thread thr;

        public  int Time { get; set; }
        public static bool isOn;
        

        //public delegate void CalcEventHandler(int A, int B, int cnt, int time);
        //public delegate void CalcEventHandler(object sender, CalcEventArgs e);
        public event CalcEventHandler EventFinishCalc;

        public Calculator(int time)
        {
            Time = time;
            isOn = true;
        }

        public void StartThread()
        {
            thr = new Thread(Calc);
            thr.Start();
        }

        public void StopThread()
        {
            isOn = false;
            if (thr != null)
                thr.Abort();
        }


        public void Calc()
        {
            if (isOn)
            {
                Random rnd = new Random();
                int A = rnd.Next(2, 1000000);
                int B = rnd.Next(A, 1000001);
                //int A = 1;
                //int B = 10;
                int ans = 0;
                for (int i = A; i <= B; i++)
                    if (isHalfSimple(i)) ans++;

                if (EventFinishCalc != null)
                    EventFinishCalc(this, new CalcEventArgs(A, B, ans, Time));
            }
        }

        public bool isHalfSimple(int x)
        {
            int d = 2, cnt = 0;
            while (d * d <= x)
            {
                if (x % d == 0)
                {
                    x /= d;
                    cnt++;
                    if (cnt > 2) return false;
                }
                else
                {
                    if (d == 2) d = 3;
                    else d += 2;
                }
            }
            if (x != 1) cnt++;
            if (cnt == 2)
                return true;
            return false;
        }
    }
}

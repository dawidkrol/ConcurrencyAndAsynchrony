using System;
using System.Threading;

namespace Threading
{
    class Program
    {
        
        static void Main(string[] args)
        {
            //ThreadTest thredTest = new ThreadTest();
            //thredTest.PropertyChanged += ThredTest_PropertyChanged;
            //Thread thread = new Thread(thredTest.thr);
            //thread.Start();
            ////thread.Join();
            //Console.WriteLine(thread.ThreadState);
            //for (int i = 0; i < 100; i++)
            //{
            //    Console.WriteLine(thread.ThreadState);
            //    Console.WriteLine($"main: {i}");
            //}

            //Thread thread = new Thread(Got);
            //thread.Start();
            //Got();

            //ThreadTest thredTest = new ThreadTest();
            //Thread th = new Thread(thredTest.thr);
            //th.Start();
            //thredTest.signal.WaitOne();
            //th.IsBackground = true;
        }

        private static void ThredTest_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Console.WriteLine(e.PropertyName);
        }
        private static void Got()
        {
            lock (ThreadTest._locker)
            {
                if (ThreadTest.AllowCheck)
                {
                    //ThreadTest.AllowCheck = false;
                    Console.WriteLine("Gotowe");
                    ThreadTest.AllowCheck = false;
                }
            }
        }
        }
    }

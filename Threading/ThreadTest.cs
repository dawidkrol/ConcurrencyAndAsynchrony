using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Threading
{
    class ThreadTest : INotifyPropertyChanged
    {
        public ManualResetEvent signal = new ManualResetEvent(false);
        public static bool AllowCheck = true;
        public static readonly object _locker = new object();
        public event PropertyChangedEventHandler PropertyChanged;
        public void thr()
        {
            OnPropertyChanged(Thread.CurrentThread.ThreadState.ToString());
            Thread.Sleep(1000);
            OnPropertyChanged(Thread.CurrentThread.ThreadState.ToString());
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine($"thr: {i}");
                Thread.Sleep(1000);
                signal.Set();
            }
        }
        protected virtual void OnPropertyChanged([CallerMemberName]string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ConcurrencyAndAsynchrony
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Progress<List<string>> _progress = new Progress<List<string>>();
        SynchronizationContext SContext = SynchronizationContext.Current;
        ManualResetEvent me = new ManualResetEvent(false);
        CancellationTokenSource Cancellation = new CancellationTokenSource();
        public MainWindow()
        {
            InitializeComponent();
            _progress.ProgressChanged += _progress_ProgressChanged;
            //new Thread(th1).Start();
            //new Thread(th2).Start();

            //Task<string> a = Task.Run(() => th1(MessageT1));
            //Task.Run(() => th1(MessageT2));
            //Task i = Task.Run(() => throw null);
            //try
            //{
            //    i.Wait();
            //}
            //catch (Exception ex)
            //{

            //    throw;
            //}

            //IMPORTANT V1
            //Task<string> a = Task.Run(() => th1(_progress));
            //var awai = a.GetAwaiter();
            //awai.OnCompleted(() =>
            //{
            //    text1.Text = "Koniec";
            //});
            // IMPORTANT V2
            //Task<string> a = Task.Run(() => th1(MessageT1));
            //a.ContinueWith((q) =>
            //{
            //    Thread.Sleep(1000);
            //});
            //IMPORTANT V3
            th3(_progress,Cancellation.Token);
        }

        private void _progress_ProgressChanged(object sender, List<string> e)
        {
            text1.Text += $"{e[e.Count - 1]}";
        }

        private string th1(IProgress<List<string>> a)
        {
            List<string> vs = new List<string>();
            string output = null;
            for (int i = 0; i < 10; i++)
            {
                //MessageT1($"{i} \n");
                output = $"{i}\n";
                vs.Add(output);
                a.Report(vs);
                Thread.Sleep(1000);
                //if(i==5)
                //    me.Set();
            }
            
            return output;
        }
        //private void th2()
        //{
        //    for (int i = 0; i < 10; i++)
        //    {
        //        me.WaitOne();
        //        MessageT2($"{i} \n");
        //        Thread.Sleep(1000);
        //    }
        //}
        private async void th3(IProgress<List<string>> progress,CancellationToken token)
        {
            List<string> q = new List<string>();
            try
            {
                List<Task<List<string>>> vs = new List<Task<List<string>>>();
                vs.Add(GetListAsync(progress, token, 1000));
                vs.Add(GetListAsync(progress, token, 2000));
                vs.Add(GetListAsync(progress, token, 3000));

                var r = await Task.WhenAll(vs);
                foreach (var item in r)
                {
                    foreach (var _item in item)
                    {
                        q.Add(_item);
                    }
                }
            }
            catch(OperationCanceledException e)
            {
                text1.Text = e.Message;
            }

            //Task<List<string>> a = Task.Run(() => GetListAsync(progress));
            //var awaiter = a.GetAwaiter();
            //awaiter.OnCompleted(() =>
            //{
            //    q = awaiter.GetResult();
            //});
        }
        private async Task<List<string>> GetListAsync(IProgress<List<string>> progress, CancellationToken token, int time)
        {
            List<string> output = new List<string>();
            token.Register(() => text1.Text = "Anulowanie zadania");
            for (int i = 0; i < 10; i++)
            {
                output.Add($"{i} \n");
                progress.Report(output);
                token.ThrowIfCancellationRequested();
                await Task.Delay(time);
            }
            return output;
        }
        //private void MessageT2(string message)
        //{
        //    SContext.Post(_ => text2.Text += message, null);
        //}
        private void MessageT1(string message)
        {
            SContext.Post(_ => text1.Text += message, null);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Cancellation.Cancel();
        }
    }
}

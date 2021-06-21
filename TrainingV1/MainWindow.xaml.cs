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

namespace TrainingV1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Progress<int> progressEvent = new Progress<int>();
        CancellationTokenSource cts = new CancellationTokenSource();
        public MainWindow()
        {
            InitializeComponent();
            progressEvent.ProgressChanged += ProgressEvent_ProgressChanged;
        }

        private void ProgressEvent_ProgressChanged(object sender, int e)
        {
            _progress.Value = e;
        }

        private async void _start_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _start.IsEnabled = false;
                await LongAsyncProcess(progressEvent, cts.Token, () => _tekst.Text = "Cancelating operation");
            }
            catch (OperationCanceledException)
            {
                _tekst.Text = "Operation is canceled";
                cts = new CancellationTokenSource();
            }
            finally
            {
                _start.IsEnabled = true;
            }
        }

        private void _cancel_Click(object sender, RoutedEventArgs e)
        {
            cts.Cancel();
        }

        private async Task<string> LongAsyncProcess(IProgress<int> _prog, CancellationToken ct,Action action)
        {
            string startValue = "Long text wchich is in long async function";
            StringBuilder sb = new StringBuilder();
            ct.Register(action);
            foreach (var item in startValue)
            {
                ct.ThrowIfCancellationRequested();
                sb.Append(item);
                _prog?.Report((int)(((decimal)sb.Length*100)/((decimal)startValue.Length)));
                await Task.Delay(100);
            }
            return sb.ToString();
        }
    }
}

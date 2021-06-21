using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace TrainingV1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Progress<int> progressEvent = new Progress<int>();
        Progress<int> progressEven2 = new Progress<int>();
        CancellationTokenSource cts = new CancellationTokenSource();
        public MainWindow()
        {
            InitializeComponent();
            progressEvent.ProgressChanged += ProgressEvent_ProgressChanged;
            progressEven2.ProgressChanged += ProgressEven2_ProgressChanged;
        }

        private void ProgressEven2_ProgressChanged(object sender, int e)
        {
            _progress2.Value = e;
        }

        private void ProgressEvent_ProgressChanged(object sender, int e)
        {
            _progress.Value = e;
        }

        private async void _start_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SomeProcess sp = new SomeProcess();
                _start.IsEnabled = false;
                List<Task<string>> doubleTask = new List<Task<string>>();
                doubleTask.Add(sp.LongAsyncProcess(progressEvent, cts.Token, () => _tekst.Text = "Cancelating operation", 500));
                doubleTask.Add(sp.LongAsyncProcess(progressEven2, cts.Token, () => _tekst.Text = "Cancelating operation", 600));

                await await Task.WhenAny(doubleTask);
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
    }
}

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

namespace AsynchronicOperations
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        CancellationTokenSource ct = new CancellationTokenSource();
        Progress<DownloadWebsitesProgress> _prog = new Progress<DownloadWebsitesProgress>();
        public MainWindow()
        {
            InitializeComponent();
            _prog.ProgressChanged += _prog_ProgressChanged;
        }
        private void Message(dynamic message)
        {
            box.Text = message;
        }

        private void _prog_ProgressChanged(object sender, DownloadWebsitesProgress e)
        {
            _progress.Value = e.percentage;
            box.Text += $"{e.name.Last()} \n";
        }

        private void Progress_ProgressChanged(object sender, int e)
        {
            _progress.Value = e;
        }


        private async void downAsync_Click(object sender, RoutedEventArgs e)
        {
            downAsync.IsEnabled = false;
            try
            {
                Message("");
                await WebsiteThings.DownloadWebsitesAsync(WebsiteThings.GetDemonstrateWebsiteUrls(), _prog, ct.Token,() => Message("Canceling operation"));
            }
            catch
            {
                Message("Download calenced");
            }
            finally 
            { 
                downAsync.IsEnabled = true;
                ct = new CancellationTokenSource();
            }
        }

        private async void downParallel_Click(object sender, RoutedEventArgs e)
        {
            Message("");
            downParallel.IsEnabled = false;
            await WebsiteThings.DownloadWebsitesParallel(WebsiteThings.GetDemonstrateWebsiteUrls(), _prog);
            downParallel.IsEnabled = true;
        }

        private void canel_Click(object sender, RoutedEventArgs e)
        {
            ct.Cancel();
        }
    }
}

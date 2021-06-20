using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AsynchronicOperations
{
    public class WebsiteThings : IWebsiteThings
    {
        public static List<string> GetDemonstrateWebsiteUrls()
        {
            List<string> output = new List<string>
            {
                "https://stackoverflow.com",
                "https://www.youtube.com/",
                "https://www.facebook.com/",
                "https://www.instagram.com/",
                "https://github.com/"
            };
            return output;
        }
        public string DownloadWebsite(string webAdress)
        {
            WebClient Client = new WebClient();
            string output = Client.DownloadString(webAdress);
            return output;
        }
        public async Task<string> DownloadWebsiteAsync(string webAdress)
        {
            WebClient Client = new WebClient();
            string output = await Client.DownloadStringTaskAsync(webAdress);
            return output;
        }
        public async Task<List<string>> DownloadWebsitesAsync(List<string> vs, IProgress<DownloadWebsitesProgress> progress, CancellationToken ct, Action action)
        {
            List<string> output = new List<string>();
            DownloadWebsitesProgress dp = new DownloadWebsitesProgress();
            ct.Register(action);
            foreach (var item in vs)
            {
                ct.ThrowIfCancellationRequested();
                string temp = item;
                output.Add(await DownloadWebsiteAsync(item));
                dp.name.Add(temp);
                dp.percentage = (int)(((decimal)dp.name.Count / (decimal)vs.Count) * 100);
                progress?.Report(dp);
            }
            return output;
        }
        public async Task<List<string>> DownloadWebsitesParallel(List<string> vs, IProgress<DownloadWebsitesProgress> progress)
        {
            List<string> output = new List<string>();
            DownloadWebsitesProgress dp = new DownloadWebsitesProgress();
            await Task.Run(() =>
              Parallel.ForEach(vs, (item) =>
              {
                  string temp = item;
                  output.Add(DownloadWebsite(item));
                  dp.name.Add(temp);
                  dp.percentage = (int)(((decimal)dp.name.Count / (decimal)vs.Count) * 100);
                  progress?.Report(dp);
              }));
            return output;
        }
    }
}

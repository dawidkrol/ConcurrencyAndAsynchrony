using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AsynchronicOperations
{
    public interface IWebsiteThings
    {
        List<string> GetDemonstrateWebsiteUrls();
        string DownloadWebsite(string webAdress);
        Task<string> DownloadWebsiteAsync(string webAdress);
        Task<List<string>> DownloadWebsitesAsync(List<string> vs, IProgress<DownloadWebsitesProgress> progress, CancellationToken ct, Action action);
        Task<List<string>> DownloadWebsitesParallel(List<string> vs, IProgress<DownloadWebsitesProgress> progress);
    }
}
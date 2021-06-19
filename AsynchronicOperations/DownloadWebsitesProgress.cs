using System.Collections.Generic;

namespace AsynchronicOperations
{
    public class DownloadWebsitesProgress
    {
        public List<string> name { get; set; } = new List<string>();
        public int percentage { get; set; }
    }
}
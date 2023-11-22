
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FinalCrawler
{
    class Crawler
    {
        public event Action<Crawler> CrawlerStopped;
        public event Action<Crawler, string, string> PageDownloaded;

        private string startUrl { get; set; }
        private string domain { get; set; }
        private HashSet<string> visitedUrls;
        private HashSet<string> errorUrls;
        public int MaxPage { get; set; }
        private Queue<string> pending;

        public Crawler(string startUrl)
        {
            this.startUrl = startUrl;
            this.domain = GetDomain(startUrl);
            //Console.WriteLine("!!!!Domain:");
            //Console.WriteLine(domain);
            this.visitedUrls = new HashSet<string>();
            this.errorUrls = new HashSet<string>();
            MaxPage = 100;
            pending = new Queue<string>();
            pending.Enqueue(startUrl);
        }
        public Crawler()
        {
            this.visitedUrls = new HashSet<string>();
            this.errorUrls = new HashSet<string>();
            MaxPage = 100;
            pending = new Queue<string>();
        }

        public async Task StartAsync()
        {
            this.visitedUrls.Clear();
            this.errorUrls.Clear();
            pending.Clear();
            Console.WriteLine("Start crawling...");
            pending.Enqueue(this.startUrl);
            while (pending.Count > 0 && visitedUrls.Count < MaxPage)
            {
                string url = pending.Dequeue();
                await CrawlAsync(url);
                Console.WriteLine(url);
            }
            CrawlerStopped(this);
            Console.WriteLine("Crawling finished.");
            Console.WriteLine("Visited URLs:");
            foreach (string url in visitedUrls)
            {
                Console.WriteLine(url);
            }
            Console.WriteLine("Error URLs:");
            foreach (string url in errorUrls)
            {
                Console.WriteLine(url);
            }
        }

        private async Task CrawlAsync(string url)
        {
            if (!visitedUrls.Contains(url))
            {
                visitedUrls.Add(url);

                try
                {
                    string html = await DownloadHtmlAsync(url);
                    ParseHtml(html, url);
                    PageDownloaded(this, url, "success");
                    //Console.WriteLine("!!!!!!!!!!" + url + "!!!!!!!!");
                    try
                    {
                        ParseHtmlPhoto(html, url);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("This is image error:");
                        Console.WriteLine(ex);
                    }

                }
                catch (Exception ex)
                {
                    errorUrls.Add(url);
                    //PageDownloaded(this, url, "  Error:" + ex.Message);
                }
            }
        }

        private async Task<string> DownloadHtmlAsync(string url)
        {
            using (WebClient client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                return await client.DownloadStringTaskAsync(url);
            }
        }

        private bool IsHtml(string html)
        {
            string pattern = @"<\s*html\s*>";
            return Regex.IsMatch(html, pattern, RegexOptions.IgnoreCase);
        }

        private void ParseHtml(string html, string baseUrl)
        {
            Console.WriteLine(html);
            string pattern = @"<\s*a\s+[^>]*href\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*>";
            MatchCollection matches = Regex.Matches(html, pattern, RegexOptions.IgnoreCase);
            foreach (Match match in matches)
            {
                string url = match.Groups[1].Value;
                if (IsRelativeUrl(url))
                {
                    url = ConvertToAbsoluteUrl(url, baseUrl);
                }
                if (IsInDomain(url))
                {
                    pending.Enqueue(url);
                }
            }
        }
        #region Picture download
        private void ParseHtmlPhoto(string html, string baseUrl)
        {
            string pattern = @"<img[^>]+?src\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*>";
            //string pattern2 = @"(?<==['""])(https?://\S+\.(?:jpg|png|jpeg))(?=['""])";
            string pattern2 = @"(?<=['""])(https?://\S+\.(?:jpg|png|jpeg|webp))(?=['""])";
            MatchCollection matches = Regex.Matches(html, pattern, RegexOptions.IgnoreCase);
            MatchCollection matches2 = Regex.Matches(html, pattern2, RegexOptions.IgnoreCase);
            foreach (Match match in matches)
            {
                string imageUrl = match.Groups[1].Value;
                //Console.WriteLine(baseUrl);
                //Console.WriteLine(imageUrl);
                if (IsRelativeUrl(imageUrl))
                {
                    imageUrl = ConvertToAbsoluteUrl(imageUrl, baseUrl);
                    //Console.WriteLine(imageUrl);
                }
                Console.WriteLine(imageUrl);
                DownloadImagesAsync(imageUrl);
            }
            foreach (Match match2 in matches2)
            {
                //Console.WriteLine(1);
                string imageUrl = match2.Groups[1].Value;
                if (IsRelativeUrl(imageUrl))
                {
                    imageUrl = ConvertToAbsoluteUrl(imageUrl, baseUrl);
                    //Console.WriteLine(imageUrl);
                }
                Console.WriteLine(imageUrl);
                DownloadImagesAsync(imageUrl);
            }

        }
        private void DownloadImagesAsync(string url)
        {
            string localFolderPath = "../../ImagesDownloaded/";
            using (WebClient client = new WebClient())
            {
                string fileName = Path.GetFileName(new Uri(url).LocalPath);
                string localPath = Path.Combine(localFolderPath, fileName);
                client.DownloadFile(url, localPath);
            }
        }
        #endregion
        private bool IsRelativeUrl(string url)
        {
            string pattern = @"^(?!www\.|(?:http|ftp)s?://|[A-Za-z]:\\).*[^/]$";
            return Regex.IsMatch(url, pattern);
        }

        private string ConvertToAbsoluteUrl(string url, string baseUrl)
        {
            if (url.StartsWith("//"))
            {
                return "http:" + url;
            }
            if (url.StartsWith("/"))
            {
                return GetProtocol(baseUrl) + "://" + domain + url;
            }
            else if (url.StartsWith("./"))
            {
                return GetProtocol(baseUrl) + "://" + GetPath(baseUrl) + url.Substring(2);
            }
            else if (url.StartsWith("../"))
            {
                string path = GetPath(baseUrl);
                while (url.StartsWith("../"))
                {
                    url = url.Substring(3);
                    int index = path.LastIndexOf('/');
                    if (index >= 0)
                    {
                        path = path.Substring(0, index);
                    }
                }
                return GetProtocol(baseUrl) + "://"  + path + "/" + url;
            }
            else
            {
                if (baseUrl.EndsWith("/"))
                    return baseUrl + url;
                else return baseUrl + '/' + url;
            }
        }

        private bool IsInDomain(string url)
        {
            return GetDomain(url) == domain;
        }

        private string GetProtocol(string url)
        {
            int index = url.IndexOf(':');
            if (index >= 0)
            {
                return url.Substring(0, index);
            }
            else
            {
                return "http";
            }
        }

        private string GetDomain(string url)
        {
            int start = url.IndexOf("://") + 3;
            int end = url.IndexOf('/', start);
            if (end < 0)
            {
                end = url.Length;
            }
            return url.Substring(start, end - start);
        }

        private string GetPath(string url)
        {
            int start = url.IndexOf("://") + 3;
            int end = url.LastIndexOf('/');
            if (end < start)
            {
                return "/";
            }
            else
            {
                return url.Substring(start, end - start);
            }
        }
    }
}


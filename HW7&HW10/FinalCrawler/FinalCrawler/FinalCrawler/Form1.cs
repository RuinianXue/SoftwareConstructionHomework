using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FinalCrawler
{

    public partial class Form1 : Form
    {


        BindingSource resultBindingSource = new BindingSource();
        BindingSource phoneNumberBindingSource = new BindingSource();
        Crawler crawler = new Crawler();

        public Form1()
        {
            InitializeComponent();
            Result.DataSource = resultBindingSource;
            phoneNumberGridView.DataSource = phoneNumberBindingSource;
            crawler.PageDownloaded += Crawler_PageDownloaded;
        }

        private void Crawler_CrawlerStopped(Crawler obj)
        {

        }


        private async void btnStart_Click(object sender, EventArgs e)
        {
            resultBindingSource.Clear();
            string startUrl = InputURL.Text.Trim();
            Console.WriteLine(startUrl);
            if (string.IsNullOrEmpty(startUrl))
            {
                MessageBox.Show("Please input start URL.");
                return;
            }
            crawler = new Crawler(startUrl);
            crawler.PageDownloaded += Crawler_PageDownloaded;
            await crawler.StartAsync();
        }


        private void Crawler_PageDownloaded(Crawler crawler, string url, string info)
        {
            var pageInfo = new
            {
                Index = resultBindingSource.Count + 1,
                URL = url,
                Status = info
            };
            Action action = () => { resultBindingSource.Add(pageInfo); };
            /*
            var phoneInfo = new
            {
                Index = phoneNumberBindingSource.Count + 1,
                //Phone = phoneNumber,
                URL = url
            };
            if (phoneNumber != null)
            {
                Action actionNumber = () => { phoneNumberBindingSource.Add(phoneInfo); };
            }*/
            if (this.InvokeRequired)
            {
                this.Invoke(action);
            }
            else
            {
                action();
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}

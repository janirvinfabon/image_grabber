using ImageGrabber.classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageGrabber.forms
{
    public partial class my_grabber : Form
    {
        private List<link_sources> linkSOurceList = new List<link_sources>();
        private WebBrowser browser = new WebBrowser();
        private BindingSource bs = new BindingSource();
        private string defaultLocation = string.Empty;
        private string appName = string.Empty;

        public my_grabber()
        {
            InitializeComponent();
            this.Init();

            this.appName = "Image Grabber";
            this.MaximizeBox = false;
        }

        private void Init()
        {
            this.browser.ScriptErrorsSuppressed = true;
            this.defaultLocation = string.Format(@"{0}\img_grabber_downloads", Environment.GetFolderPath(Environment.SpecialFolder.Desktop));

            if (!Directory.Exists(this.defaultLocation))
            {
                Directory.CreateDirectory(this.defaultLocation);
            }

            this.bs.DataSource = this.linkSOurceList;
            this.dataGridView1.DataSource = this.bs;

            this.button1.Click += this.OnNavigateUrl;
            this.button2.Click += this.OnDownloadImages;
            this.browser.DocumentCompleted += this.OnVirtualBrowserDocsComplete;
            this.browser.Navigating += this.OnPreparingNavigation;

            function_helper.DataGridViewStyler(this.dataGridView1, "No", 30);
        }

        private void OnPreparingNavigation(object sender, WebBrowserNavigatingEventArgs e)
        {
            new Thread(() => 
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() => { this.Text = string.Format("{0} | Navigating...", this.appName); }));
                }
            }).Start();
        }

        private void OnNavigateUrl(object sender, EventArgs e)
        {

            var link = this.textBox1.Text.Trim();

            if (!string.IsNullOrEmpty(link))
            {
                try
                {
                    if (this.ValidateUrl(link))
                    {
                        this.bs.DataSource = new List<link_sources>();
                        this.FormCursorWait(true);
                        this.browser.Navigate(link);
                    }
                    else
                    {
                        MessageBox.Show("Please enter a valid link", "Message");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Exception");
                }
            }
            else
            {
                this.textBox1.Focus();
            }
        }

        private void OnDownloadImages(object sender, EventArgs e)
        {
            if (this.linkSOurceList.Count > 0)
            {
                new download_form(this.linkSOurceList, this.defaultLocation, this.textBox1.Text) { Text = "Select file type"}.ShowDialog();
            }
            else
            {
                MessageBox.Show("There was no item to download.", "Message");
            }
        }

        private void OnVirtualBrowserDocsComplete(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            try
            {
                if (this.browser.ReadyState == WebBrowserReadyState.Complete)
                {
                    this.DoLoadLinkResources();
                }
            }
            catch { }
        }

        private void DoLoadLinkResources()
        {
            try
            {
                var counter = 1;
                var tmpList = new List<link_sources>();

                foreach (HtmlElement item in this.browser.Document.GetElementsByTagName("IMG"))
                {
                    var uri = new Uri(item.GetAttribute("src"));
                    if (!string.IsNullOrEmpty(Path.GetExtension(uri.AbsolutePath)))
                    {
                        var src = item.GetAttribute("src");
                        tmpList.Add(new link_sources() { No = counter++, ImageSource = src, ImageExtension =  src.Split('.').Last().ToLower() });
                    }
                }

                this.linkSOurceList = tmpList.Distinct().ToList();
                this.bs.DataSource = this.linkSOurceList;
            }
            catch { }
            finally
            {
                this.bs.ResetBindings(true);
                this.Text = string.Format("{0} | Ready", this.appName);
                this.FormCursorWait(false);
            }
        }

        private void FormCursorWait(bool status)
        {
            if (status)
            {
                this.Cursor = Cursors.WaitCursor;
            }
            else
            {
                this.Cursor = Cursors.Default;
            }
        }

        private bool ValidateUrl(string link) 
        {
            Uri uriResult;
            return Uri.TryCreate(link, UriKind.Absolute, out uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }
    }
}

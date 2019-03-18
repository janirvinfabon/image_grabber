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
    public partial class download_form : Form
    {
        private img_types img_types = new img_types();
        private List<link_sources> linkSOurceList = new List<link_sources>();

        private Thread th;
        private string defaultLocation = string.Empty;
        private string linkName = string.Empty;

        public download_form(List<link_sources> link_source_list, string default_loc, string link_name)
        {
            InitializeComponent();

            this.linkSOurceList = link_source_list;
            this.defaultLocation = default_loc;
            this.linkName = link_name.Split('/').Last();

            this.button1.Click += this.OnDownload;
            this.comboBox1.DataSource = this.img_types.Fetch(link_source_list.GroupBy(x => x.ImageExtension).Select(x => x.First()).ToList());
            this.comboBox1.DisplayMember = "ImageType";
            this.comboBox1.ValueMember = "Extension";
        }

        private void OnDownload(object sender, EventArgs e)
        {
            var type = this.comboBox1.SelectedValue.ToString();
            if (type == "all")
            {
                this.DownLoadNow(this.linkSOurceList);
            }
            else
            {
                this.DownLoadNow(this.linkSOurceList.Where(x => x.ImageExtension == type).ToList());
            }
        }

        private void DownLoadNow(List<link_sources> list) 
        {
            this.button1.Enabled = false;
            this.progressBar1.Visible = true;

            new Thread(() =>
            {
                var total = 0;
                list.ForEach((x) =>
                {
                    var link = x.ImageSource;
                    var name = x.ImageSource.Split('/').Last().Replace("%", "");
                    var file = string.Format(@"{0}\{1}\{2}", 
                        this.defaultLocation, this.linkName, name);

                    try
                    {
                        this.check_folder(file);                            // create a seperate directory for each link
                        new WebClient().DownloadFile(link, file);           // download the file
                        total++;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                });

                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() => 
                    {
                        if (total > 0)
                        {
                            MessageBox.Show(string.Format("You have successfully downloaded {0} images", 
                                total.ToString("#, ###, ###").Trim()), "Success");
                            this.Close();
                            this.Dispose();
                        }
                        else
                        {
                            this.button1.Enabled = true;
                            MessageBox.Show("Failed to download image, try again.", "Try again");
                        }
                    }));
                }

            }).Start();
        }

        private void check_folder(string file)
        {
            var dir = Path.GetDirectoryName(file);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }
    }
}

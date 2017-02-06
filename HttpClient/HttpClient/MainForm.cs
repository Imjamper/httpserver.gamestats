using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using System.Web;

namespace HttpClient
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            methodTypecb.SelectedIndex = 0;
            urlcb.SelectedIndex = 0;
        }

        private async void startbtn_Click(object sender, EventArgs e)
        {
            var url = urlcb.Items[urlcb.SelectedIndex] != null ? urlcb.Items[urlcb.SelectedIndex].ToString() : String.Empty;
            if(!String.IsNullOrEmpty(url))
            {
                var response = await DownloadPage(url);
                if (response != null)
                    responsetb.Text = response;
            }
        }

        private async Task<string> DownloadPage(string url)
        {
            using (var client = new System.Net.Http.HttpClient())
            {
                var httpContent = new StringContent(bodytb.Text, Encoding.UTF8, "application/json");
                var methodType = methodTypecb.Items[methodTypecb.SelectedIndex] != null ? methodTypecb.Items[methodTypecb.SelectedIndex].ToString() : String.Empty;
                if(!String.IsNullOrEmpty(methodType))
                {
                    switch (methodType)
                    {
                        case "PUT":
                            using (var r = await client.PutAsync(new Uri(url, UriKind.Absolute), httpContent))
                            {
                                string result = await r.Content.ReadAsStringAsync();
                                return result;
                            };
                        case "GET":
                            using (var r = await client.GetAsync(new Uri(url, UriKind.Absolute)))
                            {
                                string result = await r.Content.ReadAsStringAsync();
                                return result;
                            };
                        case "POST":
                            using (var r = await client.PostAsync(new Uri(url, UriKind.Absolute), httpContent))
                            {
                                string result = await r.Content.ReadAsStringAsync();
                                return result;
                            };
                        case "DELETE":
                            using (var r = await client.DeleteAsync(new Uri(url, UriKind.Absolute)))
                            {
                                string result = await r.Content.ReadAsStringAsync();
                                return result;
                            };
                        default: return null;
                    }
                }
                return null;
            }
        }
    }
}

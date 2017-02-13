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
            InitCombobox();
        }

        private void InitCombobox()
        {
            urlcb.Items.Add(new Item("http://localhost:8080/servers/localhost-9999/info", 1));
            urlcb.Items.Add(new Item("http://localhost:8080/servers/localhost-9999/matches/2017-01-22T15:17:00Z", 2));
            urlcb.Items.Add(new Item("http://localhost:8080/servers/info", 3));
            urlcb.Items.Add(new Item("http://localhost:8080/servers/localhost-9999/stats", 4));
            urlcb.Items.Add(new Item("http://localhost:8080/players/\" >> Sniper Heaven <</stats", 5));
            urlcb.Items.Add(new Item("http://localhost:8080/reports/recent-matches[/2]", 6));
            urlcb.Items.Add(new Item("http://localhost:8080/reports/best-players[/2]", 7));
            urlcb.Items.Add(new Item("http://localhost:8080/reports/popular-servers[/2]", 8));
            urlcb.DisplayMember = "Name";
            urlcb.ValueMember = "Id";

            methodTypecb.Items.Add(new Item("PUT", 1));
            methodTypecb.Items.Add(new Item("GET", 2));
            methodTypecb.Items.Add(new Item("POST", 3));
            methodTypecb.Items.Add(new Item("DELETE", 4));
            methodTypecb.DisplayMember = "Name";
            methodTypecb.ValueMember = "Id";
        }

        private async void startbtn_Click(object sender, EventArgs e)
        {
            var url = urlcb.Text;
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
                var methodType = methodTypecb.Text;
                if(!String.IsNullOrEmpty(methodType))
                {
                    switch (methodType)
                    {
                        case "PUT":
                            using (var r = await client.PutAsync(new Uri(url, UriKind.Absolute), httpContent))
                            {
                                string result = await r.Content.ReadAsStringAsync();
                                result += "StatusCode: " + r.StatusCode.ToString();
                                return result;
                            };
                        case "GET":
                            using (var r = await client.GetAsync(new Uri(url, UriKind.Absolute)))
                            {
                                string result = await r.Content.ReadAsStringAsync();
                                result += "StatusCode: " + r.StatusCode.ToString();
                                return result;
                            };
                        case "POST":
                            using (var r = await client.PostAsync(new Uri(url, UriKind.Absolute), httpContent))
                            {
                                string result = await r.Content.ReadAsStringAsync();
                                result += "StatusCode: " + r.StatusCode.ToString();
                                return result;
                            };
                        case "DELETE":
                            using (var r = await client.DeleteAsync(new Uri(url, UriKind.Absolute)))
                            {
                                string result = await r.Content.ReadAsStringAsync();
                                result += "StatusCode: " + r.StatusCode.ToString();
                                return result;
                            };
                        default: return null;
                    }
                }
                return null;
            }
        }
    }

    public class Item
    {
        public Item(string name, int id)
        {
            Name = name;
            Id = id;
        }

        public string Name { get; set; }

        public int Id { get; set; }
    }
}

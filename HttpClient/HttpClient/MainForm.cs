﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using System.Timers;
using System.Web;
using Kontur.GameStats.Server.Dto;
using Newtonsoft.Json;
using Timer = System.Timers.Timer;

namespace HttpClient
{
    public partial class MainForm : Form
    {
        private readonly BackgroundWorker _sendServerWorker = new BackgroundWorker();
        private readonly BackgroundWorker _sendMatch = new BackgroundWorker();
        private static bool _isRunning;
        private System.Timers.Timer _timer;
        private static ServerDto _serverDto;
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
            _sendServerWorker.RunWorkerCompleted += _sendServerWorker_RunWorkerCompleted;
            _sendServerWorker.DoWork += _sendServerWorker_DoWork;
            _sendServerWorker.WorkerSupportsCancellation = true;

            _sendMatch.RunWorkerCompleted += _sendMatch_RunWorkerCompleted;
            _sendMatch.DoWork += _sendMatch_DoWork;
            _sendMatch.WorkerSupportsCancellation = true;
            _timer = new Timer();
            _timer.AutoReset = true;
            _timer.Interval = 500;
            _timer.Elapsed += _timer_Elapsed;
        }

        private void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (_sendMatch.IsBusy) return;
            var match = RandomGenerator.GetMatch();
            _sendMatch.RunWorkerAsync(match);
        }

        private void _sendMatch_DoWork(object sender, DoWorkEventArgs e)
        {
            var match = e.Argument as MatchDto;
            var date = DateTime.Now.ToUniversalTime()
                         .ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
            var url = $"http://localhost:8080/servers/{_serverDto.Endpoint}/matches/{date}";
            var result = DownloadPage(url, JsonConvert.SerializeObject(match.Results), "PUT");
            e.Result = result;
        }

        private void _sendMatch_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Invoke(new MethodInvoker(() =>
            {
                logstb.AppendText(@"Send match request: " + Environment.NewLine);
                logstb.AppendText(e.Result + Environment.NewLine);
            }));
        }

        private void _sendServerWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var server = e.Argument as ServerDto;
            var url = $"http://localhost:8080/servers/{server.Endpoint}/info";
            var result = DownloadPage(url, JsonConvert.SerializeObject(server.Info), "PUT");
            e.Result = result;
        }

        private void _sendServerWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Invoke(new MethodInvoker(() =>
            {
                logstb.AppendText(@"Advertise server request: " + Environment.NewLine);
                logstb.AppendText(e.Result + Environment.NewLine);
                _timer.Start();
                _timer.Enabled = true;
            }));
        }

        private async void startbtn_Click(object sender, EventArgs e)
        {
            var url = urlcb.Text;
            if(!String.IsNullOrEmpty(url))
            {
                var response = await DownloadPageAsync(url, bodytb.Text, methodTypecb.Text);
                if (response != null)
                    logstb.Text = response + Environment.NewLine;
            }
        }

        private async Task<string> DownloadPageAsync(string url, string json, string methodType)
        {
            using (var client = new System.Net.Http.HttpClient())
            {
                var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
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

        private static string DownloadPage(string url, string json, string methodType)
        {
            using (var client = new System.Net.Http.HttpClient())
            {
                var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                if (!String.IsNullOrEmpty(methodType))
                {
                    switch (methodType)
                    {
                        case "PUT":
                            try
                            {
                                using (var r = client.PutAsync(new Uri(url, UriKind.Absolute), httpContent).GetAwaiter().GetResult())
                                {
                                    string result = r.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                                    result += "StatusCode: " + r.StatusCode + Environment.NewLine;
                                    return result;
                                }
                            }
                            catch (HttpRequestException)
                            {
                                return @"Server is shout down";
                            }
                        case "GET":
                            using (var r = client.GetAsync(new Uri(url, UriKind.Absolute)).GetAwaiter().GetResult())
                            {
                                string result = r.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                                result += "StatusCode: " + r.StatusCode + Environment.NewLine;
                                return result;
                            };
                        case "POST":
                            using (var r = client.PostAsync(new Uri(url, UriKind.Absolute), httpContent).GetAwaiter().GetResult())
                            {
                                string result = r.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                                result += "StatusCode: " + r.StatusCode + Environment.NewLine;
                                return result;
                            };
                        case "DELETE":
                            using (var r = client.DeleteAsync(new Uri(url, UriKind.Absolute)).GetAwaiter().GetResult())
                            {
                                string result = r.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                                result += "StatusCode: " + r.StatusCode + Environment.NewLine;
                                return result;
                            };
                        default: return null;
                    }
                }
                return null;
            }
        }

        private void runautobt_Click(object sender, EventArgs e)
        {
            if (_isRunning)
            {
                _timer.Stop();
                startbtn.Enabled = true;
                urlcb.Enabled = true;
                methodTypecb.Enabled = true;
                runautobt.Text = @"Run Automatically";
                if (_sendServerWorker.IsBusy)
                    _sendServerWorker.CancelAsync();
                if (_sendMatch.IsBusy)
                    _sendMatch.CancelAsync();
            }
            else
            {
                startbtn.Enabled = false;
                urlcb.Enabled = false;
                methodTypecb.Enabled = false;
                runautobt.Text = @"Stop";
                if (_serverDto == null)
                    _serverDto = RandomGenerator.GetServer();
                _sendServerWorker.RunWorkerAsync(_serverDto);
                _isRunning = true;
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

using System;
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
using GL.HttpServer.Extensions;
using Kontur.GameStats.Server.Dto;
using Newtonsoft.Json;
using Timer = System.Timers.Timer;

namespace HttpClient
{
    public partial class MainForm : Form
    {
        private readonly BackgroundWorker _sendServerWorker = new BackgroundWorker();
        private readonly BackgroundWorker _sendMatch = new BackgroundWorker();
        private readonly BackgroundWorker _reportsWorker = new BackgroundWorker();
        private static bool _isRunning;
        private System.Timers.Timer _timer;
        private System.Timers.Timer _reportsTimer;
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

            _reportsWorker.RunWorkerCompleted += _reportsWorker_RunWorkerCompleted;
            _reportsWorker.DoWork += _reportsWorker_DoWork;
            _reportsWorker.WorkerSupportsCancellation = true;

            _timer = new Timer();
            _timer.AutoReset = true;
            _timer.Interval = 500;
            _timer.Elapsed += _timer_Elapsed;

            _reportsTimer = new Timer();
            _reportsTimer.AutoReset = true;
            _reportsTimer.Interval = 2000;
            _reportsTimer.Elapsed += _reportTimer_Elapsed;
        }

        private void _reportsWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var url = $"http://localhost:8080/servers/{_serverDto.Endpoint}/stats";
            var result = DownloadPage(url, null, "GET");
            e.Result = result;
        }

        private void _reportsWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var response = e.Result as ClientResponse;
            Invoke(new MethodInvoker(() =>
            {
                if (response != null)
                {
                    response.Stopwatch.Stop();
                    logstb.AppendText($"Get server stats: {response.StatusCode}. Elapsed: {response.Stopwatch.ElapsedMilliseconds} ms");
                    if (!response.ErrorMessage.IsNullOrEmpty())
                        logstb.AppendText($"  Error: {response.ErrorMessage}");
                    if (!String.IsNullOrEmpty(response.JsonString))
                        logstb.AppendText(response.JsonString);
                }
                logstb.AppendText(Environment.NewLine);
            }));
        }

        private void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (_sendMatch.IsBusy) return;
            var match = RandomGenerator.GetMatch();
            _sendMatch.RunWorkerAsync(match);
        }

        private void _reportTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (_reportsWorker.IsBusy) return;
            _reportsWorker.RunWorkerAsync();
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
            var response = e.Result as ClientResponse;
            Invoke(new MethodInvoker(() =>
            {
                if (response != null)
                {
                    response.Stopwatch.Stop();
                    logstb.AppendText($"Send match request: {response.StatusCode}. Elapsed: {response.Stopwatch.ElapsedMilliseconds} ms" );
                    if (!response.ErrorMessage.IsNullOrEmpty())
                        logstb.AppendText($"  Error: {response.ErrorMessage}");
                }
                logstb.AppendText(Environment.NewLine);
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
            var response = e.Result as ClientResponse;
            Invoke(new MethodInvoker(() =>
            {
                response.Stopwatch.Stop();
                logstb.AppendText($"Advertise server request: {response.StatusCode}. Elapsed: {response.Stopwatch.ElapsedMilliseconds} ms");
                if (!response.ErrorMessage.IsNullOrEmpty())
                    logstb.AppendText($"  Error: {response.ErrorMessage}");
                logstb.AppendText(Environment.NewLine);
                _timer.Start();
                _reportsTimer.Start();
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

        private ClientResponse DownloadPage(string url, string json, string methodType)
        {
            using (var client = new System.Net.Http.HttpClient())
            {
                HttpContent httpContent = null;
                if (!json.IsNullOrEmpty())
                    httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                var response = new ClientResponse();
                response.Stopwatch.Start();
                if (!String.IsNullOrEmpty(methodType))
                {
                    try
                    {
                        switch (methodType)
                        {
                            case "PUT":
                                using (var r = client.PutAsync(new Uri(url, UriKind.Absolute), httpContent).GetAwaiter().GetResult())
                                {
                                    string result = r.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                                    response.JsonString = result;
                                    response.StatusCode = r.StatusCode.ToString();
                                    break;
                                }
                            case "GET":
                                using (var r = client.GetAsync(new Uri(url, UriKind.Absolute)).GetAwaiter().GetResult())
                                {
                                    string result = r.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                                    response.JsonString = result;
                                    response.StatusCode = r.StatusCode.ToString();
                                    break;
                                }
                            case "POST":
                                using (var r = client.PostAsync(new Uri(url, UriKind.Absolute), httpContent).GetAwaiter().GetResult())
                                {
                                    string result = r.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                                    response.JsonString = result;
                                    response.StatusCode = r.StatusCode.ToString();
                                    break;
                                }
                            case "DELETE":
                                using (var r = client.DeleteAsync(new Uri(url, UriKind.Absolute)).GetAwaiter().GetResult())
                                {
                                    string result = r.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                                    response.JsonString = result;
                                    response.StatusCode = r.StatusCode.ToString();
                                    break;
                                }
                            default:
                                response.StatusCode = @"888";
                                break;
                        }
                    }
                    catch (HttpRequestException)
                    {
                        response.ErrorMessage = @"Server is shout down";
                        response.StatusCode = @"888";
                        return response;
                    }
                    catch (Exception ex)
                    {
                        response.ErrorMessage = ex.Message;
                        response.StatusCode = @"888";
                        return response;
                    }
                }
                return response;
            }
        }

        private void runautobt_Click(object sender, EventArgs e)
        {
            if (_isRunning)
            {
                _timer.Stop();
                _reportsTimer.Stop();
                startbtn.Enabled = true;
                urlcb.Enabled = true;
                methodTypecb.Enabled = true;
                runautobt.Text = @"Run Automatically";
                if (_sendServerWorker.IsBusy)
                    _sendServerWorker.CancelAsync();
                if (_sendMatch.IsBusy)
                    _sendMatch.CancelAsync();
                _isRunning = false;
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

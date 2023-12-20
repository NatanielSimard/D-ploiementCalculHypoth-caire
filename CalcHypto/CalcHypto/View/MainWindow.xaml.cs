
using CalcHypto.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Windows;

namespace CalcHypto.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new SimulationVM();
            var assembly = Assembly.GetEntryAssembly();
            this.version.Content = $"Current Version : {assembly?.GetName().Version}";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Historique vueHis = new Historique();
            vueHis.DataContext = new InfoAvanceVM(((SimulationVM)DataContext).SimulationSelectioner);
            vueHis.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            vueHis.ShowDialog();
        }
        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string reposPath = "https://api.github.com/repos/NatanielSimard/D-ploiementCalculHypoth-caire/releases/latest";
            var client = new HttpClient()
            client.DefaultRequestHeaders.Add("User-Agent", "request"); 
            var response = await client.GetStringAsync(reposPath);
            var releaseInfo = JsonConvert.DeserializeObject<ReleaseInfo>(response);
            if (Assembly.GetEntryAssembly().GetName().Version.ToString() != releaseInfo.TagName)
            {
                DownloadAndUpdate(releaseInfo);
            }
        }
        public async void DownloadAndUpdate(ReleaseInfo releaseInfo)
        {
            string downloadUrl = releaseInfo.InstallerUrl;
            string localPath = Path.Combine(Path.GetTempPath(), "CalculHypo-download.exe");
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(downloadUrl, HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();

                using (var stream = await response.Content.ReadAsStreamAsync())
                using (var fileStream = new FileStream(localPath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    await stream.CopyToAsync(fileStream);
                }
                Process.Start(localPath);
                Application.Current.Shutdown();
            }
        }
        public class ReleaseInfo
        {
            [JsonProperty("tag_name")]
            public string TagName { get; set; }
            [JsonProperty("assets")]
            public List<GitHubAsset> Assets { get; set; }
            public string InstallerUrl => Assets.FirstOrDefault()?.BrowserDownloadUrl;
        }

        public class GitHubAsset
        {
            [JsonProperty("browser_download_url")]
            public string BrowserDownloadUrl { get; set; }
        }
    }
}

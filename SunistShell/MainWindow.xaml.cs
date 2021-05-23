using SunistShell.ViewModel.Index;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SunistShell
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            StatusInfo.ProgressValue = 30;
            StatusInfo.RootMode = true;
            Displayer.Items.Add(new DataGridRow() { Item = new { Col1 = "1-1", Col2 = "2-2", Col3 = "3-3" } });
        }

        private void OSIcon_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("SunistOS v0.0.1 beta 1\nCopyright (c) Sunist", "OS Info");
        }

        private void InstallTools_Click(object sender, RoutedEventArgs e)
        {
            Process proc = new System.Diagnostics.Process();
            proc.StartInfo.FileName = "https://www.visualstudio.com";
            proc.Start();
        }

        private void CompileTools_Click(object sender, RoutedEventArgs e)
        {
            Process proc = new System.Diagnostics.Process();
            proc.StartInfo.FileName = "https://www.visualstudio.com";
            proc.Start();
        }

        private void RuntimeEnvironment_Click(object sender, RoutedEventArgs e)
        {
            Process proc = new System.Diagnostics.Process();
            proc.StartInfo.FileName = "https://www.visualstudio.com";
            proc.Start();
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DocumentIcon_Click(object sender, RoutedEventArgs e)
        {
            Process proc = new System.Diagnostics.Process();
            proc.StartInfo.FileName = "https://www.visualstudio.com";
            proc.Start();
        }

        private void APIIcon_Click(object sender, RoutedEventArgs e)
        {
            Process proc = new System.Diagnostics.Process();
            proc.StartInfo.FileName = "https://www.visualstudio.com";
            proc.Start();
        }

        private void BiliIcon_Click(object sender, RoutedEventArgs e)
        {
            Process proc = new System.Diagnostics.Process();
            proc.StartInfo.FileName = "https://space.bilibili.com/25394898";
            proc.Start();
        }

        private void GitHubIcon_Click(object sender, RoutedEventArgs e)
        {
            Process proc = new System.Diagnostics.Process();
            proc.StartInfo.FileName = "https://www.visualstudio.com";
            proc.Start();
        }

        private void GiteeIcon_Click(object sender, RoutedEventArgs e)
        {
            Process proc = new System.Diagnostics.Process();
            proc.StartInfo.FileName = "https://www.visualstudio.com";
            proc.Start();
        }

        private void WebsiteIcon_Click(object sender, RoutedEventArgs e)
        {
            Process proc = new System.Diagnostics.Process();
            proc.StartInfo.FileName = "https://www.visualstudio.com";
            proc.Start();
        }
    }
}

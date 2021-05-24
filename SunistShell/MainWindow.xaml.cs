using SunistLibs.Core;
using SunistLibs.Core.Enums;
using SunistShell.ViewModel.Index;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
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
using SunistLibs.DataStructure.Output;
using Process = System.Diagnostics.Process;
using SunistShell.View;

namespace SunistShell
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private ProcessController controller;
        public MainWindow()
        {
            controller = new ProcessController();
            InitializeComponent();
            StatusInfo.ProgressValue = 30;
            StatusInfo.RootMode = true;
            
            controller.Display += OnControllerOnDisplay;
        }

        private bool OnControllerOnDisplay(DisplaySource source, DisplayMode mode)
        {
            switch (source.SourceName)
            {
                case "ProcessList":
                    Displayer.ItemsSource = source.DataTable.DefaultView;
                    StatusInfo.ProgressDescription = source.StatusDescription;
                    StatusInfo.ProgressValue = 0;
                    foreach (System.Data.DataRow x in source.DataTable.Rows)
                    {
                        Console.WriteLine($"Process Created: ID {x[0]}, CPU Time: {x[3]}");
                    }

                    StatusInfo.ProgressValue = 100;
                    StatusInfo.HistorySource.Add(new KeyValuePair<string, System.Data.DataTable>(InputBox.Text, source.DataTable));
                    if (StatusInfo.HistorySource.Count > 5)
                    {
                        StatusInfo.HistorySource.RemoveAt(0);
                    }

                    return true;
                default:
                    return true;
            }
        }

        private bool Controller_Display(SunistLibs.DataStructure.Output.DisplaySource displaySource, SunistLibs.Core.Enums.DisplayMode mode)
        {
            throw new NotImplementedException();
        }

        private void OSIcon_Click(object sender, RoutedEventArgs e)
        {
            new InfoView().Show();
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ulong i = 1;
            for (int ii = 0; ii < 10; ii++, i++)
            {
                controller.Create($"test{ii}", new MemoryBlock(), WeightType.Users, false);
            }
            controller.List(ProcessStatus.Ready);

            HistoryTree.Items.Add(this.InputBox.Text);
            if (HistoryTree.Items.Count > 5)
            {
                HistoryTree.Items.RemoveAt(0);
            }
        }

        private void HistoryTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            foreach(var x in StatusInfo.HistorySource)
            {
                if (x.Key == e.NewValue.ToString())
                {
                    Displayer.ItemsSource = x.Value.DefaultView;
                }
            }
        }

        // 用户自定义代码
        public T[] ArgumentTake<T>(string[] args, string argName, uint argCount)
        {
            // argument parser code
            throw new ArgumentException();
        }
        
        public void cal_add(params int[] arg)
        {
            ulong pid = controller.Create("calculator", new MemoryBlock(), WeightType.Users, true);
            int ans = 0;
            controller.Run<int>(pid, true, args =>
            {
                int[] nums = ArgumentTake<int>(args as string[], "add", 2);
                
                return nums[0] + nums[1];
                
            }, ref ans, arg);
        }
    }
}

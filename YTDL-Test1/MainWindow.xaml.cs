using Microsoft.Win32;
using System;
using System.IO;
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
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Windows.Media.TextFormatting;

namespace YTDL_Test1
{
    public partial class MainWindow : Window
    {
        private string path = Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Shell Folders", "{374DE290-123F-4565-9164-39C4925E467B}", String.Empty).ToString();

        private string SelectedURL = "";

        public MainWindow()
        {
            InitializeComponent();
            TextBox_Dizin.Text = path;
        }

        private ProcessStartInfo ProcInfo(string filename, string parameters)
        {
            ProcessStartInfo procInfo = new ProcessStartInfo();
            procInfo.FileName = filename;
            procInfo.Arguments = parameters;
            procInfo.UseShellExecute = false;
            procInfo.RedirectStandardOutput = true;
            procInfo.CreateNoWindow = true;

            return procInfo;
        }

        private void Button_ara_Click(object sender, RoutedEventArgs e)
        {


            SelectedURL = TextBox_URL.Text;
            string filename = "youtube-dl.exe";
            string parameters = "-F " + SelectedURL;

            ProcessStartInfo procInfo = ProcInfo(filename, parameters);

            using (Process proc = Process.Start(procInfo))
            {
                ComboBox_Formats.Items.Clear();

                proc.WaitForExit();

                byte s = 0;
                while (!proc.StandardOutput.EndOfStream)
                {
                    string line = proc.StandardOutput.ReadLine();

                    if (s++ > 2)
                        ComboBox_Formats.Items.Add(line);
                }
            }

        }

        private void Button_indirr_Click(object sender, RoutedEventArgs e)
        {
            if (ComboBox_Formats.SelectedItem == null) return;

            string selectedFormat = ComboBox_Formats.SelectedItem.ToString();
            string code = selectedFormat.Split(' ').ToArray()[0].ToString();
            string filename = "youtube-dl.exe";
            string parameters = $"-o {path}\\%(title)s.%(ext)s -f {code} {SelectedURL}";

            ProcessStartInfo prcInfo = ProcInfo(filename, parameters);
            using (Process proc = Process.Start(prcInfo))
            {
                proc.WaitForExit();
                MessageBox.Show("İNDİRME TAMAMLANDI...");
            }
        }

        private void Button_dizin_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = path;
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                path = dialog.FileName;
                TextBox_Dizin.Text = path;
            }
        }

        private void Window_MouseEnter(object sender, MouseEventArgs e)
        {
            string link = Clipboard.GetText();
            TextBox_URL.Text = link;
            
        }

        
    }
}

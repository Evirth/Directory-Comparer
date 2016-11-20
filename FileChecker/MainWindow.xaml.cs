using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

namespace DirectoryComparer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        FileListLoader _Files = new FileListLoader();
        public static string TrgPth { get; set; }
        public static string SrcPth { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void SourcePath_GotFocus(object sender, RoutedEventArgs e)
        {
            if (SourcePath.Text == "Source path...")
            {
                SourcePath.Text = "";
            }
        }

        private void SourcePath_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SourcePath.Text))
            {
                SourcePath.Text = "Source path...";
            }
        }

        private void SourcePathButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog _Dialog = new System.Windows.Forms.FolderBrowserDialog();
            _Dialog.ShowDialog();

            if (!string.IsNullOrEmpty(_Dialog.SelectedPath))
            {
                SourcePath.Text = _Dialog.SelectedPath;
                SrcPth = SourcePath.Text;
                SourceFilesTab.Items.Clear();
                OverFilesTab.Items.Clear();
                MissingFilesTab.Items.Clear();

                _Files.SourceFiles = FileListLoader.LoadFiles(SourcePath.Text);

                foreach (var file in _Files.SourceFiles)
                {
                    SourceFilesTab.Items.Add(file);
                }

                if (!TargetPath.Text.Equals("Target path..."))
                {
                    _Files.OverFiles = _Files.ShowOverFiles();
                    if (_Files.OverFiles.Count > 0)
                    {
                        foreach (var file in _Files.OverFiles)
                        {
                            OverFilesTab.Items.Add(file);
                        }
                    }
                    _Files.MissingFiles = _Files.ShowMissingFiles();
                    if (_Files.MissingFiles.Count > 0)
                    {
                        CopyMissingsButton.Visibility = Visibility.Visible;
                        foreach (var file in _Files.MissingFiles)
                        {
                            MissingFilesTab.Items.Add(file);
                        }
                    }
                }
            }
            else
            {
                SourcePath_LostFocus(sender, e);
            }
        }

        private void TargetPath_GotFocus(object sender, RoutedEventArgs e)
        {
            if (TargetPath.Text == "Target path...")
            {
                TargetPath.Text = "";
            }
        }

        private void TargetPath_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TargetPath.Text))
            {
                TargetPath.Text = "Target path...";
            }
        }

        private void TargetPathButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog _Dialog = new System.Windows.Forms.FolderBrowserDialog();
            _Dialog.ShowDialog();
            if (!string.IsNullOrEmpty(_Dialog.SelectedPath))
            {
                TargetPath.Text = _Dialog.SelectedPath;
                TrgPth = TargetPath.Text;
                TargetFilesTab.Items.Clear();
                OverFilesTab.Items.Clear();
                MissingFilesTab.Items.Clear();

                _Files.TargetFiles = FileListLoader.LoadFiles(TargetPath.Text);

                foreach (var file in _Files.TargetFiles)
                {
                    TargetFilesTab.Items.Add(file);
                }

                if (!SourcePath.Text.Equals("Source path..."))
                {
                    _Files.OverFiles = _Files.ShowOverFiles();
                    if (_Files.OverFiles.Count > 0)
                    {
                        foreach (var file in _Files.OverFiles)
                        {
                            OverFilesTab.Items.Add(file);
                        }
                    }
                    _Files.MissingFiles = _Files.ShowMissingFiles();
                    if (_Files.MissingFiles.Count > 0)
                    {
                        CopyMissingsButton.Visibility = Visibility.Visible;
                        foreach (var file in _Files.MissingFiles)
                        {
                            MissingFilesTab.Items.Add(file);
                        }
                    }
                }
            }
            else
            {
                TargetPath_LostFocus(sender, e);
            }
        }

        private void CopyMissingsButton_Click(object sender, RoutedEventArgs e)
        {
            if (_Files.MissingFiles.Count > 0)
            {
                System.Windows.Forms.FolderBrowserDialog _Dialog = new System.Windows.Forms.FolderBrowserDialog();
                _Dialog.ShowDialog();
                if (!string.IsNullOrEmpty(_Dialog.SelectedPath))
                {
                    _Files.Copy(TargetPath.Text, _Dialog.SelectedPath);
                } 
            }
            else
            {
                MessageBox.Show("No missing files!");
            }
        }

        private void SourceFilesTab_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Process.Start(SourcePath.Text + "\\" + SourceFilesTab.SelectedItem);
        }

        private void OverTab_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Process.Start(SourcePath.Text + "\\" + OverFilesTab.SelectedItem);
        }

        private void MissingFilesTab_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Process.Start(TargetPath.Text + "\\" + MissingFilesTab.SelectedItem);
        }

        private void TargetFilesTab_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Process.Start(TargetPath.Text + "\\" + TargetFilesTab.SelectedItem);
        }
    }
}

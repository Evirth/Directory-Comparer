using System;
using System.Collections.Generic;
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

namespace FileChecker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        FileListLoader _Files = new FileListLoader();

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
                SourceFilesTab.Items.Clear();
                _Files.SourceFiles = FileListLoader.LoadFiles(SourcePath.Text);

                foreach (var file in _Files.SourceFiles)
                {
                    SourceFilesTab.Items.Add(file);
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
                TargetPath.Text = "Target path..";
            }
        }

        private void TargetPathButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog _Dialog = new System.Windows.Forms.FolderBrowserDialog();
            _Dialog.ShowDialog();
            TargetPath.Text = _Dialog.SelectedPath;
        }
    }
}

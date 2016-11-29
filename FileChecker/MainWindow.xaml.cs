﻿using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Collections.Specialized;
using System.Collections.Generic;

namespace DirectoryComparer
{
    public partial class MainWindow : Window
    {
        FileListLoader _Files = new FileListLoader();
        StringCollection SelectedFilesToCopy = new StringCollection();

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
                CopyMissingsButton.IsEnabled = false;
                SourceFilter.IsEnabled = true;
                SourcePath.Text = _Dialog.SelectedPath;
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
                    _Files.OverFiles = _Files.ShowFilesAExceptB(_Files.SourceFiles, _Files.TargetFiles);
                    if (_Files.OverFiles.Count > 0)
                    {
                        foreach (var file in _Files.OverFiles)
                        {
                            OverFilesTab.Items.Add(file);
                        }
                    }
                    _Files.MissingFiles = _Files.ShowFilesAExceptB(_Files.TargetFiles, _Files.SourceFiles);
                    if (_Files.MissingFiles.Count > 0)
                    {
                        CopyMissingsButton.IsEnabled = true;
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
                CopyMissingsButton.IsEnabled = false;
                TargetFilter.IsEnabled = true;
                TargetPath.Text = _Dialog.SelectedPath;
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
                    _Files.OverFiles = _Files.ShowFilesAExceptB(_Files.SourceFiles, _Files.TargetFiles);
                    if (_Files.OverFiles.Count > 0)
                    {
                        foreach (var file in _Files.OverFiles)
                        {
                            OverFilesTab.Items.Add(file);
                        }
                    }
                    _Files.MissingFiles = _Files.ShowFilesAExceptB(_Files.TargetFiles, _Files.SourceFiles);
                    if (_Files.MissingFiles.Count > 0)
                    {
                        CopyMissingsButton.IsEnabled = true;
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
            if (MissingFilesTab.Items.Count > 0)
            {
                System.Windows.Forms.FolderBrowserDialog _Dialog = new System.Windows.Forms.FolderBrowserDialog();
                _Dialog.ShowDialog();
                if (!string.IsNullOrEmpty(_Dialog.SelectedPath))
                {
                    string[] FilteredMissings = new string[MissingFilesTab.Items.Count];
                    MissingFilesTab.Items.CopyTo(FilteredMissings, 0);
                    _Files.Copy(TargetPath.Text, _Dialog.SelectedPath, FilteredMissings);
                }
            }
            else
            {
                MessageBox.Show("No missing files!", "", MessageBoxButton.OK, MessageBoxImage.Warning);
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

        private void SourceFilter_GotFocus(object sender, RoutedEventArgs e)
        {
            if (SourceFilter.Text == "Filter...")
            {
                SourceFilter.Text = "";
            }
        }

        private void SourceFilter_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SourceFilter.Text))
            {
                SourceFilter.Text = "Filter...";
            }
        }

        private void TargetFilter_GotFocus(object sender, RoutedEventArgs e)
        {
            if (TargetFilter.Text == "Filter...")
            {
                TargetFilter.Text = "";
            }
        }

        private void TargetFilter_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TargetFilter.Text))
            {
                TargetFilter.Text = "Filter...";
            }
        }

        private void SourceFilter_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (!SourceFilter.Text.Equals("Filter...") && !SourcePath.Text.Equals("Source path..."))
            {
                if (SrcTab.IsSelected)
                {
                    SourceFilesTab.Items.Clear();
                    if (!string.IsNullOrWhiteSpace(SourceFilter.Text))
                    {
                        foreach (string str in _Files.SourceFiles)
                        {
                            if (str.Contains(SourceFilter.Text))
                            {
                                SourceFilesTab.Items.Add(str);
                            }
                        }
                    }
                    else
                    {
                        foreach (string str in _Files.SourceFiles)
                        {
                            SourceFilesTab.Items.Add(str);
                        }
                    }
                }
                else if (OverTab.IsSelected && !TargetPath.Text.Equals("Target path..."))
                {
                    OverFilesTab.Items.Clear();
                    if (!string.IsNullOrWhiteSpace(SourceFilter.Text))
                    {
                        foreach (string str in _Files.OverFiles)
                        {
                            if (str.Contains(SourceFilter.Text))
                            {
                                OverFilesTab.Items.Add(str);
                            }
                        }
                    }
                    else
                    {
                        foreach (string str in _Files.OverFiles)
                        {
                            OverFilesTab.Items.Add(str);
                        }
                    }
                }
                else if (MissTab.IsSelected && !TargetPath.Text.Equals("Target path..."))
                {
                    MissingFilesTab.Items.Clear();
                    if (!string.IsNullOrWhiteSpace(SourceFilter.Text))
                    {
                        foreach (string str in _Files.MissingFiles)
                        {
                            if (str.Contains(SourceFilter.Text))
                            {
                                MissingFilesTab.Items.Add(str);
                            }
                        }
                    }
                    else
                    {
                        foreach (string str in _Files.MissingFiles)
                        {
                            MissingFilesTab.Items.Add(str);
                        }
                    } 
                }
            }
        }

        private void TargetFilter_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (!TargetFilter.Text.Equals("Filter..."))
            {
                TargetFilesTab.Items.Clear();
                if (!string.IsNullOrWhiteSpace(TargetFilter.Text))
                {
                    foreach (string str in _Files.TargetFiles)
                    {
                        if (str.Contains(TargetFilter.Text))
                        {
                            TargetFilesTab.Items.Add(str);
                        }
                    }
                }
                else
                {
                    foreach (string str in _Files.TargetFiles)
                    {
                        TargetFilesTab.Items.Add(str);
                    }
                }
            }
        }

        private void SourceFilesList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (!SourceFilter.Text.Equals("Filter..."))
            {
                SourceFilesTab.Items.Clear();
                foreach (string str in _Files.SourceFiles)
                {
                    SourceFilesTab.Items.Add(str);
                }
                if (!TargetPath.Text.Equals("Target path..."))
                {
                    OverFilesTab.Items.Clear();
                    foreach (string str in _Files.OverFiles)
                    {
                        OverFilesTab.Items.Add(str);
                    }
                    MissingFilesTab.Items.Clear();
                    foreach (string str in _Files.MissingFiles)
                    {
                        MissingFilesTab.Items.Add(str);
                    }
                }
                SourceFilter.Text = "Filter..."; 
            }
        }

        private void SourceFilesTab_KeyDown(object sender, KeyEventArgs e)
        {
            if(Keyboard.IsKeyDown(Key.Enter))
            {
                foreach (string file in SourceFilesTab.SelectedItems)
                {
                    Process.Start(SourcePath.Text + "\\" + file);
                }
            }
            else if ((Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.C)) && SelectedFilesToCopy.Count > 0)
            {
                Clipboard.SetFileDropList(SelectedFilesToCopy);
            }
            else if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.A))
            {
                SourceFilesTab.SelectAll();
            }
        }

        private void OverFilesTab_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.Enter))
            {
                foreach (string file in OverFilesTab.SelectedItems)
                {
                    Process.Start(SourcePath.Text + "\\" + file);
                }
            }
            else if ((Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.C)) && SelectedFilesToCopy.Count > 0)
            {
                Clipboard.SetFileDropList(SelectedFilesToCopy);
            }
            else if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.A))
            {
                OverFilesTab.SelectAll();
            }
        }

        private void MissingFilesTab_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.Enter))
            {
                foreach (string file in MissingFilesTab.SelectedItems)
                {
                    Process.Start(TargetPath.Text + "\\" + file);
                }
            }
            else if ((Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.C)) && SelectedFilesToCopy.Count > 0)
            {
                Clipboard.SetFileDropList(SelectedFilesToCopy);
            }
            else if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.A))
            {
                MissingFilesTab.SelectAll();
            }
        }

        private void TargetFilesTab_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.Enter))
            {
                foreach (string file in TargetFilesTab.SelectedItems)
                {
                    Process.Start(TargetPath.Text + "\\" + file);
                }
            }
            else if ((Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.C)) && SelectedFilesToCopy.Count > 0)
            {
                Clipboard.SetFileDropList(SelectedFilesToCopy);
            }
            else if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.A))
            {
                TargetFilesTab.SelectAll();
            }
        }

        private void ContextMenuCopy_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedFilesToCopy.Count > 0)
            {
                Clipboard.SetFileDropList(SelectedFilesToCopy);
            }
        }

        private void SourceFilesTab_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            SelectedFilesToCopy.Clear();
            foreach (string file in SourceFilesTab.SelectedItems)
            {
                SelectedFilesToCopy.Add(SourcePath.Text + "\\" + file);
            }
        }

        private void OverFilesTab_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            SelectedFilesToCopy.Clear();
            foreach (string file in OverFilesTab.SelectedItems)
            {
                SelectedFilesToCopy.Add(SourcePath.Text + "\\" + file);
            }
        }

        private void MissingFilesTab_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            SelectedFilesToCopy.Clear();
            foreach (string file in MissingFilesTab.SelectedItems)
            {
                SelectedFilesToCopy.Add(TargetPath.Text + "\\" + file);
            }
        }

        private void TargetFilesTab_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            SelectedFilesToCopy.Clear();
            foreach (string file in TargetFilesTab.SelectedItems)
            {
                SelectedFilesToCopy.Add(TargetPath.Text + "\\" + file);
            }
        }
    }
}

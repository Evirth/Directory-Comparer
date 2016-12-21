using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Collections.Specialized;
using System.Windows.Controls;

namespace DirectoryComparer
{
    public partial class MainWindow
    {
        readonly FileListLoader _files = new FileListLoader();
        readonly StringCollection _selectedFilesToCopy = new StringCollection();
        public int PrevSelectedTabIndex;

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
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.ShowDialog();
            if (!string.IsNullOrEmpty(dialog.SelectedPath))
            {
                CopyMissingsButton.IsEnabled = false;
                SourceFilter.IsEnabled = true;
                SourcePath.Text = dialog.SelectedPath;
                SourceFilesTab.Items.Clear();
                OverFilesTab.Items.Clear();
                MissingFilesTab.Items.Clear();
                _files.SourceFiles = FileListLoader.LoadFiles(SourcePath.Text);

                foreach (var file in _files.SourceFiles)
                {
                    SourceFilesTab.Items.Add(file);
                }

                if (!TargetPath.Text.Equals("Target path..."))
                {
                    _files.OverFiles = _files.ShowFilesAExceptB(_files.SourceFiles, _files.TargetFiles);
                    if (_files.OverFiles.Count > 0)
                    {
                        foreach (var file in _files.OverFiles)
                        {
                            OverFilesTab.Items.Add(file);
                        }
                    }
                    _files.MissingFiles = _files.ShowFilesAExceptB(_files.TargetFiles, _files.SourceFiles);
                    if (_files.MissingFiles.Count > 0)
                    {
                        CopyMissingsButton.IsEnabled = true;
                        foreach (var file in _files.MissingFiles)
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
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.ShowDialog();

            if (!string.IsNullOrEmpty(dialog.SelectedPath))
            {
                CopyMissingsButton.IsEnabled = false;
                TargetFilter.IsEnabled = true;
                TargetPath.Text = dialog.SelectedPath;
                TargetFilesTab.Items.Clear();
                OverFilesTab.Items.Clear();
                MissingFilesTab.Items.Clear();
                _files.TargetFiles = FileListLoader.LoadFiles(TargetPath.Text);

                foreach (var file in _files.TargetFiles)
                {
                    TargetFilesTab.Items.Add(file);
                }

                if (!SourcePath.Text.Equals("Source path..."))
                {
                    _files.OverFiles = _files.ShowFilesAExceptB(_files.SourceFiles, _files.TargetFiles);
                    if (_files.OverFiles.Count > 0)
                    {
                        foreach (var file in _files.OverFiles)
                        {
                            OverFilesTab.Items.Add(file);
                        }
                    }
                    _files.MissingFiles = _files.ShowFilesAExceptB(_files.TargetFiles, _files.SourceFiles);
                    if (_files.MissingFiles.Count > 0)
                    {
                        CopyMissingsButton.IsEnabled = true;
                        foreach (var file in _files.MissingFiles)
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
                System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
                dialog.ShowDialog();
                if (!string.IsNullOrEmpty(dialog.SelectedPath))
                {
                    string[] filteredMissings = new string[MissingFilesTab.Items.Count];
                    MissingFilesTab.Items.CopyTo(filteredMissings, 0);
                    try
                    {
                        _files.Copy(TargetPath.Text, dialog.SelectedPath, filteredMissings);
                    }
                    catch (System.Exception exeption)
                    {
                        MessageBox.Show(exeption.Message, "Wystąpił błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
            else
            {
                MessageBox.Show("No missing files!", "", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void SourceFilesTab_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (SourceFilesTab.SelectedItems.Count > 0)
            {
                foreach (string file in SourceFilesTab.SelectedItems)
                {
                    Process.Start(SourcePath.Text + "\\" + file);
                } 
            }
        }

        private void OverFilesTab_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (OverFilesTab.SelectedItems.Count > 0)
            {
                foreach (string file in OverFilesTab.SelectedItems)
                {
                    Process.Start(SourcePath.Text + "\\" + file);
                } 
            }
        }

        private void MissingFilesTab_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (MissingFilesTab.SelectedItems.Count > 0)
            {
                foreach (string file in MissingFilesTab.SelectedItems)
                {
                    Process.Start(TargetPath.Text + "\\" + file);  
                }
            }
        }

        private void TargetFilesTab_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (TargetFilesTab.SelectedItems.Count > 0)
            {
                foreach (string file in TargetFilesTab.SelectedItems)
                {
                    Process.Start(TargetPath.Text + "\\" + file);
                } 
            }
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

        private void SourceFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!SourceFilter.Text.Equals("Filter...") && !SourcePath.Text.Equals("Source path..."))
            {
                if (SrcTab.IsSelected)
                {
                    SourceFilesTab.Items.Clear();
                    if (!string.IsNullOrWhiteSpace(SourceFilter.Text))
                    {
                        foreach (string str in _files.SourceFiles)
                        {
                            if (str.Contains(SourceFilter.Text))
                            {
                                SourceFilesTab.Items.Add(str);
                            }
                        }
                    }
                    else
                    {
                        foreach (string str in _files.SourceFiles)
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
                        foreach (string str in _files.OverFiles)
                        {
                            if (str.Contains(SourceFilter.Text))
                            {
                                OverFilesTab.Items.Add(str);
                            }
                        }
                    }
                    else
                    {
                        foreach (string str in _files.OverFiles)
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
                        foreach (string str in _files.MissingFiles)
                        {
                            if (str.Contains(SourceFilter.Text))
                            {
                                MissingFilesTab.Items.Add(str);
                            }
                        }
                    }
                    else
                    {
                        foreach (string str in _files.MissingFiles)
                        {
                            MissingFilesTab.Items.Add(str);
                        }
                    } 
                }
            }
        }

        private void TargetFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!TargetFilter.Text.Equals("Filter..."))
            {
                TargetFilesTab.Items.Clear();
                if (!string.IsNullOrWhiteSpace(TargetFilter.Text))
                {
                    foreach (string str in _files.TargetFiles)
                    {
                        if (str.Contains(TargetFilter.Text))
                        {
                            TargetFilesTab.Items.Add(str);
                        }
                    }
                }
                else
                {
                    foreach (string str in _files.TargetFiles)
                    {
                        TargetFilesTab.Items.Add(str);
                    }
                }
            }
        }

        private void SourceFilesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!SourceFilter.Text.Equals("Filter...") && PrevSelectedTabIndex != SourceFilesList.SelectedIndex)
            {
                PrevSelectedTabIndex = SourceFilesList.SelectedIndex;
                SourceFilesTab.Items.Clear();
                foreach (string str in _files.SourceFiles)
                {
                    SourceFilesTab.Items.Add(str);
                }
                if (!TargetPath.Text.Equals("Target path..."))
                {
                    OverFilesTab.Items.Clear();
                    foreach (string str in _files.OverFiles)
                    {
                        OverFilesTab.Items.Add(str);
                    }
                    MissingFilesTab.Items.Clear();
                    foreach (string str in _files.MissingFiles)
                    {
                        MissingFilesTab.Items.Add(str);
                    }
                }
                SourceFilter.Text = "Filter..."; 
            }
            PrevSelectedTabIndex = SourceFilesList.SelectedIndex;
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
            else if ((Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.C)) && _selectedFilesToCopy.Count > 0)
            {
                Clipboard.SetFileDropList(_selectedFilesToCopy);
            }
            else if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.A))
            {
                SourceFilesTab.SelectAll();
            }
            else if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.D))
            {
                SourceFilesTab.UnselectAll();
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
            else if ((Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.C)) && _selectedFilesToCopy.Count > 0)
            {
                Clipboard.SetFileDropList(_selectedFilesToCopy);
            }
            else if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.A))
            {
                OverFilesTab.SelectAll();
            }
            else if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.D))
            {
                OverFilesTab.UnselectAll();
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
            else if ((Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.C)) && _selectedFilesToCopy.Count > 0)
            {
                Clipboard.SetFileDropList(_selectedFilesToCopy);
            }
            else if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.A))
            {
                MissingFilesTab.SelectAll();
            }
            else if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.D))
            {
                MissingFilesTab.UnselectAll();
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
            else if ((Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.C)) && _selectedFilesToCopy.Count > 0)
            {
                Clipboard.SetFileDropList(_selectedFilesToCopy);
            }
            else if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.A))
            {
                TargetFilesTab.SelectAll();
            }
            else if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.D))
            {
                TargetFilesTab.UnselectAll();
            }
        }

        private void ContextMenuCopy_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedFilesToCopy.Count > 0)
            {
                Clipboard.SetFileDropList(_selectedFilesToCopy);
            }
        }

        private void ContextMenuSourceContaining_Click(object sender, RoutedEventArgs e)
        {
            string lastSelected = SourceFilesTab.SelectedItems[SourceFilesTab.SelectedItems.Count - 1].ToString();
            if (lastSelected.Contains("\\"))
            {
                lastSelected = lastSelected.Remove(lastSelected.LastIndexOf("\\")); 
            }
            else
            {
                lastSelected = "";
            }
            Process.Start(SourcePath.Text + "\\" + lastSelected);
        }

        private void ContextMenuSourceSelectAll_Click(object sender, RoutedEventArgs e)
        {
            SourceFilesTab.SelectAll();
        }

        private void ContextMenuSourceUnselectAll_Click(object sender, RoutedEventArgs e)
        {
            SourceFilesTab.UnselectAll();
        }

        private void ContextMenuOverContaining_Click(object sender, RoutedEventArgs e)
        {
            string lastSelected = OverFilesTab.SelectedItems[OverFilesTab.SelectedItems.Count - 1].ToString();
            if (lastSelected.Contains("\\"))
            {
                lastSelected = lastSelected.Remove(lastSelected.LastIndexOf("\\"));
            }
            else
            {
                lastSelected = "";
            }
            Process.Start(SourcePath.Text + "\\" + lastSelected);
        }

        private void ContextMenuOverSelectAll_Click(object sender, RoutedEventArgs e)
        {
            OverFilesTab.SelectAll();
        }

        private void ContextMenuOverUnselectAll_Click(object sender, RoutedEventArgs e)
        {
            OverFilesTab.UnselectAll();
        }

        private void ContextMenuMissingContaining_Click(object sender, RoutedEventArgs e)
        {
            string lastSelected = MissingFilesTab.SelectedItems[MissingFilesTab.SelectedItems.Count - 1].ToString();
            if (lastSelected.Contains("\\"))
            {
                lastSelected = lastSelected.Remove(lastSelected.LastIndexOf("\\"));
            }
            else
            {
                lastSelected = "";
            }
            Process.Start(TargetPath.Text + "\\" + lastSelected);
        }

        private void ContextMenuMissingSelectAll_Click(object sender, RoutedEventArgs e)
        {
            MissingFilesTab.SelectAll();
        }

        private void ContextMenuMissingUnselectAll_Click(object sender, RoutedEventArgs e)
        {
            MissingFilesTab.UnselectAll();
        }
        
        private void ContextMenuTargetContaining_Click(object sender, RoutedEventArgs e)
        {
            string lastSelected = TargetFilesTab.SelectedItems[TargetFilesTab.SelectedItems.Count - 1].ToString();
            if (lastSelected.Contains("\\"))
            {
                lastSelected = lastSelected.Remove(lastSelected.LastIndexOf("\\"));
            }
            else
            {
                lastSelected = "";
            }
            Process.Start(TargetPath.Text + "\\" + lastSelected);
        }

        private void ContextMenuTargetSelectAll_Click(object sender, RoutedEventArgs e)
        {
            TargetFilesTab.SelectAll();
        }

        private void ContextMenuTargetUnselectAll_Click(object sender, RoutedEventArgs e)
        {
            TargetFilesTab.UnselectAll();
        }

        private void SourceFilesTab_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedFilesToCopy.Clear();
            foreach (string file in SourceFilesTab.SelectedItems)
            {
                _selectedFilesToCopy.Add(SourcePath.Text + "\\" + file);
            }
        }

        private void OverFilesTab_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedFilesToCopy.Clear();
            foreach (string file in OverFilesTab.SelectedItems)
            {
                _selectedFilesToCopy.Add(SourcePath.Text + "\\" + file);
            }
        }

        private void MissingFilesTab_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedFilesToCopy.Clear();
            foreach (string file in MissingFilesTab.SelectedItems)
            {
                _selectedFilesToCopy.Add(TargetPath.Text + "\\" + file);
            }
        }

        private void TargetFilesTab_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedFilesToCopy.Clear();
            foreach (string file in TargetFilesTab.SelectedItems)
            {
                _selectedFilesToCopy.Add(TargetPath.Text + "\\" + file);
            }
        }

        private void SourcePath_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!SourcePath.Text.Equals("Source path..."))
            {
                Process.Start(SourcePath.Text);
            }
        }

        private void TargetPath_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!TargetPath.Text.Equals("Target path..."))
            {
                Process.Start(TargetPath.Text);
            }
        }
    }
}

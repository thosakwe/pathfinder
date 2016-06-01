using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PathFinder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<string> Directories = new List<string>();
        List<string> Files = new List<string>();

        public MainWindow()
        {
            InitializeComponent();
        }

        void LoadPath()
        {
            Directories.Clear();
            Files.Clear();
            problems.Items.Clear();
            results.Items.Clear();

            string PATH = Environment.GetEnvironmentVariable("PATH");

            foreach (string dir_ in PATH.Split(';'))
            {
                string dir = Regex.Replace(dir_.Trim(), "\\\\+$", "").Trim();

                if (string.IsNullOrEmpty(dir))
                    continue;

                if (Directories.Contains(dir))
                {
                    problems.Items.Add($"Duplicate directory: {dir}");
                }
                else if (Directory.Exists(dir) && !Directories.Contains(dir))
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(dir);
                    TreeViewItem root = new TreeViewItem();
                    root.Header = dir;

                    foreach (FileInfo program in directoryInfo.GetFilesByExtensions(
                        // TODO: All executable extensions
                        ".exe", ".bat", ".com", ".cmd", ".bin", ".cpl", ".gadget"
                        ))
                    {
                        TreeViewItem child = new TreeViewItem();
                        child.Header = program.Name;

                        if (!Files.Contains(program.FullName))
                        {
                            Files.Add(program.FullName);
                        }
                        else
                        {
                            problems.Items.Add($"The same program is found in multiple entries: {program.FullName}");
                        }

                        root.Items.Add(child);
                    }

                    tvPrograms.Items.Add(root);
                }
                else
                {
                    problems.Items.Add($"Nonexistent directory in PATH: \"{dir}\"");
                }
            }
        }

        void Search()
        {
            string query = tbSearch.Text;

            if (!string.IsNullOrEmpty(query))
            {
                results.Items.Clear();
                tiSearch.Visibility = Visibility.Visible;

                Regex matcher = new Regex(query.Trim().ToLower(), RegexOptions.IgnoreCase);
                string[] matches = Files.Where(file =>
                {
                    return matcher.IsMatch(file);
                }).ToArray();

                foreach (string match in matches)
                {
                    results.Items.Add(match);
                }

                tabControl.SelectedItem = tiSearch;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LoadPath();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadPath();
        }

        private void button_Click_1(object sender, RoutedEventArgs e)
        {
            Search();
        }

        private void tbSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Search();
            }
        }
    }

    public static class Ext
    {
        public static IEnumerable<FileInfo> GetFilesByExtensions(this DirectoryInfo dirInfo, params string[] extensions)
        {
            var allowedExtensions = new HashSet<string>(extensions, StringComparer.OrdinalIgnoreCase);

            return dirInfo.GetFiles()
                          .Where(f => allowedExtensions.Contains(f.Extension));
        }
    }
}

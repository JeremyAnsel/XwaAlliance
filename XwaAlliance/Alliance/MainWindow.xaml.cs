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

namespace Alliance
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly Encoding _encoding = Encoding.GetEncoding("iso-8859-1");

        public MainWindow()
        {
            InitializeComponent();
            LoadToolItems();
            Update();
        }

        public List<ToolItem> ToolItems { get; } = new List<ToolItem>();

        public void Update()
        {
            this.DataContext = null;
            this.DataContext = this;
        }

        private void LoadToolItems()
        {
            var items = new List<ToolItem>();

            if (System.IO.File.Exists("AllianceTools.txt"))
            {
                string[] lines = System.IO.File.ReadAllLines("AllianceTools.txt", _encoding);

                for (int i = 0; i < lines.Length; i++)
                {
                    string line = lines[i];

                    if (string.IsNullOrWhiteSpace(line))
                    {
                        continue;
                    }

                    if (line.StartsWith("#") || line.StartsWith(";") || line.StartsWith("//"))
                    {
                        continue;
                    }

                    string[] parts = line.Split('|');

                    if (parts.Length < 3)
                    {
                        continue;
                    }

                    string name = parts[0].Trim();
                    string path = parts[1].Trim();
                    bool closeWindow;

                    if (bool.TryParse(parts[2].Trim(), out bool result))
                    {
                        closeWindow = result;
                    }
                    else
                    {
                        closeWindow = false;
                    }

                    items.Add(new ToolItem(name, path, closeWindow));
                }
            }
            else
            {
                items.Add(new ToolItem("Babu Frik's Configurator", "BabuFriksConfigurator.exe", true));
                items.Add(new ToolItem("Palpatine Total Converter", "PalpatineTotalConverter.exe", true));
            }

            items.Add(new ToolItem("Joystick Configurator", "XwaJoystickConfig.exe", false));

            this.ToolItems.Clear();

            foreach (var item in items)
            {
#if !DEBUG
                if (System.IO.File.Exists(item.Path))
#endif
                {
                    this.ToolItems.Add(item);
                }
            }

            this.Height += this.ToolItems.Count * 35;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
#if !DEBUG
            if (!System.IO.File.Exists("XWingAlliance.exe"))
            {
                MessageBox.Show("XWingAlliance.exe was not found.", this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
#endif
        }

        private void RunXwaProcess(string fileName, string arguments)
        {
            this.Hide();

            var process = new Process();
            process.StartInfo.FileName = fileName;
            process.StartInfo.Arguments = arguments;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.EnvironmentVariables["SHIM_MCCOMPAT"] = "0x800000001";
            process.Start();

            Thread.Sleep(1000);
            this.Close();
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            RunXwaProcess("XWingAlliance.exe", string.Empty);
        }

        private void PlaySkipIntroButton_Click(object sender, RoutedEventArgs e)
        {
            RunXwaProcess("XWingAlliance.exe", "skipintro");
        }

        private void PilotButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new PilotWindow
            {
                Owner = this
            };

            window.ShowDialog();
        }

        private void JoyCplButton_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("joy.cpl");
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ToolButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is Button button))
            {
                return;
            }

            if (!(button.Tag is ToolItem toolItem))
            {
                return;
            }

            if (string.IsNullOrEmpty(toolItem.Path) || !System.IO.File.Exists(toolItem.Path))
            {
                return;
            }

            Process.Start(toolItem.Path);

            if (toolItem.CloseWindow)
            {
                Thread.Sleep(1000);
                this.Close();
            }
        }

        private void OpenXwaDirectoryMenu_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(".");
        }

        private void RunCommandLineMenu_Click(object sender, RoutedEventArgs e)
        {
            string arguments = Microsoft.VisualBasic.Interaction.InputBox("Enter the command line to use to run X-Wing Alliance:", "Run XWA with Command Line");

            if (string.IsNullOrWhiteSpace(arguments))
            {
                return;
            }

            arguments = arguments.Trim();

            RunXwaProcess("XWingAlliance.exe", arguments);
        }

        private void RunProcdumpMenu_Click(object sender, RoutedEventArgs e)
        {
            if (!System.IO.File.Exists("procdump.exe"))
            {
                MessageBox.Show("procdump.exe was not found.", this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string prompt = "If XWA crashes, a crash dump will be generated in the XWA directory.\n"
                + "The filename of the crash dump will looks like XwingAlliance.exe_######_######.dmp\n"
                + "\n"
                + "Enter the command line to use to run X-Wing Alliance:";

            string arguments = Microsoft.VisualBasic.Interaction.InputBox(prompt, "Run XWA with Procdump");

            Process.Start("procdump.exe", "-e -ma -w xwingalliance.exe");
            Thread.Sleep(1000);
            RunXwaProcess("XWingAlliance.exe", arguments);
        }
    }
}

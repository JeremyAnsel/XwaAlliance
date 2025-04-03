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
using System.Windows.Shapes;

namespace Alliance
{
    /// <summary>
    /// Logique d'interaction pour ToolsWindow.xaml
    /// </summary>
    public partial class ToolsWindow : Window
    {
        public ToolItemList ToolItemList { get; private set; }

        public string ToolItemsMenu { get; }

        public List<ToolItem> ToolItems { get; } = new List<ToolItem>();

        public ToolsWindow(Window owner, ToolItemList toolItemList, string title, string menu)
        {
            InitializeComponent();

            this.Owner = owner;
            this.Title = title;
            this.ToolItemList = toolItemList;
            ToolItemsMenu = menu;
            LoadToolItems();
            this.DataContext = this;
        }

        private void LoadToolItems()
        {
            this.ToolItems.Clear();

            foreach (var item in this.ToolItemList.GetMenuItems(ToolItemsMenu))
            {
#if !DEBUG
                if (item.PathExists)
#endif
                {
                    this.ToolItems.Add(item);
                }
            }

            this.Height += this.ToolItems.Count * 35;
        }

        private void ToolButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button button)
            {
                return;
            }

            if (button.Tag is not ToolItem toolItem)
            {
                return;
            }

            if (toolItem.IsMenu)
            {
                //this.Hide();
                var dialog = new ToolsWindow(this, this.ToolItemList, toolItem.Name, toolItem.MenuPath + "\\" + toolItem.Name);
                dialog.ShowDialog();

                if (toolItem.CloseWindow)
                {
                    this.Close();
                }
                else
                {
                    this.Show();
                }

                return;
            }

            if (!toolItem.PathExists)
            {
                return;
            }

            Process.Start(toolItem.Path, toolItem.PathArguments);

            if (toolItem.CloseWindow)
            {
                Thread.Sleep(1000);
                this.Close();
            }
        }
    }
}

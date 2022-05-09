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
using System.Windows.Shapes;

namespace Alliance
{
    /// <summary>
    /// Logique d'interaction pour PilotWindow.xaml
    /// </summary>
    public partial class PilotWindow : Window
    {
        public PilotWindow()
        {
            InitializeComponent();

            this.LoadPilotList();
        }

        private void LoadPlayerButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.IsNewPlayerSelected())
            {
                ConfigHelpers.SetLastPilot(string.Empty);

                MessageBox.Show(this, "New Player Selected");
            }
            else
            {
                string pilot = (string)this.playerList.SelectedItem;
                ConfigHelpers.SetLastPilot(pilot);

                MessageBox.Show(this, "Player " + pilot + " Loaded");
            }
        }

        private void DeletePlayerButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.IsNewPlayerSelected())
            {
                return;
            }

            string pilot = (string)this.playerList.SelectedItem;
            ConfigHelpers.DeletePilot(pilot);
            ConfigHelpers.SetLastPilot(string.Empty);

            this.LoadPilotList();

            MessageBox.Show(this, "Player " + pilot + " Deleted");
        }

        private void CreatePlayerButton_Click(object sender, RoutedEventArgs e)
        {
            this.SelectNewPlayer();
            ConfigHelpers.SetLastPilot(string.Empty);

            MessageBox.Show(this, "New Player Selected");
        }

        private void DoneButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void LoadPilotList()
        {
            List<string> pilots = ConfigHelpers.GetPilotList();
            string lastPilot = ConfigHelpers.GetLastPilot();
            int pilotIndex = pilots.FindIndex(t => t.Equals(lastPilot, StringComparison.OrdinalIgnoreCase));

            this.playerList.Items.Clear();

            foreach (string pilot in pilots)
            {
                this.playerList.Items.Add(pilot);
            }

            this.playerList.Items.Add("Create New Player");

            if (pilotIndex == -1)
            {
                this.SelectNewPlayer();
            }
            else
            {
                this.playerList.SelectedIndex = pilotIndex;
            }
        }

        private bool IsNewPlayerSelected()
        {
            return this.playerList.SelectedIndex == this.playerList.Items.Count - 1;
        }

        private void SelectNewPlayer()
        {
            this.playerList.SelectedIndex = this.playerList.Items.Count - 1;
        }
    }
}

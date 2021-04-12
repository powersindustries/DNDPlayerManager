using DnDPlayerManager.View;
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
using System.Xml;

namespace DnDPlayerManager
{
    public partial class MainWindow : Window
    {


        // ----------------------------------------------------------------
        // ----------------------------------------------------------------
        public MainWindow()
        {
            InitializeComponent();

            InitializePlayerManagers();
        }


        // ----------------------------------------------------------------
        // ----------------------------------------------------------------
        private void InitializePlayerManagers()
        {
            string saveDirectory = System.IO.Path.Combine( Directory.GetCurrentDirectory(), "..\\..\\Data\\Save\\");
            string[] files = Directory.GetFiles(saveDirectory);

            if (files.Count() > 0)
            {
                for (int x = 0; x < files.Count(); ++x)
                {
                    string currentFile = files[x];

                    AddNewTab();

                    TabItem newTab = MainTabcontrol.Items.GetItemAt(MainTabcontrol.Items.Count -2) as TabItem;
                    PlayerManager newPlayerManager = newTab.Content as PlayerManager;

                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(currentFile);

                    XmlNode characterNode = xmlDoc.SelectSingleNode("Character");

                    newTab.Header = characterNode.Attributes["Name"].Value;

                    newPlayerManager.NameTextbox.Text = characterNode.Attributes["Name"].Value;
                    newPlayerManager.CampaignTextbox.Text = characterNode.Attributes["CampaignName"].Value;
                    newPlayerManager.LevelCombobox.SelectedItem = characterNode.Attributes["Level"].Value;
                    newPlayerManager.RaceCombobox.SelectedItem = characterNode.Attributes["Race"].Value;
                    newPlayerManager.ClassCombobox.SelectedItem = characterNode.Attributes["Class"].Value;
                    newPlayerManager.SubclassCombobox.SelectedItem = characterNode.Attributes["Subclass"].Value;
                    newPlayerManager.MaxHPTextBox.Text = characterNode.Attributes["MaxHP"].Value;
                    newPlayerManager.CurrentHPTextbox.Text = newPlayerManager.MaxHPTextBox.Text;

                    newPlayerManager.NPCManagerStackpanel.Children.Clear();
                    XmlNodeList npcNodeList = characterNode.SelectNodes("NPCs/NPC");
                    for (int y=0; y < npcNodeList.Count; ++y)
                    {
                        XmlNode currentNode = npcNodeList[y];
                        newPlayerManager.AddNewNPCSlot();
                        
                        NPCSlot newSlot = newPlayerManager.NPCManagerStackpanel.Children[newPlayerManager.NPCManagerStackpanel.Children.Count - 1] as NPCSlot;
                        newSlot.NameNPCTextbox.Text = currentNode.Attributes["Name"].Value;
                        newSlot.NotesNPCTextbox.Text = currentNode.Attributes["Notes"].Value;
                    }

                    XmlNode notesNode = characterNode.SelectSingleNode("GeneralNotes");
                    newPlayerManager.GeneralNotesTextbox.Text = notesNode.Attributes["Notes"].Value;
                }
            }
            else
            {
                AddNewTab();
            }
        } 


        // ----------------------------------------------------------------
        // ----------------------------------------------------------------
        public void AddNewTab()
        {
            // Remove old 'Add' button
            if (MainTabcontrol.Items.Count > 0)
            {
                TabItem lastTab = MainTabcontrol.Items.GetItemAt(MainTabcontrol.Items.Count -1) as TabItem;
                MainTabcontrol.Items.Remove(lastTab);
            }

            // Add New "Character" tab
            TabItem addCharacterTabItem = new TabItem();
            addCharacterTabItem.Header  = "Character";
            addCharacterTabItem.Height  = 30;
            addCharacterTabItem.Content = new PlayerManager();
            MainTabcontrol.Items.Add(addCharacterTabItem);

            // Add new 'Add' button to the end
            TabItem addNewTabItem   = new TabItem();
            addNewTabItem.Header    = "+";
            addNewTabItem.GotFocus += AddNewPlayerTab_GotFocus;
            MainTabcontrol.Items.Add(addNewTabItem);

            // Set index to newly added PlayerManager
            MainTabcontrol.SelectedIndex = (MainTabcontrol.Items.Count -2);
        }


        // ----------------------------------------------------------------
        // ----------------------------------------------------------------
        private void AddNewPlayerTab_GotFocus(object sender, RoutedEventArgs e)
        {
            AddNewTab();
        }

    }
}

using DnDPlayerManager.Components;
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

namespace DnDPlayerManager.View
{
    public partial class PlayerManager : UserControl
    {
        int[] m_Levels = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 };

        RaceData m_RaceData;
        ClassData m_ClassData;


        // ----------------------------------------------------------------
        // ----------------------------------------------------------------
        public PlayerManager()
        {
            InitializeComponent();

            m_RaceData   = new RaceData();
            m_ClassData  = new ClassData();

            InitializePlayerInfo();
        }


        // ----------------------------------------------------------------
        // ----------------------------------------------------------------
        public void InitializePlayerInfo()
        {
            LevelCombobox.ItemsSource   = m_Levels;
            LevelCombobox.SelectedIndex = 0;

            RaceCombobox.ItemsSource   = m_RaceData.RaceList;
            RaceCombobox.SelectedIndex = -1;

            ClassCombobox.ItemsSource = m_ClassData.Classes;

            SubclassCombobox.IsEnabled = false;

            AddNewNPCSlot();
        }


        // ----------------------------------------------------------------
        // ----------------------------------------------------------------
        public void SavePlayerManager()
        {
            XmlDocument doc = new XmlDocument();
            XmlElement root = doc.CreateElement("Character");

            // Basic Player info
            XmlAttribute nameAttribute = doc.CreateAttribute("Name");
            nameAttribute.Value = NameTextbox.Text;
            root.Attributes.Append(nameAttribute);

            XmlAttribute campaignAttribute = doc.CreateAttribute("CampaignName");
            campaignAttribute.Value = CampaignTextbox.Text;
            root.Attributes.Append(campaignAttribute);

            XmlAttribute levelAttribute = doc.CreateAttribute("Level");
            levelAttribute.Value = LevelCombobox.SelectedItem != null ? LevelCombobox.SelectedItem.ToString() : "";
            root.Attributes.Append(levelAttribute);

            XmlAttribute raceAttribute = doc.CreateAttribute("Race");
            raceAttribute.Value = RaceCombobox.SelectedItem != null ? RaceCombobox.SelectedItem.ToString() : "";
            root.Attributes.Append(raceAttribute);

            XmlAttribute classAttribute = doc.CreateAttribute("Class");
            classAttribute.Value = ClassCombobox.SelectedItem != null ? ClassCombobox.SelectedItem.ToString() : "";
            root.Attributes.Append(classAttribute);

            XmlAttribute subclassAttribute = doc.CreateAttribute("Subclass");
            subclassAttribute.Value = SubclassCombobox.SelectedItem != null ? SubclassCombobox.SelectedItem.ToString() : "";
            root.Attributes.Append(subclassAttribute);

            XmlAttribute maxHPAttribute = doc.CreateAttribute("MaxHP");
            maxHPAttribute.Value = MaxHPTextBox.Text;
            root.Attributes.Append(maxHPAttribute);

            // NPC Slots
            XmlElement npcElement = doc.CreateElement("NPCs");
            List<XmlElement> npcElementList = GetNPCElementList(doc);
            for (int x=0; x < npcElementList.Count; ++x)
            {
                npcElement.AppendChild(npcElementList[x]);
            }
            root.AppendChild(npcElement);

            XmlElement generalNotesElement = doc.CreateElement("GeneralNotes");
            XmlAttribute notesAttribute = doc.CreateAttribute("Notes");
            notesAttribute.Value = GeneralNotesTextbox.Text != "Notes" ? GeneralNotesTextbox.Text : "";
            generalNotesElement.Attributes.Append(notesAttribute);
            root.AppendChild(generalNotesElement);

            doc.AppendChild(root);

            // Format xml
            StringWriter stringWriter = new StringWriter();
            XmlTextWriter textWriter = new XmlTextWriter(stringWriter);
            textWriter.Formatting = Formatting.Indented;
            doc.WriteTo(textWriter);

            string exportFilepath = System.IO.Path.Combine( Directory.GetCurrentDirectory(), "..\\..\\Data\\Save\\") + NameTextbox.Text + ".xml";
            File.WriteAllText(exportFilepath, stringWriter.ToString());

            MessageBox.Show("Save Complete.");
        }


        // ----------------------------------------------------------------
        // ----------------------------------------------------------------
        public List<XmlElement> GetNPCElementList(XmlDocument doc)
        {
            List<XmlElement> outputList = new List<XmlElement>();

            for (int x = 0; x < NPCManagerStackpanel.Children.Count; ++x)
            {
                NPCSlot currentSlot = NPCManagerStackpanel.Children[x] as NPCSlot;

                XmlElement currentSlotElement = doc.CreateElement("NPC");

                XmlAttribute nameAttribute = doc.CreateAttribute("Name");
                nameAttribute.Value = currentSlot.NameNPCTextbox.Text != "Enter Name" ? currentSlot.NameNPCTextbox.Text : "";
                currentSlotElement.Attributes.Append(nameAttribute);

                XmlAttribute notesAttribute = doc.CreateAttribute("Notes");
                notesAttribute.Value = currentSlot.NotesNPCTextbox.Text != "Notes" ? currentSlot.NotesNPCTextbox.Text : "";
                currentSlotElement.Attributes.Append(notesAttribute);

                outputList.Add(currentSlotElement);
            }

            return outputList;
        }


        // ----------------------------------------------------------------
        // ----------------------------------------------------------------
        public void AddNewNPCSlot()
        {
            NPCManagerStackpanel.Children.Add(new NPCSlot());
        }


        // ----------------------------------------------------------------
        // ----------------------------------------------------------------
        private void ClassCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string parentClass = ClassCombobox.SelectedItem.ToString();

            if (parentClass != "" && parentClass != null)
            {
                SubclassCombobox.IsEnabled     = true;
                SubclassCombobox.SelectedIndex = -1;

                SubclassCombobox.ItemsSource = m_ClassData.Subclasses[parentClass].m_Subclasses.ToArray();

            }
        }


        // ----------------------------------------------------------------
        // ----------------------------------------------------------------
        private void IntCheckPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            string inputText = e.Text;
            e.Handled = !inputText.All(c=>Char.IsDigit(c) || Char.IsControl(c));
        }


        // ----------------------------------------------------------------
        // ----------------------------------------------------------------
        private void DamageButton_Click(object sender, RoutedEventArgs e)
        {
            if (DamageTextbox.Text == "")
            {
                return;
            }

            int damageAmount    = Int16.Parse(DamageTextbox.Text);
            int currentHPAmount = Int16.Parse(CurrentHPTextbox.Text);

            CurrentHPTextbox.Text = (currentHPAmount - damageAmount).ToString();
        }


        // ----------------------------------------------------------------
        // ----------------------------------------------------------------
        private void HPBackButton_Click(object sender, RoutedEventArgs e)
        {
            if (HPBackTextbox.Text == "")
            {
                return;
            }

            int healthAmount    = Int16.Parse(HPBackTextbox.Text);
            int currentHPAmount = Int16.Parse(CurrentHPTextbox.Text);

            CurrentHPTextbox.Text = (currentHPAmount + healthAmount).ToString();
        }


        // ----------------------------------------------------------------
        // ----------------------------------------------------------------
        private void RemoveTextbox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox senderTextbox = sender as TextBox;

            if (senderTextbox.Text == "Notes" || senderTextbox.Text == "Name")
            {
                senderTextbox.Text = "";
            }

        }


        // ----------------------------------------------------------------
        // ----------------------------------------------------------------
        private void DamageTextbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                int damageAmount    = Int16.Parse(DamageTextbox.Text);
                int currentHPAmount = Int16.Parse(CurrentHPTextbox.Text);

                CurrentHPTextbox.Text = (currentHPAmount - damageAmount).ToString();
            }
        }


        // ----------------------------------------------------------------
        // ----------------------------------------------------------------
        private void HPBackTextbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                int healthAmount    = Int16.Parse(HPBackTextbox.Text);
                int currentHPAmount = Int16.Parse(CurrentHPTextbox.Text);

                CurrentHPTextbox.Text = (currentHPAmount + healthAmount).ToString();
            }
        }


        // ----------------------------------------------------------------
        // ----------------------------------------------------------------
        private void SaveMenu_Click(object sender, RoutedEventArgs e)
        {
            SavePlayerManager();    
        }


        // ----------------------------------------------------------------
        // ----------------------------------------------------------------
        private void NameTextbox_LostFocus(object sender, RoutedEventArgs e)
        {
            StackPanel currentStackpanel       = (StackPanel)((TextBox)sender).Parent;
            StackPanel secondaryStackpanel     = (StackPanel)(currentStackpanel).Parent;
            StackPanel parentStackpanel        = (StackPanel)(secondaryStackpanel).Parent;
            PlayerManager currentPlayerManager = (PlayerManager)(parentStackpanel).Parent;

            TabItem tabItem = (TabItem)(currentPlayerManager).Parent;
            tabItem.Header = NameTextbox.Text;
        }


        // ----------------------------------------------------------------
        // ----------------------------------------------------------------
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Window window = Window.GetWindow(this);
            window.KeyDown += CtrSKeyDown;
        }


        // ----------------------------------------------------------------
        // ----------------------------------------------------------------
        private void AddNPCButton_Click(object sender, RoutedEventArgs e)
        {
            AddNewNPCSlot();
        }


        // ----------------------------------------------------------------
        // ----------------------------------------------------------------
        private void CtrSKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.S && Keyboard.Modifiers == ModifierKeys.Control)
            {
                SavePlayerManager();
            }
        }

    }
}

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

namespace DnDPlayerManager.View
{
    public partial class NPCSlot : UserControl
    {


        // ----------------------------------------------------------------
        // ----------------------------------------------------------------
        public NPCSlot()
        {
            InitializeComponent();
        }


        // ----------------------------------------------------------------
        // ----------------------------------------------------------------
        private void NPCSlotTextbox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox nameTextbox = sender as TextBox;

            if (nameTextbox.Text == "Enter Name" || nameTextbox.Text == "Notes")
            {
                nameTextbox.Text = "";
            }
        }


        // ----------------------------------------------------------------
        // ----------------------------------------------------------------
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            StackPanel currentStackpanel   = (StackPanel)((Button)sender).Parent;
            StackPanel secondaryStackpanel = (StackPanel)(currentStackpanel).Parent;
            NPCSlot currentNPCSlot         = (NPCSlot)(secondaryStackpanel).Parent;
            StackPanel parentStackpanel    = (StackPanel)(currentNPCSlot).Parent;
            parentStackpanel.Children.Remove(this);
        }

    }
}

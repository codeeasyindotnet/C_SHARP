using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using WpfPageTransitions;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using System.IO;

namespace Folder_Structure
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {


        public MainWindow()
        {
            InitializeComponent();
        }

        private   void checkBox_Checked(object sender, RoutedEventArgs e)
        {
             this.ShowMessageAsync("Folder Structure","The folder are created in this way\n 1A\n1B\n1C...");
        }

        private   void button1_Click(object sender, RoutedEventArgs e)
        {
           // string x = await this.ShowInputAsync("Folder Structure", "This is the creation path?", null);
            //var result = await DialogManager.ShowMessageAsync(this,"Folder Structure", "This is the creation path?", MessageDialogStyle.AffirmativeAndNegative,null);

            //if (Convert.ToBoolean(result)) label1.Content = "tr";

        }

        private void button_location_for_creation_Click(object sender, RoutedEventArgs e)
        {
            string username = string.Empty;
            username = Environment.UserName;
            
            FolderBrowserDialog browse_path = new FolderBrowserDialog();
            browse_path.RootFolder = Environment.SpecialFolder.DesktopDirectory;
            DialogResult dialog = browse_path.ShowDialog();
            if (dialog.ToString() == "OK")
            {
                listBox.Items.Clear();
                listBox.Items.Add (browse_path.SelectedPath);
                ListDirectory(treeView, browse_path.SelectedPath.ToString());
            }
        }

        private void ListDirectory(System.Windows.Controls.TreeView treeView, string path)
        {
            treeView.Items.Clear();
            var rootDirectoryInfo = new DirectoryInfo(path);
            treeView.Items.Add(CreateDirectoryNode(rootDirectoryInfo));
        }

        private static TreeViewItem CreateDirectoryNode(DirectoryInfo directoryInfo)
        {
            var directoryNode = new TreeViewItem { Header = directoryInfo.Name };
            foreach (var directory in directoryInfo.GetDirectories())
                directoryNode.Items.Add(CreateDirectoryNode(directory));

            foreach (var file in directoryInfo.GetFiles())
                directoryNode.Items.Add(new TreeViewItem { Header = file.Name });

            return directoryNode;

        }

        private void button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            listBox.Items.Add("Your selected Directory shows here");
            listBox.Items.Add("El folder seleccionado estará aquí");
        }

        private void button1_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void button_Click_1(object sender, RoutedEventArgs e)
        {
            for (int i = 65; i <= 90; i++)
            {
                listBox.Items.Add(Convert.ToChar(i));
            }
        }
    }
}

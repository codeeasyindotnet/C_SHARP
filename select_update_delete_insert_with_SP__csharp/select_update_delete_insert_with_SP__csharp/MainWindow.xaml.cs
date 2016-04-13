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
using MahApps.Metro.Controls;

namespace select_delete_insert_update_from_csharp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        seleccionar_control seleccionar_form = new seleccionar_control();
        Insertar_Control insertar_form = new Insertar_Control();
        borrar_control borrar_form = new borrar_control();
        actualizar_control actualizar_form = new actualizar_control();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_seleccionar_Click(object sender, RoutedEventArgs e)
        {

            pageTransition.ShowPage(seleccionar_form);
        }

        private void button_borrar_Click(object sender, RoutedEventArgs e)
        {

            pageTransition.ShowPage(borrar_form);
        }

        private void button_actualizar_Click(object sender, RoutedEventArgs e)
        {

            pageTransition.ShowPage(actualizar_form);
        }

        private void button_insertar_Click(object sender, RoutedEventArgs e)
        {

            pageTransition.ShowPage(insertar_form);
        }
    }
}

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
using System.Data;
using System.Data.SqlClient;

namespace select_delete_insert_update_from_csharp
{
    /// <summary>
    /// Interaction logic for borrar_control.xaml
    /// </summary>
    public partial class borrar_control : UserControl
    {
        #region Variables
        static string con_db = @"server=LAPTOP-K17D07D6\SQL2014;database=AdventureWorks2014;trusted_connection=true";
        SqlConnection sql_con = new SqlConnection(con_db);
        SqlCommand sql_cmd;
        SqlDataAdapter sql_da = new SqlDataAdapter();
        DataTable dt = new DataTable();
        string query = string.Empty;
        #endregion

        public borrar_control()
        {
            InitializeComponent();
        }

        private void button_borrar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                query = string.Empty;
                query = "borrar_campos";
                sql_cmd = new SqlCommand(query, sql_con);
                sql_cmd.CommandType = CommandType.StoredProcedure;
                sql_cmd.Parameters.AddWithValue("@BUSINESSENTITYID", Convert.ToInt32(textBox_condicion.Text));

                sql_con.Open();
                sql_cmd.ExecuteNonQuery();


            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                sql_con.Close();
            }
            seleccionar_todo();
        }

        private void button_seleccionar_todo_Click(object sender, RoutedEventArgs e)
        {
         
            seleccionar_todo();
        }

        public void seleccionar_todo()
        {

            try
            {
                dt.Clear();
                query = string.Empty;
                query = "seleccionar_todo";
                sql_cmd = new SqlCommand(query, sql_con);
                sql_cmd.CommandType = CommandType.StoredProcedure;
                sql_da = new SqlDataAdapter(sql_cmd);
                sql_da.Fill(dt);
                dataGrid1.ItemsSource = dt.DefaultView;
                dataGrid1.AutoGenerateColumns = true;
                dataGrid1.CanUserAddRows = false;
            }

            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

    }
}

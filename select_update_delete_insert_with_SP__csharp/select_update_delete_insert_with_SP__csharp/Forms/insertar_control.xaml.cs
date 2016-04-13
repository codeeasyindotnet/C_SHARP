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
    /// Interaction logic for Insertar_Control.xaml
    /// </summary>
    public partial class Insertar_Control : UserControl
    {
        static string con_db = @"server=LAPTOP-K17D07D6\SQL2014;database=AdventureWorks2014;trusted_connection=true";
        SqlConnection sql_con = new SqlConnection(con_db);
        SqlCommand sql_cmd;
        DataTable dt = new DataTable();
        string query = string.Empty;
        SqlDataAdapter sql_da = new SqlDataAdapter();
        public Insertar_Control()
        {
            InitializeComponent();
        }
        private void button_insertar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                query = string.Empty;
                query = "insertar_campos";
                sql_cmd = new SqlCommand(query, sql_con);
                sql_cmd.CommandType = CommandType.StoredProcedure;
                sql_cmd.Parameters.AddWithValue("@BUSINESSENTITYID", Convert.ToInt64(textBox_BUSINESSENTITYID.Text));
                sql_cmd.Parameters.AddWithValue("@ACCOUNTNUMBER", textBox_ACCOUNT_NUMBER.Text);
                sql_cmd.Parameters.AddWithValue("@NAME", textBox_NAME.Text);
                sql_cmd.Parameters.AddWithValue("@MODIFIEDDATE", Convert.ToDateTime(textBox_MODIFIEDDATE.Text));

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

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            textBox_MODIFIEDDATE.Text = DateTime.Today.ToString("MM/dd/yy");
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

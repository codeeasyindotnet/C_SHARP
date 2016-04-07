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
using System.Data.SqlClient;
using System.Data;
using MahApps.Metro.Controls.Dialogs;

namespace select_delete_insert_update_from_csharp
{
    /// <summary>
    /// Interaction logic for seleccionar_control.xaml
    /// </summary>

    public partial class seleccionar_control : UserControl
    {

        static string con_db = @"server=LAPTOP-K17D07D6\SQL2014;database=AdventureWorks2014;trusted_connection=true";
        SqlConnection sql_con = new SqlConnection(con_db);
        SqlCommand sql_cmd;
        DataTable dt =  new DataTable();
        string query = string.Empty;
        SqlDataReader sql_dr;
        SqlDataAdapter sql_da = new SqlDataAdapter();
        public seleccionar_control()
        {
            InitializeComponent();
        } 

        private void buttonseleccion_condicionada_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                query = string.Empty;
                query = "seleccion_condicionada";
                sql_cmd = new SqlCommand(query, sql_con);
                sql_cmd.CommandType = CommandType.StoredProcedure;
                sql_cmd.Parameters.AddWithValue("@businessid", Convert.ToInt32(textBox_condicion.Text));

                sql_con.Open();
                sql_dr = sql_cmd.ExecuteReader();

                while(sql_dr.Read())
                {
                    textBox_BUSINESSENTITYID.Text = sql_dr["BUSINESSENTITYID"].ToString();
                    textBox_ACCOUNT_NUMBER.Text = sql_dr["ACCOUNTNUMBER"].ToString();
                    textBox_NAME.Text = sql_dr["NAME"].ToString();
                    textBox_MODIFIEDDATE.Text = sql_dr["MODIFIEDDATE"].ToString();
                    
                }


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
        }

        private void button_seleccionar_todo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dt.Clear();
                query = string.Empty;
                query = "seleccionar_todo";
                sql_cmd = new SqlCommand(query,sql_con);
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

        private void button_llenar_combobox_Click(object sender, RoutedEventArgs e)
        {
            try
            { 
            query = string.Empty;
            query = "SELECT BUSINESSENTITYID FROM ADVENTUREWORKS2014.PURCHASING.VENDOR2  ORDER BY BUSINESSENTITYID ASC ";
            sql_cmd = new SqlCommand(query, sql_con);
            sql_cmd.CommandType = CommandType.Text;
            
            sql_con.Open();
            sql_dr = sql_cmd.ExecuteReader();

            while (sql_dr.Read())
            {
                comboBox.Items.Add( sql_dr["BUSINESSENTITYID"].ToString()); 
            }


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
        }


    }
}

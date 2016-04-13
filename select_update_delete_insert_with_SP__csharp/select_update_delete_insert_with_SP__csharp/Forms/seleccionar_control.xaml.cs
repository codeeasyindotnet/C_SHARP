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
        DataTable dt = new DataTable();
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

                while (sql_dr.Read())
                {
                    textBox_BUSINESSENTITYID.Text = sql_dr["BUSINESSENTITYID"].ToString();
                    textBox_ACCOUNT_NUMBER.Text = sql_dr["ACCOUNTNUMBER"].ToString();
                    textBox_NAME.Text = sql_dr["NAME"].ToString();
                    textBox_MODIFIEDDATE.Text = sql_dr["MODIFIEDDATE"].ToString();

                }


            }

            //De tener algún error con la base de datos se mostrara a través de este catch
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }

            //De tener algún error con el programa se mostrara a través de este catch
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            //Cerramos conexión a la base de datos.
            finally
            {
                sql_con.Close();
            }
        }

        private void button_seleccionar_todo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Borramos el DataTable para cuando se oprima nuevamente el botón refresque la data con nueva información.
                dt.Clear();

                //inicializamos nuestra variable con el nombre del SP que se ejecutará.
                query = string.Empty;
                query = "seleccionar_todo";

                //Inicializamos el SqlCommand con la variable query y la connexión de la base de datos
                sql_cmd = new SqlCommand(query, sql_con);

                //Declaramos que es un Stored procedure
                sql_cmd.CommandType = CommandType.StoredProcedure;

                //Inicializamos el Sql Data Adpater con el Sql Command              
                sql_da = new SqlDataAdapter(sql_cmd);

                //Llenamos el data table con los records que obtuvo del Sql Data Adapter
                sql_da.Fill(dt);

                //Le indicamos al datagrid que la información que presentará viene del data table
                dataGrid1.ItemsSource = dt.DefaultView;

                //Opciones adicionales para el datagrid
                dataGrid1.AutoGenerateColumns = true;
                dataGrid1.CanUserAddRows = false;
            }

            //De tener algún error con la base de datos se mostrara a través de este catch
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }

            //De tener algún error con el programa se mostrara a través de este catch
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void button_llenar_combobox_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //inicializamos nuestra variable con el nombre del SP que se ejecutará.
                query = string.Empty;
                query = "seleccionar_todo";

                //Inicializamos el SqlCommand con la variable query y la connexión de la base de datos
                sql_cmd = new SqlCommand(query, sql_con);

                //Declaramos que es un Stored procedure
                sql_cmd.CommandType = CommandType.StoredProcedure;

                //Inicializamos el Sql Data Adpater con el Sql Command              
                sql_da = new SqlDataAdapter(sql_cmd);

                //Abrimos conexión a la base de datos
                sql_con.Open();

                //Inicializamos el Sql Data Reader con la información que obtenemos del stored procedure
                sql_dr = sql_cmd.ExecuteReader();

                //Declaramos el loop para leer la información que fue guardada en el Sql Data Reader.
                while (sql_dr.Read())
                {
                    //Como solo quiero presentar el campo, BUSINESSENTITYID, lo declaro de la siguiente manera para leer los datos de ese campo.
                    comboBox.Items.Add(sql_dr["BUSINESSENTITYID"].ToString());
                }


            }

            //De tener algún error con la base de datos se mostrara a través de este catch
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }

            //De tener algún error con el programa se mostrara a través de este catch
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            //Cerramos conexión a la base de datos.
            finally
            {
                sql_con.Close();
            }
        }


    }
}

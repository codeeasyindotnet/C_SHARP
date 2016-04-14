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
        #region Variables
        //Conexión al servidor
        static string con_db = @"server=LAPTOP-K17D07D6\SQL2014;database=AdventureWorks2014;trusted_connection=true";

        //Objetos y variables para interactuar con la base de datos.
        SqlConnection sql_con = new SqlConnection(con_db);
        SqlCommand sql_cmd;
        SqlDataReader sql_dr;
        SqlDataAdapter sql_da = new SqlDataAdapter();
        DataTable dt = new DataTable();
        string query = string.Empty;
        #endregion

        public Insertar_Control()
        {
            InitializeComponent();
        }
        private void button_insertar_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                //inicializamos nuestra variable con el nombre del SP que se ejecutará.
                query = string.Empty;
                query = "insertar_campos";
                //Inicializamos el SqlCommand con la variable query y la connexión de la base de datos
                sql_cmd = new SqlCommand(query, sql_con);

                //Declaramos que es un Stored procedure
                sql_cmd.CommandType = CommandType.StoredProcedure;

                //Le enviamos los campos que seran actualizados, convirtiendolos según el parámetro que espera el SP
                sql_cmd.Parameters.AddWithValue("@BUSINESSENTITYID", Convert.ToInt64(textBox_BUSINESSENTITYID.Text));
                sql_cmd.Parameters.AddWithValue("@ACCOUNTNUMBER", textBox_ACCOUNT_NUMBER.Text);
                sql_cmd.Parameters.AddWithValue("@NAME", textBox_NAME.Text);
                sql_cmd.Parameters.AddWithValue("@MODIFIEDDATE", Convert.ToDateTime(textBox_MODIFIEDDATE.Text));

                //Abrimos conexión a la base de datos
                sql_con.Open();

                //Ejecuta el SP para borrar el record deseado
                sql_cmd.ExecuteNonQuery();



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

            //Se llama a la funcion para mostrar todos los record
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

                //Con esta propiedad se crean las columnas con los nombres de los campos del SP
                dataGrid1.AutoGenerateColumns = true;

                //Opcion adicional para el datagrid
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

    }
}

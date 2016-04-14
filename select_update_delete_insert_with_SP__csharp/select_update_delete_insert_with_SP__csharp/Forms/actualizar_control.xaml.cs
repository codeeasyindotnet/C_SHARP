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
    /// Interaction logic for actualizar_control.xaml
    /// </summary>
    public partial class actualizar_control : UserControl
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

        public actualizar_control()
        {
            InitializeComponent();
        }

        private void button_ACTUALIZAR_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //inicializamos nuestra variable con el nombre del SP que se ejecutará.
                query = string.Empty;
                query = "actualizar_campos";

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
            seleccion_condicionada();
           
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

        public void seleccion_condicionada()
        {

            //inicializamos nuestra variable con el nombre del SP que se ejecutará.
            query = string.Empty;
            query = "seleccion_condicionada";

            //Inicializamos el SqlCommand con la variable query y la connexión de la base de datos
            sql_cmd = new SqlCommand(query, sql_con);

            //Declaramos que es un Stored procedure
            sql_cmd.CommandType = CommandType.StoredProcedure;

            //Ya que condicionamos la seleccion en el stored procedure, le enviamos el parámetro para que filtre nuestra selección.
            //Como la variable en el SP es integer, convertimos el textbox de string a integer.
            sql_cmd.Parameters.AddWithValue("@businessid", Convert.ToInt32(textBox_condicion.Text));

            //Abrimos conexión a la base de datos
            sql_con.Open();

            //Inicializamos el Sql Data Reader con la información que obtenemos del stored procedure
            sql_dr = sql_cmd.ExecuteReader();

            //Declaramos el loop para leer la información que fue guardada en el Sql Data Reader.
            while (sql_dr.Read())
            {

                //Lo declaro de la siguiente manera para leer los datos de los campos que se presentaran.
                textBox_BUSINESSENTITYID.Text = sql_dr["BUSINESSENTITYID"].ToString();
                textBox_ACCOUNT_NUMBER.Text = sql_dr["ACCOUNTNUMBER"].ToString();
                textBox_NAME.Text = sql_dr["NAME"].ToString();
                textBox_MODIFIEDDATE.Text = sql_dr["MODIFIEDDATE"].ToString();

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

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            textBox_MODIFIEDDATE.Text = DateTime.Today.ToString("MM/dd/yy");
        }

        private void button_seleccionar_todo_Click(object sender, RoutedEventArgs e)
        {
            //Llamar al método
            seleccionar_todo();
        }

        private void button_seleccion_condicionada_Click(object sender, RoutedEventArgs e)
        {
            //Llamar al método
            seleccion_condicionada();
        }
    }
}

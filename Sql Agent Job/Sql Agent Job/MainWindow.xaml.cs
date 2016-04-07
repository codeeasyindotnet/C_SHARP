using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using MahApps.Metro.Controls;
using MahApps.Metro;
using System.Windows.Threading;
using MahApps.Metro.Controls.Dialogs;

namespace Sql_Agent_Job
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        #region VARIABLES
        // static string con_string = "server=DESKTOP-AGC4KIM\\SQL2014; database=ceidn_develop; Trusted_Connection=True;";
        static string con_string = "server=; database=; Trusted_Connection=True;";

        SqlConnection sql_con = new SqlConnection(con_string);
        SqlCommand sql_cmd;
        SqlDataReader sql_dr;
        DispatcherTimer timer = new DispatcherTimer();
        DateTime jobtime ;

        string query;
        string jobname = string.Empty;
        string jobstep = string.Empty;
        string actual_step = string.Empty;
        string jobid = string.Empty;
        int end_status;
        int job_step;
        int run_status;
        string message;
       
        #endregion

        public MainWindow()
        {
            InitializeComponent();

        } 

        private void read_job_Click(object sender, RoutedEventArgs e)
        {
            //Inicializamos el query
            //initialize variables
            query = "";
            query = "select * from msdb..sysjobs";
            sql_cmd = new SqlCommand(query, sql_con);

            //Le decimos que es tipo texto para que sql server reconozca que es un query.
            // We say it's text for SQL Server recognize  is a query .
            sql_cmd.CommandType = CommandType.Text;

            try
            {
                //abrimos conexión
                // Open connection
                sql_con.Open();

                //inicializamos el datareader
                //initialize datareader
                sql_dr = sql_cmd.ExecuteReader();

                //lee el resultado que tiene el data reader
                // Read the result has the data reader
                while (sql_dr.Read())
                {
                    //Llenamos el combobox del job con los nombres de los job
                    //fill the combobox  with the name of the jobs
                    comboBox_Jobs.Items.Add(sql_dr["NAME"].ToString().ToUpper());
                }
            }

            //De tener algún error en SQL Server lo recibiremos por el catch
            //To have an error in the SQL Server will receive the catch
            catch (SqlException ex)
            {
                timer.Stop();
                this.ShowMessageAsync("Certificados SSRS", ex.Message, MessageDialogStyle.Affirmative, null);
            }

            //De tener algún error en el programa lo recibiremos por el catch
            // To have an error in the program will receive the catch
            catch (Exception ex)
            {
                timer.Stop();
                this.ShowMessageAsync("Certificados SSRS", ex.Message, MessageDialogStyle.Affirmative, null);
            }
            finally
            {
                //Cerramos la conexión.
                //Close connection
                sql_con.Close();
            }
        }

        private void comboBox_Jobs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Inicializamos las variables
            //initialize variables
            query = "";
            query = "SELECT step_name, a.job_id jobid FROM msdb..sysjobsteps A INNER JOIN msdb..SYSJOBS B ON A.job_id = B.job_id WHERE B.name = '" + comboBox_Jobs.SelectedValue.ToString()+ "'";
            sql_cmd = new SqlCommand(query, sql_con);
            sql_cmd.CommandType = CommandType.Text;

            try
            {
                //abrimos conexión
                // Open connection
                sql_con.Open();

                //inicializamos el datareader
                //initialize datareader
                sql_dr = sql_cmd.ExecuteReader();

                //Borramos los item que tenga el combobox
                //delete the item that has the combobox
                comboBox_Jobs_steps.Items.Clear();

                //lee el resultado que tiene el data reader
                // Read the result has the data reader
                while (sql_dr.Read())
                {
                    //llena el combo box con los nombres de los step
                    // Fill the combo box with the names of the step
                    comboBox_Jobs_steps.Items.Add(sql_dr["step_Name"].ToString().ToUpper());

                    //Guarda el job id en una variable
                    // Save the job id in a variable
                    jobid = sql_dr["jobid"].ToString().ToUpper();
                }
            }

            //De tener algún error en SQL Server lo recibiremos por el catch
            //To have an error in the SQL Server will receive the catch
            catch (SqlException ex)
            {
                timer.Stop();
                this.ShowMessageAsync("Certificados SSRS", ex.Message, MessageDialogStyle.Affirmative, null);
            }

            //De tener algún error en el programa lo recibiremos por el catch
            // To have an error in the program will receive the catch
            catch (Exception ex)
            {
                timer.Stop();
                this.ShowMessageAsync("Certificados SSRS", ex.Message, MessageDialogStyle.Affirmative, null);
            }
            finally
            {
                //Cerramos la conexión.
                //Close connection
                sql_con.Close();
            }
        } 

        private void execute_job_Click(object sender, RoutedEventArgs e)
        {
            //Borramos los item que tenga el combobox
            //delete the item that has the combobox
            listBox_job_steps.Items.Clear();

            //Inicializamos variables para que el timer ejecute nuevamente.
            //initialize variables for the timer run again.
            end_status = 0;
            job_step = 1;

            //inicializamos para que obtenga la hora en cual se procedió a ejecutar el job nuevamente.
            // Initialize to get the time in which we proceeded to run the job again.
            jobtime = DateTime.Now;

            //Inicializamos Variables
            //initialize variables
            query = "";
            query = "msdb.dbo.sp_start_job";
            sql_cmd = new SqlCommand(query, sql_con);

            //Le indicamos que es un Store Procedure para que lo entienda en SQL Server.
            // We said is a Stored Procedure to SQL Server understands .
            sql_cmd.CommandType = CommandType.StoredProcedure;

            //Inicializamos las variables internas del Store Procedure.
            // Initialize the internal variables of Store Procedure .
            sql_cmd.Parameters.AddWithValue("@job_name", comboBox_Jobs.SelectedValue.ToString());
            sql_cmd.Parameters.AddWithValue("@step_name", comboBox_Jobs_steps.SelectedValue.ToString());
            try
            {
                // Abrimos Conexión.
                //Open Connection
                sql_con.Open();

                //Ejecutamos el Stored Procedure.
                //Execute the Stored Procedure.
                sql_cmd.ExecuteNonQuery();

            }

            //De tener algún error en SQL Server lo recibiremos por el catch
            //To have an error in the SQL Server will receive the catch
            catch (SqlException ex)
            {
                timer.Stop();
                this.ShowMessageAsync("Certificados SSRS", ex.Message, MessageDialogStyle.Affirmative, null);
            }

            //De tener algún error en el programa lo recibiremos por el catch
            // To have an error in the program will receive the catch
            catch (Exception ex)
            {
                timer.Stop();
                this.ShowMessageAsync("Certificados SSRS", ex.Message, MessageDialogStyle.Affirmative, null);
            }
            finally
            {
                //Cerramos la conexión.
                //Close connection
                sql_con.Close();
            }

            //Idicamos el método que usará el Timer.
            // We indicate the method to use the Timer .
            timer.Tick += new EventHandler(timer_Tick);

            //Aquí le decimos que el timer se ejecutara cada segundo.
            // Here we tell the timer will run every second.
            timer.Interval = new TimeSpan(0, 0, 0, 1);

            //Empieza a correr el timer.
            // Start the timer running .
            timer.Start();
        } 

        public void timer_Tick(object sender, EventArgs e)
        {
            //Cuando las variables obtengan estos valores terminará el timer de correr.
            // When variables obtain these values ​​will end the timer running.
            if (job_step == 0  && end_status == 1)
            {
                timer.Stop();     
            }
            else
            {
                //Entrar al metodo  
                //Login to the method 
                actualstep();
                current_job_status(); 
            } 
        }

        public void actualstep()
        {
            //Inicializamos las variables
            //initialize variables
            jobstep = "";
            jobname = ""; 
            query = ""; 
            query = "MSDB.DBO.SP_HELP_JOB"; 
            sql_cmd = new SqlCommand(query, sql_con);

            //Le indicamos que es un Store Procedure para que lo entienda en SQL Server.
            //We said is a Stored Procedure to SQL Server understands .
            sql_cmd.CommandType = CommandType.StoredProcedure;

            //Inicializamos las variables internas del Store Procedure.
            //Initialize the internal variables of Store Procedure .
            sql_cmd.Parameters.AddWithValue("@execution_status", 1); 

            try
            {
                //abrimos conexión
                //Open connection
                sql_con.Open();

                //inicializamos el datareader
                //initialize datareader
                sql_dr = sql_cmd.ExecuteReader();

                //lee el resultado que tiene el data reader
                //Read the result has the data reader
                while (sql_dr.Read())
                {
                    jobstep = sql_dr["current_execution_step"].ToString();//Guardamos el paso por donde va
                    jobname = sql_dr["name"].ToString();// Guardamos el job name
                    
                }

              if ( // en caso de que el actual_step no sea igual al jobstep
                   actual_step != jobstep &&
                   //y jobstep no sea "0 (unknown)"
                   jobstep != "0 (unknown)" &&
                   //y el que el jobname del servidor sea el mismo que el seleccionaod en el programa
                   jobname == comboBox_Jobs.SelectedValue.ToString()
                 )
                  {
                    //inicializa variable y añade al listbox
                    actual_step = jobstep;
                    listBox_job_steps.Items.Add(actual_step);
                  }

            }

            //De tener algún error en SQL Server lo recibiremos por el catch
            //To have an error in the SQL Server will receive the catch
            catch (SqlException ex)
            {
                timer.Stop();
                this.ShowMessageAsync("Certificados SSRS", ex.Message, MessageDialogStyle.Affirmative, null);
            }

            //De tener algún error en el programa lo recibiremos por el catch
            // To have an error in the program will receive the catch
            catch (Exception ex)
            {
                timer.Stop();
                this.ShowMessageAsync("Certificados SSRS", ex.Message, MessageDialogStyle.Affirmative, null);
            }
            finally
            {
                //Cerramos la conexión.
                //Close connection
                sql_con.Close();
                sql_dr.Close();
            }
        }

        public void current_job_status()
        {
            
            query = "";    
            query = "current_job_status"; 
                                                                                          
            sql_cmd = new SqlCommand(query, sql_con);
            
            //Enviamos el día de hoy para el stored procedure.
            //Send actual date to the stored procedure.
            sql_cmd.Parameters.AddWithValue("@rundate", jobtime.ToString("yyyyMMdd"));

            //Indicamos el jobid que leímos anteriormente.
            //We indicate the jobid we read earlier.
            sql_cmd.Parameters.AddWithValue("@jobid ", jobid);

            //Agarramos la hora en cual se ejecutó el proceso nuevamente.
            //We pick the time in which the process is executed again.
            sql_cmd.Parameters.AddWithValue("@runtime", jobtime.TimeOfDay.ToString().Replace(":", "").Substring(0, 6));

            //Le decimos que es tipo stored procedure para que sql server reconozca que es un query.
            // We say it's stored procedure for SQL Server recognize is a query .
            sql_cmd.CommandType = CommandType.StoredProcedure;
             
            try
            {
                //abrimos conexión
                // Open connection
                sql_con.Open();

                //inicializamos el datareader
                //initialize datareader
                sql_dr = sql_cmd.ExecuteReader();

                //lee el resultado que tiene el data reader
                // Read the result has the data reader
                while (sql_dr.Read())
                {
                    //guardamos este campos
                    // We keep these fields
                    run_status = Convert.ToInt32(sql_dr["run_status"].ToString());
                    message = sql_dr["message"].ToString(); 
                    end_status = run_status;  
                    job_step = Convert.ToInt32(sql_dr["step_id"].ToString());
                    
                
                    switch (run_status) 
                    {
                        //En caso de tener 0 el timer para y nos indica el error.
                        // If you have 0 the timer stop and indicates the error.
                        case 0:
                            timer.Stop();
                            this.ShowMessageAsync("Certificados SSRS", "Hubo error siguiente error en corrida \n\n" + message , MessageDialogStyle.Affirmative, null);
                            break;

                        //En caso de ser 1 sigue el proceso. 
                        // Should be 1 continues the process.
                        case 1:
                            break;

                        // En caso de ser 3 el timer para y nos indica el error.
                        // If receive 3 the timer will stop and indicates the error.
                        case 3:
                             timer.Stop();
                             this.ShowMessageAsync("Certificados SSRS", "El proceso fue suspendido en el servidor \n\n" + message, MessageDialogStyle.Affirmative, null);
                             break;
                    }
                }
            }

            //De tener algún error en SQL Server lo recibiremos por el catch
            //To have an error in the SQL Server will receive in the catch
            catch (SqlException ex)
            {
                timer.Stop();
                this.ShowMessageAsync("Certificados SSRS", ex.Message, MessageDialogStyle.Affirmative, null);
            }

            //De tener algún error en el programa lo recibiremos por el catch
            // To have an error in the program will receive the catch
            catch (Exception ex)
            {
                timer.Stop();
                this.ShowMessageAsync("Certificados SSRS", ex.Message, MessageDialogStyle.Affirmative, null);
            }
            finally
            {
                //Cerramos la conexión.
                //Close connection
                sql_con.Close();
                sql_dr.Close();
            }

        }

        
    }
}

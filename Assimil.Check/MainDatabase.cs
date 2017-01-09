using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
namespace Assimil
{
    public class MainDatabase
    {
        #region Members

        /// <summary>
        ///  Stringa di connessione al database
        /// </summary>
        protected String m_strConnectionString = "";

        /// <summary>
        /// Stringa di connessione al database
        /// </summary>
        protected SqlConnection m_sqlConnection = null;

        /// <summary>
        ///  Ultimo errore verificatosi
        /// </summary>
        protected Exception m_excLastError = null;

        #endregion

        #region Public properties

        /// <summary>
        /// Stringa di connessione al database
        /// </summary>
        public String ConnectionString
        {
            get
            {
                return m_strConnectionString;
            }
            set
            {
                m_strConnectionString = value;
            }
        }

        /// <summary>
        /// Istanza della connessione al database SQL Server
        /// </summary>
        public SqlConnection Connection
        {
            get
            {
                if (m_sqlConnection == null)
                {
                    m_sqlConnection = new SqlConnection(ConnectionString);
                }
                return m_sqlConnection;
            }
        }

        /// <summary>
        /// Ultimo errore verificatosi
        /// </summary>
        public Exception LastError
        {
            get
            {
                return m_excLastError;
            }
        }

        /// <summary>
        /// Indica se si è verificato un errore durante l'ultima procedura eseguita
        /// </summary>
        public Boolean ErrorOccurred
        {
            get
            {
                return m_excLastError != null;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Fornisce le funzioni per interfacciarsi con il database
        /// </summary>
        /// <param name="strConnectionString"></param>
        public MainDatabase(String strConnectionString = "")
        {
            m_strConnectionString = strConnectionString;
        }

        #endregion

        #region Private utility methods

        /// <summary>
        /// Prova ad aprire una connessione verso il database
        /// </summary>
        /// <returns>
        /// Restituisce true se la connessione avviene con successo, false altrimenti
        /// </returns>
        protected bool OpenConnection()
        {
            try
            {
                // Verifica lo stato della connessione e nel caso ne avvia l'apertura
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();

                // Procedura terminata con successo
                return true;
            }
            catch (Exception ex)
            {
                m_excLastError = ex;

                // Notifica l'errore verificatosi
                return false;
            }
        }

        /// <summary>
        ///  Prova a chiudere una connessione verso il database
        /// </summary>
        /// <returns>Restituisce true se la connessione viene terminata con successo, false altrimenti</returns>
        protected bool CloseConnection()
        {

            try
            {
                // Verifica lo stato della connessione e nel caso ne avvia la chiusura
                if (Connection.State != ConnectionState.Closed)
                    Connection.Close();

                //Procedura terminata con successo
                return true;
            }
            catch (Exception ex)
            {
                // Notifica l'errore verificatosi
                m_excLastError = ex;
                return false;
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Esegue uno script SQL
        /// </summary>
        /// <param name="sql">Script SQL da eseguire</param>
        /// <param name="intAffectedRows">Numero di record impattati dall'esecuzione dello script</param>
        /// <returns>
        /// Restituisce true se l'esecuzione termina con successo, false altrimenti
        /// </returns>
        public bool ExecCommand(string sql, ref int intAffectedRows)
        {
            bool blnResult = true;

            try
            {
                m_excLastError = null;

                if (!OpenConnection())
                    return false;

                using (SqlCommand command = new SqlCommand(sql, Connection))
                {
                    intAffectedRows = command.ExecuteNonQuery();
                }

            }
            catch (Exception ex)
            {
                m_excLastError = ex;
                blnResult = false;
            }
            finally
            {
                CloseConnection();
            }
            return blnResult;
        }

        /// <summary>
        /// Esegue uno script SQL
        /// </summary>
        /// <param name="sql">Script SQL da eseguire</param>
        /// <param name="cmdType">Tipologia di comando da eseguire</param>
        /// <param name="intAffectedRows">Numero di record impattati dall'esecuzione dello script</param>
        /// <returns>
        ///  Restituisce true se l'esecuzione termina con successo, false altrimenti
        /// </returns>
        public bool ExecCommand(string sql, CommandType cmdType, ref int intAffectedRows)
        {
            bool blnResult = true;

            try
            {
                m_excLastError = null;

                if (!OpenConnection())
                    return false;

                using (SqlCommand command = new SqlCommand(sql, Connection))
                {
                    command.CommandType = cmdType;
                    intAffectedRows = command.ExecuteNonQuery();
                }

            }
            catch (Exception ex)
            {
                m_excLastError = ex;
                blnResult = false;
            }
            finally
            {
                CloseConnection();
            }
            return blnResult;
        }

        /// <summary>
        /// Esegue uno script SQL
        /// </summary>
        /// <param name="sql">Script SQL da eseguire</param>
        /// <param name="cmdType">Tipologia di comando da eseguire</param>
        /// <param name="sqlParameters">Collezione di parametri per lo script SQL</param>
        /// <param name="intAffectedRows">Numero di record impattati dall'esecuzione dello script</param>
        /// <returns>
        /// Restituisce true se l'esecuzione termina con successo, false altrimenti
        /// </returns>
        public bool ExecCommand(string sql, CommandType cmdType, List<SqlParameter> sqlParameters, out int intAffectedRows)
        {
            bool blnResult = true;
            intAffectedRows = 0;

            try
            {
                m_excLastError = null;

                if (!OpenConnection())
                    return false;

                using (SqlCommand command = new SqlCommand(sql, Connection))
                {
                    using (SqlTransaction trans = Connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                    {
                        command.Transaction = trans;
                        command.CommandType = cmdType;
                        if (sqlParameters != null)
                            foreach (SqlParameter parameter in sqlParameters)
                                command.Parameters.Add(parameter);

                        intAffectedRows = command.ExecuteNonQuery();
                        trans.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                m_excLastError = ex;
                blnResult = false;
            }
            finally
            {
                CloseConnection();
            }
            return blnResult;
        }

        /// <summary>
        /// Esegue uno script SQL
        /// </summary>
        /// <param name="sql">Script SQL da eseguire</param>
        /// <param name="cmdType">Tipologia di comando da eseguire</param>
        /// <param name="sqlParameters">Collezione di parametri per lo script SQL</param>
        /// <param name="intAffectedRows">Numero di record impattati dall'esecuzione dello script</param>
        /// <returns>
        /// Restituisce true se l'esecuzione termina con successo, false altrimenti
        /// </returns>
        public bool ExecCommand(string sql, CommandType cmdType, List<SqlParameter> sqlParameters, out object retValue)
        {
            bool blnResult = true;
            retValue = 0;

            try
            {
                m_excLastError = null;

                if (!OpenConnection())
                    return false;

                using (SqlCommand command = new SqlCommand(sql, Connection))
                {
                    using (SqlTransaction trans = Connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                    {
                        command.Transaction = trans;
                        command.CommandType = cmdType;
                        if (sqlParameters != null)
                            foreach (SqlParameter parameter in sqlParameters)
                                command.Parameters.Add(parameter);

                        retValue = command.ExecuteScalar();
                        trans.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                m_excLastError = ex;
                blnResult = false;
            }
            finally
            {
                CloseConnection();
            }
            return blnResult;
        }

        /// <summary>
        /// Esegue uno script SQL
        /// </summary>
        /// <param name="sql">Script SQL da eseguire</param>
        /// <param name="cmdType">Tipologia di comando da eseguire</param>
        /// <param name="sqlParameters">Collezione di parametri per lo script SQL</param>
        /// <param name="intAffectedRows">Numero di record impattati dall'esecuzione dello script</param>
        /// <returns>
        /// Restituisce true se l'esecuzione termina con successo, false altrimenti
        /// </returns>
        public bool ExecuteSqlTransaction(string sql1, string sql2, CommandType cmdType, List<SqlParameter> sqlParameters1, List<SqlParameter> sqlParameters2)
        {
            bool blnResult = true;
            object retValue = 0;

            try
            {
                m_excLastError = null;

                if (!OpenConnection())
                    return false;

                using (SqlCommand command = new SqlCommand())
                {
                    using (SqlTransaction trans = Connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                    {
                        command.CommandText = sql1;
                        command.Transaction = trans;
                        command.CommandType = cmdType;
                        command.Connection = Connection;
                        if (sqlParameters1 != null)
                            foreach (SqlParameter parameter in sqlParameters1)
                                command.Parameters.Add(parameter);

                        retValue = command.ExecuteScalar();

                        if (retValue != null)
                        {
                            // delete old parameters
                            command.Parameters.Clear();

                            // new query
                            command.CommandText = sql2;

                            // replace all parameters
                            if (sqlParameters2 != null)
                                foreach (SqlParameter parameter in sqlParameters2)
                                    command.Parameters.Add(parameter);
                            command.Parameters.Add(new SqlParameter("@queueId", retValue));

                            // execute
                            command.ExecuteNonQuery();
                        }

                        trans.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                m_excLastError = ex;
                blnResult = false;
            }
            finally
            {
                CloseConnection();
            }
            return blnResult;
        }


        /// <summary>
        /// Esegue uno script SQL
        /// </summary>
        /// <param name="sqlList">Lista di comandi SQL da eseguire</param>
        /// <param name="intAffectedRows">Numero di record impattati dall'esecuzione degli script</param>
        /// <returns>
        /// Restituisce true se l'esecuzione termina con successo, false altrimenti
        /// </returns>
        public bool ExecCommand(string[] sqlList, ref int intAffectedRows)
        {
            bool blnResult = true;
            SqlTransaction transaction = null;
            try
            {
                m_excLastError = null;

                if (!OpenConnection())
                    return false;

                intAffectedRows = 0;
                transaction = Connection.BeginTransaction();

                foreach (string sql in sqlList)
                {
                    using (SqlCommand command = new SqlCommand(sql, Connection, transaction))
                    {
                        command.CommandType = CommandType.Text;
                        intAffectedRows += command.ExecuteNonQuery();
                    }
                }

                transaction.Commit();
                blnResult = true;
            }
            catch (Exception ex)
            {
                m_excLastError = ex;
                blnResult = false;

                if ((transaction != null))
                    transaction.Rollback();
            }
            finally
            {
                CloseConnection();
            }
            return blnResult;
        }

        /// <summary>
        /// Esegue una query SQL ed utilizza il DataReader fornito per restituire il set di risultati
        /// </summary>
        /// <param name="sql">Comando SQL da eseguire</param>
        /// <param name="drdData">DataReader utilizzato per la restituizione del set di risultati</param>
        /// <param name="cmdBehaviour">
        /// Comportamento da adottare sulla connessione al completamento della richiesta
        /// </param>
        /// <returns>
        /// Restituisce true se l'esecuzione termina con successo, false altrimenti
        /// </returns>
        public bool GetDataReader(string sql, ref SqlDataReader drdData, CommandBehavior cmdBehaviour)
        {
            bool blnResult = true;

            try
            {
                m_excLastError = null;

                if (!OpenConnection())
                    return false;

                SqlCommand command = new SqlCommand(sql, Connection);
                drdData = command.ExecuteReader(cmdBehaviour);

                blnResult = true;
            }
            catch (Exception ex)
            {
                m_excLastError = ex;
                blnResult = false;
            }
            finally
            {
                CloseConnection();
            }
            return blnResult;
        }


        /// <summary>
        /// Verifica la connezione al Database
        /// </summary>
        /// <returns>true se la connezione è aperta false altrimenti</returns>
        public bool IsConnected()
        {
            bool blnResult = true;

            try
            {
                m_excLastError = null;
                OpenConnection();
            }
            catch (Exception ex)
            {
                m_excLastError = ex;
                blnResult = false;
            }
            finally
            {
                CloseConnection();
            }

            return blnResult;
        }

        public bool GetDataTable(string sql, ref DataTable dtData)
        {
            bool blnResult = true;

            try
            {
                m_excLastError = null;
                if (!OpenConnection())
                    return false;
                using (SqlDataAdapter adapter = new SqlDataAdapter(sql, Connection))
                {
                    DataSet dsData = new DataSet();
                    adapter.Fill(dsData);
                    dtData = dsData.Tables[0];
                }
            }
            catch (Exception ex)
            {
                m_excLastError = ex;
                dtData = null;
                blnResult = false;
            }
            finally
            {
                CloseConnection();

            }

            return blnResult;
        }

        /// <summary>
        ///  Esegue una query SQL e valorizza un parametro con i risultati ottenuti
        /// </summary>
        /// <param name="sql">Comando SQL da eseguire</param>
        /// <param name="sqlParameters">Collezione di parametri per lo script SQL</param>
        /// <param name="dtData">Set di risultati ottenuti</param>
        /// <returns>
        /// Restituisce true se l'esecuzione termina con successo, false altrimenti
        /// </returns>
        public bool GetDataTable(string sql, List<SqlParameter> sqlParameters, ref DataTable dtData)
        {
            bool blnResult = true;

            try
            {
                m_excLastError = null;

                if (!OpenConnection())
                    return false;

                using (SqlDataAdapter adapter = new SqlDataAdapter())
                {
                    adapter.SelectCommand = new SqlCommand(sql, Connection);

                    if (sqlParameters != null)
                        foreach (SqlParameter parameter in sqlParameters)
                            adapter.SelectCommand.Parameters.Add(parameter);

                    DataSet dsData = new DataSet();
                    adapter.Fill(dsData);
                    dtData = dsData.Tables[0];
                }

                blnResult = true;
            }
            catch (Exception ex)
            {
                m_excLastError = ex;
                dtData = null;
                blnResult = false;
            }
            finally
            {
                CloseConnection();
            }

            return blnResult;

        }

        public bool GetDataTableFromProc(string procedureName, List<SqlParameter> sqlParameters, ref DataTable dtData)
        {
            bool blnResult = true;

            try
            {
                m_excLastError = null;
                if (!OpenConnection())
                    return false;

                using (SqlCommand cmd = new SqlCommand(procedureName, Connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        if (sqlParameters != null)
                            foreach (SqlParameter parameter in sqlParameters)
                                cmd.Parameters.Add(parameter);

                        cmd.CommandText = procedureName;
                        cmd.CommandType = CommandType.StoredProcedure;
                        DataSet dsData = new DataSet();
                        adapter.Fill(dsData);
                        dtData = dsData.Tables[0];
                    }
                }
            }
            catch (Exception ex)
            {
                m_excLastError = ex;
                dtData = null;
                blnResult = false;
            }
            finally
            {
                CloseConnection();
            }

            return blnResult;
        }

        #endregion
    }
}

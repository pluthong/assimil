
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Assimil
{
   
        /// <summary>
        /// This class  provides access to the functions of database query
        /// </summary>
        public class DataProvider
        {

            #region Members

            /// <summary>
            /// Stringa di connessione al database
            /// </summary>
            protected string _strConnectionString = "";

            /// <summary>
            /// Schema per la connessione al database
            /// </summary>
            protected string _strSchema = "";

            /// <summary>
            /// Istanza della classe di interfaccia con il database LOCAL
            /// </summary>
            protected MainDatabase _objDatabaseLOCAL = null;
           

            /// <summary>
            ///  Last error occured
            /// </summary>
            protected Exception _excLastError = null;

            #endregion

            #region Public properties

            /// <summary>
            /// Stringa di connessione al database LOCAL
            /// </summary>
            public string ConnectionStringLOCAL
            {
                get
                {
                    return @"Data Source=(LocalDB)\v11.0;Initial Catalog=GreenCard;Integrated Security=True";
                }

            }

         

            /// <summary>
            /// Istanza della classe di interfaccia con il database LOCAL
            /// </summary>
            public MainDatabase DatabaseLOCAL
            {
                get
                {
                    if (_objDatabaseLOCAL == null)
                        _objDatabaseLOCAL = new MainDatabase(ConnectionStringLOCAL);
                    return _objDatabaseLOCAL;
                }
            }

            /// <summary>
            /// Ultimo errore avvenuto
            /// </summary>
            public Exception LastError
            {
                get { return _excLastError; }
            }

            /// <summary>
            /// Indica se è avvenuto un errore
            /// </summary>
            public bool ErrorOccurred
            {
                get { return _excLastError != null; }
            }

            #endregion

            #region  Public methods

            public DataTable StatusCheckForRegion(string reg)
            {
                DataTable dtData = null;

                try
                {
                    // Inizializzazione della variabile di errore
                    _excLastError = null;

                    String sql = @"SELECT *
                                   FROM  [dbo].[DVLottery] dv
                                   WHERE  dv.REG = '" + reg + "' AND (dv.Status IS NULL OR (dv.Status != 'Issued' AND (dv.REFUSED != dv.FamilyMembers))) ORDER BY dv.CN";

                    if (this.DatabaseLOCAL.GetDataTable(sql, null, ref dtData))
                    {
                        return dtData;
                    }
                }
                catch (Exception ex)
                {
                    _excLastError = ex;
                }

                return null;

            }

            public int UpdateList(DVLotteryUser lot)
            {
                 int affectedRows = 0;

                try
                {
                    // Inizializzazione della variabile di errore
                     _excLastError = null;

                     String sqlUpdate = @"UPDATE [dbo].[DVLottery]
                                          SET    CON           = @CON,
                                                 Status        = @Status,
                                                 SubmitDate    = @SubmitDate,
                                                 StatusDate    = @StatusDate,
                                                 FamilyMembers = @FamilyMembers,
                                                 ISSUED        = @ISSUED,
                                                 REFUSED       = @REFUSED,
                                                 AP            = @AP,
                                                 READY         = @READY,
                                                 TRANSFER      = @TRANSFER,
                                                 UpdateDt      = getdate()
                                          WHERE  FCN = @FCN              ";


                     List<SqlParameter> parameterList = new List<SqlParameter>();
                     parameterList.Add(new SqlParameter("@CON", lot.CON));
                     parameterList.Add(new SqlParameter("@Status",lot.Status));
                     parameterList.Add(new SqlParameter("@SubmitDate", lot.SubmitDate));
                     parameterList.Add(new SqlParameter("@StatusDate", lot.StatusDate));
                     parameterList.Add(new SqlParameter("@FamilyMembers", lot.FamilyMembers));
                     parameterList.Add(new SqlParameter("@ISSUED", lot.ISSUED));
                     parameterList.Add(new SqlParameter("@REFUSED", lot.REFUSED));
                     parameterList.Add(new SqlParameter("@AP", lot.AP));
                     parameterList.Add(new SqlParameter("@READY", lot.READY));
                     parameterList.Add(new SqlParameter("@TRANSFER", lot.TRANSFER));
                     parameterList.Add(new SqlParameter("@FCN", lot.FCN));

                     this.DatabaseLOCAL.ExecCommand(sqlUpdate, CommandType.Text, parameterList, out affectedRows);

                }
                catch (Exception ex)
                {
                    _excLastError = ex;
                    affectedRows = 0;
                }

                return affectedRows;
            }

            public int AtNVCList(DVLotteryUser lot)
            {
                int affectedRows = 0;

                try
                {
                    // Inizializzazione della variabile di errore
                    _excLastError = null;

                    String sqlUpdate = @"UPDATE [dbo].[DVLottery]
                                          SET    UpdateDt      = getdate()
                                          WHERE  FCN = @FCN              ";


                    List<SqlParameter> parameterList = new List<SqlParameter>();
                    parameterList.Add(new SqlParameter("@FCN", lot.FCN));

                    this.DatabaseLOCAL.ExecCommand(sqlUpdate, CommandType.Text, parameterList, out affectedRows);

                }
                catch (Exception ex)
                {
                    _excLastError = ex;
                    affectedRows = 0;
                }

                return affectedRows;
            }

            public int TransitList(DVLotteryUser lot)
            {
                int affectedRows = 0;

                try
                {
                    // Inizializzazione della variabile di errore
                    _excLastError = null;

                    String sqlUpdate = @"UPDATE [dbo].[DVLottery]
                                          SET    Status        = @Status,
                                                 UpdateDt      = getdate()
                                          WHERE  FCN = @FCN              ";


                    List<SqlParameter> parameterList = new List<SqlParameter>();
                    parameterList.Add(new SqlParameter("@Status", lot.Status));
                    parameterList.Add(new SqlParameter("@FCN", lot.FCN));

                    this.DatabaseLOCAL.ExecCommand(sqlUpdate, CommandType.Text, parameterList, out affectedRows);

                }
                catch (Exception ex)
                {
                    _excLastError = ex;
                    affectedRows = 0;
                }

                return affectedRows;
            }
            #endregion
        }
    
}

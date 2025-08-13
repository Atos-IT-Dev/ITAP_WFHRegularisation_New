using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using com;

namespace WFHRegularisationDAL
{
    /// <summary>
    /// Class deals with general database activities.
    /// </summary>
    internal class DataConnection
    {
        #region Constants.
        private const string STAGING = "STAGING";
        private const string PRODUCTION = "PRODUCTION";
        private readonly bool _isMasterBetiConnection;

        #endregion

        #region Variables. 
        private SqlConnection objSQLConnection;
        #endregion

        #region Methods.

        public DataConnection(bool isMasterBetiConnection)
        {
            _isMasterBetiConnection = isMasterBetiConnection;
        }

        /// <summary>
        /// Opens a database connection.
        /// </summary>
        /// <returns>SQL connectin object.</returns>
        private SqlConnection openConnection()
        {
            string connectionString = "";
            if (_isMasterBetiConnection)
                connectionString = ConfigurationManager.AppSettings["MasterBETISQLcon"].ToString();
            else
                connectionString = ConfigurationManager.AppSettings["SQLcon"].ToString();

            if (objSQLConnection == null)
            {
                objSQLConnection = new SqlConnection();
                objSQLConnection.ConnectionString = connectionString;
            }

            if (objSQLConnection.State == ConnectionState.Closed)
                objSQLConnection.Open();
            return objSQLConnection;
        }

        /// <summary>
        /// Closes a database connection.
        /// </summary>
        /// <returns></returns>
        internal void closeConnection()
        {
            //SqlConnection objSQLConnection = new SqlConnection();

            //if (ConfigurationManager.AppSettings["ENV"].ToString() == STAGING)
            //    objSQLConnection.ConnectionString = ConfigurationManager.AppSettings["stagingConnectionInfo"].ToString();
            //else
            //    objSQLConnection.ConnectionString = ConfigurationManager.AppSettings["productionConnectionInfo"].ToString();

            if (objSQLConnection != null && objSQLConnection.State == ConnectionState.Open)
                objSQLConnection.Close();
        }

        /// <summary>
        /// Opens a command to execute a stored procedure.
        /// </summary>
        /// <param name="commandText">Name of the stored procedure.</param>
        /// <returns>SQL command object.</returns>
        internal SqlCommand OpenCommand(string commandText)
        {
            if (objSQLConnection == null)
                openConnection();
            SqlCommand objSQLCommand = new SqlCommand(commandText, objSQLConnection);

            objSQLCommand.CommandType = CommandType.StoredProcedure;
            return objSQLCommand;
        }

        /// <summary>
        /// Opens a command to execute a query.
        /// </summary>
        /// <param name="commandText">Query.</param>
        /// <returns>SQL command object.</returns>
        internal SqlCommand OpenQueryCommand(string commandText)
        {
            if (objSQLConnection == null)
                openConnection();
            SqlCommand objSQLCommand = new SqlCommand(commandText, objSQLConnection);

            objSQLCommand.CommandType = CommandType.Text;
            return objSQLCommand;
        }

        /// <summary>
        /// Returns a data table using a stored procedure.
        /// </summary>
        /// <param name="storedProcedureName">Name of the stored procedure.</param>
        /// <returns>Data table.</returns>
        internal DataTable GetDataTableFromStoredProcedure(string storedProcedureName)
        {
            SqlDataAdapter objSQLDataAdapter = new SqlDataAdapter(OpenCommand(storedProcedureName));
            DataTable objDataTable = new DataTable();

            objSQLDataAdapter.Fill(objDataTable);
            return objDataTable;

        }

        /// <summary>
        /// Returns a data table using a parameterised stored procedure.
        /// </summary>
        /// <param name="storedProcedureName">SQLCommand object.</param>
        /// <returns>Data table.</returns>
        internal DataTable GetDataTableFromParameterisedStoredProcedure(SqlCommand objSqlCommand)
        {
            objSqlCommand.CommandType = CommandType.StoredProcedure;
            objSqlCommand.Connection = openConnection();
            SqlDataAdapter objSQLDataAdapter = new SqlDataAdapter(objSqlCommand);
            DataTable objDataTable = new DataTable();

            objSQLDataAdapter.Fill(objDataTable);
            return objDataTable;
        }

        /// <summary>
        /// Returns a data set using a parameterised stored procedure.
        /// </summary>
        /// <param name="storedProcedureName">SQLCommand object.</param>
        /// <returns>Data set.</returns>
        internal DataSet GetDataSetFromParameterisedStoredProcedure(SqlCommand objSqlCommand)
        {
            objSqlCommand.CommandType = CommandType.StoredProcedure;
            objSqlCommand.Connection = openConnection();
            SqlDataAdapter objSQLDataAdapter = new SqlDataAdapter(objSqlCommand);
            DataSet objDataSet = new DataSet();

            objSQLDataAdapter.Fill(objDataSet);
            return objDataSet;
        }

        /// <summary>
        /// Returns a data table using a stored procedure.
        /// </summary>
        /// <param name="storedProcedureName">SQLCommand object.</param>
        /// <returns>Data table.</returns>
        internal int SaveDataFromStoredProcedure(SqlCommand objSqlCommand)
        {
            objSqlCommand.CommandType = CommandType.StoredProcedure;
            objSqlCommand.Connection = openConnection();
            int intSaved = (int)objSqlCommand.ExecuteScalar();
            return intSaved;
        }

        /// <summary>
        /// Inserts a row of data at a time in a table.
        /// </summary>
        /// <param name="storedProcedureName">SQLCommand object.</param>
        /// <returns>Data table.</returns>
        internal int SaveDataFromQuery(SqlCommand objSqlCommand)
        {
            objSqlCommand.CommandType = CommandType.Text;

            if (objSqlCommand.Connection == null)
                objSqlCommand.Connection = openConnection();
            int intSaved = objSqlCommand.ExecuteNonQuery();
            return intSaved;
        }
        #endregion
    }
}

using System;
using System.Data;
using System.Data.SqlClient;

namespace BusinessLogicLayer.BusinessLogic
{
    public class DAL
    {
        SqlConnection _connection;
        string _connectionStr = null;
        public DAL(string connectionString)
        {
            Init(connectionString);
        }

        private void Init(string connectionString)
        {
            _connectionStr = connectionString;
            _connection = new SqlConnection(connectionString);
        }

        private void OpenConnection()
        {
            if (_connection.State == ConnectionState.Open)
                _connection.Close();
            _connection.Open();
        }

        private void CloseConnection()
        {
            if (_connection.State == ConnectionState.Open)
                _connection.Close();
        }

        private string GetConnectionString()
        {
            return _connectionStr;
        }

        private int GetCommandTimeOut()
        {
            int cto = 30;
            return cto;
        }

        public DataSet ExecuteDataset(string sql)
        {
            var ds = new DataSet();
            var cmd = new SqlCommand(sql, _connection);
            cmd.CommandTimeout = GetCommandTimeOut();
            SqlDataAdapter da;
            try
            {
                OpenConnection();
                da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                da.Dispose();
                CloseConnection();
            }

            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                da = null;
                cmd.Dispose();
                CloseConnection();
            }
            return ds;
        }

        public DataTable ExecuteDataTable(string sql)
        {
            using (var ds = ExecuteDataset(sql))
            {
                if (ds == null || ds.Tables.Count == 0)
                    return null;

                return ds.Tables[0];
            }
        }

        public void ExecuteProcedure(string spName, SqlParameter[] param)
        {

            using (SqlConnection _connection = new SqlConnection(GetConnectionString()))
            {
                try
                {
                    _connection.Open();
                    SqlCommand command = new SqlCommand(spName, _connection);
                    command.CommandType = CommandType.StoredProcedure;
                    if (param != null)
                        command.Parameters.AddRange(param);

                    command.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public void ExecuteSQL(string spName, SqlParameter[] param)
        {

            using (SqlConnection _connection = new SqlConnection(GetConnectionString()))
            {
                try
                {
                    _connection.Open();
                    SqlCommand command = new SqlCommand(spName, _connection);
                    command.CommandType = CommandType.Text;
                    if (param != null)
                        command.Parameters.AddRange(param);

                    command.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}

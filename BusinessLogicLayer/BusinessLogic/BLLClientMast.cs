using MongoDocumentSyncService.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace BusinessLogicLayer.BusinessLogic
{
    public class BLLClientMast : DAL
    {
        public BLLClientMast(string ConnStr) : base(connectionString: ConnStr)
        {

        }

        public List<ClientMast> GetClientList()
        {
            try
            {
                string sql = "SELECT * FROM CLIENT_MAST WHERE STATUS = 1 ORDER BY CLIENT_ID";
                DataTable dt = ExecuteDataTable(sql);
                return dt.AsEnumerable().Select(r => new ClientMast()
                {
                    ClientId = r.Field<short>("CLIENT_ID").ToString(),
                    ClientName = r.Field<string>("CLIENT_NAME"),
                    ClientAlias = r.Field<string>("CLIENT_ALIAS"),
                    DataSource = r.Field<string>("DATASOURCE"),
                    DbName = r.Field<string>("DB_NAME"),
                    DbUserId = r.Field<string>("DB_USER_ID"),
                    DbPassword = r.Field<string>("DB_USER_PW"),
                    // document server related
                    DocServerDataSource = r.Field<string>("DOC_SERVER_DATASOURCE"),
                    DocServerUserID = r.Field<string>("DOC_SERVER_USERID"),
                    DocServerPassword = r.Field<string>("DOC_SERVER_PWD")
                    // end document server related


                }).ToList();

            }
            catch (Exception ex)
            {
                Log.WriteLog("Exception occured...");
                Log.WriteLog(ex.ToString());
                return null;
            }
        }
    }
}

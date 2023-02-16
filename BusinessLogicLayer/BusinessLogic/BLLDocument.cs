using MongoDocumentSyncService.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace BusinessLogicLayer.BusinessLogic
{
    public class BLLDocument : DAL
    {
        string cStr = "";
        public BLLDocument(string ConnStr) : base(connectionString: ConnStr)
        {
            cStr = ConnStr;
        }

        public void SyncDocuments()
        {

            // get all active customer list
            List<ClientMast> clientMasts = new BLLClientMast(cStr).GetClientList();

            // if document server credentials are present
            foreach (var client in clientMasts.Where(r => r.DocServerDataSource != "" && r.DocServerUserID != "" && r.DocServerPassword != ""))
            {
                try
                {
                    Log.WriteLog($"Sync started in client :- {client.ClientName}, DB {client.DbName}...");

                    string connectionStr = $"Data Source = {client.DataSource}; Initial Catalog = {client.DbName}; User ID = {client.DbUserId}; Password = {client.DbPassword}";
                    DAL dal = new DAL(connectionStr);

                    #region Sync all documents from DOC_STORE table
                    // NOTE: If there are tens of thousands of documents, then we might have to implement batch selection from database

                    string sql = "SELECT * FROM DOC_STORE WHERE ISNULL(IS_SYNCED_WITH_DOC_SERVER ,0) = 0";
                    DataTable dt = dal.ExecuteDataTable(sql);
                    List<DocStore> docs = dt.AsEnumerable().Select(r => new DocStore()
                    {
                        DocGuid = r.Field<string>("DOC_GUID"),
                        DocFile = r.Field<byte[]>("DOC_FILE")
                    }).ToList();

                    // if unsynced documents are found, then insert into mongo db 
                    if (docs?.Count > 0)
                    {
                        bool isSuccess = new MongoConnection(client.DocServerDataSource, client.DocServerUserID, client.DocServerPassword, client.DbName).SaveToDatabase(docs);
                        if (isSuccess)
                        {
                            string guids = string.Join(",", docs.Select(r => "'" + r.DocGuid + "'").ToArray());


                            sql = $"UPDATE DOC_STORE SET IS_SYNCED_WITH_DOC_SERVER  = 1 WHERE DOC_GUID IN ({guids})";

                            dal.ExecuteSQL(sql, null);

                            Log.WriteLog($"Successfully synced DOC_STORE in client {client.ClientName}...");
                        }
                    }

                    #endregion             

                }
                catch (Exception ex)
                {
                    Log.WriteLog("Exception occured...");
                    Log.WriteLog(ex.ToString());
                }
            }
        }
    }
}

using MongoDocumentSyncService.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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

                            // set IS_SYNCED_WITH_DOC_SERVER  = true for currently updated documents
                            SqlParameter[] paramGuid = new SqlParameter[] { new SqlParameter() { Value = guids, ParameterName = "@GUIDS" } };

                            sql = $"UPDATE DOC_STORE SET IS_SYNCED_WITH_DOC_SERVER  = 1 WHERE DOC_GUID IN (SELECT ITEMS FROM DBO.SPLIT(@GUIDS, ','))";

                            dal.ExecuteSQL(sql, paramGuid);

                            // delete currently and(or) previously synced documents 
                            // commented for now
                            sql = $"DELETE FROM DOC_STORE WHERE IS_SYNCED_WITH_DOC_SERVER  = 1";

                            dal.ExecuteSQL(sql, null);

                            Log.WriteLog($"Successfully synced DOC_STORE in client {client.ClientName}...");
                        }
                    }

                    #endregion             

                    // may not be required, because signature_hist table is not used in the newer versions of CBS.. 

                    #region Sync all images/documents from SIGNATURE_HIST table
                    //sql = "SELECT * FROM SIGNATURE_HIST WHERE ISNULL(IS_SYNCED_WITH_DOC_SERVER ,0) = 0";
                    //dt = dal.ExecuteDataTable(sql);
                    //List<SignatureHist> signatures = dt.AsEnumerable().Select(r => new SignatureHist()
                    //{
                    //    Id = r.Field<int>("ID"),
                    //    ImageId = r.Field<string>("IMAGE_ID"),
                    //    DocFile = r.Field<byte[]>("IMAGE")
                    //}).ToList();
                    //if (signatures?.Count > 0)
                    //{
                    //    bool isSuccess = new MongoConnection(client.DocServerDataSource, client.DocServerUserID, client.DocServerPassword, client.DbName).SaveToDatabase(signatures);
                    //    if (isSuccess)
                    //    {
                    //        string ids = string.Join(",", signatures.Select(r => "'" + r.Id + "'").ToArray());

                    //        // set IS_SYNCED_WITH_DOC_SERVER  = true and set IMAGE field to null for currently updated document
                    //        sql = $@"UPDATE SIGNATURE_HIST SET IS_SYNCED_WITH_DOC_SERVER  = 1, [IMAGE] = null WHERE ID IN ({ids})";

                    //        dal.ExecuteSQL(sql, null);

                    //        Log.WriteLog($"Successfully synced SIGNATURE_HIST in client {client.ClientName}...");
                    //    }
                    //}

                    //// set image to null for records which are synced
                    //dal.ExecuteSQL("UPDATE SIGNATURE_HIST SET [IMAGE] = null WHERE IS_SYNCED_WITH_DOC_SERVER  = 1", null);


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

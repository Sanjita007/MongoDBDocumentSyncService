using BusinessLogicLayer.Models;
using MongoDB.Driver;
using MongoDocumentSyncService.Models;
using System;
using System.Collections.Generic;

namespace BusinessLogicLayer.BusinessLogic
{
    public class MongoConnection
    {
        private string _MongoDBServer;

        private string _MongoDbUserId;
        private string _MongoDbPassword;
        private string _DataBase;

        public MongoConnection()
        {

        }
        public MongoConnection(string mongoServer, string mongoUserId, string dbPassword, string dbName)
        {
            _MongoDBServer = mongoServer;
            _MongoDbUserId = mongoUserId;
            _MongoDbPassword = dbPassword;
            _DataBase = dbName;
        }
        private IMongoDatabase GetMongoClientDB
        {
            get
            {
                var credential = MongoCredential.CreateCredential("admin", _MongoDbUserId ?? "", _MongoDbPassword ?? "");

                MongoClientSettings settings = new MongoClientSettings
                {
                    Server = new MongoServerAddress(_MongoDBServer),
                    ConnectTimeout = TimeSpan.FromSeconds(20),
                    Credential = credential
                };

                var Client = new MongoClient(settings);
                var db = Client.GetDatabase(_DataBase);

                return db;
            }
        }

        public bool SaveToDatabase(List<DocStore> documents)
        {
            try
            {
                var collection = GetMongoClientDB.GetCollection<DocStore>("DOC_STORE");
                collection.InsertMany(documents);
                return true;
            }
            catch (Exception ex)
            {
                Log.WriteLog("Exception occured while saving records of DOC_STORE...");
                Log.WriteLog(ex.ToString());
                return false;
            }
        }

        public bool SaveToDatabase(List<SignatureHist> documents)
        {
            try
            {
                var collection = GetMongoClientDB.GetCollection<SignatureHist>("SIGNATURE_HIST");
                collection.InsertMany(documents);
                return true;
            }
            catch (Exception ex)
            {
                Log.WriteLog("Exception occured while saving files of SIGNATURE_HIST...");
                Log.WriteLog(ex.ToString());
                return false;
            }
        }

    }
}

namespace MongoDocumentSyncService.Models
{
    public class ClientMast
    {
        public string ClientId{ get; set; }
        public string ClientName{ get; set; }
        public string ClientAlias{ get; set; }
        public string DataSource{ get; set; }
        public string DbName{ get; set; }
        public string DbUserId{ get; set; }
        public string DbPassword{ get; set; }
        public string Status{ get; set; }

        #region document server related

        public string DocServerDataSource { get; set; }
        public string DocServerUserID { get; set; }
        public string DocServerPassword { get; set; }

        #endregion

    }
}

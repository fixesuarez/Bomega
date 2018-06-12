using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace Bomega.DAL
{
    public class UserGateway
    {
        readonly CloudStorageAccount _storageAccount;
        readonly CloudTableClient _tableClient;
        readonly CloudTable _tableUser;

        public UserGateway( string connectionString )
        {
            _storageAccount = CloudStorageAccount.Parse( connectionString );
            _tableClient = _storageAccount.CreateCloudTableClient();
            _tableUser = _tableClient.GetTableReference( "User" );
            _tableUser.CreateIfNotExistsAsync().Wait();
        }
    }
}
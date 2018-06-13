using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.Threading.Tasks;

namespace Bomega.DAL
{
    public class UserGateway
    {
        readonly CloudStorageAccount _storageAccount;
        readonly CloudTableClient _tableClient;
        readonly CloudTable _tableUser;
        readonly CloudTable _tableUserIndex;

        public UserGateway( string connectionString )
        {
            _storageAccount = CloudStorageAccount.Parse( connectionString );
            _tableClient = _storageAccount.CreateCloudTableClient();
            _tableUser = _tableClient.GetTableReference( "User" );
            _tableUser.CreateIfNotExistsAsync().Wait();

            _tableUserIndex = _tableClient.GetTableReference( "UserIndex" );
            _tableUserIndex.CreateIfNotExistsAsync().Wait();
        }

        public async Task<User> FindBySpotifyId( string idSpotify)
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<User>( "", idSpotify );
            TableResult retrievedResult = await _tableUserIndex.ExecuteAsync( retrieveOperation );
            return retrievedResult != null ? (User)retrievedResult.Result : null;
        }

        public async Task CreateOrUpdateSpotifyUser(string idSpotify, string accessStoken)
        {
            User user = new User()
            {
                PartitionKey = string.Empty,
                RowKey = idSpotify,
            };

            TableOperation insertOperation = TableOperation.Insert( user );
            await _tableUserIndex.ExecuteAsync( insertOperation );
        }
    }
}
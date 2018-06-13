using Microsoft.WindowsAzure.Storage.Table;

namespace Bomega.DAL
{
    public class User : TableEntity
    {
        public string Guid { get; set; }
        public string IdSpotify { get; set; }
        public string IdDeezer { get; set; }
        public string IdFacebook { get; set; }
        public string Name { get; set; }
    }
}

using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;

namespace EmotionalRatingBot.Storage
{
    public static class ConfigurationProvider
    {
        public static CloudStorageAccount CreateCloudStorageAccount()
        {
            return CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
        }
    }
}
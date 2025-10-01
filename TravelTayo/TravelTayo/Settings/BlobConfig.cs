namespace TravelTayo.Settings
{
    public static class BlobConfig
    {
        public static string ConnectionString { get; set; } 
        public static string ContainerName { get; set; }

        public static void Load(IConfiguration configuration)
        {
            ConnectionString = configuration["AzureBlobStorage:ConnectionString"];
            ContainerName = configuration["AzureBlobStorage:ContainerName"];
        }
    }


}
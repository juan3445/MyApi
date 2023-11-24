namespace SprintAPI.Context
{
    public class SprintsContext
    {
        private string connectionString = string.Empty;
        private string keyString = string.Empty;
        public SprintsContext()
        {
            var constructor = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            connectionString = constructor.GetSection("ConnectionStrings:DefaultConnection").Value;
            keyString = constructor.GetSection("JwtSettings:SecretKey").Value;
        }
        public string cadenaSQL()
        {
            return connectionString;
        }
        public string secretKey()
        {
            return keyString;
        }
    }
}

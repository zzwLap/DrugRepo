using Microsoft.Data.Sqlite;
using System.Data;

namespace webapi
{
    public class DapperContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public DapperContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("MyContext")!;
        }

        public IDbConnection CreateConnection()
            => new SqliteConnection(_connectionString);
    }
}

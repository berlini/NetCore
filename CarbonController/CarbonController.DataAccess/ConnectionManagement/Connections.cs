using System.Data;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;

namespace CarbonController.DataAccess.ConnectionManagement
{
    public class Connection : IConnection
    {
        public IConfiguration Configuration { get; }

        public Connection(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IDbConnection GetGenesysConnection()
        {
            var connectionString = Configuration.GetSection("ConnectionStrings").GetSection("GENESYS_CONNECTION").Value;
            var conn = new OracleConnection(connectionString);           
            return conn;
        }

        public IDbConnection GetCampaignConnection()
        {
            var connectionString = Configuration.GetSection("ConnectionStrings").GetSection("GENESYS_CONNECTION").Value;
            var conn = new OracleConnection(connectionString);           
            return conn;
        }
    }
}
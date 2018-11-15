using System;
using System.Data;
using CarbonController.DataAccess.ConnectionManagement;
using CarbonController.DataAccess.Interfaces;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;

namespace DataAccess.Repository
{
    public class RepositoryBase: IRepositoryBase
    {
        public IConfiguration Configuration { get; }
        public OracleConnection GenesysConnection { get; }

        public RepositoryBase(IConfiguration configuration, IConnection connection)
        {
            Configuration = configuration;
            GenesysConnection = connection.GetGenesysConnection() as OracleConnection;
        }

        public void Dispose()
        {
            if(GenesysConnection.State != ConnectionState.Closed)
            {
                GenesysConnection.Close();
            }
            
            GenesysConnection.Dispose();
        }
    }
}
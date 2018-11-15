using System.Data;

namespace CarbonController.DataAccess.ConnectionManagement
{
    public interface IConnection
    {
         IDbConnection GetGenesysConnection();
         IDbConnection GetCampaignConnection();
    }
}
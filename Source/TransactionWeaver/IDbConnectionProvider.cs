using System.Data.Common;

namespace TransactionWeaver
{
    public interface IDbConnectionProvider
    {
        DbConnection Current { get; set; }

        DbConnection CreateConnection();
    }
}
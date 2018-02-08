using System;
using System.Data;
using System.Data.Common;
using System.Threading;
using Autofac;

namespace TransactionWeaver.Autofac
{
    public class DbConnectionProvider : IDbConnectionProvider
    {
        private readonly Func<DbConnection> _createConnection;

        private AsyncLocal<DbConnection> LocalDbConnection { get; } = new AsyncLocal<DbConnection>();

        public DbConnection Current
        {
            get => LocalDbConnection.Value;
            set => LocalDbConnection.Value = value;
        }

        public DbConnectionProvider(Func<DbConnection> createConnection)
        {
            _createConnection = createConnection;
        }

        public DbConnection CreateConnection()
        {
            return _createConnection();
        }

        public static void Register(ContainerBuilder builder, Func<DbConnection> createConnection)
        {
            builder.RegisterInstance(new DbConnectionProvider(createConnection)).As<IDbConnectionProvider>();
            builder.Register(c => c.Resolve<IDbConnectionProvider>().Current).As<IDbConnection>();
            builder.RegisterType<TransactionScopeManager>().As<ITransactionManager>();
            builder.RegisterType<TransactionInterceptor>();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extras.DynamicProxy;
using Xunit;

namespace TransactionWeaver.Autofac.Tests
{
    public class Class1
    {
        [Fact]
        public void Hoge()
        {
            var settings = ConfigurationManager.ConnectionStrings["AdventureWorks2017"];
            var factory = DbProviderFactories.GetFactory(settings.ProviderName);

            var builder = new ContainerBuilder();
            DbConnectionProvider.Register(builder, () =>
            {
                var connection = factory.CreateConnection();
                connection.ConnectionString = settings.ConnectionString;
                return connection;
            });

            builder.RegisterType<Mock>().As<IMock>().EnableInterfaceInterceptors();
            builder.RegisterType<MockDao>().As<IMockDao>();
            var container = builder.Build();

            var mock = container.Resolve<IMock>();
            mock.Invoke();
            //builder.Register()
        }

        public interface IMock
        {
            [EnableTransaction]
            void Invoke();
        }
        [Intercept(typeof(TransactionInterceptor))]
        public class Mock : IMock
        {
            private readonly IMockDao _mockDao;

            public Mock(IMockDao mockDao)
            {
                _mockDao = mockDao;
            }

            [EnableTransaction]
            public virtual void Invoke()
            {
                
            }
        }

        public interface IMockDao
        {

        }

        public class MockDao : IMockDao
        {
            private readonly IDbConnectionProvider _dbConnectionProvider;

            public MockDao(IDbConnectionProvider dbConnectionProvider)
            {
                _dbConnectionProvider = dbConnectionProvider;
            }
        }
    }
}

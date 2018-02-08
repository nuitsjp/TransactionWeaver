using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Transactions;
using Castle.DynamicProxy;

namespace TransactionWeaver
{
    public class ScopeTransaction : ITransaction
    {
        private readonly IDbConnectionProvider _dbConnectionProvider;

        public ScopeTransaction(IDbConnectionProvider dbConnectionProvider)
        {
            _dbConnectionProvider = dbConnectionProvider;
        }

        public void Invoke(IInvocation invocation)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            using (var connection = _dbConnectionProvider.CreateConnection())
            {
                connection.Open();
                _dbConnectionProvider.Current = connection;
                try
                {
                    invocation.Proceed();

                    scope.Complete();
                }
                finally
                {
                    _dbConnectionProvider.Current = null;
                }
            }
        }
    }
}

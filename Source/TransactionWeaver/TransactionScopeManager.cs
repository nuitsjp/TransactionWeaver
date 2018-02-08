using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Transactions;

namespace TransactionWeaver
{
    public class TransactionScopeManager : ITransactionManager
    {
        private readonly IDbConnectionProvider _dbConnectionProvider;


        public TransactionScopeManager(IDbConnectionProvider dbConnectionProvider)
        {
            _dbConnectionProvider = dbConnectionProvider;
        }

        public ITransaction BeginTransaction()
        {
            return new ScopeTransaction(_dbConnectionProvider);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Reflection;
using System.Text;
using System.Transactions;
using Castle.DynamicProxy;

namespace TransactionWeaver
{
    public class TransactionInterceptor : IInterceptor
    {
        private readonly ITransactionManager _transactionManager;

        public TransactionInterceptor(ITransactionManager transactionManager)
        {
            _transactionManager = transactionManager;
        }

        public void Intercept(IInvocation invocation)
        {
            if (invocation.Method.GetCustomAttribute<EnableTransactionAttribute>() == null)
            {
                invocation.Proceed();
            }
            else
            {
                var transaction = _transactionManager.BeginTransaction();
                transaction.Invoke(invocation);
            }
        }
    }
}

using System;
using Castle.DynamicProxy;

namespace TransactionWeaver
{
    public interface ITransaction
    {
        void Invoke(IInvocation invocation);
    }
}
namespace TransactionWeaver
{
    public interface ITransactionManager
    {
        ITransaction BeginTransaction();
    }
}
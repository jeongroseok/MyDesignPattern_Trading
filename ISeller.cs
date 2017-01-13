namespace Trading
{
    public interface ISeller<TTransaction> where TTransaction : ITransaction
    {
        TTransaction BeginTransaction();
    }
}

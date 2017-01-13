namespace Trading
{
    public interface IBuyer<TTransaction> where TTransaction : ITransaction
    {
        void Approve(TTransaction transaction);
    }
}

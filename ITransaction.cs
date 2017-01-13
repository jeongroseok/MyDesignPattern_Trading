using System;

namespace Trading
{
    public interface ITransaction : IDisposable
    {
        void Commit();
        void Rollback();
    }
}

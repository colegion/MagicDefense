namespace Interfaces
{
    public interface IPoolable
    {
        public abstract void EnableObject();
        public abstract void ReturnToPool();
    }
}

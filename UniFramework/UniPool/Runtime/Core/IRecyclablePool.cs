
namespace Uni.GOPool
{
    public interface IRecyclablePool<T> : IAbstractPool<T> where T : class, IRecyclable { }
}

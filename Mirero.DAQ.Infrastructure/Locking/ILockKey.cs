namespace Mirero.DAQ.Infrastructure.Locking;

public interface ILockKey
{
    public object Key { get; }
}

public class EmptyLockKey : ILockKey
{
    public object Key { get; }

    public EmptyLockKey()
    {
        Key = new object();
    }
}

public static class LockKeyExtension
{
    public static T Key<T>(this ILockKey lockKey)
    {
        return (T) lockKey.Key;
    }

    public static EmptyLockKey EmptyLockKey(this ILockKey? lockKey) => new();
}
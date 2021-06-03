namespace Ondato.Application.Abstractions
{
    public interface ILifetimePolicySubject
    {
        bool HasLifetimeEnded { get; }
    }

    public interface ILifetimePolicySubject<T> : ILifetimePolicySubject
    {
        T Apply(ILifetimePolicy lifetimePolicy, LifetimeEvent lifetimeEvent);
    }
}
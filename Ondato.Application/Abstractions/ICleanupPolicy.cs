using System;

namespace Ondato.Application.Abstractions
{
    public interface ICleanupPolicy<T>
    {
        TimeSpan NextCleanupAfter { get; }
        void Apply(ICleanable<T> cleanable);
    }
}
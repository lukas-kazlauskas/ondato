using System;

namespace Ondato.Application.Abstractions
{
    public interface ICleanable<TValue>
    {
        void Clean(Predicate<TValue> predicate);
    }
}
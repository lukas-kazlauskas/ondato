using System;

namespace Ondato.Application.Abstractions
{
    public interface ILifetimePolicy
    {
        DateTimeOffset CalculateExpiration(LifetimeEvent lifetimeEvent, DateTimeOffset expiresIn,
            int? subjectLifetimeInSeconds = null);
    }
}
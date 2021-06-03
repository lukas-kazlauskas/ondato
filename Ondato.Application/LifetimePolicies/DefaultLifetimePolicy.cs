using System;
using Microsoft.Extensions.Options;
using Ondato.Application.Abstractions;
using Ondato.Core.Configuration;

namespace Ondato.Application.LifetimePolicies
{
    public class DefaultLifetimePolicy : ILifetimePolicy
    {
        private readonly OndatoConfig _ondatoConfig;

        public DefaultLifetimePolicy(IOptions<OndatoConfig> ondatoConfig)
        {
            _ondatoConfig = ondatoConfig.Value;
        }

        public DateTimeOffset CalculateExpiration(LifetimeEvent lifetimeEvent, 
            DateTimeOffset expiresIn,
            int? subjectLifetimeInSeconds = null)
        {
            if (expiresIn != default && DateTimeOffset.Now > expiresIn)
            {
                return expiresIn;
            }
            
            switch (lifetimeEvent)
            {
                case LifetimeEvent.Created:
                case LifetimeEvent.Update:
                case LifetimeEvent.Read:
                    return ExtendExpiration(subjectLifetimeInSeconds);
                default:
                    throw new ArgumentOutOfRangeException(nameof(lifetimeEvent), lifetimeEvent, null);
            }
        }

        private DateTimeOffset ExtendExpiration(int? lifetimeInSeconds)
        {
            lifetimeInSeconds ??= _ondatoConfig.MaxExpireInSeconds;

            if (lifetimeInSeconds > _ondatoConfig.MaxExpireInSeconds)
            {
                throw new ArgumentException(
                    $"Lifetime must not be longer than {_ondatoConfig.MaxExpireInSeconds} seconds");
            }

            return DateTimeOffset.Now + TimeSpan.FromSeconds(lifetimeInSeconds.Value);
        }
    }
}
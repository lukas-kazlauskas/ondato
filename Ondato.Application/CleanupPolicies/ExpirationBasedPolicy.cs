using System;
using Microsoft.Extensions.Options;
using Ondato.Application.Abstractions;
using Ondato.Core.Configuration;

namespace Ondato.Application.CleanupPolicies
{
    public class ExpirationBasedPolicy<T> : ICleanupPolicy<T> 
        where T: ILifetimePolicySubject<T>
    {
        public TimeSpan NextCleanupAfter => TimeSpan.FromSeconds(_ondatoConfig.CleanUpInSeconds);
        
        private readonly OndatoConfig _ondatoConfig;

        public ExpirationBasedPolicy(IOptions<OndatoConfig> ondatoConfig)
        {
            _ondatoConfig = ondatoConfig.Value;
        }

        public void Apply(ICleanable<T> dictionary)
        {
            dictionary.Clean(x => x.HasLifetimeEnded);
        }
    }
}
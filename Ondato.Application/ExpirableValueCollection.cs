using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Ondato.Application.Abstractions;

namespace Ondato.Application
{
    public class ExpirableValueCollection<T> : ILifetimePolicySubject<ExpirableValueCollection<T>>
    {
        [JsonProperty]
        private readonly int? _defaultLifetimeInSeconds;
        
        [JsonProperty]
        private DateTimeOffset _expiresOn;
        
        [JsonProperty]
        public IEnumerable<T> Values { get; private set; }
        public bool HasLifetimeEnded => DateTimeOffset.Now > _expiresOn;

        [JsonConstructor]
        public ExpirableValueCollection(IEnumerable<T> values, DateTimeOffset expiresOn, int? defaultLifetimeInSeconds)
        {
            Values = values;
            _expiresOn = expiresOn;
            _defaultLifetimeInSeconds = defaultLifetimeInSeconds;
        }
        public ExpirableValueCollection(IEnumerable<T> values, int? defaultLifetimeInSeconds)
        {
            Values = values;
            _defaultLifetimeInSeconds = defaultLifetimeInSeconds;
        }

        public ExpirableValueCollection<T> Append(IEnumerable<T> toAppend)
        {
            return new ExpirableValueCollection<T>(Values.Concat(toAppend), _expiresOn, _defaultLifetimeInSeconds);
        }

        public ExpirableValueCollection<T> Apply(ILifetimePolicy lifetimePolicy, LifetimeEvent lifetimeEvent)
        {
            var newExpiration = lifetimePolicy.CalculateExpiration(lifetimeEvent, _expiresOn, _defaultLifetimeInSeconds);
            return new ExpirableValueCollection<T>(Values, newExpiration, _defaultLifetimeInSeconds);
        }
    }
}
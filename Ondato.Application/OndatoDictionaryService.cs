using System.Collections.Generic;
using Ondato.Application.Abstractions;

namespace Ondato.Application
{
    public class OndatoDictionaryService : IOndatoDictionaryService
    {
        private readonly ControlledLifetimeStore<string, ExpirableValueCollection<object>> _controlledLifetimeStore;

        public OndatoDictionaryService(ControlledLifetimeStore<string, ExpirableValueCollection<object>> controlledLifetimeStore)
        {
            _controlledLifetimeStore = controlledLifetimeStore;
        }

        public IEnumerable<object> Create(string key, IEnumerable<object> values, int? expireInSeconds = null)
        {
            var existing = _controlledLifetimeStore.Get(key);
            if (existing != null)
            {
                _controlledLifetimeStore.Remove(key);
            }
            var valueCollection = new ExpirableValueCollection<object>(values, expireInSeconds);
            _controlledLifetimeStore.Create(key, valueCollection);
            return valueCollection.Values;
        }

        public IEnumerable<object> Append(string key, IEnumerable<object> toAppend)
        {
            var existing = _controlledLifetimeStore.Get(key);
            if (existing == null)
            {
                return Create(key, toAppend);
            }
            
            var appended = existing.Append(toAppend);
            return _controlledLifetimeStore.Update(key, appended).Values;
        }

        public void Delete(string key)
        {
            _controlledLifetimeStore.Remove(key);
        }

        public IEnumerable<object> Get(string key)
        {
            var valueCollection = _controlledLifetimeStore.Get(key);
            return valueCollection?.Values;
        }
    }
}
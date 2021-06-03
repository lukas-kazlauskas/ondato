using System.Collections.Generic;

namespace Ondato.Application.Abstractions
{
    public interface IOndatoDictionaryService
    {
        IEnumerable<object> Create(string key, IEnumerable<object> values, int? expireInSeconds = null);
        IEnumerable<object> Append(string key, IEnumerable<object> toAppend);
        void Delete(string key);
        IEnumerable<object> Get(string key);
    }
}
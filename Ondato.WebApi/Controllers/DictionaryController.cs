using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Ondato.Application.Abstractions;
using ondato.Requests;

namespace ondato.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DictionaryController : ControllerBase
    {
        private readonly IOndatoDictionaryService _ondatoDictionaryService;

        public DictionaryController(IOndatoDictionaryService ondatoDictionaryService)
        {
            _ondatoDictionaryService = ondatoDictionaryService;
        }

        [HttpPost]
        [Route("create/{key}")]
        public IEnumerable<object> Create([FromRoute] string key, ExpirableKeyValueDto expirableKeyValue)
        {
            return _ondatoDictionaryService.Create(key, expirableKeyValue.Values, expirableKeyValue.ExpireInSeconds);
        }
        
        [HttpPut]
        [Route("append/{key}")]
        public ActionResult<IEnumerable<object>> Append([FromRoute] string key, KeyValueDto keyValue)
        {
            var result = _ondatoDictionaryService.Append(key, keyValue.Values);
            return result == null 
                ? NotFound() 
                : new ActionResult<IEnumerable<object>>(result);
        }
        
        [HttpDelete]
        [Route("{key}")]
        public ActionResult Delete(string key)
        {
            _ondatoDictionaryService.Delete(key);
            return Ok();
        }

        [HttpGet]
        [Route("{key}")]
        public ActionResult<IEnumerable<object>> Get(string key)
        {
            var result = _ondatoDictionaryService.Get(key);
            return result == null 
                ? NotFound() 
                : new ActionResult<IEnumerable<object>>(result);
        }
    }
}
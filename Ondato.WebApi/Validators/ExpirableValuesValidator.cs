using FluentValidation;
using Microsoft.Extensions.Options;
using Ondato.Core.Configuration;
using ondato.Requests;

namespace ondato.Validators
{
    public class ExpirableValuesValidator : AbstractValidator<ExpirableKeyValueDto>
    {
        public ExpirableValuesValidator(IOptions<OndatoConfig> config, IValidator<KeyValueDto> parentValidator)
        {
            Include(parentValidator);
            
            var configValue = config.Value;
            RuleFor(x => x.ExpireInSeconds).ExclusiveBetween(0, configValue.MaxExpireInSeconds);
        }
    }
}
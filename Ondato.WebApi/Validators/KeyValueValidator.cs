using FluentValidation;
using ondato.Requests;

namespace ondato.Validators
{
    public class KeyValueValidator : AbstractValidator<KeyValueDto>
    {
        public KeyValueValidator()
        {
            CascadeMode = CascadeMode.Continue;
            RuleFor(x => x.Values).NotNull().NotEmpty();
        }
    }
}
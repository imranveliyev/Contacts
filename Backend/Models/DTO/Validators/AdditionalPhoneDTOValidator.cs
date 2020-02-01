using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactsAppApi.Models.DTO.Validators
{
    public class AdditionalPhoneDTOValidator : AbstractValidator<AdditionalPhoneDTO>
    {
        public AdditionalPhoneDTOValidator()
        {
            RuleFor(x => x.Phone)
                .NotEmpty()
                .MaximumLength(100)
                .Matches("^[0-9,-,(,)]*$").WithMessage("Phone is not in a correct format!");

            RuleFor(x => x.ContactId)
                .NotEmpty();
        }
    }
}

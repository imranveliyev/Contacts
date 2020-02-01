using ContactsAppApi.Models.DTO;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactsAppApi.Models.DTO.Validators
{
    public class ContactDTOValidator : AbstractValidator<ContactDTO>
    {
        public ContactDTOValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Surname)
                .MaximumLength(100);

            RuleFor(x => x.Phone)
                .NotEmpty()
                .MaximumLength(100)
                .Matches("^[0-9,-,(,)]*$").WithMessage("Phone is not valid!");

            RuleFor(x => x.Email)
                .MaximumLength(100)
                .EmailAddress();
        }
    }
}

using Business.Dtos.Requests.TaskRequests;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Rules.ValidationRules.FluentValidation.TaskValidators
{
    public class CreateTaskValidator : AbstractValidator<CreateTaskRequest>
    {
        public CreateTaskValidator()
        {
            RuleFor(t => t.Title).NotEmpty();
            RuleFor(t => t.Description).NotEmpty();
            RuleFor(t => t.Status).NotEmpty();

            RuleFor(t => t.Title).MinimumLength(4);
            RuleFor(t => t.Description).MinimumLength(2);
            RuleFor(t => t.Status).MinimumLength(5);
        }
    }
}

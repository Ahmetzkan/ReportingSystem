using Business.Dtos.Requests.ProjectRequests;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Rules.ValidationRules.FluentValidation.ProjectValidators
{
    public class UpdateProjectValidator : AbstractValidator<UpdateProjectRequest>
    {
        public UpdateProjectValidator()
        {
            RuleFor(p => p.Name).NotEmpty();
            RuleFor(p => p.StartDate).NotEmpty();
            RuleFor(p => p.EndDate).NotEmpty();
            RuleFor(p => p.Status).NotEmpty();

            RuleFor(p => p.Name).MinimumLength(2);
            RuleFor(p => p.Status).MinimumLength(5);
        }
    }
}

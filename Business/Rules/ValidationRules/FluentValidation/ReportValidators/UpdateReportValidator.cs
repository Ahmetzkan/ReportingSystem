using Business.Dtos.Requests.ReportRequests;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Rules.ValidationRules.FluentValidation.ReportValidators
{
    public class UpdateReportValidator : AbstractValidator<UpdateReportRequest>
    {
        public UpdateReportValidator()
        {
            RuleFor(r => r.Title).NotEmpty();
            RuleFor(r => r.Content).NotEmpty();

            RuleFor(r => r.Title).MinimumLength(2);
            RuleFor(r => r.Content).MinimumLength(2);
        }
    }
}

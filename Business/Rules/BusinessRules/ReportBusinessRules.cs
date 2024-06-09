using Business.Messages;
using Core.Business.Rules;
using DataAccess.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Rules.BusinessRules
{
    public class ReportBusinessRules : BaseBusinessRules
    {
        private readonly IReportDal _ReportDal;

        public ReportBusinessRules(IReportDal ReportDal)
        {
            _ReportDal = ReportDal;
        }

        public async Task IsExistsReport(Guid reportId)
        {
            var result = await _ReportDal.GetAsync(
                predicate: r => r.Id == reportId,
                enableTracking: false);

            if (result == null)
            {
                throw new BusinessException(BusinessMessages.DataNotFound);
            }
        }
    }
}

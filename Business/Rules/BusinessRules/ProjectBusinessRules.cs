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
    public class ProjectBusinessRules : BaseBusinessRules
    {
        private readonly IProjectDal _projectDal;

        public ProjectBusinessRules(IProjectDal projectDal)
        {
            _projectDal = projectDal;
        }

        public async Task IsExistsProject(Guid projectId)
        {
            var result = await _projectDal.GetAsync(
                predicate: p => p.Id == projectId,
                enableTracking: false);

            if (result == null)
            {
                throw new BusinessException(BusinessMessages.DataNotFound);
            }
        }
    }
}

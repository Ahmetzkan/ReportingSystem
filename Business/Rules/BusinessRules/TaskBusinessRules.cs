using Business.Messages;
using DataAccess.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Rules.BusinessRules
{
    public class TaskBusinessRules
    {
        private readonly ITaskDal _taskDal;

        public TaskBusinessRules(ITaskDal taskDal)
        {
            _taskDal = taskDal;
        }

        public async Task IsExistsTask(Guid taskId)
        {
            var result = await _taskDal.GetAsync(
                predicate: p => p.Id == taskId,
                enableTracking: false
                );
            if (result == null)
            {
                throw new BusinessException(BusinessMessages.DataNotFound);
            }
        }
    }
}

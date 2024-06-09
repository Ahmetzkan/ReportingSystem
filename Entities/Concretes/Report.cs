using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concretes
{
    public class Report : Entity<Guid>
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public Guid? ProjectId { get; set; }
        public Guid? TaskId { get; set; }
        public Project Project { get; set; }
        public Task Task { get; set; }

    }
}
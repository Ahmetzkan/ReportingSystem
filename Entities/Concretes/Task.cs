using Core.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concretes
{
    public class Task : Entity<Guid>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public virtual ICollection<Report>? Reports { get; set; }
    }
}

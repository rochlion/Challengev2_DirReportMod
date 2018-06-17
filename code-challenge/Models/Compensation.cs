using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace challenge.Models
{
    public class Compensation
    {
        [Key]
        public String CompensationId { get; set; }
        public String EmployeeId { get; set; }
        public int Salary { get; set; }
        public DateTime EffectiveDate { get; set; }
    }
}

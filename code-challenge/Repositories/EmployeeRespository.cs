using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using challenge.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using challenge.Data;

namespace challenge.Repositories
{
    public class EmployeeRespository : IEmployeeRepository
    {
        private readonly EmployeeContext _employeeContext;
        private readonly ILogger<IEmployeeRepository> _logger;

        public EmployeeRespository(ILogger<IEmployeeRepository> logger, EmployeeContext employeeContext)
        {
            _employeeContext = employeeContext;
            _logger = logger;
        }

        public Employee Add(Employee employee)
        {
            employee.EmployeeId = Guid.NewGuid().ToString();
            _employeeContext.Employees.Add(employee);
            return employee;
        }

        public Employee GetById(string id)
        {
#if true // PVC Mod
            List<Employee> list = _employeeContext.Employees.ToList();
            return list.Where(i => i.EmployeeId == id).FirstOrDefault();
#else   // Original - returned null for Direct Reports unless call to Employees.ToList() made prior.
            return _employeeContext.Employees.SingleOrDefault(e => e.EmployeeId == id);
#endif
        }

        public ReportingStructure GetReportingStructureById(string id)
        {
            ReportingStructure rs = new ReportingStructure();
            rs.Employee = GetById(id);
            rs.NumberOfReports = CountReports(rs.Employee);
            return rs;
        }

        private int CountReports(Employee employee)
        {
            int reportCnt = 0;

            if (employee.DirectReports == null)
                return reportCnt;
            else
                reportCnt = employee.DirectReports.Count;
            
            foreach (Employee report in employee.DirectReports)
            {
                reportCnt += CountReports(report);
            }

            return reportCnt;
        }


        /// <summary>
        /// PVC Added
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Compensation GetCompensationById(string id)
        {
            List<Compensation> list =_employeeContext.Compensation.ToList();
            return list.Where(i => i.CompensationId == id).FirstOrDefault();
        }

        /// <summary>
        /// PVC Added
        /// </summary>
        /// <param name="compensation"></param>
        /// <returns></returns>
        public Compensation AddCompensation(Compensation compensation)
        {
            compensation.CompensationId = Guid.NewGuid().ToString();
            _employeeContext.Compensation.Add(compensation);
            return compensation;
        }

        public Task SaveAsync()
        {
            return _employeeContext.SaveChangesAsync();
        }

        public Employee Remove(Employee employee)
        {
            return _employeeContext.Remove(employee).Entity;
        }
    }
}

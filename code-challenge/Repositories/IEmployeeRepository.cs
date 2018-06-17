using challenge.Models;
using System;
using System.Threading.Tasks;

namespace challenge.Repositories
{
    public interface IEmployeeRepository
    {
        Employee GetById(String id);
        Employee Add(Employee employee);
        Employee Remove(Employee employee);
        ReportingStructure GetReportingStructureById(String id);
        Compensation GetCompensationById(String id);
        Compensation AddCompensation(Compensation compensation);
        Task SaveAsync();
    }
}
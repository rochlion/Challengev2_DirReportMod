using challenge.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.Extensions.Logging;

namespace challenge.Controllers
{
    [Route("api/reportingstructure")]
    public class ReportingStructureController : Controller
    {
        private readonly ILogger _logger;
        private readonly IEmployeeService _employeeService;


        public ReportingStructureController(ILogger<EmployeeController> logger, IEmployeeService employeeService)
        {
            _logger = logger;
            _employeeService = employeeService;
        }

        [HttpGet("{id}", Name = "getReportingStructureById")]
        public IActionResult GetReportingStructureById(String id)
        {
            _logger.LogDebug($"Received Reporting Structure get request for '{id}'");

            var reportingStructure = _employeeService.GetReportingStructureById(id);

            if (reportingStructure == null)
                return NotFound();

            return Ok(reportingStructure);
        }
    }
}

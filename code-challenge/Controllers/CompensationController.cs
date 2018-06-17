using challenge.Models;
using challenge.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace challenge.Controllers
{
    [Route("api/compensation")]
    public class CompensationController : Controller
    {
        private readonly ILogger _logger;
        private readonly IEmployeeService _employeeService;

        public CompensationController(ILogger<EmployeeController> logger, IEmployeeService employeeService)
        {
            _logger = logger;
            _employeeService = employeeService;
        }

        [HttpGet("{id}", Name = "getCompensationById")]
        public IActionResult GetCompensationById(String id)
        {
            var comp = _employeeService.GetCompensationById(id);

            if (comp == null)
                return NotFound();

            return Ok(comp);
        }

        [HttpPost]
        public IActionResult CreateCompensation([FromBody] Compensation compensation)
        {
            _logger.LogDebug($"Received compensation create request for '{compensation.EmployeeId}'");

            _employeeService.CreateCompensation(compensation);
            
            return CreatedAtRoute("getCompensationById", new { id = compensation.CompensationId }, compensation);
        }
    }
}

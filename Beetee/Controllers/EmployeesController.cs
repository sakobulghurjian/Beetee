using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Beetee.Models;
using Beetee.Models.DB;
using Beetee.ViewModels;

namespace Beetee.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly AppDBContext _context;

        public EmployeesController(AppDBContext context)
        {
            _context = context;
        }

        // GET: api/Employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeVM>>> GetEmployees()
        {
          if (_context.Employees == null)
          {
              return NotFound();
          }
          var employees = await _context.Employees.ToListAsync();
          var HR= await _context.HrData.ToListAsync();
          var emp = new List<EmployeeVM>();
            foreach (var employee in employees)
            {
                emp.Add(new EmployeeVM
                {
                    Employee = employee
                });

            }
            for (int i = 0; i < HR.Count; i++)
            {
                emp[i].HrData = HR[i];
            }
            return emp;
        }

        // PUT: api/Employees/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(int id, EmployeeVM employee)
        {
            if (id != employee.Employee.ID || id != employee.HrData.EmployeeID)
            {
                return BadRequest();
            }

            _context.Entry(employee.Employee).State = EntityState.Modified;
            _context.Entry(employee.HrData).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Employees
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Employee>> PostEmployee(EmployeeVM employee)
        {
          if (_context.Employees == null)
          {
              return Problem("Entity set 'AppDBContext.Employees'  is null.");
          }
          if (_context.HrData == null)
          {
              return Problem("Entity set 'AppDBContext.HRData'  is null.");
          }
            _context.Employees.Add(employee.Employee);
            await _context.SaveChangesAsync();
            employee.HrData.EmployeeID = employee.Employee.ID;
            _context.HrData.Add(employee.HrData);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            if (_context.Employees == null)
            {
                return NotFound();
            }
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            var HrData = _context.HrData.Where(m => m.EmployeeID == employee.ID).FirstOrDefault();

            _context.Employees.Remove(employee);
            _context.HrData.Remove(HrData);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmployeeExists(int id)
        {
            return (_context.Employees?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}

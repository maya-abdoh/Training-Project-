using Microsoft.AspNetCore.Mvc;
using Fleet_Management_system.Data;
using Fleet_Management_system.Models;
using System.Collections.Concurrent;
using System.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using FPro;

namespace Fleet_Management_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriversController : ControllerBase
    {
        private readonly Contextdata _context;

        public DriversController(Contextdata context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<GVAR>> GetDrivers()
        {
            var gvar = new GVAR();
            var driversTable = new DataTable("Drivers");
            driversTable.Columns.Add("DriverID", typeof(long));
            driversTable.Columns.Add("DriverName", typeof(string));
            driversTable.Columns.Add("PhoneNumber", typeof(long));

            var drivers = await _context.Driver.ToListAsync();
            foreach (var driver in drivers)
            {
                var row = driversTable.NewRow();
                row["DriverID"] = driver.Driverid;
                row["DriverName"] = driver.Drivername;
                row["PhoneNumber"] = driver.Phonenumber;
                driversTable.Rows.Add(row);
            }

            gvar.DicOfDT.TryAdd("Drivers", driversTable);
            return Ok(gvar);
        }
        [HttpPost]
        public async Task<IActionResult> PostDriver([FromBody] GVAR gvar)
        {
            if (!gvar.DicOfDT.TryGetValue("Tags", out DataTable tagsTable) || tagsTable.Rows.Count == 0)
            {
                return BadRequest("Incorrect data format or empty tags data.");
            }

            DataRow row = tagsTable.Rows[0];

            string driverName = row["DriverName"]?.ToString();
            string phoneNumber = row["PhoneNumber"]?.ToString();

            if (string.IsNullOrEmpty(driverName) || string.IsNullOrEmpty(phoneNumber))
            {
                return BadRequest("Missing driver name or phone number.");
            }

            long? phoneNumberLong = long.TryParse(phoneNumber, out long num) ? num : (long?)null;

            var newDriver = new Driver
            {
                Drivername = driverName,
                Phonenumber = phoneNumberLong
            };

            await _context.Driver.AddAsync(newDriver);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetDrivers), new { id = newDriver.Driverid }, newDriver);
        }



        [HttpGet("{id}")]
        public async Task<ActionResult<GVAR>> GetDriver(long id)
        {
            var gvar = new GVAR();
            var driversTable = new DataTable("Drivers");
            driversTable.Columns.Add("DriverID", typeof(long));
            driversTable.Columns.Add("DriverName", typeof(string));
            driversTable.Columns.Add("PhoneNumber", typeof(long));

            var driver = await _context.Driver.FindAsync(id);
            if (driver == null)
            {
                return NotFound();
            }

            DataRow row = driversTable.NewRow();
            row["DriverID"] = driver.Driverid;
            row["DriverName"] = driver.Drivername;
            row["PhoneNumber"] = (object)driver.Phonenumber ?? DBNull.Value;
            driversTable.Rows.Add(row);

            gvar.DicOfDT.TryAdd("Drivers", driversTable);

            return Ok(gvar);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutDriver(long id, [FromBody] Driver updatedDriver)
        {
            if (id != updatedDriver.Driverid)
            {
                return BadRequest("Mismatched driver ID in URL and body.");
            }

            if (!DriverExists(id))
            {
                return NotFound();
            }

            _context.Entry(updatedDriver).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return StatusCode(500, ex.Message);
            }

            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDriver(long id)
        {
            var driver = await _context.Driver
                .Include(d => d.Vehiclesinformations) // Include related vehicles information
                .FirstOrDefaultAsync(d => d.Driverid == id);

            if (driver == null)
            {
                return NotFound();
            }

            // Remove or handle related vehicles information
            _context.Vehiclesinformation.RemoveRange(driver.Vehiclesinformations);

            // Now remove the driver
            _context.Driver.Remove(driver);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, "An error occurred while deleting the driver: " + ex.InnerException?.Message);
            }

            return NoContent();
        }


        private bool DriverExists(long id)
        {
            return _context.Driver.Any(e => e.Driverid == id);
        }
    }
}

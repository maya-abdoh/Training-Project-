using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Fleet_Management_system.Data;
using Fleet_Management_system.Models;
using FPro;
using System.Data;
<<<<<<< Updated upstream
=======
using Fleet_Management_system.Utils;
>>>>>>> Stashed changes

namespace Fleet_Management_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CirclegeofencesController : ControllerBase
    {
        private readonly Contextdata _context;

        public CirclegeofencesController(Contextdata context)
        {
            _context = context;
        }

<<<<<<< Updated upstream
        // GET: api/Circlegeofences
        [HttpGet]
        public async Task<ActionResult<GVAR>> GetCirclegeofence()
        {
            var gvar = new GVAR();
            var circlegeofencesTable = new DataTable("Circlegeofences");
            circlegeofencesTable.Columns.Add("GeofenceID", typeof(long));
            circlegeofencesTable.Columns.Add("Radius", typeof(long));
            circlegeofencesTable.Columns.Add("Latitude", typeof(float));
            circlegeofencesTable.Columns.Add("Longitude", typeof(float));

            var circlegeofences = await _context.Circlegeofence.ToListAsync();
            foreach (var circlegeofence in circlegeofences)
            {
                var row = circlegeofencesTable.NewRow();
                row["GeofenceID"] = circlegeofence.Geofenceid ?? 0;
                row["Radius"] = circlegeofence.Radius ?? 0;
                row["Latitude"] = circlegeofence.Latitude ?? 0;
                row["Longitude"] = circlegeofence.Longitude ?? 0;
                circlegeofencesTable.Rows.Add(row);
            }

            gvar.DicOfDT.TryAdd("Circlegeofences", circlegeofencesTable);
            return Ok(gvar);
        }

        // GET: api/Circlegeofences/5
=======
        [HttpGet]
        public async Task<ActionResult<GVAR>> GetCirclegeofence(){
            var gvar = new GVAR();
            var circlegeofences = await _context.Circlegeofence.ToListAsync();
            gvar.DicOfDT.TryAdd("Circlegeofences", ControllersUtils.ToDataTable(circlegeofences));
            return Ok(new { STS = 1, Data = gvar });
        }

>>>>>>> Stashed changes
        [HttpGet("{id}")]
        public async Task<ActionResult<GVAR>> GetCirclegeofence(long id)
        {
            var gvar = new GVAR();
<<<<<<< Updated upstream
            var circlegeofencesTable = new DataTable("Circlegeofences");
            circlegeofencesTable.Columns.Add("GeofenceID", typeof(long));
            circlegeofencesTable.Columns.Add("Radius", typeof(long));
            circlegeofencesTable.Columns.Add("Latitude", typeof(float));
            circlegeofencesTable.Columns.Add("Longitude", typeof(float));
=======
            var circlegeofencesTable = CreateCirclegeofencesTable();
>>>>>>> Stashed changes

            var circlegeofence = await _context.Circlegeofence.FindAsync(id);
            if (circlegeofence == null)
            {
<<<<<<< Updated upstream
                return NotFound();
            }

            var row = circlegeofencesTable.NewRow();
=======
                return NotFound(new { STS = 0, Error = "Geofence not found" });
            }

            var row = circlegeofencesTable.NewRow();
            PopulateCirclegeofenceDataRow(row, circlegeofence);
            circlegeofencesTable.Rows.Add(row);

            gvar.DicOfDT.TryAdd("Circlegeofences", circlegeofencesTable);
            return Ok(new { STS = 1, Data = gvar });
        }



        private DataTable CreateCirclegeofencesTable()
        {
            var table = new DataTable("Circlegeofences");
            table.Columns.Add("GeofenceID", typeof(long));
            table.Columns.Add("Radius", typeof(long));
            table.Columns.Add("Latitude", typeof(float));
            table.Columns.Add("Longitude", typeof(float));
            return table;
        }

        private void PopulateCirclegeofenceDataRow(DataRow row, Circlegeofence circlegeofence)
        {
>>>>>>> Stashed changes
            row["GeofenceID"] = circlegeofence.Geofenceid ?? 0;
            row["Radius"] = circlegeofence.Radius ?? 0;
            row["Latitude"] = circlegeofence.Latitude ?? 0;
            row["Longitude"] = circlegeofence.Longitude ?? 0;
<<<<<<< Updated upstream
            circlegeofencesTable.Rows.Add(row);

            gvar.DicOfDT.TryAdd("Circlegeofences", circlegeofencesTable);
            return Ok(gvar);
        }

        // PUT: api/Circlegeofences/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCirclegeofence(long id, Circlegeofence circlegeofence)
        {
            if (id != circlegeofence.Id)
            {
                return BadRequest();
            }

            _context.Entry(circlegeofence).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CirclegeofenceExists(id))
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

        // POST: api/Circlegeofences
        [HttpPost]
        public async Task<IActionResult> PostCirclegeofence([FromBody] GVAR gvar)
        {
            if (!gvar.DicOfDT.TryGetValue("Tags", out DataTable dt) || dt.Rows.Count == 0)
            {
                return BadRequest("Missing data for Circlegeofences.");
            }

            foreach (DataRow row in dt.Rows)
            {
                var circlegeofence = new Circlegeofence
                {
                    Geofenceid = row.IsNull("GeofenceID") ? null : Convert.ToInt64(row["GeofenceID"]),
                    Radius = row.IsNull("Radius") ? null : Convert.ToInt64(row["Radius"]),
                    Latitude = row.IsNull("Latitude") ? null : Convert.ToSingle(row["Latitude"]),
                    Longitude = row.IsNull("Longitude") ? null : Convert.ToSingle(row["Longitude"]),
                };
                _context.Circlegeofence.Add(circlegeofence);
            }

            await _context.SaveChangesAsync();

            // Assuming you want to return the first added item's details
            return CreatedAtAction("GetCirclegeofence", new { id = ((DataRow)dt.Rows[0])["GeofenceID"] }, gvar);
        }


        // DELETE: api/Circlegeofences/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCirclegeofence(long id)
        {
            var circlegeofence = await _context.Circlegeofence.FindAsync(id);
            if (circlegeofence == null)
            {
                return NotFound();
            }

            _context.Circlegeofence.Remove(circlegeofence);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CirclegeofenceExists(long id)
        {
            return _context.Circlegeofence.Any(e => e.Id == id);
=======
        }

        private Circlegeofence ConvertDataRowToCirclegeofence(DataRow row)
        {
            return new Circlegeofence
            {
                Geofenceid = row.IsNull("GeofenceID") ? null : Convert.ToInt64(row["GeofenceID"]),
                Radius = row.IsNull("Radius") ? null : Convert.ToInt64(row["Radius"]),
                Latitude = row.IsNull("Latitude") ? null : Convert.ToSingle(row["Latitude"]),
                Longitude = row.IsNull("Longitude") ? null : Convert.ToSingle(row["Longitude"]),
            };
>>>>>>> Stashed changes
        }
    }
}

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
using System.Collections.Concurrent;
<<<<<<< Updated upstream
=======
using Fleet_Management_system.Utils;
>>>>>>> Stashed changes

namespace Fleet_Management_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PolygongeofencesController : ControllerBase
    {
        private readonly Contextdata _context;

        public PolygongeofencesController(Contextdata context)
        {
            _context = context;
        }

<<<<<<< Updated upstream
        // GET: api/Polygongeofences
        [HttpGet]
        public async Task<ActionResult<GVAR>> GetPolygongeofence()
        {
            var gvar = new GVAR();
            var polygongeofencesTable = new DataTable("Polygongeofences");
            // Removed the Id column
            polygongeofencesTable.Columns.Add("GeofenceId", typeof(long));
            polygongeofencesTable.Columns.Add("Latitude", typeof(float));
            polygongeofencesTable.Columns.Add("Longitude", typeof(float));

            var polygongeofences = await _context.Polygongeofence.ToListAsync();
            foreach (var polygongeofence in polygongeofences)
            {
                var row = polygongeofencesTable.NewRow();
                row["GeofenceId"] = polygongeofence.Geofenceid ?? 0; // Ensuring null values are handled by coalescing to 0
                row["Latitude"] = polygongeofence.Latitude ?? 0;     // Default to 0 if null
                row["Longitude"] = polygongeofence.Longitude ?? 0;   // Default to 0 if null
                polygongeofencesTable.Rows.Add(row);
            }

            gvar.DicOfDT.TryAdd("Polygongeofences", polygongeofencesTable);
            return Ok(gvar);
        }


        // GET: api/Polygongeofences/5
=======
        [HttpGet]
        public async Task<ActionResult<GVAR>> GetPolygongeofences(){
            var gvar = new GVAR();
            var polygongeofences = await _context.Polygongeofence.ToListAsync();
            gvar.DicOfDT.TryAdd("Polygongeofences", ControllersUtils.ToDataTable(polygongeofences));
            return Ok(new { STS = 1, Data = gvar });
        }

>>>>>>> Stashed changes
        [HttpGet("{id}")]
        public async Task<ActionResult<GVAR>> GetPolygongeofence(long id)
        {
            var gvar = new GVAR();
<<<<<<< Updated upstream
            var polygongeofencesTable = new DataTable("Polygongeofences");
            polygongeofencesTable.Columns.Add("GeofenceId", typeof(long));
            polygongeofencesTable.Columns.Add("Latitude", typeof(float));
            polygongeofencesTable.Columns.Add("Longitude", typeof(float));
=======
            var polygongeofencesTable = CreatePolygongeofenceTable();
>>>>>>> Stashed changes

            var polygongeofence = await _context.Polygongeofence.FindAsync(id);
            if (polygongeofence == null)
            {
<<<<<<< Updated upstream
                return NotFound();
            }

            DataRow row = polygongeofencesTable.NewRow();
            row["GeofenceId"] = polygongeofence.Geofenceid ?? 0;  // Ensuring null values are handled by coalescing to 0
            row["Latitude"] = polygongeofence.Latitude ?? 0;      // Default to 0 if null
            row["Longitude"] = polygongeofence.Longitude ?? 0;    // Default to 0 if null
            polygongeofencesTable.Rows.Add(row);

            gvar.DicOfDT.TryAdd("Polygongeofences", polygongeofencesTable);
            return Ok(gvar);
        }


        // PUT: api/Polygongeofences/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPolygongeofence(long id, Polygongeofence polygongeofence)
        {
            if (id != polygongeofence.Id)
            {
                return BadRequest();
            }

            _context.Entry(polygongeofence).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PolygongeofenceExists(id))
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

        // POST: api/Polygongeofences
        [HttpPost]
        public async Task<ActionResult<GVAR>> PostPolygongeofence([FromBody] GVAR gvar)
        {
            // Retrieve the "Tags" DataTable directly from DicOfDT
            if (!gvar.DicOfDT.TryGetValue("Tags", out DataTable tagsTable) || tagsTable.Rows.Count == 0)
            {
                return BadRequest("Incorrect data format or missing data.");
            }

            // Access data from the first DataRow (assuming the structure matches the expected format)
            DataRow row = tagsTable.Rows[0];

            // Extracting values directly from the DataRow
            long? geofenceId = row.IsNull("GeofenceId") ? null : Convert.ToInt64(row["GeofenceId"]);
            float? latitude = row.IsNull("Latitude") ? null : Convert.ToSingle(row["Latitude"]);
            float? longitude = row.IsNull("Longitude") ? null : Convert.ToSingle(row["Longitude"]);

            // Create a new Polygongeofence object using the extracted data
            var newPolygongeofence = new Polygongeofence
            {
                Geofenceid = geofenceId,
                Latitude = latitude,
                Longitude = longitude
            };

            // Add the new Polygongeofence to the database context and save changes
            _context.Polygongeofence.Add(newPolygongeofence);
            await _context.SaveChangesAsync();

            // Return the action result with the created object and its ID
            return CreatedAtAction(nameof(GetPolygongeofence), new { id = newPolygongeofence.Id }, newPolygongeofence);
        }

        // DELETE: api/Polygongeofences/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePolygongeofence(long id)
        {
            var polygongeofence = await _context.Polygongeofence.FindAsync(id);
            if (polygongeofence == null)
            {
                return NotFound();
            }

            _context.Polygongeofence.Remove(polygongeofence);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PolygongeofenceExists(long id)
        {
            return _context.Polygongeofence.Any(e => e.Id == id);
=======
                return NotFound(new { STS = 0, Error = "Geofence not found" });
            }

            var row = polygongeofencesTable.NewRow();
            PopulateDataRow(row, polygongeofence);
            polygongeofencesTable.Rows.Add(row);

            gvar.DicOfDT.TryAdd("Polygongeofences", polygongeofencesTable);
            return Ok(new { STS = 1, Data = gvar });
        }

 

        private DataTable CreatePolygongeofenceTable()
        {
            var table = new DataTable("Polygongeofences");
            table.Columns.Add("GeofenceId", typeof(long));
            table.Columns.Add("Latitude", typeof(float));
            table.Columns.Add("Longitude", typeof(float));
            return table;
        }

        private void PopulateDataRow(DataRow row, Polygongeofence polygongeofence)
        {
            row["GeofenceId"] = polygongeofence.Geofenceid ?? 0;
            row["Latitude"] = polygongeofence.Latitude ?? 0;
            row["Longitude"] = polygongeofence.Longitude ?? 0;
        }

        private Polygongeofence ConvertDataRowToPolygongeofence(DataRow row)
        {
            return new Polygongeofence
            {
                Geofenceid = row.IsNull("GeofenceId") ? null : Convert.ToInt64(row["GeofenceId"]),
                Latitude = row.IsNull("Latitude") ? null : Convert.ToSingle(row["Latitude"]),
                Longitude = row.IsNull("Longitude") ? null : Convert.ToSingle(row["Longitude"])
            };
>>>>>>> Stashed changes
        }
    }
}

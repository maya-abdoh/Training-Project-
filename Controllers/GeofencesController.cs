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

namespace Fleet_Management_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeofencesController : ControllerBase
    {
        private readonly Contextdata _context;

        public GeofencesController(Contextdata context)
        {
            _context = context;
        }

        // GET: api/Geofences
        [HttpGet]
        public async Task<ActionResult<GVAR>> GetGeofence()
        {
            var gvar = new GVAR();
            var geofencesTable = new DataTable("Geofences");
            geofencesTable.Columns.Add("GeofenceID", typeof(long));
            geofencesTable.Columns.Add("GeofenceType", typeof(string));
            geofencesTable.Columns.Add("AddedDate", typeof(long));
            geofencesTable.Columns.Add("StrokeColor", typeof(string));
            geofencesTable.Columns.Add("StrokeOpacity", typeof(int));
            geofencesTable.Columns.Add("StrokeWeight", typeof(int));
            geofencesTable.Columns.Add("FillColor", typeof(string));
            geofencesTable.Columns.Add("FillOpacity", typeof(int));

            var geofences = await _context.Geofence.ToListAsync();
            foreach (var geofence in geofences)
            {
                var row = geofencesTable.NewRow();
                row["GeofenceID"] = geofence.Geofenceid;
                row["GeofenceType"] = geofence.Geofencetype;
                row["AddedDate"] = geofence.Addeddate ?? 0;
                row["StrokeColor"] = geofence.Strokecolor;
                row["StrokeOpacity"] = geofence.Strokeopacity ?? 0;
                row["StrokeWeight"] = geofence.Strokeweight ?? 0;
                row["FillColor"] = geofence.Fillcolor;
                row["FillOpacity"] = geofence.Fillopacity ?? 0;
                geofencesTable.Rows.Add(row);
            }

            gvar.DicOfDT.TryAdd("Geofences", geofencesTable);
            return Ok(gvar);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GVAR>> GetGeofence(long id)
        {
            var gvar = new GVAR();
            var geofencesTable = new DataTable("Geofences");
            geofencesTable.Columns.Add("GeofenceID", typeof(long));
            geofencesTable.Columns.Add("GeofenceType", typeof(string));
            geofencesTable.Columns.Add("AddedDate", typeof(long));
            geofencesTable.Columns.Add("StrokeColor", typeof(string));
            geofencesTable.Columns.Add("StrokeOpacity", typeof(int));
            geofencesTable.Columns.Add("StrokeWeight", typeof(int));
            geofencesTable.Columns.Add("FillColor", typeof(string));
            geofencesTable.Columns.Add("FillOpacity", typeof(int));

            var geofence = await _context.Geofence.FindAsync(id);
            if (geofence == null)
            {
                return NotFound();
            }

            var row = geofencesTable.NewRow();
            row["GeofenceID"] = geofence.Geofenceid;
            row["GeofenceType"] = geofence.Geofencetype;
            row["AddedDate"] = geofence.Addeddate ?? 0;
            row["StrokeColor"] = geofence.Strokecolor;
            row["StrokeOpacity"] = geofence.Strokeopacity ?? 0;
            row["StrokeWeight"] = geofence.Strokeweight ?? 0;
            row["FillColor"] = geofence.Fillcolor;
            row["FillOpacity"] = geofence.Fillopacity ?? 0;
            geofencesTable.Rows.Add(row);

            gvar.DicOfDT.TryAdd("Geofences", geofencesTable);
            return Ok(gvar);
        }


        // PUT: api/Geofences/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGeofence(long id, Geofence geofence)
        {
            if (id != geofence.Geofenceid)
            {
                return BadRequest();
            }

            _context.Entry(geofence).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GeofenceExists(id))
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

        [HttpPost]
        public async Task<IActionResult> PostGeofence([FromBody] GVAR gvar)
        {
            // Change "Geofences" to "Tags" in the TryGetValue method
            if (!gvar.DicOfDT.TryGetValue("Tags", out DataTable tagsTable) || tagsTable.Rows.Count == 0)
            {
                return BadRequest("Incorrect data format or empty tags data.");
            }

            DataRow row = tagsTable.Rows[0]; // Assuming you are sending one geofence at a time

            var geofence = new Geofence
            {
                Geofencetype = row["GeofenceType"].ToString(),
                Addeddate = Convert.ToInt64(row["AddedDate"]),
                Strokecolor = row["StrokeColor"].ToString(),
                Strokeopacity = Convert.ToInt32(row["StrokeOpacity"]),
                Strokeweight = Convert.ToInt32(row["StrokeWeight"]),
                Fillcolor = row["FillColor"].ToString(),
                Fillopacity = Convert.ToInt32(row["FillOpacity"])
            };

            _context.Geofence.Add(geofence);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGeofence", new { id = geofence.Geofenceid }, geofence);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGeofence(long id)
        {
            var geofence = await _context.Geofence.FindAsync(id);
            if (geofence == null)
            {
                return NotFound();
            }

            // Handle dependent rectangle geofences
            var dependentRectangles = _context.Rectanglegeofence.Where(rg => rg.Geofenceid == id).ToList();
            if (dependentRectangles.Any())
            {
                _context.Rectanglegeofence.RemoveRange(dependentRectangles);
            }

            // Handle dependent polygon geofences
            var dependentPolygons = _context.Polygongeofence.Where(pg => pg.Geofenceid == id).ToList();
            if (dependentPolygons.Any())
            {
                _context.Polygongeofence.RemoveRange(dependentPolygons);
            }

            // Handle dependent circle geofences
            var dependentCircles = _context.Circlegeofence.Where(cg => cg.Geofenceid == id).ToList();
            if (dependentCircles.Any())
            {
                _context.Circlegeofence.RemoveRange(dependentCircles);
            }

            try
            {
                await _context.SaveChangesAsync();
                _context.Geofence.Remove(geofence);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                // If the update fails due to other dependencies or data issues
                return StatusCode(500, $"Error removing geofence: {ex.InnerException?.Message ?? ex.Message}");
            }
        }





        private bool GeofenceExists(long id)
        {
            return _context.Geofence.Any(e => e.Geofenceid == id);
        }
    }
}

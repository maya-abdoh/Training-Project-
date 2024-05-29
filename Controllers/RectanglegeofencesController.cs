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

namespace Fleet_Management_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RectanglegeofencesController : ControllerBase
    {
        private readonly Contextdata _context;

        public RectanglegeofencesController(Contextdata context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<GVAR>> GetRectanglegeofence()
        {
            var gvar = new GVAR();
            var rectanglegeofencesTable = new DataTable("Rectanglegeofences");
            rectanglegeofencesTable.Columns.Add("GeofenceId", typeof(long));
            rectanglegeofencesTable.Columns.Add("North", typeof(float));
            rectanglegeofencesTable.Columns.Add("East", typeof(float));
            rectanglegeofencesTable.Columns.Add("South", typeof(float));
            rectanglegeofencesTable.Columns.Add("West", typeof(float));

            var rectanglegeofences = await _context.Rectanglegeofence.ToListAsync();
            foreach (var rectanglegeofence in rectanglegeofences)
            {
                var row = rectanglegeofencesTable.NewRow();
                row["GeofenceId"] = rectanglegeofence.Geofenceid ?? 0;
                row["North"] = rectanglegeofence.North ?? 0;
                row["East"] = rectanglegeofence.East ?? 0;
                row["South"] = rectanglegeofence.South ?? 0;
                row["West"] = rectanglegeofence.West ?? 0;
                rectanglegeofencesTable.Rows.Add(row);
            }

            gvar.DicOfDT.TryAdd("Rectanglegeofences", rectanglegeofencesTable);
            return Ok(gvar);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GVAR>> GetRectanglegeofence(long id)
        {
            var gvar = new GVAR();
            var rectanglegeofencesTable = new DataTable("Rectanglegeofences");
            rectanglegeofencesTable.Columns.Add("GeofenceId", typeof(long));
            rectanglegeofencesTable.Columns.Add("North", typeof(float));
            rectanglegeofencesTable.Columns.Add("East", typeof(float));
            rectanglegeofencesTable.Columns.Add("South", typeof(float));
            rectanglegeofencesTable.Columns.Add("West", typeof(float));

            var rectanglegeofence = await _context.Rectanglegeofence.FindAsync(id);
            if (rectanglegeofence == null)
            {
                return NotFound();
            }

            DataRow row = rectanglegeofencesTable.NewRow();
            row["GeofenceId"] = rectanglegeofence.Geofenceid ?? 0;
            row["North"] = rectanglegeofence.North ?? 0;
            row["East"] = rectanglegeofence.East ?? 0;
            row["South"] = rectanglegeofence.South ?? 0;
            row["West"] = rectanglegeofence.West ?? 0;
            rectanglegeofencesTable.Rows.Add(row);

            gvar.DicOfDT.TryAdd("Rectanglegeofences", rectanglegeofencesTable);
            return Ok(gvar);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutRectanglegeofence(long id, Rectanglegeofence rectanglegeofence)
        {
            if (id != rectanglegeofence.Id)
            {
                return BadRequest();
            }

            _context.Entry(rectanglegeofence).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RectanglegeofenceExists(id))
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
        public async Task<ActionResult<GVAR>> PostRectanglegeofence([FromBody] GVAR gvar)
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
            float? north = row.IsNull("North") ? null : Convert.ToSingle(row["North"]);
            float? east = row.IsNull("East") ? null : Convert.ToSingle(row["East"]);
            float? south = row.IsNull("South") ? null : Convert.ToSingle(row["South"]);
            float? west = row.IsNull("West") ? null : Convert.ToSingle(row["West"]);

            var newRectanglegeofence = new Rectanglegeofence
            {
                Geofenceid = geofenceId,
                North = north,
                East = east,
                South = south,
                West = west
            };

            _context.Rectanglegeofence.Add(newRectanglegeofence);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRectanglegeofence), new { id = newRectanglegeofence.Id }, newRectanglegeofence);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRectanglegeofence(long id)
        {
            var rectanglegeofence = await _context.Rectanglegeofence.FindAsync(id);
            if (rectanglegeofence == null)
            {
                return NotFound();
            }

            _context.Rectanglegeofence.Remove(rectanglegeofence);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RectanglegeofenceExists(long id)
        {
            return _context.Rectanglegeofence.Any(e => e.Id == id);
        }
    }
}

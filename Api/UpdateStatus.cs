using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TaskManager.Database;
using TaskManager.Class;

namespace TaskManager.Controllers
{
    [Route("api")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly TakClassDatabase _context;

        public TaskController(TakClassDatabase context)
        {
            _context = context;
        }

        [HttpPost("update-tile-position")]
        public async Task<IActionResult> UpdateTilePosition([FromBody] UpdateTilePositionRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { success = false, message = "Invalid data" });
            }

            var tile = await _context.TaskClasses.FindAsync(request.Id);
            if (tile == null)
            {
                return NotFound(new { success = false, message = "Tile not found" });
            }

            var status = await _context.Statuses
                .FirstOrDefaultAsync(s => s.Name == request.NewStatus);

            if (status == null)
            {
                return BadRequest(new { success = false, message = $"Status '{request.NewStatus}' not found" });
            }

            tile.StatusS = status.Name;

            _context.TaskClasses.Update(tile);
            await _context.SaveChangesAsync();

            return Ok(new { success = true });
        }




        public class UpdateTilePositionRequest
        {
            public int Id { get; set; }
            public string NewStatus { get; set; }
        }
    }
}

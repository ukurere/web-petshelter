// Controllers/Api/VolunteerTasksController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using web_petshelter.Data;
using web_petshelter.Models;
using web_petshelter.RealTime;

[Route("api/volunteer-tasks")]
[ApiController]
public class VolunteerTasksApiController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IHubContext<TasksHub, ITasksClient> _hub;
    public VolunteerTasksApiController(AppDbContext db, IHubContext<TasksHub, ITasksClient> hub)
        => (_db, _hub) = (db, hub);

    [HttpGet]
    public async Task<IEnumerable<VolunteerTask>> Get() =>
        await _db.VolunteerTasks.AsNoTracking().OrderByDescending(t => t.UpdatedAt).ToListAsync();

    [HttpPost]
    public async Task<ActionResult<VolunteerTask>> Create([FromBody] VolunteerTask input)
    {
        input.Id = 0; input.UpdatedAt = DateTime.UtcNow;
        _db.VolunteerTasks.Add(input);
        await _db.SaveChangesAsync();
        await _hub.Clients.All.TaskAdded(input);          // realtime
        return Created($"/api/volunteer-tasks/{input.Id}", input);
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult> Patch(int id, [FromBody] VolunteerTask patch)
    {
        var t = await _db.VolunteerTasks.FindAsync(id);
        if (t is null) return NotFound();
        t.Title = patch.Title ?? t.Title;
        t.IsDone = patch.IsDone;
        t.Assignee = patch.Assignee ?? t.Assignee;
        t.ShelterId = patch.ShelterId ?? t.ShelterId;
        t.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        await _hub.Clients.All.TaskUpdated(t);           // realtime
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var t = await _db.VolunteerTasks.FindAsync(id);
        if (t is null) return NotFound();
        _db.Remove(t); await _db.SaveChangesAsync();
        await _hub.Clients.All.TaskDeleted(id);          // realtime
        return NoContent();
    }
}

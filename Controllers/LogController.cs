using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinimalChatAppApi.Data;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text.Json;

namespace MinimalChatAppApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LogController : ControllerBase
    {
        private readonly ChatContext _context;
        public LogController(ChatContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetLogs([FromQuery] DateTime? startTime = null, [FromQuery] DateTime? endTime = null)
        {

            var logsQuery = _context.Log.AsQueryable();

            // Filter logs based on the provided start and end times, if any
            if (startTime != null)
            {
                logsQuery = logsQuery.Where(l => l.Timestamp >= startTime);
            }

            if (endTime != null)
            {
                logsQuery = logsQuery.Where(l => l.Timestamp <= endTime);
            }

            var logs = logsQuery.ToList();

            //logs.ForEach(l => {
            //    l.RequestBody = JObject.Parse(l.RequestBody).ToString(Formatting.None);
            //});

            // If no logs found based on the provided filter, return 404 Not Found
            //if (logs.isEmpty)
            //{
            //    return NotFound(new { error = "No logs found." });
            //}

            return Ok(new { Logs = logs });


        }
    }
}

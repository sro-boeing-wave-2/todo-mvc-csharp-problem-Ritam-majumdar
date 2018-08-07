using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using ProjectX.Services;
using ProjectX.Contract;
using ProjectX.Models;

namespace ProjectX.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly INotesService _NoteServices;
        //private readonly NoteContext _context;

        public NotesController(NoteContext context)
        {
            _NoteServices = new NoteServices(context);
        }

        [HttpGet]
        public IActionResult GetNoteByPrimitive(
            [FromQuery(Name = "Id")] int Id,
            [FromQuery(Name = "Title")] string Title,
            [FromQuery(Name = "Message")] string Message,
            [FromQuery(Name = "Pinned")] string Pinned,
            [FromQuery(Name = "Label")] string Label)
        {
            List<Note> temp = _NoteServices.GetNoteByPrimitive(Id, Title, Message, Pinned, Label);

            if (temp.Count == 0)
            {
                return NotFound();
            }

            return Ok(temp);
        }

        // PUT: api/Notes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNote([FromRoute] int id, [FromBody] Note note)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != note.Id)
            {
                return BadRequest();
            }
            bool flag = await _NoteServices.PutNote(id, note);
            if (flag)
                return CreatedAtAction("GetNoteByPrimitive", new { id = note.Id } , note);
            else
                return NotFound();

        }

        // POST: api/Notes
        [HttpPost]
        public async Task<IActionResult> PostNote([FromBody] Note note)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _NoteServices.Post(note);
            return CreatedAtAction(nameof(GetNoteByPrimitive), new { id = note.Id }, note);
        }
        // DELETE: api/Notes/5
        [HttpDelete]
        public async Task<IActionResult> DeleteNote(
            [FromQuery(Name = "Id")] int Id,
            [FromQuery(Name = "Title")] string Title,
            [FromQuery(Name = "Message")] string Message,
            [FromQuery(Name = "Pinned")] string Pinned,
            [FromQuery(Name = "Label")] string Label)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            List<Note> note;
            note = await _NoteServices.DeleteNote(Id, Title, Message, Pinned, Label);
            if (note.Count == 0)
            {
                return NotFound();
            }

            return Ok(note);
        }

        private bool NoteExists(int id)
        {
            return _NoteServices.NoteExists(id);
        }
    }
}
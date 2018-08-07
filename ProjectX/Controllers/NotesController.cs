using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectX.Models;
namespace ProjectX.Controllers
{ 
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly NoteContext _context;
        public NotesController(NoteContext context)
        {
            _context = context;
        }
        //// GET: api/Notes
        //[HttpGet]
        // public IEnumerable<Note> GetNote()
        // {
        //     return _context.Note.Include(x => x.labellist).Include(y => y.check);
        // }
        // GET: api/Notes/5
        [HttpGet]
        public IActionResult GetNoteByPrimitive(
             [FromQuery(Name = "Id")] int Id,
             [FromQuery(Name = "Title")] string Title,
             [FromQuery(Name = "Message")] string Message,
             [FromQuery(Name = "Pinned")] bool Pinned)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            List<Note> temp = new List<Note>();
            temp = _context.Note.Include(x => x.CheckList).Include(x => x.Label)
                .Where(element => element.Title == ((Title == null) ? element.Title : Title)
                      && element.Message == ((Message == null) ? element.Message : Message)
                      && element.Pinned == ((!Pinned) ? element.Pinned : Pinned)
                      && element.Id == ((Id == 0) ? element.Id : Id)).ToList();
            if (temp == null)
            {
                return NotFound();
            }
            return Ok(temp);
        }
        //public async Task<IActionResult> GetNote([FromRoute] int id)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    var note = await _context.Note.FindAsync(id);
        //    if (note == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(note);
        //}
        // PUT: api/Notes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNote([FromRoute] int id, [FromBody] Note note)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _context.Note.Include(x => x.Label).Include(x => x.CheckList).ForEachAsync(x =>
            {
                if (x.Id == note.Id)
                {
                    x.Title = note.Title;
                    x.Message = note.Message;
                    foreach (Label y in note.Label)
                    {
                        Label a = x.Label.Find(z => z.Id == y.Id);
                        if (a != null)
                        {
                            a.label = y.label;
                        }
                        else
                        {
                            Label lab = new Label() { label = y.label };
                            x.Label.Add(lab);
                        }
                    }
                    foreach (CheckList obj in note.CheckList)
                    {
                        CheckList c = x.CheckList.Find(z => z.Id == obj.Id);
                        if (c != null)
                        {
                            c.Checklist = obj.Checklist;
                        }
                        else
                        {
                            CheckList a = new CheckList { Checklist = obj.Checklist };
                            x.CheckList.Add(a);
                        }
                    }
                }
            });
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NoteExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            //return CreatedAtAction(nameof(GetNoteByPrimitive), new
            //{
            //    note
            //});
            return Ok(note);
        }
        // POST: api/Notes
        [HttpPost]
        public async Task<IActionResult> PostNote([FromBody] Note note)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _context.Note.Add(note);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetNoteByPrimitive", new { id = note.Id }, note);
        }
        // DELETE: api/Notes/5
        [HttpDelete]
        public async Task<IActionResult> DeleteNote([FromQuery(Name = "Id")] int Id,
             [FromQuery(Name = "Title")] string Title,
             [FromQuery(Name = "Message")] string Message,
             [FromQuery(Name = "Pinned")] bool Pinned)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            List<Note> temp = new List<Note>();
            temp = _context.Note.Include(x => x.CheckList).Include(x => x.Label)
                .Where(element => element.Title == ((Title == null) ? element.Title : Title)
                      && element.Message == ((Message == null) ? element.Message : Message)
                      && element.Pinned == ((!Pinned) ? element.Pinned : Pinned)
                      && element.Id == ((Id == 0) ? element.Id : Id)).ToList();
            if (temp == null)
            {
                return NotFound();
            }
            //_context.Note.Remove(note);
            //await _context.SaveChangesAsync();
            //return Ok(note);
            temp.ForEach(NoteDel => _context.CheckList.RemoveRange(NoteDel.CheckList));
            temp.ForEach(NoteDel => _context.Label.RemoveRange(NoteDel.Label));
            _context.Note.RemoveRange(temp);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        private bool NoteExists(int id)
        {
            return _context.Note.Any(e => e.Id == id);
        }
    }
}
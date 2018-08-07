using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectX.Contract;
using ProjectX.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectX.Services
{
    public class NoteServices : INotesService
    {
        private readonly NoteContext _context;
        public NoteServices(NoteContext context)
        {
            _context = context;
        }
        public List<Note> GetNoteByPrimitive(int Id, string Title, string Message, string Pinned, string Label)
        {
            List<Note> temp;
            temp = _context.Note.Include(x => x.CheckList).Include(x => x.Label)
                .Where(element => element.Title == ((Title == null) ? element.Title : Title)
                      && element.Message == ((Message == null) ? element.Message : Message)
                      && element.Pinned == ((Pinned == null) ? element.Pinned : Convert.ToBoolean(Pinned))
                      && element.Id == ((Id == 0) ? element.Id : Id)
                      && element.Label.Any(x => (Label != null) ? x.label == Label : true)).ToList();
            return temp;
        }
        public async Task<bool> PutNote(int id, Note note)
        {
            bool flag = false;
            await _context.Note.Include(x => x.CheckList).Include(x => x.Label).ForEachAsync(element =>
            {
                if (element.Id == note.Id)
                {
                    flag = true;
                    element.Message = note.Message;
                    element.Pinned = note.Pinned;
                    element.Title = note.Title;
                    _context.Label.RemoveRange(element.Label);
                    element.Label.AddRange(note.Label);
                    _context.CheckList.RemoveRange(element.CheckList);
                    element.CheckList.AddRange(note.CheckList);
                }
            });
            if (flag)
                await _context.SaveChangesAsync();
            return flag;
        }
        public async Task Post(Note note)
        {
            _context.Note.Add(note);
            await _context.SaveChangesAsync();
        }
        public async Task<List<Note>> DeleteNote(int Id, string Title, string Message, string Pinned, string Label)
        {

            List<Note> note = _context.Note.Include(x => x.CheckList).Include(x => x.Label)
                .Where(element =>
                element.Id == ((Id == 0) ? element.Id : Id)
                && element.Title == ((Title == null) ? element.Title : Title)
                && element.Message == ((Message == null) ? element.Message : Message)
                && element.Pinned == ((Pinned == null) ? element.Pinned : Convert.ToBoolean(Pinned))
                && element.Label.Any(label => label.label == ((Label == null) ? label.label : Label))
                ).ToList();

            if (note == null)
            {
                return note;
            }
            else
            {
                note.ForEach(NoteDel => _context.CheckList.RemoveRange(NoteDel.CheckList));
                note.ForEach(NoteDel => _context.Label.RemoveRange(NoteDel.Label));
                _context.Note.RemoveRange(note);
            }
            await _context.SaveChangesAsync();
            return note;
        }
        public bool NoteExists(int id)
        {
            return _context.Note.Any(e => e.Id == id);
        }

    }
}
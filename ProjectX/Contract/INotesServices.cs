using Microsoft.AspNetCore.Mvc;
using ProjectX.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectX.Contract
{
    public interface INotesService
    {
        List<Note> GetNoteByPrimitive(int Id, string Title, string Message, string Pinned, string Label);
        Task<bool> PutNote(int Id, Note note);
        Task Post(Note note);
        Task<List<Note>> DeleteNote(int Id, string Title, string Message, string Pinned, string Label);
        bool NoteExists(int id);
    }
}
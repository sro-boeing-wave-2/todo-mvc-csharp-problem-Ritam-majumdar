using System;
using Xunit;
using ProjectX;
using ProjectX.Models;
using ProjectX.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using System.Collections.Generic;
namespace XUnitTestingProjectX
{
    public class UnitTest1
    {
        private readonly NotesController _controller;
        private readonly NoteContext _context;
        Note TestNoteInitial = new Note
        {
            Title = "Initial Note",
            Message = "Some Text",
            CheckList = new List<CheckList>()
                       {
                           new CheckList(){ Checklist = "checklist-2-1"},
                           new CheckList(){ Checklist = "checklist-2-2"}
                       },
            Label = new List<Label>()
                       {
                           new Label(){label = "Label-2-1-Deletable"},
                           new Label(){ label = "Label-2-2-Deletable"}
                       },
            Pinned = false
        };
        
        Note TestNotePost = new Note
        {
            Title = "PostNote",
            Message = "Note to Post",
            CheckList = new List<CheckList>()
                       {
                           new CheckList(){ Checklist = "checklist-2-1"},
                           new CheckList(){ Checklist = "checklist-2-2"}
                       },
            Label = new List<Label>()
                       {
                           new Label(){label = "Label-2-1-Deletable"},
                           new Label(){ label = "Label-2-2-Deletable"}
                       },
            Pinned = false
        };
        Note TestNotePut = new Note
        {
            Id = 1,
            Title = "PutNote",
            Message = "Note to Put",
            CheckList = new List<CheckList>()
                       {
                           new CheckList(){ Checklist = "checklist-1"},
                           new CheckList(){ Checklist = "checklist-2"}
                       },
            Label = new List<Label>()
                       {
                           new Label(){label = "Label-1-Deletable"},
                           new Label(){ label = "Label-2-Deletable"}
                       },
            Pinned = false
        };
        public UnitTest1()
        {
            var dbBuilder = new DbContextOptionsBuilder<NoteContext>();
            dbBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
            var context = new NoteContext(dbBuilder.Options);

            _context = context;
            _context.Note.AddRange(TestNoteInitial);
            _context.SaveChangesAsync();

            _controller = new NotesController(_context);
        }
       
        
        [Fact]
        public async void TestGetNotes()
        {
            var result =  _controller.GetNoteByPrimitive(0, null, null, false);
            var objectresult = result as OkObjectResult;
            var notes = objectresult.Value as List<Note>;
            Console.WriteLine(notes);
            Assert.Equal(1, notes.Count);
        }
        [Fact]
        public async void TestGetByTitle()
        {

            var result = _controller.GetNoteByPrimitive(0, "Initial Note", null, false);
            var objectresult = result as OkObjectResult;
            var notes = objectresult.Value as List<Note>;
            Console.WriteLine(notes);
            Assert.Equal(notes[0],TestNoteInitial);
        }
        [Fact]
        public async void TestPostNotes()
        {
            var response = await _controller.PostNote(TestNotePost);
            var responseOkObject = response as CreatedAtActionResult;
            Note note = responseOkObject.Value as Note;
            Assert.Equal(note,TestNotePost);
        }
        [Fact]
        public async void TestPostNotesInvalid()
        {
            var response = await _controller.PostNote(TestNotePost);
            var responseOkObject = response as CreatedAtActionResult;
            Note note = responseOkObject.Value as Note;
            Assert.Equal(note, TestNotePost);
        }
        [Fact]
        public async void Put()
        {
            var response = await _controller.PutNote(1, TestNotePut);
            var responseOkObject = response as OkObjectResult;
            Note note = responseOkObject.Value as Note;
            //Console.WriteLine(GetNoteByPrimitive.Id);
            Assert.Equal(note,TestNotePut);
        }
        [Fact]
        public async void TestGetByID()
        {

            var result = _controller.GetNoteByPrimitive(6, null, null, false);
            var objectresult = result as OkObjectResult;
            var notes = objectresult.Value as List<Note>;
            Console.WriteLine(notes);
            Assert.Equal(notes[0], TestNoteInitial);
        }
        [Fact]
        public async void Delete()
        {
            var result = await _controller.DeleteNote(0, null, null, true);
            Assert.True(result is NoContentResult);
        }
    }
}

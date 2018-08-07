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
        Note TestNoteProper = new Note
        {
            Title = "Title-1-Deletable",
            Message = "Message-1-Deletable",
            CheckList = new List<CheckList>()
                        {
                            new CheckList(){ Checklist = "checklist-1", IsChecked = true},
                            new CheckList(){ Checklist = "checklist-2", IsChecked = false}
                        },
            Label = new List<Label>()
                        {
                            new Label(){label = "Label-1-Deletable"},
                            new Label(){ label = "Label-2-Deletable"}
                        },
            Pinned = false
        };
        Note TestNotePut = new Note
        {
            Id = 6,
            Title = "Title-1-Deletable",
            Message = "Message-1-Deletable",
            CheckList = new List<CheckList>()
                        {
                            new CheckList(){ Checklist = "checklist-1", IsChecked = true},
                            new CheckList(){ Checklist = "checklist-2", IsChecked = false}
                        },
            Label = new List<Label>()
                        {
                            new Label(){label = "Label-1-Deletable"},
                            new Label(){ label = "Label-2-Deletable"}
                        },
            Pinned = false
        };
        Note TestNotePost = new Note
        {
            Title = "Title-2-Deletable",
            Message = "Message-2-Deletable",
            CheckList = new List<CheckList>()
                        {
                            new CheckList(){ Checklist = "checklist-2-1", IsChecked = true},
                            new CheckList(){ Checklist = "checklist-2-2", IsChecked = false}
                        },
            Label = new List<Label>()
                        {
                            new Label(){label = "Label-2-1-Deletable"},
                            new Label(){ label = "Label-2-2-Deletable"}
                        },
            Pinned = false
        };
        Note TestNoteDelete = new Note()
        {
            Title = "this is deleted title",
            Message = "some text",
            Pinned = false,
            Label = new List<Label>
               {
                   new Label{ label="My First Tag" },
                   new Label{ label = "My second Tag" },
                   new Label{ label = "My third Tag" },
               },
            CheckList = new List<CheckList>
               {
               new CheckList{Checklist="first item" , IsChecked = true},
               new CheckList{Checklist="second item", IsChecked = true},
               new CheckList{Checklist="third item", IsChecked = true},
               }
        };
        public UnitTest1()
        {
            var optionsBuilder = new DbContextOptionsBuilder<NoteContext>();
            optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());

            //var dbBuilder = new DbContextOptionsBuilder<NoteContext>();
            //dbBuilder.UseInMemoryDatabase("NoteContext");
            var context = new NoteContext(optionsBuilder.Options);
            _context = context;
            _context.Note.AddRange(TestNoteProper);
            _context.Note.AddRange(TestNoteDelete);
            _context.SaveChangesAsync();
            _controller = new NotesController(_context);
        }

        [Fact]
        public void GetAll()
        {

            var response = _controller.GetNoteByPrimitive(0, null, null, "false", null);
            var responseOkObject = response as OkObjectResult;
            List<Note> Notes = responseOkObject.Value as List<Note>;
            Assert.Equal(2, Notes.Count);

        }
        [Fact]
        public void GetByTitle()
        {
            var response = _controller.GetNoteByPrimitive(0, "Title-1-Deletable", null, "false", null);
            var responseOkObject = response as OkObjectResult;
            List<Note> Notes = responseOkObject.Value as List<Note>;
            Assert.Equal(TestNoteProper, Notes[0]);
        }
        //[Fact]
        //public void GetByPinned()
        //{
        //    var response = _controller.GetNoteByPrimitive(0, null, null, "true", null);
        //    var responseOkObject = response as OkObjectResult;
        //    List<Note> Notes = responseOkObject.Value as List<Note>;
        //    Assert.Equal(TestNoteProper, Notes[0]);
        //}
        [Fact]
        public async void Post()
        {
            var response = await _controller.PostNote(TestNotePost);
            var responseOkObject = response as CreatedAtActionResult;
            Note note = responseOkObject.Value as Note;
            Assert.Equal(note.Title, TestNotePost.Title);
        }
        [Fact]
        public async void Put()
        {
            var response = await _controller.PutNote(6, TestNotePut);
            var responseOkObject = response as CreatedAtActionResult;
            Note note = responseOkObject.Value as Note;
            //Console.WriteLine(TestNoteProper.Id);
            Assert.Equal(note.Id, TestNotePut.Id);
        }

        //[Fact]
        //public async void Delete()
        //{
        //    var result = await _controller.DeleteNote(0, "this is deleted title", null, "false", null);
        //    Assert.True(result is NoContentResult);
        //}
    }
}
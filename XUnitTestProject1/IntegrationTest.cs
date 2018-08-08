using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;
using ProjectX;
using ProjectX.Models;

namespace XUnitTestProject1
{
    public class IntegrationTest
    {
        private HttpClient _client;

        private NoteContext _context;
        public IntegrationTest()
        {
            
            var host = new TestServer(
                new WebHostBuilder()
                .UseEnvironment("Testing")
                .UseStartup<Startup>()
                );
            _context = host.Host.Services.GetService(typeof(NoteContext)) as NoteContext;
            _client = host.CreateClient();
            _context.Note.AddRange(TestNoteInitial);
            _context.SaveChanges();


        }
        Note TestNoteInitial = new Note
        {
            Title = "First Note",
            Message = "Text in the first Note",
            CheckList = new List<CheckList>
                {
                    new CheckList{Checklist="checklist 1 in first Note"},
                    new CheckList {Checklist="checklist 2 in first Note"}

                },
            Label = new List<Label>
                {
                    new Label{label="label 1 in first Note"},
                    new Label{label="label 2 in first Note"}
                },
            Pinned = true
        };

        [Fact]
        public async Task TestGetRequestAsync()
        {
            
            var response = await _client.GetAsync("/api/Notes");
            var responsestring = await response.Content.ReadAsStringAsync();
            var responsenote = JsonConvert.DeserializeObject<List<Note>>(responsestring);
            Console.WriteLine(responsenote.ToString());
            Assert.Equal(1, responsenote.Count);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task TaskPostRequestAsync()
        {
            var noteToAdd = new Note
            {
                Title = "First Note",
                Message = "Text in the first Note",
                CheckList = new List<CheckList>
                {
                    new CheckList{Checklist="checklist 1 in first Note"},
                    new CheckList {Checklist="checklist 2 in first Note"}

                },
                Label = new List<Label>
                {
                    new Label{label="label 1 in first Note"},
                    new Label{label="label 2 in first Note"}
                },
                Pinned = true
            };
            var content = JsonConvert.SerializeObject(noteToAdd);
            var stringContent = new StringContent(content, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/Notes", stringContent);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var note = JsonConvert.DeserializeObject<Note>(responseString);
            //Assert.Equal("First Note", note.Title);
            Assert.True(note.IsEquals(noteToAdd));
            //Console.WriteLine(note.Id);
        }
        [Fact]
        public async Task TestPutAsync()
        {
            
            var noteToPut = new Note
            {
                Id = 1,
                Title = "Updated Note",
                Message = "Text in the first Note",
                CheckList = new List<CheckList>
                {
                    new CheckList{Id = 1, Checklist="checklist 1 in first Note"},
                    new CheckList{Id = 2, Checklist="checklist 2 in first Note"}

                },
                Label = new List<Label>
                {
                    new Label{Id = 1, label="label 1 in first Note"},
                    new Label{Id = 2, label="label 2 in first Note"}
                },
                Pinned = true
            };
            //var contentPost = JsonConvert.SerializeObject(noteToPost);
            //var stringContentPost = new StringContent(contentPost, Encoding.UTF8, "application/json");
            var contentPut = JsonConvert.SerializeObject(noteToPut);
            var stringContentPut = new StringContent(contentPut, Encoding.UTF8, "application/json");
            // Act
            //var responsePost = await _client.PostAsync("/api/Notes", stringContentPost);
            var responsePut = await _client.PutAsync("/api/Notes/1", stringContentPut);
            // Assert
            responsePut.EnsureSuccessStatusCode();
            var responseString = await responsePut.Content.ReadAsStringAsync();
            var note = JsonConvert.DeserializeObject<Note>(responseString);
            //Assert.Equal("Updated Note", note.Title);
            Console.WriteLine(noteToPut);

            Console.WriteLine(note);
            Assert.True(note.IsEquals(noteToPut));

            
            Console.WriteLine(note.Id);



        }
        [Fact]
        public async void TestGetById()
        {
            var response = await _client.GetAsync("/api/Notes?Id=1");
            var responsestring = await response.Content.ReadAsStringAsync();
            var responsenote = JsonConvert.DeserializeObject<List<Note>>(responsestring);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(TestNoteInitial.IsEquals(responsenote[0]));
        }
        [Fact]
        public async void TestGetByTitle()
        {
            var response = await _client.GetAsync("/api/Notes?Title=First Note");
            var responsestring = await response.Content.ReadAsStringAsync();
            var responsenote = JsonConvert.DeserializeObject<List<Note>>(responsestring);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(TestNoteInitial.IsEquals(responsenote[0]));
        }

        [Fact]
        public async Task TestDeleteAsync()
        {
            var responseDelete = await _client.DeleteAsync("/api/Notes?Id=1");
            var responseCode = responseDelete.StatusCode;
            Assert.Equal(HttpStatusCode.NoContent, responseCode);
            

        }
        
        
        

    }
}

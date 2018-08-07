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
        public IntegrationTest()
        {
            var host = new TestServer(new WebHostBuilder()
                .UseEnvironment("Testing")
                .UseStartup<Startup>());
            _client = host.CreateClient();
        }
        [Fact]
        public async Task TestGetRequestAsync()
        {
            var Response = await _client.GetAsync("/api/Notes");
            var ResponseBody = await Response.Content.ReadAsStringAsync();
            //Console.WriteLine(ResponseBody);
            Assert.Equal(2, ResponseBody.Length);
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
            //var noteToPost = new Note
            //{
            //    Title = "First Note",
            //    Message = "Text in the first Note",
            //    CheckList = new List<CheckList>
            //    {
            //        new CheckList{Checklist="checklist 1 in first Note", IsChecked = false},
            //        new CheckList {Checklist="checklist 2 in first Note", IsChecked = true}

            //    },
            //    Label = new List<Label>
            //    {
            //        new Label{label="label 1 in first Note"},
            //        new Label{label="label 2 in first Note"}
            //    },
            //    Pinned = true
            //};
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
        public async Task TestDeleteAsync()
        {
            //var noteToAdd = new Note
            //{
            //    Title = "First Note",
            //    Message = "Text in the first Note",
            //    CheckList = new List<CheckList>
            //    {
            //        new CheckList{Checklist="checklist 1 in first Note", IsChecked = false},
            //        new CheckList {Checklist="checklist 2 in first Note", IsChecked = true}

            //    },
            //    Label = new List<Label>
            //    {
            //        new Label{label="label 1 in first Note"},
            //        new Label{label="label 2 in first Note"}
            //    },
            //    Pinned = true
            //};
            //var content = JsonConvert.SerializeObject(noteToAdd);
            //var stringContent = new StringContent(content, Encoding.UTF8, "application/json");

            //// Act
            //var responsePost = await _client.PostAsync("/api/Notes", stringContent);
            var responseDelete = await _client.DeleteAsync("/api/Notes?Id=1");
            var Response = await _client.GetAsync("/api/Notes");
            var ResponseBody = await Response.Content.ReadAsStringAsync();

            Assert.Equal(2, ResponseBody.Length);

        }
        //[Fact]
        //public async Task TestGetSpecificAsync()
        //{
        //    var noteToAdd1 = new Note
        //    {
        //        Title = "First Note",
        //        Message = "Text in the first Note",
        //        CheckList = new List<CheckList>
        //        {
        //            new CheckList{Checklist="checklist 1 in first Note", IsChecked = false},
        //            new CheckList {Checklist="checklist 2 in first Note", IsChecked = true}

        //        },
        //        Label = new List<Label>
        //        {
        //            new Label{label="label 1 in first Note"},
        //            new Label{label="label 2 in first Note"}
        //        },
        //        Pinned = true
        //    };
        //    var noteToAdd2 = new Note
        //    {
        //        Title = "Second Note",
        //        Message = "Text in the second Note",
        //        CheckList = new List<CheckList>
        //        {
        //            new CheckList{Checklist="checklist 1 in second Note", IsChecked = false},
        //            new CheckList {Checklist="checklist 2 in second Note", IsChecked = true}

        //        },
        //        Label = new List<Label>
        //        {
        //            new Label{label="label 1 in second Note"},
        //            new Label{label="label 2 in second Note"}
        //        },
        //        Pinned = true
        //    };
        //    var content1 = JsonConvert.SerializeObject(noteToAdd1);
        //    var stringContent1 = new StringContent(content1, Encoding.UTF8, "application/json");
        //    var content2 = JsonConvert.SerializeObject(noteToAdd2);
        //    var stringContent2 = new StringContent(content2, Encoding.UTF8, "application/json");
        //    // Act
        //    var response1 = await _client.PostAsync("/api/Notes", stringContent1);
        //    var response2 = await _client.PostAsync("/api/Notes", stringContent2);

        //    // Assert
        //    response1.EnsureSuccessStatusCode();
        //    var responseString1 = await response1.Content.ReadAsStringAsync();
        //    var note1 = JsonConvert.DeserializeObject<Note>(responseString1);
        //    response2.EnsureSuccessStatusCode();
        //    var responseString2 = await response2.Content.ReadAsStringAsync();
        //    var note2 = JsonConvert.DeserializeObject<Note>(responseString1);
        //    var Response = await _client.GetAsync("/api/Notes?Id=1");
        //    var ResponseBody = await Response.Content.ReadAsStringAsync();
        //    var noteSpecific = JsonConvert.DeserializeObject<Note>(ResponseBody);
            
        //    Assert.Equal("First Note", noteSpecific.Title);
        //    Console.WriteLine(note1.Id);
        //    Console.WriteLine(ResponseBody);
        //}
        

    }
}

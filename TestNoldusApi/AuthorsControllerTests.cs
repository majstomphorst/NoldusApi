using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NoldusApi.Dtos;
using NoldusApi.Dtos.BookDtos;
using NoldusApi.Models;
using Xunit;

namespace TestNoldusApi
{
    public class AuthorsControllerTests : BaseIntergrationTest
    {
        [Fact]
        public async Task GetAllAuthorsTest()
        {
            var response = await _client.GetAsync("api/authors");
            var body = await response.Content.ReadAsStringAsync();
            var authors = JsonConvert.DeserializeObject<IList<AuthorReadDto>>(body);
            
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal(5, authors.Count);
        }

        [Fact]
        public async Task PostAuthorsTest()
        {
            // var response = await _client.PostAsync("api/authors", );
        }
        
        [Fact]
        public async Task GetAllBooksTest()
        {
            var response = await _client.GetAsync("api/books");
            var body = await response.Content.ReadAsStringAsync();
            var books = JsonConvert.DeserializeObject<IList<BookReadDto>>(body);
            
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal(1, books.Count);
        }
        
        
        
    }
}
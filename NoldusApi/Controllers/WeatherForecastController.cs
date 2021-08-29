using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NoldusApi.DataAccess;
using NoldusApi.Models;

namespace NoldusApi.Controllers
{
    [ApiController]
    [Route("WeatherForecast")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private ApiContext _context;

        public WeatherForecastController(ApiContext context, 
            ILogger<WeatherForecastController> logger)
        {
            _context = context;
            _logger = logger;

        }
        
        [HttpPost]
        public async Task<IActionResult> OnPostUploadAsync(IFormFile file)
        {
            var dir = Directory.GetCurrentDirectory() + "/AppData/";
            var path = Path.Combine(dir , Path.GetRandomFileName());
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            if (file.Length > 0)
            {
                using (var stream = System.IO.File.Create(path + ".png"))
                {
                    await file.CopyToAsync(stream);
                }
            }
            
            return Ok(new { file.Length });
        }
        
        // public ActionResult Post(FileUploadAp obj)
        // {
        //     if (obj.files.Length > 0)
        //     {
        //         return NotFound();
        //     }
        //
        //     var path = _env.WebRootPath + "Appdata";
        //     if (!Directory.Exists(path))
        //     {
        //         Directory.CreateDirectory(path);
        //     }
        //
        //     using (FileStream stream = System.IO.File.Create(path + obj.files.Name))
        //     {
        //         obj.files.CopyTo(stream);
        //         stream.Flush();
        //         return Ok(path + obj.files.Name);
        //     }
        //
        // }
    }
}


// Author author = new Author
// {
//     FirstName = "Henk",
//     LastName = "schrijver",
//     Pseudonym = "the bos"
// };
// Book book = new Book
// {
//     Title = "henks first book",
//     Description = "awesome book",
//     CoverImage = null,
//     Release = DateTime.Now,
//     price = 1,
//     Author = author
// };
//
// _context.Book.Add(book);
// _context.SaveChanges();

// var book = _context.Authors.Include(x => x.Books).Where(x => x.FirstName == "Henk").ToList();

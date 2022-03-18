using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestIdentityServer.API2.Models;

namespace TestIdentityServer.API2.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PicturesController : ControllerBase
    {
        [Authorize]
        public IActionResult GetPictures()
        {
            var pictures = new List<Picture>()
            {
                new Picture { Id = 1, Name="Doğa Resmi", Url="dogaresmi.jpg"},
                new Picture { Id = 2, Name="Kedi Resmi", Url="kediresmi.jpg"},
                new Picture { Id = 3, Name="Köpek Resmi", Url="köpekresmi.jpg"},
            };

            return Ok(pictures);
        }
    }
}

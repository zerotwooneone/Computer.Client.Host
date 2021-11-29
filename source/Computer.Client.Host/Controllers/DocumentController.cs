using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Computer.Client.Host.Controllers
{
    //[ValidateAntiForgeryToken]
    public class DocumentController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Get(string id)
        {
            
            return Ok(new DocumentModel
            {
                Strings =
                {
                    {"this", "one"},
                    {"id", id}
                },
                Decimals =
                {
                    {"somthing", 42.42m}
                }
            });
        }

        [HttpPut]
        public async Task<IActionResult> Put(string id, [FromBody] DocumentModel document)
        {
            return Ok(id);
        }
    }
}
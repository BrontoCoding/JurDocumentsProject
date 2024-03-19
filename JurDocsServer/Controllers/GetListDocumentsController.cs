using JurDocsServer.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JurDocsServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetListDocumentsController : ControllerBase
    {
        // GET: api/<ValuesController>
        [HttpGet]

        public ActionResult<string[]> Get(string docName)
        {
            var myJsonResponse = System.IO.File.ReadAllText(@"Data\data.json");

            var securityInfo = JsonConvert.DeserializeObject<SecurityInfo>(myJsonResponse);

            var docNameInfo = securityInfo!.Catalogs!.Where(x => x.Name == docName).ToArray();

            if (docNameInfo.Length == 1)
            {
                List<string> list = [];

                var files = Directory.GetFiles(docNameInfo.First().Path);
                foreach (var file in files)
                    list.Add(Path.GetFileName(file));

                return Ok(list);
            }

            return BadRequest();
        }

        [HttpGet("aaa")]
        public ActionResult<bool> GetFileName([FromQuery]string docName,[FromQuery] string fileName,[FromQuery]int userId)
        {
            var myJsonResponse = System.IO.File.ReadAllText(@"Data\data.json");

            var securityInfo = JsonConvert.DeserializeObject<SecurityInfo>(myJsonResponse);

            var docNameInfo = securityInfo!.Catalogs!.Where(x => x.Name == docName).ToArray();

            if (docNameInfo.Length != 1)
                return BadRequest();

            List<string> list = [];

            var fileSource = Path.Combine(docNameInfo.First().Path, fileName);

            var users = securityInfo!.Users!.Where(x => x.Id == userId).ToArray();

            if (users.Length != 1)
                return BadRequest();

            var fileDest = Path.Combine(users.First().Path, fileName);

            if (!System.IO.File.Exists(fileSource))
                return BadRequest();

            System.IO.File.Copy(fileSource, fileDest);
            return Ok();
        }

        // POST api/<ValuesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

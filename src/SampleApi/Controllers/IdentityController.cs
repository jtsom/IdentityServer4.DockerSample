using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace SampleApi.Controllers
{
    [Route("[controller]")]
    [Authorize]
    public class IdentityController : Controller
    {
        // get /Identity
        [HttpGet]
        public ActionResult Get()
        {
            return Json(User.Claims.Select(c => new { c.Type, c.Value }));
        }
    }
}
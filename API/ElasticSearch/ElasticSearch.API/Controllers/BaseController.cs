using ElasticSearch.API.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ElasticSearch.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {

        [NonAction] // Dış dünyaya bir endpoint olmayacak, metot olacak.
        public ActionResult CreateActionResult<T>(ResponseDto<T> response) 
        {

             if(response.Status == HttpStatusCode.NoContent) // NoContent = 204
            { 
                return new ObjectResult(null) {StatusCode = response.Status.GetHashCode()};
                // GetHashCode() ---> 01,04 artık ne ise o dönecek.
            }


            return new ObjectResult(response) { StatusCode = response.Status.GetHashCode() };

            // ObjectResult() --> badrequest'in,ok result'un miras aldığı sınıftır.
        }
    }
}

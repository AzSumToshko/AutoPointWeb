using AutoPointAPI.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Scrypt;

namespace AutoPointAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigController : ControllerBase
    {
        private readonly DbA91afaAutopointContext _context;
        private readonly ScryptEncoder _encoder;

        public ConfigController(DbA91afaAutopointContext context)
        {
            _encoder = new ScryptEncoder();
            _context = context;
        }

        // GET: api/Config/geocode-api
        [HttpGet("/geocode-api/{lat}&{lng}")]
        public async Task<IActionResult> GetGeocodeAPI(double lat, double lng)
        {
            if(lat == null || lng == null)
            {
                return BadRequest();
            }
            else
            {
                try
                {
                    //here we establish the client and the request
                    var client = new HttpClient();
                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri("https://maps.googleapis.com/maps/api/geocode/json?latlng=" + lat + "," + lng + "&key=AIzaSyDOVXDLWITbnyYrnHQCk1svB4l6b1TOXAo"),
                    };
                    using (var response = client.Send(request))
                    {
                        //here we get the response and convert it into a view model
                        response.EnsureSuccessStatusCode();
                        var body = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(body);


                        return Ok(result);
                    }
                }
                catch
                {
                    return NotFound();
                }
            }
        }
    }
}

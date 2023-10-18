using AutoPoint.Tools;
using AutoPoint.ViewModel.HomeVM;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AutoPoint.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        ///         This action returns the user to the index page
        /// </summary>

        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        ///         This method sends request to an API that reads OBD-2 module error codes and returns a JSON result 
        ///         which we convert into a view model and put it to the view
        /// </summary>
        public IActionResult DTCReader(string errorCode)
        {
            //This is a simple API that will return the human readable version of DTC codes (OBD-II Trouble Codes).

            //here we check if theres a error code typen
            if (string.IsNullOrEmpty(errorCode) || errorCode.Equals(Constants.NONE))
            {
                return View();
            }

            try
            {
                //here we establish the client and the request
                var client = new HttpClient();
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(Constants.OBD_MODULE_API_URL + errorCode),
                    Headers =
                    {
                        { Constants.RAPID_API_KEY, Constants.OBD_MODULE_API_KEY },
                        { Constants.RAPID_API_HOST, Constants.OBD_MODULE_API_HOST },
                    },
                };
                using (var response = client.Send(request))
                {
                    //here we get the response and convert it into a view model
                    response.EnsureSuccessStatusCode();
                    var body = response.Content.ReadAsStringAsync();

                    DTCReaderVM model = JsonConvert.DeserializeObject<DTCReaderVM>(body.Result);

                    //we get returned to the view
                    return View(model);
                }
            }
            catch
            {
                //here we create a default model and put the error
                DTCReaderVM model = new DTCReaderVM();
                model.definition = Constants.NO_RESULT;

                //we get returned to the view
                return View(model);
            }
        }

        /// <summary>
        ///         This is a simple method that sends a request to a car API and it returns a JSON format with
        ///         all engines from that make and model , from 2020 year which we parse into view model and put
        ///         them to the view
        /// </summary>
        public IActionResult EngineSpecifications(string make , string model)
        {
            //here we check if there is input typen
            if (string.IsNullOrEmpty(make) && string.IsNullOrEmpty(model))
            {
                return View();
            }

            EngineSpecsVM VM = new EngineSpecsVM();

            try
            {
                //here we establish the client and the request
                var client = new HttpClient();
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(Constants.CAR_API_URL+make+"&model="+model+"&verbose=yes&year=2020&direction=asc&sort=id"),
                    Headers =
                    {
                        { Constants.RAPID_API_KEY, Constants.CAR_API_KEY },
                        { Constants.RAPID_API_HOST, Constants.CAR_API_HOST },
                    },
                };
                using (var response = client.Send(request))
                {
                    //here we get the response and convert it to a viewmodel
                    response.EnsureSuccessStatusCode();
                    var body = response.Content.ReadAsStringAsync();

                    //the JsonConverter settings for handling properties with null values
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };

                    VM = JsonConvert.DeserializeObject<EngineSpecsVM>(body.Result,settings);
                    
                    VM.make = make;
                    VM.model = model;
                    //if there are no engines we return and error
                    if (VM.data.Count <= 0)
                    {
                        VM.error = Constants.NO_CAR_FOUND;
                    }
                    return View(VM);
                }
            }
            catch
            {
                //if something happens with the request the program returns this error
                VM.error = Constants.INVALID_MAKE_OR_MODEL;
                return View(VM);
            }
            
        }

        /// <summary>
        ///         This method switches the culture(language) of the site
        ///         by creating a cookie
        /// </summary>
        public IActionResult CultureManagement(string culture)
        {
            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName, CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.Now.AddDays(30) });

            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}
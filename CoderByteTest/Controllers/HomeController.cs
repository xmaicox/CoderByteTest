using CoderByteTest.ApiRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CoderByteTest.Controllers
{
    public class HomeController : Controller
    {

        private ILogger<HomeController> _Logger { get; set; }
        private IApiEndpointRepository _Repository { get; set; }
        public HomeController(ILogger<HomeController> logger, IApiEndpointRepository repository)
        {
            try
            {
                _Logger = logger;
                _Repository = repository;
            }
            catch (Exception ex) {
                throw ex;
            }
        }

        [HttpGet]
        [Route("")]
        [Route("/{limit:int?}")]
        [Route("/topArticles/{limit:int?}")]
        public async Task<IActionResult> Index(int limit = 0)
        {
            object articleNameResults;
            try
            {
                var result = await _Repository.GetArticles();

                var filtered = result.Where(r => !string.IsNullOrEmpty(r.DisplayName));

                var res = filtered.OrderByDescending(r => r.NumComments).ThenByDescending(r => r.DisplayName);

                if (limit != 0)
                    articleNameResults = res.Take(limit).Select(r => new { Name = r.DisplayName, Count = r.NumComments}).ToList();
                else
                    articleNameResults = res.Select(r => r.DisplayName ).ToList();

            }
            catch (Exception ex)
            {
                _Logger.LogError(ex.Message);
                return StatusCode(422, ex.Message);
            }

            return Ok(articleNameResults);
        }
    }
}


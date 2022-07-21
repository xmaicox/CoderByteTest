using CoderByteTest.Model;
using CoderByteTest.ModelHelper;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CoderByteTest.ApiRepository
{
    public class ApiEndpointRepository : IApiEndpointRepository
    {
        private readonly ILogger<ApiEndpointRepository> _Logger;
        private HttpClient _Client { get; set; }
        private IMemoryCache _Cache { get; set; }

        public ApiEndpointRepository(ILogger<ApiEndpointRepository> _logger, IMemoryCache _cache)
        {
            _Logger = _logger;
            _Client = new HttpClient();
            _Cache = _cache;
        }

        public async Task<List<Article>> GetArticles()
        {
            List<Article> articles = null;
            try
            {

                if (!_Cache.TryGetValue("ArticleItems", out articles))
                {
                    //Repopulate
               
                    articles = new List<Article>();
                    //Get all first and save in cache
                    HttpResponseMessage response = await _Client.GetAsync("https://jsonmock.hackerrank.com/api/articles?page=1");
                    response.EnsureSuccessStatusCode();
                    dynamic responseBody = null;
                    if (response.IsSuccessStatusCode)
                    {
                        responseBody = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());
                        string data = JsonConvert.SerializeObject(responseBody.data);
                        articles.AddRange(JsonConvert.DeserializeObject<IEnumerable<Article>>(data, new MicrosecondEpochConverter()));

                        if (responseBody.total_pages.ToObject<int>() > 1)
                        {
                            for (int start = 2; start <= responseBody.total_pages.ToObject<int>(); start++)
                            {
                                response = await _Client.GetAsync($"https://jsonmock.hackerrank.com/api/articles?page={start}");
                                response.EnsureSuccessStatusCode();
                                responseBody = null;
                                if (response.IsSuccessStatusCode)
                                {
                                    responseBody = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());
                                    data = JsonConvert.SerializeObject(responseBody.data);
                                    articles.AddRange(JsonConvert.DeserializeObject<IEnumerable<Article>>(data, new MicrosecondEpochConverter()));
                                }
                            }
                        }

                        _Cache.Set("ArticleItems", articles, new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(10)));
                    }
                    else
                    {
                        _Logger.LogError($"Endpoint error, status code:{response.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex.Message);
            }

            return articles;
        }
    }
}

using CoderByteTest.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoderByteTest.ApiRepository
{
    public interface IApiEndpointRepository
    {

        public Task<List<Article>> GetArticles();        
    }
}

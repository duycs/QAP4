using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QAP4.Infrastructure.Repositories;
using QAP4.Models;

namespace QAP4.Controllers
{
    [Produces("application/json")]
    [Route("[controller]")]
    public class QuotesController : Controller
    {
        private readonly IQuoteRepository _quoteRepository;

        public QuotesController(IQuoteRepository quoteRepository)
        {
            _quoteRepository = quoteRepository;
        }

        /// <summary>
        /// route: /quotes
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        public Quotes GetAutoQuote()
        {
            return _quoteRepository.GetAutoQuote();
        }
        

    }
}
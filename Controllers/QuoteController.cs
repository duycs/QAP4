using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QAP4.Repository;
using QAP4.Models;

namespace QAP4.Controllers
{
    [Produces("application/json")]
    [Route("[controller]")]
    public class QuoteController : Controller
    {
        private IQuoteRepository QuoteRepo { get; set; }

        public QuoteController(IQuoteRepository _repo)
        {
            QuoteRepo = _repo;
        }

        [HttpGet]
        [Route("/api/quotes")]
        public Quotes GetAutoQuote()
        {
            return QuoteRepo.GetAutoQuote();
        }
        

    }
}
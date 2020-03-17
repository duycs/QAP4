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
        private readonly IRepository<Quotes> _quotesRepository;

        public QuoteController(IRepository<Quotes> quotesRepository)
        {
            _quotesRepository = quotesRepository;
        }

        [HttpGet]
        [Route("/api/quotes")]
        public IActionResult GetAutoQuote()
        {
            var quotes = _quotesRepository.Table.AsEnumerable().ToList();
            if (!quotes.Any())
                return NotFound();

            int num = new Random().Next(1, quotes.Count());
            var quote = quotes.FirstOrDefault(o => o.Id.Equals(num));

            return Ok(quote);
        }


    }
}
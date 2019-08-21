using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QAP4.Domain.Core.Commands;
using QAP4.Domain.Core.Notifications;
using QAP4.Infrastructure.Repositories;
using QAP4.Models;

namespace QAP4.Controllers
{
    [Route("api/[controller]")]
    public class QuotesController : ApiController
    {
        private readonly INotificationHandler<DomainNotification> _notificationHandler;
        private readonly ICommandDispatcher _commandBusHandler;
        private readonly IQuoteRepository _quoteRepository;

        public QuotesController(
                INotificationHandler<DomainNotification> notificationHandler,
                ICommandDispatcher commandBusHandler,
                IQuoteRepository quoteRepository)
              : base(notificationHandler, commandBusHandler)
        {
            _notificationHandler = notificationHandler;
            _commandBusHandler = commandBusHandler;

            _quoteRepository = quoteRepository;
        }

        /// <summary>
        /// route: /quotes
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("random")]
        public IActionResult GetQuoteRandom()
        {
            try
            {
                var quote = _quoteRepository.GetAutoQuote();

                if (quote == null)
                    return NoContent();

                return Ok(quote);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


    }
}
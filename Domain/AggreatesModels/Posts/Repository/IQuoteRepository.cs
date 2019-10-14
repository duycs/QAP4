using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QAP4.Domain.AggreatesModels.Posts.Models;
using QAP4.Domain.Core.Repositories;

namespace QAP4.Infrastructure.Repositories
{
    public interface IQuoteRepository : IRepository<Quote>
    {

        IEnumerable<Quote> GetQuotes();
        Quote GetQuote(int id);
        Quote GetAutoQuote();
        //IEnumerable<Quotes> GetCommentsAuthor(int? author);
        void Create(Quote model);
        void Update(Quote model);
        void Delete(int? id);
    }
}

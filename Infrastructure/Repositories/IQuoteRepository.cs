using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QAP4.Models;

namespace QAP4.Infrastructure.Repositories
{
    public interface IQuoteRepository
    {

        IEnumerable<Quotes> GetQuotes();
        Quotes GetQuote(int id);
        Quotes GetAutoQuote();
        //IEnumerable<Quotes> GetCommentsAuthor(int? author);
        void Create(Quotes model);
        void Update(Quotes model);
        void Delete(int? id);
    }
}

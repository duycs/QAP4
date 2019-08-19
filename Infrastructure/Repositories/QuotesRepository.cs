using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QAP4.Models;
using Microsoft.EntityFrameworkCore;

namespace QAP4.Infrastructure.Repositories
{
    public class QuotesRepository : IQuoteRepository
    {
        private QAPContext context;
        private DbSet<Quotes> quoteEntity;
        public QuotesRepository(QAPContext context)
        {
            this.context = context;
            quoteEntity = context.Set<Quotes>();
        }

        public void Create(Quotes model)
        {
            throw new NotImplementedException();
        }

        public void Delete(int? id)
        {
            throw new NotImplementedException();
        }

        public Quotes GetAutoQuote()
        {
            var list = quoteEntity.AsEnumerable().ToArray();
            if (list.Length > 0)
            {
                int num = new Random().Next(1, list.Count());
                return quoteEntity.FirstOrDefault(o => o.Id.Equals(num));
            }
            return null;
        }

        public Quotes GetQuote(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Quotes> GetQuotes()
        {
            throw new NotImplementedException();
        }

        public void Update(Quotes model)
        {
            throw new NotImplementedException();
        }
    }
}

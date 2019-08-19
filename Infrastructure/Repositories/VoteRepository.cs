using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QAP4.Models;
using Microsoft.EntityFrameworkCore;

namespace QAP4.Infrastructure.Repositories
{
    public class VoteRepository : IVoteRepository
    {
        private QAPContext context;
        private DbSet<Votes> voteEntity;
        public VoteRepository(QAPContext context)
        {
            this.context = context;
            voteEntity = context.Set<Votes>();
        }

        public Votes GetVote(int? userId, int? postId, int? voteTypeId)
        {
            var sql = "SELECT * FROM Votes WHERE  PostsId = " + postId + " AND VoteTypeId = " + voteTypeId + " AND UserId = " + userId;
            return voteEntity.FromSql<Votes>(sql).FirstOrDefault();
        }

        public IEnumerable<Votes> GetVotes(int? postId, int? voteTypeId, int? userId)
        {
            var sql = "SELECT * FROM Votes WHERE  PostsId = " + postId + " AND VoteTypeId = " + voteTypeId + " AND UserId = " + userId + " ORDER BY CreationDate ";
            return voteEntity.FromSql<Votes>(sql).AsEnumerable();
        }


        public bool CheckUserVoted(int? userId, int? postsId, int? voteTypeId, bool? isOn)
        {
            var isOnInt = isOn == false ? 0 : 1;
            var sql = "SELECT * FROM Votes WHERE UserId = " + userId + " AND PostsId = " + postsId + " AND VoteTypeId = " + voteTypeId + " AND IsOn = " + isOnInt;
            var vote = voteEntity.FromSql<Votes>(sql).FirstOrDefault();
            if (vote != null)
                return true;
            else return false;
        }

        public void Create(Votes vote)
        {
            voteEntity.Add(vote);
            context.SaveChanges();
        }

        public void Delete(int? id)
        {
            var vote = voteEntity.Where(o => o.Id.Equals(id)).FirstOrDefault();
            if (vote != null)
            {
                voteEntity.Remove(vote);
                context.SaveChanges();
            }
        }

        public void Delete(int? userId, int? postsId, int? voteTypeId)
        {
            var sql = "SELECT * FROM Votes WHERE UserId = " + userId + " AND PostsId = " + postsId + " AND VoteTypeId = " + voteTypeId;
            var vote = voteEntity.FromSql<Votes>(sql).FirstOrDefault();
            if (vote != null)
            {
                voteEntity.Remove(vote);
                context.SaveChanges();
            }
        }


        public void Update(Votes model)
        {
            voteEntity.Update(model);
            context.SaveChanges();
        }
    }
}

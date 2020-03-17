using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QAP4.Models;
using Microsoft.EntityFrameworkCore;
using QAP4.Repository;

namespace QAP4.Application.Services
{
    public class VoteService : IVoteService
    {
        private readonly IRepository<Votes> _voteRepository;
        public VoteService(IRepository<Votes> voteRepository)
        {
            _voteRepository = voteRepository;
        }

        public Votes GetVote(int? userId, int? postsId, int? voteTypeId)
        {
            //var sql = "SELECT * FROM Votes WHERE  PostsId = " + postsId + " AND VoteTypeId = " + voteTypeId + " AND UserId = " + userId;
            //return voteEntity.FromSql<Votes>(sql).FirstOrDefault();
            var query = _voteRepository.Table;
            if (postsId > 1)
                query = query.Where(w => w.PostsId == postsId);

            if (voteTypeId > 1)
                query = query.Where(w => w.VoteTypeId == voteTypeId);

            if (userId > 1)
                query = query.Where(w => w.UserId == userId);

            var votes = query.FirstOrDefault();

            return votes;
        }

        public IEnumerable<Votes> GetVotes(int? userId, int? postsId, int? voteTypeId)
        {
            //var sql = "SELECT * FROM Votes WHERE  PostsId = " + postsId + " AND VoteTypeId = " + voteTypeId + " AND UserId = " + userId + " ORDER BY CreationDate ";
            //return voteEntity.FromSql<Votes>(sql).AsEnumerable();
            var query = _voteRepository.Table;
            if (postsId > 1)
                query = query.Where(w => w.PostsId == postsId);

            if (voteTypeId > 1)
                query = query.Where(w => w.VoteTypeId == voteTypeId);

            if (userId > 1)
                query = query.Where(w => w.UserId == userId);

            var votes = query.AsEnumerable();

            return votes;
        }

        public Votes AddVote(Votes voteViewModel)
        {
            if (voteViewModel == null)
                throw new NullReferenceException(nameof(voteViewModel));

            var vote = voteViewModel;
            return _voteRepository.Insert(vote);
        }

        public void DeleteVote(int? id)
        {
            var vote = _voteRepository.GetById(id);
            _voteRepository.Delete(vote);
        }

        public void UpdateVote(Votes voteViewModel)
        {
            var vote = voteViewModel;
            _voteRepository.Update(vote);
        }


        public bool IsUserVoted(int? userId, int? postsId, int? voteTypeId, bool? isOn)
        {
            //var isOnInt = isOn == false ? 0 : 1;
            //var sql = "SELECT * FROM Votes WHERE UserId = " + userId + " AND PostsId = " + postsId + " AND VoteTypeId = " + voteTypeId + " AND IsOn = " + isOnInt;
            //var vote = voteEntity.FromSql<Votes>(sql).FirstOrDefault();
            var vote = _voteRepository.Table.Where(w => w.PostsId == postsId && w.VoteTypeId == voteTypeId && w.UserId == userId && w.IsOn == isOn).FirstOrDefault();

            if (vote == null)
                return false;

            return true;
        }

    }
}

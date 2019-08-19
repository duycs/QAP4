using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QAP4.Models;

namespace QAP4.Infrastructure.Repositories
{
    public interface IVoteRepository
    {

        Votes GetVote(int? userId, int? postId, int? voteTypeId);
        IEnumerable<Votes> GetVotes(int? userId, int? postId, int? voteTypeId);
        void Create(Votes model);
        void Update(Votes model);
        void Delete(int? id);
        void Delete(int? userId, int? postsId, int? voteTypeId);
        bool CheckUserVoted(int? userId, int? postsId, int? voteTypeId, bool? isOn);
    }
}

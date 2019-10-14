using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QAP4.Domain.AggreatesModels.Posts.Models;
using QAP4.Domain.Core.Repositories;
using QAP4.Models;

namespace QAP4.Infrastructure.Repositories
{
    public interface IVoteRepository : IRepository<Vote>
    {
        Vote GetVote(int? userId, int? postId, int? voteTypeId);
        IEnumerable<Vote> GetVotes(int? userId, int? postId, int? voteTypeId);
        void Create(Vote model);
        void Update(Vote model);
        void Delete(int? id);
        void Delete(int? userId, int? postsId, int? voteTypeId);
        bool CheckUserVoted(int? userId, int? postsId, int? voteTypeId, bool? isOn);
    }
}

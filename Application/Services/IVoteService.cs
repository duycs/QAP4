using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QAP4.Models;

namespace QAP4.Application.Services
{
    public interface IVoteService
    {

        Votes GetVote(int? userId, int? postId, int? voteTypeId);
        IEnumerable<Votes> GetVotes(int? userId, int? postId, int? voteTypeId);
        Votes AddVote(Votes voteViewModel);
        void UpdateVote(Votes voteViewModel);
        void DeleteVote(int? id);
        // void Delete(int? userId, int? postsId, int? voteTypeId);
        bool IsUserVoted(int? userId, int? postsId, int? voteTypeId, bool? isOn);
    }
}

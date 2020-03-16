using System;
using System.Collections.Generic;
using System.Linq;
using QAP4.Extensions;
using QAP4.Models;
using QAP4.Repository;

namespace QAP4.Application.Services
{

    public class CommentService : ICommentService
    {
        private readonly IRepository<Comments> _commentRepository;
        private readonly IRepository<Users> _userRepository;
        private readonly IRepository<Posts> _postRepository;

        public CommentService(IRepository<Comments> commentRepository, IRepository<Users> userRepository, IRepository<Posts> postsRepository)
        {
            _commentRepository = commentRepository;
            _userRepository = userRepository;
            _postRepository = postsRepository;
        }

        public Comments AddComment(Comments commentViewModel)
        {
            if (commentViewModel == null)
                throw new ArgumentNullException(nameof(commentViewModel));

            var userComment = _userRepository.GetById(commentViewModel.UserId);
            var now = DateTime.Now;
            var comment = new Comments();

            // Add comment
            comment.UserDisplayName = userComment.DisplayName;
            comment.CreationDate = commentViewModel.CreationDate == null ? now : commentViewModel.CreationDate;
            comment.ModificationDate = now;
            comment.ParentId = commentViewModel.ParentId == 0 ? null : commentViewModel.ParentId;
            comment.UpvoteCount = commentViewModel.UpvoteCount == null ? 0 : commentViewModel.UpvoteCount;

            var commentAdded = _commentRepository.Insert(comment);

            // Update posts
            var posts = _postRepository.GetById(commentViewModel.PostsId);
            var commentCount = posts.CommentCount;

            commentCount++;
            posts.CommentCount = commentCount;
            _postRepository.Insert(posts);

            return commentAdded;

            // notify event
        }
        public void UpdateComment(Comments comment)
        {
            if (comment == null)
                throw new ArgumentNullException(nameof(comment));

            _commentRepository.Update(comment);

            // notify event

        }

        public Comments GetCommentById(int id)
        {
            return _commentRepository.Table.Where(w => w.Id == id).FirstOrDefault();
        }

        public IPagedList<Comments> GetComments(int pageIndex = 0, int pageSize = int.MaxValue, int postsId = 0)
        {
            var query = _commentRepository.Table;
            query = query.Where(w => w.PostsId == postsId);

            var comments = new PagedList<Comments>(query, pageIndex, pageSize);
            return comments;
        }
    }
}
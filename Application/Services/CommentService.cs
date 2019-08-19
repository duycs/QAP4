using System;
using System.Collections.Generic;
using System.Linq;
using QAP4.Application.Services;
using QAP4.Infrastructure.Repositories;
using QAP4.Models;

public class CommentService : ICommentService
{
    private readonly ICommentRepository _commentRepository;
    private readonly IPostsRepository _postsRepository;
    private readonly ITagRepository _tagRepository;
    private readonly IUserRepository _userRepository;
    public CommentService( ICommentRepository commentRepository,
    IPostsRepository postsRepository,
    ITagRepository tagRepository,
    IUserRepository userRepository){
        _commentRepository = commentRepository;
        _postsRepository = postsRepository;
        _tagRepository = tagRepository;
        _userRepository = userRepository;
    }

    public int AddComment(Comments comment)
    {
        //create comment
        var userComment = _userRepository.GetById(comment.UserId);
        var time = DateTime.Now;
        comment.UserDisplayName = userComment.DisplayName;
        comment.CreationDate = comment.CreationDate == null ? time : comment.CreationDate;
        comment.ModificationDate = time;
        comment.ParentId = comment.ParentId == 0 ? null : comment.ParentId;
        comment.UpvoteCount = comment.UpvoteCount == null ? 0 : comment.UpvoteCount;
        _commentRepository.Create(comment);

        //update posts
        var posts = _postsRepository.GetPosts(comment.PostsId);
        var commentCount = posts.CommentCount;
        commentCount++;
        posts.CommentCount = commentCount;

        _postsRepository.Update(posts);

        return comment.Id;
    }

    public List<Comments> FindCommentsByPostId(int postId)
    {
        return _commentRepository.GetCommentsByPosts(postId).ToList();
    }
}
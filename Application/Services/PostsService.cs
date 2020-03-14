using System;
using System.Collections.Generic;
using QAP4.Domain.AggreatesModels.Posts.Models;
using QAP4.Domain.AggreatesModels.Users.Models;
using QAP4.Domain.Core.Repositories;
using QAP4.Domain.Core.Specification;
using System.Linq;
using QAP4.Extensions;
using QAP4.Infrastructure.Context;

namespace QAP4.Application.Services
{
    public class PostsService : IPostsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Posts> _postsRepository;
        private readonly IRepository<Tag> _tagRepository;
        private readonly IRepository<PostsTag> _postsTagRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<PostsLinks> _postsLinkRepository;

        public PostsService(
         IUnitOfWork unitOfWork,
         IRepository<Posts> postsRepository,
         IRepository<Tag> tagRepository,
         IRepository<PostsTag> postsTagRepository,
         IRepository<User> userRepository,
         IRepository<PostsLink> postsLinkRepository)
        {
            _unitOfWork = unitOfWork;
            _postsRepository = postsRepository;
            _tagRepository = tagRepository;
            _postsTagRepository = postsTagRepository;
            _userRepository = userRepository;
            _postsLinkRepository = postsLinkRepository;
        }


        public void AddPosts(int userId, Posts posts)
        {
            if (posts == null)
                throw new ArgumentNullException(nameof(posts));

            var user = _userRepository.FindById(userId);
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var dateTime = DateTime.Now;

            // Related posts or chirld posts
            List<string> relatedPosts = new List<string>();
            if (!string.IsNullOrEmpty(posts.RelatedPosts))
            {
                relatedPosts = posts.RelatedPosts.Split(',').ToList();
            }

            // Set posts
            posts.OwnerUserId = userId;
            posts.UserDisplayName = user.DisplayName;
            posts.UserAvatar = user.Avatar;
            posts.CreationDate = dateTime;
            posts.LastEditDate = dateTime;
            posts.AnswerCount = 0;
            posts.ViewCount = 0;
            posts.VoteCount = 0;
            posts.Score = 0;
            posts.CommentCount = 0;

            int postsTypeId = (int)posts.PostTypeId;
            if (PostsType.POSTS.Equals(postsTypeId) || PostsType.QUESTION.Equals(postsTypeId) || PostsType.TUTORIAL.Equals(postsTypeId))
            {
                if (!string.IsNullOrEmpty(posts.HtmlContent))
                {
                    posts.BodyContent = StringExtensions.StripHTML(posts.HtmlContent);
                    posts.HeadContent = StringExtensions.GetWords(posts.BodyContent, 40);
                }

            }
            // If posts is answer, update answer count of parent posts
            else if (PostsType.ANSWER.Equals(postsTypeId))
            {
                var postsParent = _postsRepository.FindById((int)posts.ParentId);
                if (postsParent != null)
                {
                    var answerCount = postsParent.AnswerCount;
                    answerCount++;
                    postsParent.AnswerCount = answerCount;
                    _postsRepository.Update(postsParent);
                }
            }

            // Add the posts
            _postsRepository.Add(posts);
            _unitOfWork.Commit();

            // Handle update parentId in related posts
            if (relatedPosts.Any())
            {
                UpdateParentIdChirldPosts(posts.Id, relatedPosts);
            }

            // Handler tag
            if (!string.IsNullOrEmpty(posts.Tags))
            {
                string[] tags = posts.Tags.Split(',');
                UpdatePostsTags(userId, posts.Id, tags);
            }

            if (!PostsType.ANSWER.Equals(postsTypeId) && !PostsType.POSTS.Equals(postsTypeId))
            {
                // Handler same as handler related tag
                UpdatePostLinks(0, relatedPosts, (int)posts.PostTypeId);

                // Create table of content 
                string tableOfcontent = GetTableOfContent(posts.Id, relatedPosts);
                posts.TableContent = tableOfcontent;
                _postsRepository.Update(posts);
                _unitOfWork.Commit();
            }
        }

        public Posts FindPostsById(int id)
        {
            if (id == 0)
                return null;

            return _postsRepository.FindById(id);
        }

        public IList<Posts> FindPostsByIds(int[] ids)
        {
            if (ids == null || ids.Length == 0)
                return null;

            var postsTable = _postsRepository.Table;
            var query = from p in postsTable where ids.Contains(p.Id) select p;
            var posts = query.ToList();

            return posts;
        }

        public IPagedList<Posts> FindPostsByUserAndType(int userId, int? postsType, int? parentId,
            DateTime? dateFrom = null, DateTime? dateTo = null,
            int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _postsRepository.Table;

            // Query Where conditions
            if (userId > 0)
                query = query.Where(w => w.OwnerUserId == userId);

            if (postsType.HasValue)
                query = query.Where(w => w.PostTypeId == postsType);

            if (parentId.HasValue)
                query = query.Where(w => w.ParentId == parentId);

            if (dateFrom.HasValue)
                query = query.Where(w => w.CreationDate >= dateFrom);

            if (dateTo.HasValue)
                query = query.Where(w => w.CreationDate <= dateTo);

            // Order by
            query = query.OrderByDescending(o => o.CreationDate);

            // Pagging
            var posts = new PagedList<Posts>(query, pageIndex, pageSize);
            return posts;
        }

        public void RemovePosts(Posts posts)
        {
            if (posts == null)
                throw new ArgumentNullException(nameof(posts));

            var dateTime = DateTime.Now;
            posts.LastEditDate = dateTime;
            posts.DeletionDate = dateTime;
            posts.IsDelete = true;

            _postsRepository.Update(posts);
        }

        public void UpdatePosts(Posts posts)
        {
            try
            {
                if (posts == null)
                    throw new ArgumentNullException(nameof(posts));

                _postsRepository.Update(posts);
                _unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdatePostsInEditor(int userId, Posts posts)
        {
            if (posts == null)
                throw new ArgumentNullException(nameof(posts));

            var oldPosts = _postsRepository.FindById(posts.Id);

            if (oldPosts == null)
                throw new ArgumentNullException(nameof(posts));

            //can't use posts = model;
            //use copy object value
            //posts = (Posts)ReflectionExtensions.CopyObjectValue(model, posts);
            posts.Title = posts.Title;
            if (!string.IsNullOrEmpty(posts.UserAvatar))
                posts.UserAvatar = posts.UserAvatar;

            // TODO: model
            posts.HtmlContent = posts.HtmlContent;
            posts.Description = posts.Description;
            posts.Tags = posts.Tags;
            posts.RelatedPosts = posts.RelatedPosts;
            posts.TableContent = posts.TableContent;
            posts.BodyContent = StringExtensions.StripHTML(posts.HtmlContent);
            posts.HeadContent = StringExtensions.GetWords(posts.BodyContent, 40);
            posts.LastEditDate = DateTime.Now;
            posts.CoverImg = posts.CoverImg;

            if (posts.PostTypeId != 0 && !string.IsNullOrEmpty(posts.RelatedPosts))
            {
                // handler create notebook same as handler tag
                List<string> relatedPosts = posts.RelatedPosts.Split(',').ToList();
                UpdatePostLinks(posts.Id, relatedPosts, (int)posts.PostTypeId);

                // create table of content 
                string tableOfcontent = GetTableOfContent(posts.Id, relatedPosts);
                posts.TableContent = tableOfcontent;
            }

            _postsRepository.Update(posts);
            _unitOfWork.Commit();

            string[] oldTags = new string[0];
            string[] newTags = new string[0];
            if (oldPosts != null && !string.IsNullOrEmpty(oldPosts.Tags))
            {
                oldTags = oldPosts.Tags.Split(',');
            }

            if (!string.IsNullOrEmpty(posts.Tags))
            {
                newTags = posts.Tags.Split(',');
            }
            //handler tag
            //if tag change
            if (!oldTags.Equals(newTags))
            {
                //check tags add new and tags delete in table PostsTag
                string[] tagsDiff = GetTagsDiff(oldTags, newTags);
                UpdatePostsTags(userId, posts.Id, tagsDiff);
            }

        }


        #region Private methods

        private void UpdateParentIdChirldPosts(int parentPostsId, List<string> relatedPosts)
        {
            try
            {
                foreach (var chirldPostId in relatedPosts)
                {
                    int postsId = Int32.Parse(chirldPostId);
                    var posts = _postsRepository.FindById(postsId);
                    if (posts != null)
                    {
                        posts.ParentId = parentPostsId;
                        _postsRepository.Update(posts);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void UpdatePostsTags(int? userId, int postsId, string[] tags)
        {
            try
            {
                foreach (string tag in tags)
                {
                    //method handler: if tag not exist, create new tag and tag exist then get tag id
                    var tagId = CreateOrGetTagId(userId, tag);

                    //method handler: if object exist then remove and return false, not exist then create and return true
                    bool isCreate = CreateOrDeletePostsTag(postsId, tagId);

                    //statistic count tag: is create then is up, do not reverse
                    bool isUp = isCreate;
                    UpdateTagCount(isUp, tagId);
                }
                _unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int CreateOrGetTagId(int? userId, string name)
        {
            //var sql = @"select * from Tags where Name like N'" + name.Trim() + "'";
            var tag = _tagRepository.Table.Where(w => w.Name == name.Trim()).FirstOrDefault();

            //tag not exsit, insert new tag
            if (null == tag)
            {
                tag = new Tag();
                tag.UserCreatedId = userId;
                tag.Name = name.ToLower();
                tag.Count = 0;
                _tagRepository.Add(tag);
                _unitOfWork.Commit();
            }

            return tag.Id;
        }

        public void UpdateTagCount(bool isUp, int id)
        {
            var tag = _tagRepository.Table.Where(o => o.Id.Equals(id)).FirstOrDefault();
            if (tag == null)
                return;

            if (isUp)
                tag.Count++;
            else
                tag.Count--;

            _tagRepository.Update(tag);
        }

        private string[] GetTagsDiff(string[] oldTags, string[] newTags)
        {
            try
            {
                string[] diff1 = newTags.Except(oldTags).ToArray();
                string[] diff2 = oldTags.Except(newTags).ToArray();
                if (diff1.Length > 0)
                    return diff1;
                else
                    return diff2;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void UpdatePostLinks(int relatedPostId, List<string> relatedPosts, int linkTypeId)
        {
            try
            {
                foreach (string postId in relatedPosts)
                {
                    int postsId = Int32.Parse(postId);
                    bool isCreate = CreateOrDeletePostsLink(relatedPostId, postsId, linkTypeId);
                    if (isCreate)
                    {
                        //TODO: statistic 
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public bool CreateOrDeletePostsLink(int? postId, int? relatedPostId, int? linkTypeId)
        {
            var postsLink = _postsLinkRepository.Table.Where(o => o.PostId.Equals(postId) && o.RelatedPostId.Equals(relatedPostId) && o.LinkTypeId.Equals(linkTypeId)).FirstOrDefault();

            //create
            if (postsLink == null)
            {
                postsLink = new PostsLink();
                postsLink.RelatedPostId = (int)relatedPostId;
                postsLink.PostId = (int)postId;
                postsLink.LinkTypeId = (byte)linkTypeId;
                postsLink.CreationDate = DateTime.Now;
                _postsLinkRepository.Add(postsLink);
                return true;
            }

            //delele
            _postsLinkRepository.Delete(postsLink);
            return false;
        }

        public bool CreateOrDeletePostsTag(int postsId, int tagId)
        {
            var postsTag = _postsTagRepository.Table.Where(o => o.PostsId.Equals(postsId) && o.TagId.Equals(tagId)).FirstOrDefault();

            //create
            if (postsTag == null)
            {
                postsTag = new PostsTag();
                postsTag.TagId = tagId;
                postsTag.PostsId = postsId;
                _postsTagRepository.Add(postsTag);
                return true;
            }

            //delele
            _postsTagRepository.Delete(postsTag);
            //context.SaveChanges();
            return false;
        }

        private int[] GetPostsDiff(int[] oldPosts, int[] newPosts)
        {
            try
            {
                int[] diff1 = newPosts.Except(oldPosts).ToArray();
                int[] diff2 = oldPosts.Except(newPosts).ToArray();
                if (diff1.Length > 0)
                    return diff1;
                else
                    return diff2;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string GetTableOfContent(int relatedPostId, List<string> relatedPosts)
        {
            try
            {
                string[] tableOfContent = new string[relatedPosts.Count()];
                for (int i = 0; i < relatedPosts.Count(); ++i)
                {
                    int postsId = Int32.Parse(relatedPosts[i]);
                    var model = _postsRepository.FindById(postsId);
                    if (model != null)
                    {
                        tableOfContent[i] = model.Title;
                    }
                }
                return String.Join(",", tableOfContent);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion Private methods
    }
}
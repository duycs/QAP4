using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QAP4.Models;
using Microsoft.EntityFrameworkCore;
using QAP4.Extensions;
using QAP4.Repository;

namespace QAP4.Application.Services
{
    public class TagService : ITagService
    {
        private readonly IRepository<Tags> _tagRepository;
        private readonly IRepository<Posts> _postsRepository;
        private readonly IRepository<PostsTag> _postsTagRepository;
        public TagService(IRepository<Tags> tagRepository, IRepository<Posts> postsRepository, IRepository<PostsTag> postsTagRepository)
        {
            _tagRepository = tagRepository;
            _postsRepository = postsRepository;
            _postsTagRepository = postsTagRepository;
        }

        public Tags AddTag(Tags tagViewModel)
        {
            if (tagViewModel == null)
                throw new ArgumentNullException(nameof(tagViewModel));

            var tag = tagViewModel;
            return _tagRepository.Insert(tag);
        }

        // public int GetTagId(int? userId, string name)
        // {
        //     var tagId = 0;
        //     //var sql = @"select * from Tags where Name like N'" + name.Trim() + "'";
        //     //var tag = tagEntity.FromSql<Tags>(sql).FirstOrDefault();
        //     var query = _tagRepository.Table;
        //     var tag = query.Where(w => w.Name.Contains(name)).FirstOrDefault();

        //     //tag not exsit, insert new tag
        //     if (tag == null)
        //         throw new ArgumentNullException(nameof(tag));

        //     tag = new Tags();
        //     tag.UserCreatedId = userId;
        //     tag.Name = name.ToLower();
        //     tag.Count = 0;
        //     tagEntity.Add(tag);
        //     context.SaveChanges();

        //     tagId = tag.Id;
        //     return tagId;
        // }

        public void UpdateTagCount(bool isUp, int id)
        {
            var tag = _tagRepository.Table.Where(o => o.Id.Equals(id)).FirstOrDefault();
            if (tag == null)
                throw new ArgumentNullException(nameof(tag));

            if (isUp)
                tag.Count++;
            else
                tag.Count--;

            _tagRepository.Update(tag);
        }

        public Tags GetTagById(int id)
        {
            var tag = _tagRepository.Table.Where(o => o.Id.Equals(id)).FirstOrDefault();
            if (tag == null)
                throw new ArgumentNullException(nameof(tag));

            return tag;
        }

        public Tags GetTagByName(string name)
        {
            var tag = _tagRepository.Table.Where(o => o.Name.Equals(name)).FirstOrDefault();
            if (tag == null)
                throw new ArgumentNullException(nameof(tag));

            return tag;
        }

        public IEnumerable<Tags> GetTagsByName(string name)
        {
            var tags = _tagRepository.Table.Where(w => w.Name.Contains(name)).ToList();
            return tags;
        }

        public void UpdateTag(Tags tagViewModel)
        {
            var tag = tagViewModel;

            _tagRepository.Update(tag);
        }

        public void DeleteTagById(int id)
        {
            var tag = _tagRepository.Table.Where(o => o.Id.Equals(id)).FirstOrDefault();
            if (tag == null)
                throw new ArgumentNullException(nameof(tag));

            _tagRepository.Delete(tag);
        }

        public void DeleteTagByName(string name)
        {
            var tag = _tagRepository.Table.Where(w => w.Name.Equals(name)).FirstOrDefault();
            if (tag == null)
                throw new ArgumentNullException(nameof(tag));

            _tagRepository.Delete(tag);
        }

        public IEnumerable<Tags> GetTagsByPostsId(int postsId)
        {
            //var sql = @"select t.* from Tags t inner join PostsTag pt on t.Id=pt.tagId where pt.PostsId=" + postsId + " order by Name";
            //return tagEntity.FromSql<Tags>(sql).AsEnumerable();
            var query = (from a in _tagRepository.Table
                         join b in _postsTagRepository.Table
                         on a.Id equals b.TagId
                         where b.PostsId == postsId
                         orderby a.Name
                         select a);

            var tags = query.OrderBy(o => o.Name).ToList();

            return tags;
        }

        public IEnumerable<Tags> GetTagsFeature(int take = 5)
        {
            return _tagRepository.Table.OrderByDescending(o => o.Count).Take(take).AsEnumerable();
        }

        public IEnumerable<Tags> GetTagsRelation(string key, int take = 5)
        {
            //TODO: how to get tag relation
            return _tagRepository.Table.OrderByDescending(o => o.Count).Take(take).AsEnumerable();
        }

        public IEnumerable<Tags> SearchInTags(string key)
        {
            //var sql = @"SELECT * FROM Tags WHERE FREETEXT (Name, '" + key + "')";
            //return tagEntity.FromSql<Tags>(sql).AsEnumerable();

            // TODO: search free text?
            var tags = _tagRepository.Table.Where(w => w.Name.Contains(key)).ToList();
            return tags;
        }


        public int CreateOrGetTagId(int? userId, string name)
        {
            var tagId = 0;
            //var sql = @"select * from Tags where Name like N'" + name.Trim() + "'";
            //var tag = tagEntity.FromSql<Tags>(sql).FirstOrDefault();
            var tag = _tagRepository.Table.Where(w => w.Name.Contains(name)).FirstOrDefault();
            //tag not exsit, insert new tag
            if (null == tag)
            {
                tag = new Tags();
                tag.UserCreatedId = userId;
                tag.Name = name.ToLower();
                tag.Count = 0;
                _tagRepository.Insert(tag);
            }

            tagId = tag.Id;
            return tagId;
        }


        // Tag to Posts, use PostsTag relation
        public void AddPostsTag(int postsId, int tagId)
        {
            var postsTag = _postsTagRepository.Table.Where(o => o.PostsId.Equals(postsId) && o.TagId.Equals(tagId)).FirstOrDefault();
            if (postsTag != null)
                return;

            var newPostsTag = new PostsTag();
            newPostsTag.TagId = tagId;
            newPostsTag.PostsId = postsId;

            _postsTagRepository.Insert(newPostsTag);
        }

        public void DeletePostsTag(int? postsId, int? tagId)
        {
            var postsTag = _postsTagRepository.Table.Where(o => o.PostsId.Equals(postsId) && o.TagId.Equals(tagId)).FirstOrDefault();

            _postsTagRepository.Delete(postsTag);
        }

        public bool CreateOrDeletePostsTag(int postsId, int tagId)
        {
            var postsTag = _postsTagRepository.Table.Where(o => o.PostsId.Equals(postsId) && o.TagId.Equals(tagId)).FirstOrDefault();

            //create
            if (postsTag == null)
            {
                var newPostsTag = new PostsTag();
                newPostsTag.TagId = tagId;
                newPostsTag.PostsId = postsId;
                _postsTagRepository.Insert(newPostsTag);

                return true;
            }
            else
            {
                //delele
                _postsTagRepository.Delete(postsTag);
                return false;
            }
        }
    }
}

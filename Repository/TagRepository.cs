using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QAP4.Models;
using Microsoft.EntityFrameworkCore;
using QAP4.Extensions;

namespace QAP4.Repository
{
    public class TagRepository : ITagRepository
    {
        private QAPContext context;
        private DbSet<Tags> tagEntity;
        public TagRepository(QAPContext context)
        {
            this.context = context;
            tagEntity = context.Set<Tags>();
        }
        
        public void Create(Tags tag)
        {
            tagEntity.Add(tag);
        }

        public int CreateOrGetTagId(int? userId, string name)
        {
            var tagId = 0;
            var sql = @"select * from Tags where Name like N'" + name.Trim() + "'";
            var tag = tagEntity.FromSql<Tags>(sql).FirstOrDefault();

            //tag not exsit, insert new tag
            if (null == tag)
            {
                tag = new Tags();
                tag.UserCreatedId = userId;
                tag.Name = name.ToLower();
                tag.Count = 0;
                tagEntity.Add(tag);
                context.SaveChanges();
            }

            tagId = tag.Id;
            return tagId;
        }

        public void UpdateTagCount(bool isUp, int id)
        {
            var tag = tagEntity.Where(o => o.Id.Equals(id)).FirstOrDefault();
            if (null != tag)
            {
                if (isUp)
                    tag.Count++;
                else
                    tag.Count--;
                tagEntity.Update(tag);
                context.SaveChanges();
            }
        }

        public Tags GetTag(int id)
        {
            return tagEntity.Where(o => o.Id.Equals(id)).FirstOrDefault();
        }

        public Tags GetTagByName(string name)
        {
            return tagEntity.Where(o => o.Name.Equals(name)).FirstOrDefault();
        }

        public IEnumerable<Tags> GetTagsByName(string name)
        {
            var sql = @"select * from Tags where Name like N'%" + name.Trim() + "%'";
            return tagEntity.FromSql<Tags>(sql).AsEnumerable();
        }

        public void UpdateTag(Tags tag)
        {
            tagEntity.Update(tag);
            context.SaveChanges();
        }

        public void DeleteTag(int id)
        {
            var tag = tagEntity.Where(o => o.Id.Equals(id)).FirstOrDefault();
            tagEntity.Remove(tag);
            context.SaveChanges();
        }

        public void DeleteTagByName(string name)
        {
            var sql = @"select * from Tags where Name like N'" + name + "'";
            var tag = tagEntity.FromSql<Tags>(sql).FirstOrDefault();
            tagEntity.Remove(tag);
            context.SaveChanges();
        }

        public IEnumerable<Tags> GetTagsByPosts(int postsId)
        {
            var sql = @"select t.* from Tags t inner join PostsTag pt on t.Id=pt.tagId where pt.PostsId=" + postsId + " order by Name";
            return tagEntity.FromSql<Tags>(sql).AsEnumerable();
        }

        public IEnumerable<Tags> GetTagsFeature()
        {
            return tagEntity.OrderByDescending(o => o.Count).Take(5).AsEnumerable();
        }

        public IEnumerable<Tags> GetTagsRelation(string key)
        {
            //TODO: how to get tag relation
            return tagEntity.OrderByDescending(o => o.Count).Take(5).AsEnumerable();
        }

        public IEnumerable<Tags> SearchInTags(string key)
        {
            var sql = @"SELECT * FROM Tags WHERE FREETEXT (Name, '" + key + "')";
            return tagEntity.FromSql<Tags>(sql).AsEnumerable();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QAP4.Domain.AggreatesModels.Posts.Models;
using QAP4.Domain.Core.Repositories;
using QAP4.Models;

namespace QAP4.Infrastructure.Repositories
{
    public interface ITagRepository : IRepository<Tag>
    {
        void Create(Tag tag);
        Tag GetTag(int id);
        Tag GetTagByName(string name);
        IEnumerable<Tag> GetTagsByName(string name);
        IEnumerable<Tag> GetTagsByPosts(int postId);

        int CreateOrGetTagId(int? userId, string name);
        void UpdateTagCount(bool isUp, int id);
        void UpdateTag(Tag tag);
        void DeleteTagByName(string name);
        void DeleteTag(int id);

        //get with relation other object
        IEnumerable<Tag> GetTagsFeature();
        IEnumerable<Tag> GetTagsRelation(string key);
        IEnumerable<Tag> SearchInTags(string key);
    }
}

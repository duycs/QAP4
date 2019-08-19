using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QAP4.Models;

namespace QAP4.Infrastructure.Repositories
{
    public interface ITagRepository
    {
        void Create(Tags tag);
        Tags GetTag(int id);
        Tags GetTagByName(string name);
        IEnumerable<Tags> GetTagsByName(string name);
        IEnumerable<Tags> GetTagsByPosts(int postId);

        int CreateOrGetTagId(int? userId, string name);
        void UpdateTagCount(bool isUp, int id);
        void UpdateTag(Tags tag);
        void DeleteTagByName(string name);
        void DeleteTag(int id);

        //get with relation other object
        IEnumerable<Tags> GetTagsFeature();
        IEnumerable<Tags> GetTagsRelation(string key);
        IEnumerable<Tags> SearchInTags(string key);
    }
}

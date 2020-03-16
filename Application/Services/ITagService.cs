using System;
using System.Collections.Generic;
using System.Linq;
using QAP4.Extensions;
using QAP4.Models;

namespace QAP4.Application.Services
{
    public interface ITagService
    {

        Tags AddTag(Tags tagViewModel);
        void UpdateTag(Tags tagViewModel);
        Tags GetTagById(int id);
        Tags GetTagByName(string name);
        IEnumerable<Tags> GetTagsByName(string name);
        IEnumerable<Tags> GetTagsByPostsId(int postId);

        void UpdateTagCount(bool isUp, int id);
        void DeleteTagByName(string name);
        void DeleteTagById(int id);

        //get with relation other object
        IEnumerable<Tags> GetTagsFeature(int take);
        IEnumerable<Tags> GetTagsRelation(string key, int take);
        IEnumerable<Tags> SearchInTags(string key);

        // TODO: refactor
        int CreateOrGetTagId(int? userId, string name);

    }
}
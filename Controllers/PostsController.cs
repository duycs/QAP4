using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using QAP4.Extensions;
using Microsoft.AspNetCore.Http;
using QAP4.Models;
using QAP4.ViewModels;
using QAP4.Repository;
using System.Collections.Generic;

namespace QAP4.Controllers
{
    [Route("[controller]")]
    public class PostsController : Controller
    {
        //private QAPContext DBContext;
        private IPostsRepository PostsRepo { get; set; }
        private ITagRepository TagRepo { get; set; }
        private IPostsTagRepository PostsTagRepo { get; set; }
        private IUserRepository UserRepo { get; set; }
        private IPostLinkRepository PostLinkRepo { get; set; }

        public PostsController(IPostsRepository _postsRepo
            , ITagRepository _tagRepo
            , IPostsTagRepository _postsTag
            , IUserRepository _userRepo
            , IPostLinkRepository _postLinkRepo)
        {
            PostsRepo = _postsRepo;
            TagRepo = _tagRepo;
            PostsTagRepo = _postsTag;
            UserRepo = _userRepo;
            PostLinkRepo = _postLinkRepo;
        }

        // methods for MVC

        // GET: /posts/manager?t=1&u=6&p=1
        // t: type of object
        // u: id of user
        // p: page
        [HttpGet]
        [Route("/posts/manager")]
        public IActionResult PostsManagerView([FromQuery]int pg, [FromQuery]string or_b, [FromQuery]int u_i, [FromQuery]int po_t, [FromQuery]int po_lst_t)
        {
            var thisUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.Path}?po_lst_t={po_lst_t}&po_t={po_t}&u_i={u_i}";
            HttpContext.Session.SetString("thisUrl", thisUrl);

            var userId = HttpContext.Session.GetInt32(AppConstants.Session.USER_ID);
            var user = UserRepo.GetById(userId);

            if (user == null)
            {
                return RedirectToAction("login", "user", new MessageView(0, AppConstants.Warning.WAR_2003, AppConstants.Screen.POSTS_MANAGER));
            }

            ViewBag.UserName = HttpContext.Session.GetString(AppConstants.Session.USER_NAME);
            ViewBag.UserId = userId;

            //var PostsLst = PostsRepo.GetPosts(pg, or_b, u_i, po_lst_t, 0);

            return View("PostsManager");
        }



        // GET: /posts?pg=1&or_b=1&po_t=0
        // posts can be a posts normal (1), a question (2) or an answer (postType=3)
        [HttpGet]
        public IActionResult PostsView([FromQuery]int pg, [FromQuery]string type)
        {
            var userId = HttpContext.Session.GetInt32(AppConstants.Session.USER_ID);
            var user = UserRepo.GetById(userId);

            ViewBag.UserName = HttpContext.Session.GetString(AppConstants.Session.USER_NAME);
            ViewBag.UserId = HttpContext.Session.GetInt32(AppConstants.Session.USER_ID);

            //anything can read and good
            if ("read".Equals(type))
            {
                PostsView postsView = new PostsView();
                postsView.User = user;

                var postsSimple = PostsRepo.GetPostsByType(pg, 1);
                var tutorials = PostsRepo.GetPostsByType(pg, 6);
                var questions = PostsRepo.GetPostsByType(pg, 2);

                //tutorial answer
                var tutorialsAnswer = tutorials.Where(w => !string.IsNullOrEmpty(w.RelatedPosts));

                //questions answer
                var questionsAnswer = questions.Where(w => w.AnswerCount > 0);

                postsView.PostsSimple = postsSimple;
                postsView.QuestionsAnswer = questionsAnswer;
                postsView.TutorialsAnswer = tutorialsAnswer;

                //tags and users feature
                postsView.TagsFeature = TagRepo.GetTagsFeature();
                postsView.UsersFeature = UserRepo.GetUsersFeature();

                return View("Posts", postsView);
            }
            else if ("answer".Equals(type))
            {
                QuestionsView questionsView = new QuestionsView();
                questionsView.User = user;
                var postWaitAnswer = new List<Posts>();
                var postsAnswer = new List<Posts>();

                //all posts, these are tutorials or questions
                var tutorials = PostsRepo.GetPostsByType(pg, 6);
                var questions = PostsRepo.GetPostsByType(pg, 2);

                //tutorial wait answer
                var tutorialsWaitAnswer = tutorials.Where(w => string.IsNullOrEmpty(w.RelatedPosts));

                //tutorial answer
                var tutorialsAnswer = tutorials.Where(w => !string.IsNullOrEmpty(w.RelatedPosts));

                //question wait answer
                var questionsWaitAnswer = questions.Where(w => w.AnswerCount == 0);
                var questionsAnswer = questions.Where(w => w.AnswerCount > 0);

                //set posts wait answer, include tutorial and questions
                postWaitAnswer.AddRange(questionsWaitAnswer);
                postWaitAnswer.AddRange(tutorialsWaitAnswer);
                questionsView.PostsWaitAnswer = postWaitAnswer;

                //set posts answer, include tutorials and questions
                postsAnswer.AddRange(questionsAnswer);
                postsAnswer.AddRange(tutorialsAnswer);
                questionsView.PostsAnswer = postsAnswer;

                //tags and users feature
                questionsView.TagsFeature = TagRepo.GetTagsFeature();
                questionsView.UsersFeature = UserRepo.GetUsersFeature();

                //get total posts
                questionsView.Count = questionsView.PostsAnswer.Count() + questionsView.PostsWaitAnswer.Count();

                return View("Questions", questionsView);
            }
            else
            {
                return Redirect("\\");
            }
        }


        // GET: /posts/questions?t=2
        // posts can be a posts normal (1), a question (2) or an answer (postType=3)
        [HttpGet]
        [Route("/posts/questions")]
        public ActionResult QuestionsView(int pg, [FromQuery]int postsTypeId)
        {
            ViewBag.UserName = HttpContext.Session.GetString(AppConstants.Session.USER_NAME);
            ViewBag.UserId = HttpContext.Session.GetInt32(AppConstants.Session.USER_ID);
            QuestionsView questionsView = new QuestionsView();
            var postWaitAnswer = new List<Posts>();

            //get posts wait answer
            var tutorialWaitAnswer = PostsRepo.GetPostsByType(pg, postsTypeId);

            var questionWaitAnswer = PostsRepo.GetQuestionsQueue(pg, 0);
            postWaitAnswer.AddRange(questionWaitAnswer);
            postWaitAnswer.AddRange(tutorialWaitAnswer);
            questionsView.PostsWaitAnswer = postWaitAnswer;

            //get posts answer
            questionsView.PostsAnswer = PostsRepo.GetAnswersNewest(pg, 0);

            //get tags
            questionsView.TagsFeature = TagRepo.GetTagsFeature();
            questionsView.UsersFeature = UserRepo.GetUsersFeature();

            return View("Posts", questionsView);
        }

        // GET: /posts/ask?u_i=6
        // u_i: id of user
        // posts can be a posts normal (1), a question (2) or an answer (postType=3)
        [HttpGet]
        [Route("/posts/ask")]
        public ActionResult AskView([FromQuery]int u_i)
        {
            var thisUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.Path}";
            HttpContext.Session.SetString("thisUrl", thisUrl);

            var userId = HttpContext.Session.GetInt32(AppConstants.Session.USER_ID);
            var user = UserRepo.GetById(userId);

            if (user == null)
            {
                return RedirectToAction("login", "user", new MessageView(AppConstants.Warning.WAR_2003, AppConstants.Screen.POSTS_ASK));
            }

            ViewBag.UserName = HttpContext.Session.GetString(AppConstants.Session.USER_NAME);
            ViewBag.UserId = userId;

            return View("Ask");
        }


        [HttpGet("{id:int}")]
        public IActionResult PostDetail(int id, [FromQuery]int po_br_i)
        {
            var userId = HttpContext.Session.GetInt32(AppConstants.Session.USER_ID);
            var user = UserRepo.GetById(userId);
            ViewBag.UserName = HttpContext.Session.GetString(AppConstants.Session.USER_NAME);
            ViewBag.UserId = HttpContext.Session.GetInt32(AppConstants.Session.USER_ID);
            var posts = PostsRepo.GetPosts(id);
            if (null == posts)
            {
                return BadRequest();
            }

            var postTypeId = (int)posts.PostTypeId;
            if (AppConstants.PostsType.POSTS.Equals(postTypeId))
            {
                var PostsDetailView = new PostsDetailView();
                PostsDetailView.User = user;
                PostsDetailView.Posts = posts;
                PostsDetailView.PostsSameTags = PostsRepo.GetPostsSameTags(id, posts.Tags, postTypeId);
                PostsDetailView.PostsSameAuthor = PostsRepo.GetPostsSameAuthor(id, posts.OwnerUserId, postTypeId);
                return View("PostsDetail", PostsDetailView);
            }
            else if (AppConstants.PostsType.QUESTION.Equals(postTypeId))
            {

                var questionDetailView = new QuestionDetailView();
                questionDetailView.User = user;
                questionDetailView.Posts = posts;
                questionDetailView.Answers = PostsRepo.GetPosts(0, AppConstants.QueryString.CREATION_DATE, 0, AppConstants.PostsType.ANSWER, id);

                //same question
                questionDetailView.SameQuestions = PostsRepo.GetSameQuestion(posts.Title, 5);

                return View("QuestionDetail", questionDetailView);
            }
            else if (AppConstants.PostsType.TUTORIAL.Equals(postTypeId))
            {
                var TutorialView = new TutorialDetailView();
                int relatedPost = 0;
                TutorialView.Tutorial = posts;
                TutorialView.User = user;
                List<KeyValuePair<int, string>> relatedPosts = new List<KeyValuePair<int, string>>();

                if (posts.RelatedPosts != null && posts.RelatedPosts.Any())
                {
                    //gen list link key val
                    if (!string.IsNullOrEmpty(posts.TableContent))
                    {
                        var lstTitle = posts.TableContent.Split(',').ToArray();
                        var lstId = posts.RelatedPosts.Split(',').ToArray();
                        int i = 0;
                        foreach (var item in lstId)
                        {
                            relatedPosts.Add(new KeyValuePair<int, string>(Int32.Parse(lstId[i]), lstTitle[i]));
                            //relatedPosts.Add(new KeyValuePair<int, string>(Int32.Parse(lstId[i]), "bug title???"));
                            i++;
                        }

                        if (po_br_i == 0)
                        {
                            relatedPost = Int32.Parse(lstId[0]);
                        }
                        else
                        {
                            relatedPost = po_br_i;
                        }
                        TutorialView.Posts = PostsRepo.GetPosts(relatedPost);
                        TutorialView.RelatedPosts = relatedPosts;

                    }
                }
                else
                {
                    //select all posts where parternId = this posts
                    var chirldPosts = PostsRepo.GetChirldPosts(0, id, 6);
                    if (chirldPosts != null && chirldPosts.Any())
                    {

                        //get first element
                        TutorialView.Posts = chirldPosts.ElementAt(0);

                        int i = 0;
                        foreach (var item in chirldPosts)
                        {
                            relatedPosts.Add(new KeyValuePair<int, string>(item.Id, i + ""));
                            i++;
                        }

                        TutorialView.RelatedPosts = relatedPosts;
                    }

                }
                //same question
                //AnswerView.SameQuestions = PostsRepo.GetSameQuestion(posts.Title, 5);

                return View("TutorialDetail", TutorialView);
            }

            return View("Error");
        }

        [HttpGet]
        [Route("/posts/list")]
        public IActionResult PostsList([FromQuery]int pg, [FromQuery]string or_b, [FromQuery]int u_i, [FromQuery]int po_t, [FromQuery]int pr_i)
        {
            ViewBag.UserName = HttpContext.Session.GetString(AppConstants.Session.USER_NAME);
            ViewBag.UserId = HttpContext.Session.GetInt32(AppConstants.Session.USER_ID);
            var postsList = PostsRepo.GetPosts(pg, or_b, u_i, po_t, pr_i);
            return View("PostsList", postsList);
        }


        // methods for API

        // GET: /posts?u=6&t=1&p=1
        // posts can be a posts normal (1), a question (2) or an answer (postType=3)
        [HttpGet]
        [Route("/api/posts")]
        public IEnumerable<Posts> Posts([FromQuery]int pg, [FromQuery]string or_b, [FromQuery]int u_i, [FromQuery]int po_t, [FromQuery]int pr_i)
        {
            return PostsRepo.GetPosts(pg, or_b, u_i, po_t, pr_i);
        }

        [HttpGet]
        [Route("/api/exportBook")]
        public IEnumerable<Book> ExportBook()
        {
            return PostsRepo.GetAll().Where(w => w.DeletionDate == null && w.PostTypeId != 2 && w.PostTypeId != 3).Select(s => new Book
            {
                Uid = s.Id,
                Id = 0,
                Isbn = 0,
                CoverImage = s.CoverImg,
                Description = s.HeadContent,
                Title = s.Title,
                Subject = null,
                Publisher = null,
                Language = null,
                PageNumber = 0,
            }).ToList();
        }

        // GET: /posts/7?t=1
        [HttpGet]
        [Route("/api/posts/{id:int}")]
        public Posts Posts(int id)
        {
            return PostsRepo.GetPosts(id);
        }


        // POST create: /posts/0 
        // POST update: /posts/1
        // posts can be a posts normal (1), a question (2) or an answer (postType=3)

        //auto check insert or update
        [HttpPost]
        [Route("/api/posts/{id:int}")]
        public ActionResult InsertOrUpdate(int id, Posts model)
        {
            if (null == model)
            {
                return BadRequest();
            }

            var thisUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
            HttpContext.Session.SetString("thisUrl", thisUrl);

            var userId = HttpContext.Session.GetInt32(AppConstants.Session.USER_ID);
            var user = UserRepo.GetById(userId);
            int postTypeId = (int)model.PostTypeId;
            if (null == user)
            {
                return Json(new MessageView(AppConstants.Warning.WAR_2003));
            }

            var posts = new Posts();

            if (id == 0)
            {
                //related posts or chirld posts
                List<string> relatedPosts = new List<string>();
                if (!string.IsNullOrEmpty(posts.RelatedPosts))
                {
                    relatedPosts = posts.RelatedPosts.Split(',').ToList();
                }

                //insert
                var dateTime = DateTime.Now;
                posts = model;

                // edit some infor posts for correct
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


                if (AppConstants.PostsType.POSTS.Equals(postTypeId))
                {
                    if (string.IsNullOrEmpty(model.HtmlContent))
                    {
                        return Json(new MessageView(id, AppConstants.Error.ERR_3003));
                    }
                    posts.BodyContent = StringExtensions.StripHTML(posts.HtmlContent);
                    posts.HeadContent = StringExtensions.GetWords(posts.BodyContent, 40);

                }
                else if (AppConstants.PostsType.QUESTION.Equals(postTypeId))
                {
                    if (!string.IsNullOrEmpty(model.HtmlContent))
                    {
                        posts.BodyContent = StringExtensions.StripHTML(posts.HtmlContent);
                        posts.HeadContent = StringExtensions.GetWords(posts.BodyContent, 40);
                    }
                }
                else if (AppConstants.PostsType.TUTORIAL.Equals(postTypeId))
                {
                    if (!string.IsNullOrEmpty(model.HtmlContent))
                    {
                        posts.BodyContent = StringExtensions.StripHTML(posts.HtmlContent);
                        posts.HeadContent = StringExtensions.GetWords(posts.BodyContent, 40);
                    }
                }
                //if posts answer, update answer count of parent posts
                else if (AppConstants.PostsType.ANSWER.Equals(postTypeId))
                {
                    var postsParent = PostsRepo.GetPosts(model.ParentId);
                    if (postsParent != null)
                    {
                        var answerCount = postsParent.AnswerCount;
                        answerCount++;
                        postsParent.AnswerCount = answerCount;
                        PostsRepo.Update(postsParent);
                    }
                }
                else if (postTypeId != 0)
                {

                    if (string.IsNullOrEmpty(posts.RelatedPosts))
                    {
                        return Json(new MessageView(id, AppConstants.Error.ERR_3003));
                    }

                    relatedPosts = posts.RelatedPosts.Split(',').ToList();

                    // handler same as handler tag
                    UpdatePostLinks(id, relatedPosts, postTypeId);
                }

                // add  posts
                id = PostsRepo.Add(posts);

                //handle update parentId in related posts
                if (relatedPosts.Any())
                {
                    UpdateParentIdChirldPosts(id, relatedPosts);
                }

                // handler tag
                if (!string.IsNullOrEmpty(posts.Tags))
                {
                    string[] tags = posts.Tags.Split(',');
                    UpdatePostsTags(userId, id, tags);
                }

                if (!AppConstants.PostsType.ANSWER.Equals(postTypeId) && !AppConstants.PostsType.POSTS.Equals(postTypeId))
                {
                    // handler same as handler tag
                    UpdatePostLinks(id, relatedPosts, postTypeId);

                    // create table of content 
                    string tableOfcontent = GetTableOfContent(posts.Id, relatedPosts);
                    posts.TableContent = tableOfcontent;
                    PostsRepo.Update(posts);
                }

            }
            else
            {
                //update
                posts = PostsRepo.GetPosts(id);
                if (null != posts)
                {
                    //var oldPosts = posts;
                    string[] oldTags = new string[0];
                    string[] newTags = new string[0];
                    if (!string.IsNullOrEmpty(posts.Tags))
                    {
                        oldTags = posts.Tags.Split(',');
                    }
                    if (!string.IsNullOrEmpty(model.Tags))
                    {
                        newTags = model.Tags.Split(',');
                    }

                    //can't use posts = model;
                    //use copy object value
                    //posts = (Posts)ReflectionExtensions.CopyObjectValue(model, posts);
                    posts.Title = model.Title;
                    if (!string.IsNullOrEmpty(model.UserAvatar))
                        posts.UserAvatar = model.UserAvatar;
                    posts.HtmlContent = model.HtmlContent;
                    posts.Description = model.Description;
                    posts.Tags = model.Tags;
                    posts.RelatedPosts = model.RelatedPosts;
                    posts.TableContent = model.TableContent;
                    posts.BodyContent = StringExtensions.StripHTML(posts.HtmlContent);
                    posts.HeadContent = StringExtensions.GetWords(posts.BodyContent, 40);
                    posts.LastEditDate = DateTime.Now;
                    posts.CoverImg = model.CoverImg;

                    if (model.PostTypeId != 0 && !string.IsNullOrEmpty(posts.RelatedPosts))
                    {
                        // handler create notebook same as handler tag
                        List<string> relatedPosts = posts.RelatedPosts.Split(',').ToList();
                        UpdatePostLinks(id, relatedPosts, (int)model.PostTypeId);

                        // create table of content 
                        string tableOfcontent = GetTableOfContent(posts.Id, relatedPosts);
                        posts.TableContent = tableOfcontent;
                    }

                    id = PostsRepo.Update(posts);

                    //handler tag
                    //if tag change
                    if (!oldTags.Equals(newTags))
                    {
                        //check tags add new and tags delete in table PostsTag
                        string[] tagsDiff = GetTagsDiff(oldTags, newTags);
                        UpdatePostsTags(userId, id, tagsDiff);
                    }
                }
            }
            return Json(new MessageView(id, AppConstants.Message.MSG_1000));

        }

        private void UpdateParentIdChirldPosts(int parentPostsId, List<string> relatedPosts)
        {
            try
            {
                foreach (var chirldPostId in relatedPosts)
                {
                    int postsId = Int32.Parse(chirldPostId);
                    var posts = PostsRepo.GetPosts(postsId);
                    if (posts != null)
                    {
                        posts.ParentId = parentPostsId;
                        PostsRepo.Update(posts);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private Posts initPost(Posts post)
        {
            var userId = HttpContext.Session.GetInt32(AppConstants.Session.USER_ID);
            var postTmp = new Posts();
            int postId = post.Id;
            int postTypeId = (int)post.PostTypeId;

            List<string> relatedPosts = new List<string>();

            postTmp = post;
            switch (postTypeId)
            {
                case AppConstants.PostsType.POSTS:
                    postTmp.BodyContent = StringExtensions.StripHTML(post.HtmlContent);
                    postTmp.HeadContent = StringExtensions.GetWords(post.BodyContent, 40);

                    break;
                case AppConstants.PostsType.QUESTION:
                    postTmp.BodyContent = StringExtensions.StripHTML(post.HtmlContent);
                    postTmp.HeadContent = StringExtensions.GetWords(post.BodyContent, 40);
                    break;

                case AppConstants.PostsType.ANSWER:
                    //if posts answer, update answer count of parent posts
                    var parent = PostsRepo.GetPosts(post.ParentId);
                    if (parent != null)
                    {
                        var answerCount = parent.AnswerCount;
                        answerCount++;
                        parent.AnswerCount = answerCount;
                        PostsRepo.Update(parent);
                    }
                    break;
                case AppConstants.PostsType.TUTORIAL:
                    break;
                case AppConstants.PostsType.EXAMINATION:
                    break;
                case AppConstants.PostsType.NOTEBOOK:
                    break;
            }

            // handler tag
            if (!string.IsNullOrEmpty(post.Tags))
            {
                string[] tags = post.Tags.Split(',');
                UpdatePostsTags(userId, postId, tags);
            }
            //handler related post
            if (!string.IsNullOrEmpty(post.RelatedPosts))
            {
                relatedPosts = post.RelatedPosts.Split(',').ToList();

                // handler same as handler tag
                UpdatePostLinks(postId, relatedPosts, postTypeId);

                // create table of content 
                string tableOfcontent = GetTableOfContent(postId, relatedPosts);
                postTmp.TableContent = tableOfcontent;
            }

            return postTmp;
        }

        //DELETE: /posts/2
        [HttpDelete]
        [Route("/api/posts/{id:int}")]
        public ActionResult Delete(int id)
        {
            if (0 == id)
            {
                return BadRequest();
            }
            var posts = PostsRepo.GetPosts(id);
            var dateTime = DateTime.Now;
            posts.LastEditDate = dateTime;
            posts.DeletionDate = dateTime;
            PostsRepo.Update(posts);
            return Json(new MessageView(id, AppConstants.Message.MSG_1000));
        }

        //PUT: /posts/acorde/3
        [HttpPut]
        [Route("/api/acorde/{id:int}")]
        public ActionResult ActiveOrDeactive(int id)
        {
            var posts = PostsRepo.GetPosts(id);
            if (null == posts)
            {
                return BadRequest();
            }

            var dateTime = DateTime.Now;
            var lastActivityDate = posts.LastActivityDate;
            posts.LastEditDate = dateTime;

            if (null == lastActivityDate)
                posts.LastActivityDate = dateTime;
            else
                posts.LastActivityDate = null;

            PostsRepo.Update(posts);
            return Json(new MessageView(id, AppConstants.Message.MSG_1000));
        }


        // private methods

        private void UpdatePostsTags(int? userId, int postsId, string[] tags)
        {
            foreach (string tag in tags)
            {
                //method handler: if tag not exist, create new tag and tag exist then get tag id
                var tagId = TagRepo.CreateOrGetTagId(userId, tag);

                //method handler: if object exist then remove and return false, not exist then create and return true
                bool isCreate = PostsTagRepo.CreateOrDelete(postsId, tagId);

                //statistic count tag: is create then is up, do not reverse
                bool isUp = isCreate;
                TagRepo.UpdateTagCount(isUp, tagId);
            }
        }

        private string[] GetTagsDiff(string[] oldTags, string[] newTags)
        {
            string[] diff1 = newTags.Except(oldTags).ToArray();
            string[] diff2 = oldTags.Except(newTags).ToArray();
            if (diff1.Length > 0)
                return diff1;
            else
                return diff2;
        }


        private void UpdatePostLinks(int relatedPostId, List<string> relatedPosts, int linkTypeId)
        {
            foreach (string postId in relatedPosts)
            {
                int postsId = Int32.Parse(postId);
                bool isCreate = PostLinkRepo.CreateOrDelete(relatedPostId, postsId, linkTypeId);
                if (isCreate)
                {
                    //TODO: statistic 

                }
            }
        }

        private int[] GetPostsDiff(int[] oldPosts, int[] newPosts)
        {
            int[] diff1 = newPosts.Except(oldPosts).ToArray();
            int[] diff2 = oldPosts.Except(newPosts).ToArray();
            if (diff1.Length > 0)
                return diff1;
            else
                return diff2;
        }

        private string GetTableOfContent(int relatedPostId, List<string> relatedPosts)
        {
            string[] tableOfContent = new string[relatedPosts.Count()];
            for (int i = 0; i < relatedPosts.Count(); ++i)
            {
                int postsId = Int32.Parse(relatedPosts[i]);
                var model = PostsRepo.GetPosts(postsId);
                if (model != null)
                {
                    tableOfContent[i] = model.Title;
                }
            }
            return String.Join(",", tableOfContent);
        }

    }

    public class Book
    {
        public int Uid { get; set; }
        public int Id { get; set; }
        public int Isbn { get; set; }
        public string CoverImage { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public string Subject { get; set; }
        public string Publisher { get; set; }
        public string Language { get; set; }
        public int PageNumber { get; set; }
    }
}

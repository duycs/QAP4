namespace QAP4.Domain.AggreatesModels.Posts.Models
{
    public enum PostsTypes
    {
        /// <summary>
        /// Note: simple posts are not have any parent or childs, 
        /// first create simple posts can be update to an other type which this is a parent of other simple posts,
        /// if childs are answers then the posts is question
        /// other case will set direct type when first create or update later
        /// </summary>
        POSTS = 1,
        /// <summary>
        /// Note: question will have some answers which there are simple posts
        /// </summary>
        QUESTION = 2,
        /// <summary>
        /// Note: answer is a simple posts and have a parent posts
        /// </summary>
        ANSWER = 3,
        /// <summary>
        /// Note: examination have many quesion-answers which there are simple posts
        /// </summary>
        EXAMINATION = 4,
        /// <summary>
        /// Note: test have many quiz question-answers which there are simple posts
        /// </summary>
        TEST = 5,
        /// <summary>
        /// Note: tutorial have many simple posts
        /// </summary>
        TUTORIAL = 6,
        /// <summary>
        /// Note: notebook have many simple posts
        /// </summary>
        NOTEBOOK = 7
    }
}
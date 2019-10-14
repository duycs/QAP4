namespace QAP4.Domain.AggreatesModels.Posts.Models
{
    public enum RankTypes
    {
        /// <summary>
        /// Condition: over 10 of posts or vote in one posts
        /// </summary>
        NORMAL = 1,
        /// <summary>
        /// Condition: over 20 of posts or vote in one posts
        /// </summary>
        SILVER = 2,
        /// <summary>
        /// Condition: over 30 of posts or vote in one posts
        /// </summary>
        TITAN = 3,
        /// <summary>
        /// Condition: over 40 of posts or vote in one posts
        /// </summary>
        GOLD = 4,
        /// <summary>
        /// Condition: over 50 of posts or vote in one posts
        /// </summary>
        PLATIUM = 5,
        /// <summary>
        /// Condition: over 100 of posts or vote in one posts
        /// </summary>
        DINAMOND = 6,
    }
}
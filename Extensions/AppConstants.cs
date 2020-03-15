using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QAP4.Extensions
{
    public static class AppConstants
    {


        // Order
        public const string ORDER_CREATION_DATE = "CreationDate";

        //session
        public static class Session
        {
            public const string USER_ID = "userId";
            public const string USER_NAME = "userName";
            public const string EMAIL = "email";
            public const string AVARTAR = "avatar";
            public const string FULL_NMAE = "userName";
            public const string FIRST_NAME = "firstName";
            public const string LAST_NAME = "lastName";
            public const string POSTS_EDIT_ID = "postsEditId";
            public const string WARNING_MESSAGE = "warningMessgae";
            public const string ERROR_MESSAGE = "errorMessage";
            public const string SUCCESS_MESSAGE = "successMessage";
        }


        // query string
        public static class QueryString
        {
            public const string CREATION_DATE = "cr_d";
            public const string VOTE_COUNT = "vo_c";
            public const string VIEW_COUNT = "vi_c";
            public const string ANSWER_COUNT = "an_c";
            public const string SCORE = "sc";

            public const string PAGE = "pg";
            public const string POSTS_TYPE = "po_t";
            public const string USER_ID = "u_i";
            public const string POSTS_ID = "po_i";
            public const string TAB = "tab";


        }


        public static class Status
        {
            public const string SUCCESS = "success";
            public const string WARNING = "warning";
            public const string ERROR = "error";
        }

        //object type
        public static class ObjectType
        {
            public const string ALL = "all";
            public const string TAG = "tag";
            public const string POSTS = "posts";
            public const string QUESTION = "question";
            public const string TUTORIAL = "tutorial";
            public const string TEST = "test";
            public const string USER = "user";
            public const string GROUP = "group";
        }

        public static class PostsType
        {
            //normal all type is posts
            public const int POSTS = 1;
            public const int QUESTION = 2;
            public const int ANSWER = 3;
            public const int EXAMINATION = 4;
            public const int TEST = 5;
            public const int TUTORIAL = 6;
            public const int NOTEBOOK = 7;
            
        }

        public static class VoteTypeId
        {
            public const int VOTE_UP_ID = 1;
            public const int VOTE_DOWN_ID = 0;
        }

        public static class Screen
        {
            public const string POSTS_MANAGER = "manager";
            public const string POSTS_QUESTION = "question";
            public const string POSTS_ASK = "ask";
            public const string POSTS_ANSWER = "answer";
            public const string POSTS_DETAIL = "postDetail";
        }

        public static class Paging
        {
            public const int PAGE_SIZE = 4;
        }

        //message: success or normal
        public static class Message
        {
            public const string MSG_1000 = "msg:1000:Thực hiện thành công";
            public const string MSG_1001 = "msg:1001:Tạo người dùng thành công";
            public const string MSG_1002 = "msg:1002:Tạo người bài viết thành công";
           
            public const string MSG_1004 = "msg:1004:Xin chào!";
            public const string MSG_1005 = "msg:1005:Tạm biệt!";
        }

        //warning
        public static class Warning
        {
            public const string WAR_2000 = "war:2000:Cảnh báo";
            public const string WAR_2001 = "war:2001:Tài khoản hoặc mật khẩu chưa đúng";
            public const string WAR_2002 = "war:2002:Bạn đã vote rồi";
            public const string WAR_2003 = "war:2003:Bạn vui lòng đăng nhập để định danh";

        }

        //exception
        public static class Error
        {
            public const string ERR_3000 = "err:3000:Đã có lỗi xảy ra";
            public const string ERR_3001 = "err:3001:Tài khoản đã tồn tại";
            public const string ERR_3002 = "err:3002:Tạo bài viết không thành công";
            public const string ERR_3003 = "err:3003:Bạn không thể đăng một bài viết mà không có nội dung";
        }



    }
}

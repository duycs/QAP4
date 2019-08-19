using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QAP4.ViewModels
{
    public class MessageView
    {
        public int itemId { get; set; }
        public string type { get; set; }
        public int code { get; set; }
        public string message { get; set; }
        public string sc { get; set; }

        //constructor
        public MessageView(int itemId, string fullMessage, string sc)
        {
            this.itemId = itemId;
            this.type = GetType(fullMessage);
            this.code = GetCode(fullMessage);
            this.message = GetMessage(fullMessage);
            this.sc = sc;
        }

        public MessageView(string fullMessage, string sc)
        {
            this.itemId = itemId;
            this.type = GetType(fullMessage);
            this.code = GetCode(fullMessage);
            this.message = GetMessage(fullMessage);
            this.sc = sc;
        }

        public MessageView(int itemId, string fullMessage)
        {
            this.itemId = itemId;
            this.type = GetType(fullMessage);
            this.code = GetCode(fullMessage);
            this.message = GetMessage(fullMessage);
        }

        public MessageView(string fullMessage)
        {
            this.type = GetType(fullMessage);
            this.code = GetCode(fullMessage);
            this.message = GetMessage(fullMessage);
        }

        //get full message
        public string GetFullMessage()
        {
            string fullMsg = "item id: " + itemId + ") ";
            switch (type)
            {
                case "msg":
                    fullMsg = message;
                    break;
                case "err":
                    fullMsg = "Lỗi: " + message;
                    break;
                case "war":
                    fullMsg = "Cảnh báo: " + message;
                    break;
                default:
                    fullMsg = message;
                    break;
            }
            return fullMsg;
        }

        //get type
        private string GetType(string s)
        {
            string type = "";
            string[] list = s.Split(':');
            type = list[0];
            return type;
        }

        //get code
        private int GetCode(string s)
        {
            int code = 0;
            string[] list = s.Split(':');
            code = Int32.Parse(list[1]);
            return code;
        }

        //get message
        private string GetMessage(string s)
        {
            string message = "";
            string[] list = s.Split(':');
            message = list[2];
            return message;
        }
    }
}

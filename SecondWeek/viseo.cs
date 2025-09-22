using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Video
{
    class video
    {
        public string title;
        public string author;
        private string type;
        public static int videoCount = 0;

        public video(string title, string author, string type)
        {
            this.title = title;
            this.author = author;
            Type = type;
            videoCount++;
        }

        public int GetVideoCount()
        {
            return videoCount;
        }
        public string Type
        {
            get { return type; }
            set
            {
                if (value == "程式設計" || value == "旅遊" || value == "美食")
                {
                    type = value;
                }
                else
                {
                    type = "其他";
                }
            }
        }
    }
}


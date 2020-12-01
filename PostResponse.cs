using System;
using System.Collections.Generic;
using System.Text;

namespace Bill
{
    public class PostResponse
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public PostResponse()
        {

        }
    }

    public class PostRetyrResponse
    {
        public int Code { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public int Days { get; set; }
        public int Interval { get; set; }
        public PostRetyrResponse()
        {

        }
    }
}

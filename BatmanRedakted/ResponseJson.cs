using System;
using System.Collections.Generic;
using System.Text;

namespace BatmanRedakted
{
  
    public class ResponseJson
    {
        public int dayNumber { get; set; }
        public object code { get; set; }
        public bool success { get; set; }
        public object spritesheetPath { get; set; }
        public object downloadPath { get; set; }
        public int correctChars { get; set; }
        public int codeLength { get; set; }
        public object videoUrl { get; set; }
        public object overlayHtml { get; set; }
    }

}

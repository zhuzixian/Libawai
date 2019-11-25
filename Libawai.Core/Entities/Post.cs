﻿using System;

namespace Libawai.Core.Entities
{
    public class Post:Entity
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public string Author { get; set; }
        public DateTime LastModified { get; set; }

        public string Remark { get; set; }
    }
}

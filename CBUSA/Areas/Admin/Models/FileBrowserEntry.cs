﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CBUSA.Areas.Admin.Models
{
    public class FileBrowserEntry
    {
        public string Name { get; set; }
        public EntryType Type { get; set; }
        public long Size { get; set; }
    }
}
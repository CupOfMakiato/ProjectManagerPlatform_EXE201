﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Contracts.Abstractions.CloudinaryService
{
    public class CloudinaryResponse
    {
        public string? FileUrl { get; set; }
        public string? PublicFileId { get; set; }
        public bool IsCover { get; set; } = false;
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperCyklisterneAPI.Models
{
    public class MedlemDto
    {
        public int MedlemsId { get; set; }
        public string Email { get; set; }
        public string Kodeord { get; set; }
        public string Navn { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace SuperCyklisterneAPI.Entities
{
    public partial class Medlemmer
    {
        public int MedlemsId { get; set; }
        public string Email { get; set; }
        public string Kodeord { get; set; }
        public string Navn { get; set; }
    }
}

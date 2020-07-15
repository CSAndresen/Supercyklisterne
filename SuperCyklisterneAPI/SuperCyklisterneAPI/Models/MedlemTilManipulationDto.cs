using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperCyklisterneAPI.Models
{
    public class MedlemTilManipulationDto
    {
        public virtual string Email { get; set; }
        public virtual string Kodeord { get; set; }
        public virtual string Navn { get; set; }
    }
}

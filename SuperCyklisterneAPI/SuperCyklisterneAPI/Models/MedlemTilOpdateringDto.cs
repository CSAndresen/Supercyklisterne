using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperCyklisterneAPI.Models
{
    public class MedlemTilOpdateringDto : MedlemTilManipulationDto
    {
        public override string Email { get => base.Email; set => base.Email = value; }
        public override string Kodeord { get => base.Kodeord; set => base.Kodeord = value; }
        public override string Navn { get => base.Navn; set => base.Navn = value; }
    }
}

using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperCyklisterneAPI.Profiles
{
    public class MedlemmerProfil : Profile
    {
        public MedlemmerProfil() {
            CreateMap<Entities.Medlemmer, Models.MedlemDto>();
            CreateMap<Models.MedlemTilTilføjelseDto, Entities.Medlemmer>();
            CreateMap<Models.MedlemTilOpdateringDto, Entities.Medlemmer>();
            CreateMap<Entities.Medlemmer, Models.MedlemTilOpdateringDto>();
        }
    }
}

using SuperCyklisterneAPI.Entities;
using SuperCyklisterneAPI.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SuperCyklisterneAPI.Services
{
    public interface IHjemmesideRepository
    {
        // Medlem Login 
        Medlemmer MedlemLogin(string email, string kodeord);

        // Hent Medlemmer
        IEnumerable<Medlemmer> HentMedlemmer();

        // Hent individuel medlem
        Medlemmer HentMedlem(int MedlemsId);

        // Tilføj medlem
        void TilføjMedlem(Medlemmer medlem);

        // Opdater medlem
        void OpdaterMedlem(Medlemmer medlem);
        
        // Gem nyt medlem
        bool Save();

        // Slet medlem
        void SletMedlem(Medlemmer medlem);

        // Eksisterer medlem
        bool TjekForMedlem(int MedlemsId);

    }
}
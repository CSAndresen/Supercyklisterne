using SuperCyklisterneAPI.Entities;
using SuperCyklisterneAPI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SuperCyklisterneAPI.Services
{
    public class HjemmesideRepository : IHjemmesideRepository, IDisposable
    {
        private readonly SupercyklisterneContext _context;
        public HjemmesideRepository(SupercyklisterneContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose resources when needed
            }
        }

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }

        public Medlemmer HentMedlem(int MedlemsId)
        {
            return _context.Medlemmer.FirstOrDefault(m => m.MedlemsId == MedlemsId);
        }

        public Medlemmer HentMedlemViaEmail(string email)
        {
            return _context.Medlemmer.FirstOrDefault(m => m.Email == email);
        }

        public IEnumerable<Medlemmer> HentMedlemmer()
        {
            return _context.Medlemmer.ToList<Medlemmer>();
        }

        public void OpdaterMedlem(Medlemmer medlem)
        {
            // Efterlad tom
        }

        public void SletMedlem(Medlemmer medlem)
        {
            _context.Medlemmer.Remove(medlem);
        }

        public void TilføjMedlem(Medlemmer medlem)
        {
            var MedlemFraRepo = _context.Medlemmer.SingleOrDefault(m => m.Email == medlem.Email);
            if (MedlemFraRepo == null) {
                _context.Medlemmer.Add(medlem);
            } else {
                throw new ArgumentNullException(nameof(medlem));
            }
        }

        public bool TjekForMedlem(int MedlemsId) {
            var medlemFraRepo = _context.Medlemmer.Any(m => m.MedlemsId == MedlemsId);

            if (medlemFraRepo == false) {
                throw new ArgumentNullException(nameof(MedlemsId));
            }

            return medlemFraRepo;
        }

        public Medlemmer MedlemLogin(string email, string kodeord)
        {
            var medlem = _context.Medlemmer.SingleOrDefault(m => m.Email == email && m.Kodeord == kodeord);

            if (medlem == null) {
                return null;
            }

            return medlem;
        }
    }
}

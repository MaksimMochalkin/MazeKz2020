using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.DbStuff.Model.Medicine;

namespace WebMaze.DbStuff.Repository.MedicineRepo
{
    public class MedicineCertificateRepository : BaseRepository<MedicineCertificate>
    {
        public MedicineCertificateRepository(WebMazeContext context) : base(context) { }

        public MedicineCertificate GetUser(long userId)
        {
            return dbSet.SingleOrDefault(x => x.UserId == userId);
        }

        public List<MedicineCertificate> GetCertificateByPosition(string position)
        {
            return dbSet.Where(p => p.Position == position).ToList();
        }

        public List<MedicineCertificate> GetCertificateByDate(DateTime? date)
        {
            return dbSet.Where(d => d.DateOfIssue == date || d.DateExpiration == date).ToList();
        }

        
    }
}

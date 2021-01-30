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

    }
}

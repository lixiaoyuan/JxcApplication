using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApplicationDb.Cor.EntityModels;
using ApplicationDb.Cor.Helper;

namespace ApplicationDb.Cor.Business
{
    public class FileCabinetsManager
    {
        public static Guid InserFile(byte[] data, string fullNam)
        {
            FileCabinets fileCabinets = new FileCabinets();
            fileCabinets.Id = Guid.NewGuid();
            fileCabinets.CreateTime = DBUnit.GetDbTime();
            fileCabinets.Data = data;
            fileCabinets.FilName = fullNam;
            fileCabinets.Size = data.LongLength;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                db.FileCabinetses.Add(fileCabinets);
                if (db.SaveChanges() > 0)
                {
                    return fileCabinets.Id;
                }
                return Guid.Empty;
            }
        }

        public static FileCabinets GetFile(Guid id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return db.FileCabinetses.Find(id);
            }
        }
    }
}

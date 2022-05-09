using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.Cart
{
    public class CartAtlet
    {
        //Atlet

        private List<SuProfil1> collection1 = new List<SuProfil1>();

        public virtual void AddItem1(
            int suProfilId,
            decimal amaun,
            decimal amaunsebelum,
            decimal tunggakan,
            decimal jumlah
            )
        {
            SuProfil1 line = collection1
            .Where(p => p.SuProfilId == suProfilId)
            .FirstOrDefault();

            if (line == null)
            {
                collection1.Add(new SuProfil1
                {
                    SuProfilId = suProfilId,
                    Amaun = amaun,
                    AmaunSebelum = amaunsebelum,
                    Tunggakan = tunggakan,
                    Jumlah = jumlah
                });
            }
        }

        public virtual void RemoveItem1(int id) =>
            collection1.RemoveAll(l => l.SuProfilId == id);


        public virtual void Clear1() => collection1.Clear();

        public virtual IEnumerable<SuProfil1> Lines1 => collection1;
        //Atlet End

    }
}

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
            int? suAtletId,
            int jSukanId,
            int? jCarabayarId,
            string noCekEFT,
            DateTime? tarCekEFT,
            decimal amaun,
            decimal amaunsebelum,
            decimal tunggakan,
            decimal jumlah
            )
        {
            SuProfil1 line = collection1
            .Where(p => p.SuAtletId == suAtletId)
            .FirstOrDefault();

            if (line == null)
            {
                collection1.Add(new SuProfil1
                {
                    SuProfilId = suProfilId,
                    SuAtletId = suAtletId,
                    JSukanId = jSukanId,
                    JCaraBayarId = jCarabayarId,
                    NoCekEFT = noCekEFT,
                    TarCekEFT = tarCekEFT,
                    Amaun = amaun,
                    AmaunSebelum = amaunsebelum,
                    Tunggakan = tunggakan,
                    Jumlah = jumlah
                });
            }
        }

        public virtual void RemoveItem1(int? id) =>
            collection1.RemoveAll(l => l.SuAtletId == id);


        public virtual void Clear1() => collection1.Clear();

        public virtual IEnumerable<SuProfil1> Lines1 => collection1;
        //Atlet End

    }
}

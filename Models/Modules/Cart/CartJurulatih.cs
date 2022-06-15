using System;
using System.Collections.Generic;
using System.Linq;

namespace MSNK.Models.Modules.Cart
{
    public class CartJurulatih
    {
        //Jurulatih

        private List<SuProfil1> collection1 = new List<SuProfil1>();

        public virtual void AddItem1(
            int suProfilId,
            int? suJurulatihId,
            int jSukanId,
            int? jCaraBayarId,
            string noCekEFT,
            DateTime? tarCekEFT,
            decimal amaun,
            decimal amaunsebelum,
            decimal tunggakan,
            string catatan,
            decimal jumlah
            )
        {
            SuProfil1 line = collection1
            .Where(p => p.SuJurulatihId == suJurulatihId)
            .FirstOrDefault();

            if (line == null)
            {
                collection1.Add(new SuProfil1
                {
                    SuProfilId = suProfilId,
                    SuJurulatihId = suJurulatihId,
                    JSukanId = jSukanId,
                    JCaraBayarId = jCaraBayarId,
                    NoCekEFT = noCekEFT,
                    TarCekEFT = tarCekEFT,
                    Amaun = amaun,
                    AmaunSebelum = amaunsebelum,
                    Tunggakan = tunggakan,
                    Catatan = catatan,
                    Jumlah = jumlah
                });
            }
        }

        public virtual void RemoveItem1(int? id) =>
            collection1.RemoveAll(l => l.SuJurulatihId == id);


        public virtual void Clear1() => collection1.Clear();

        public virtual IEnumerable<SuProfil1> Lines1 => collection1;
        //Atlet End
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.Cart
{
    public class CartPekerja
    {
        private List<SuTanggunganPekerja> collection1 = new List<SuTanggunganPekerja>();
        public virtual void AddItem1(
            int SuPekerjaId,
            string Nama,
            string Hubungan,
            string NoKP)
        {
            SuTanggunganPekerja line = collection1
            .Where(p => p.NoKP == NoKP)
            .FirstOrDefault();

            if (line == null)
            {
                collection1.Add(new SuTanggunganPekerja
                {
                    SuPekerjaId = SuPekerjaId,
                    Nama = Nama,
                    Hubungan = Hubungan,
                    NoKP = NoKP
                });
            }

        }
        public virtual void RemoveItem1(string nokp) =>
            collection1.RemoveAll(l => l.NoKP == nokp);

        public virtual void Clear1() => collection1.Clear();

        public virtual IEnumerable<SuTanggunganPekerja> Lines1 => collection1;
    }
}

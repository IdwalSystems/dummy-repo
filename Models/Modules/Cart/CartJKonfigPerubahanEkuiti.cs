using MSNK.Models.Operations;
using System.Collections.Generic;
using System.Linq;

namespace MSNK.Models.Modules.Cart
{
    public class CartJKonfigPerubahanEkuiti
    {
        private List<JKonfigPerubahanEkuitiBaris> collectionBaris = new List<JKonfigPerubahanEkuitiBaris>();

        public virtual void AddItemBaris(
            int jKonfigPerubahanEkuitiId,
            EnBarisPerubahanEkuiti enBaris,
            EnJenisOperasi enJenisOperasi,
            bool isPukal,
            string enJenisCartaList,
            bool isKecuali,
            string kodList,
            string setKodList
            )
        {
            JKonfigPerubahanEkuitiBaris line = collectionBaris.FirstOrDefault(b => b.EnBaris == enBaris && b.EnJenisOperasi == enJenisOperasi)!;

            if (line == null )
            {
                collectionBaris.Add(new JKonfigPerubahanEkuitiBaris()
                {
                    JKonfigPerubahanEkuitiId = jKonfigPerubahanEkuitiId,
                    EnBaris = enBaris,
                    EnJenisOperasi = enJenisOperasi,
                    IsPukal = isPukal,
                    EnJenisCartaList = enJenisCartaList,
                    IsKecuali = isKecuali,
                    KodList = kodList,
                    SetKodList = setKodList
                });
            }
        }

        public virtual void RemoveItemBaris(EnBarisPerubahanEkuiti enBaris, EnJenisOperasi enJenisOperasi) => collectionBaris.RemoveAll(b => b.EnBaris == enBaris && b.EnJenisOperasi == enJenisOperasi);

        public virtual void ClearBaris() => collectionBaris.Clear();

        public virtual IEnumerable<JKonfigPerubahanEkuitiBaris> JKonfigPerubahanEkuitiBaris => collectionBaris;
    }
}

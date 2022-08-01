using System.Collections.Generic;
using System.Linq;

namespace MSNK.Models.Modules.Cart
{
    public class CartPenyataPemungut
    {
        //AkPenyataPemungut 1

        private List<AkPenyataPemungut1> collection1 = new List<AkPenyataPemungut1>();

        public virtual void AddItem1(
            int akPenyataPemungutId,
            int indek,
            int jBahagianId,
            int akCartaId,
            decimal amaun
            )
        {
            AkPenyataPemungut1 line = collection1
            .Where(p => p.Indek == indek)
            .FirstOrDefault();

            if (line == null)
            {
                collection1.Add(new AkPenyataPemungut1
                {
                    AkPenyataPemungutId = akPenyataPemungutId,
                    Indek = indek,
                    JBahagianId = jBahagianId,
                    AkCartaId = akCartaId,
                    Amaun = amaun
                });
            }
        }

        public virtual void RemoveItem1(int id) =>
            collection1.RemoveAll(l => l.Indek == id);


        public virtual void Clear1() => collection1.Clear();

        public virtual IEnumerable<AkPenyataPemungut1> Lines1 => collection1;
        // AkPenyataPemungut1 End

        //AkPenyataPemungut 2
        private List<AkPenyataPemungut2> collection2 = new List<AkPenyataPemungut2>();

        public virtual void AddItem2 (
            int akPenyataPemungutId,
            int indek,
            int akTerima2Id,
            decimal amaun
            )
        {
            collection2.Add(new AkPenyataPemungut2
            {
                AkPenyataPemungutId = akPenyataPemungutId,
                Indek = indek,
                AkTerima2Id = akTerima2Id,
                Amaun = amaun
            });
        }

        public virtual void RemoveItem2(int id) =>
            collection2.RemoveAll(l => l.Indek == id);

        public virtual void Clear2() => collection2.Clear();

        public virtual IEnumerable<AkPenyataPemungut2> Lines2 => collection2;

        // AkPenyataPemungut2 End
    }
}

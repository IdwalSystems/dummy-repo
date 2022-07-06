using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.Cart
{
    public class CartPO
    {
        //PO 1

        private List<AkPO1> collection1 = new List<AkPO1>();

        public virtual void AddItem1(
            int AkPOId,         
            int akCartaId,
            decimal Amaun
            )
        {
            AkPO1 line = collection1
            .Where(p => p.AkCartaId == akCartaId)
            .FirstOrDefault();

            if (line == null)
            {
                collection1.Add(new AkPO1
                {
                    AkPOId = AkPOId,
                    AkCartaId = akCartaId,
                    Amaun = Amaun
                });
            }
        }

        public virtual void RemoveItem1(int id) =>
            collection1.RemoveAll(l => l.AkCartaId == id);


        public virtual void Clear1() => collection1.Clear();

        public virtual IEnumerable<AkPO1> Lines1 => collection1;
        // PO 1 End

        //PO 2
        private List<AkPO2> collection2 = new List<AkPO2>();

        public virtual void AddItem2(
            int akPOId,
            int Indek,
            decimal Bil,
            string NoStok,
            string Perihal,
            decimal Kuantiti,
            string Unit,
            decimal Harga,
            decimal Amaun
            )
        {

            {
                collection2.Add(new AkPO2
                {
                    AkPOId = akPOId,
                    Indek = Indek,
                    Bil = Bil,
                    NoStok = NoStok,
                    Perihal = Perihal,
                    Kuantiti = Kuantiti,
                    Unit = Unit,
                    Harga = Harga,
                    Amaun = Amaun
                });
            }
        }

        public virtual void RemoveItem2(int id) =>
            collection2.RemoveAll(l => l.Indek == id);


        public virtual void Clear2() => collection2.Clear();

        public virtual IEnumerable<AkPO2> Lines2 => collection2;
    }
}

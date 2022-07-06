using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.Cart
{
    public class CartPOLaras
    {
        //POLaras 1

        private List<AkPOLaras1> collection1 = new List<AkPOLaras1>();

        public virtual void AddItem1(
            int AkPOLarasId,
            int akCartaId,
            decimal Amaun
            )
        {
            AkPOLaras1 line = collection1
            .Where(p => p.AkCartaId == akCartaId)
            .FirstOrDefault();

            if (line == null)
            {
                collection1.Add(new AkPOLaras1
                {
                    AkPOLarasId = AkPOLarasId,
                    AkCartaId = akCartaId,
                    Amaun = Amaun
                });
            }
        }

        public virtual void RemoveItem1(int id) =>
            collection1.RemoveAll(l => l.AkCartaId == id);


        public virtual void Clear1() => collection1.Clear();

        public virtual IEnumerable<AkPOLaras1> Lines1 => collection1;
        // POLaras 1 End

        //POLaras 2
        private List<AkPOLaras2> collection2 = new List<AkPOLaras2>();

        public virtual void AddItem2(
            int akPOLarasId,
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
                collection2.Add(new AkPOLaras2
                {
                    AkPOLarasId = akPOLarasId,
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

        public virtual IEnumerable<AkPOLaras2> Lines2 => collection2;
    }
}

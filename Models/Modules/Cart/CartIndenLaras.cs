using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.Cart
{
    public class CartIndenLaras
    {
        //IndenLaras 1

        private List<AkIndenLaras1> collection1 = new List<AkIndenLaras1>();

        public virtual void AddItem1(
            int AkIndenLarasId,
            int akCartaId,
            decimal Amaun
            )
        {
            AkIndenLaras1 line = collection1
            .Where(p => p.AkCartaId == akCartaId)
            .FirstOrDefault();

            if (line == null)
            {
                collection1.Add(new AkIndenLaras1
                {
                    AkIndenLarasId = AkIndenLarasId,
                    AkCartaId = akCartaId,
                    Amaun = Amaun
                });
            }
        }

        public virtual void RemoveItem1(int id) =>
            collection1.RemoveAll(l => l.AkCartaId == id);


        public virtual void Clear1() => collection1.Clear();

        public virtual IEnumerable<AkIndenLaras1> Lines1 => collection1;
        // IndenLaras 1 End

        //IndenLaras 2
        private List<AkIndenLaras2> collection2 = new List<AkIndenLaras2>();

        public virtual void AddItem2(
            int akIndenLarasId,
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
                collection2.Add(new AkIndenLaras2
                {
                    AkIndenLarasId = akIndenLarasId,
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

        public virtual IEnumerable<AkIndenLaras2> Lines2 => collection2;
    }
}

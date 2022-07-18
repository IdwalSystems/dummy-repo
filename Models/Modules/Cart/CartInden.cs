using System.Collections.Generic;
using System.Linq;

namespace MSNK.Models.Modules.Cart
{
    public class CartInden
    {
        //Inden 1

        private List<AkInden1> collection1 = new List<AkInden1>();

        public virtual void AddItem1(
            int AkIndenId,
            int akCartaId,
            decimal Amaun
            )
        {
            AkInden1 line = collection1
            .Where(p => p.AkCartaId == akCartaId)
            .FirstOrDefault();

            if (line == null)
            {
                collection1.Add(new AkInden1
                {
                    AkIndenId = AkIndenId,
                    AkCartaId = akCartaId,
                    Amaun = Amaun
                });
            }
        }

        public virtual void RemoveItem1(int id) =>
            collection1.RemoveAll(l => l.AkCartaId == id);


        public virtual void Clear1() => collection1.Clear();

        public virtual IEnumerable<AkInden1> Lines1 => collection1;
        // Inden 1 End

        //Inden 2
        private List<AkInden2> collection2 = new List<AkInden2>();

        public virtual void AddItem2(
            int akIndenId,
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
                collection2.Add(new AkInden2
                {
                    AkIndenId = akIndenId,
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

        public virtual IEnumerable<AkInden2> Lines2 => collection2;
    }
}

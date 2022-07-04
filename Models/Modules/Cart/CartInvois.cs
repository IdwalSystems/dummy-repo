using System.Collections.Generic;
using System.Linq;

namespace MSNK.Models.Modules.Cart
{
    public class CartInvois
    {
        //Invois 1

        private List<AkInvois1> collection1 = new List<AkInvois1>();

        public virtual void AddItem1(
            int akInvoisId,
            decimal amaun,
            int akCartaId
            )
        {
            AkInvois1 line = collection1
            .Where(p => p.AkCartaId == akCartaId)
            .FirstOrDefault();

            if (line == null)
            {
                collection1.Add(new AkInvois1
                {
                    AkInvoisId = akInvoisId,
                    Amaun = amaun,
                    AkCartaId = akCartaId
                });
            }
        }

        public virtual void RemoveItem1(int id) =>
            collection1.RemoveAll(l => l.AkCartaId == id);


        public virtual void Clear1() => collection1.Clear();

        public virtual IEnumerable<AkInvois1> Lines1 => collection1;
        // Invois1 End

        //Invois 2
        private List<AkInvois2> collection2 = new List<AkInvois2>();

        public virtual void AddItem2(
            int akInvoisId,
            int Indek,
            int Baris,
            string Bil,
            string NoStok,
            string Perihal,
            decimal Kuantiti,
            string Unit,
            decimal Harga,
            decimal Amaun
            )
        {
            collection2.Add(new AkInvois2
            {
                AkInvoisId = akInvoisId,
                Indek = Indek,
                Baris = Baris,
                Bil = Bil,
                NoStok = NoStok,
                Perihal = Perihal,
                Kuantiti = Kuantiti,
                Unit = Unit,
                Harga = Harga,
                Amaun = Amaun
            });
        }

        public virtual void RemoveItem2(int id) =>
            collection2.RemoveAll(l => l.Indek == id);


        public virtual void Clear2() => collection2.Clear();

        public virtual IEnumerable<AkInvois2> Lines2 => collection2;
        // Invois 2 end
    }
}

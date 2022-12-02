using System.Collections.Generic;
using System.Linq;

namespace MSNK.Models.Modules.Cart
{
    public class CartNotaDebitKreditBelian
    {
        //Nota Debit Kredit 1
        private List<AkNotaDebitKreditBelian1> collection1 = new List<AkNotaDebitKreditBelian1>();

        public virtual void AddItem1(
            int akNotaDebitKreditBelianId,
            decimal amaun,
            int akCartaId
            )
        {
            AkNotaDebitKreditBelian1 line = collection1
                .Where(p => p.AkCartaId == akCartaId)
                .FirstOrDefault();

            if (line == null)
            {
                collection1.Add(new AkNotaDebitKreditBelian1
                {
                    AkNotaDebitKreditBelianId = akNotaDebitKreditBelianId,
                    Amaun = amaun,
                    AkCartaId = akCartaId
                });
            }
        }

        public virtual void RemoveItem1(int id) =>
            collection1.RemoveAll(l => l.AkCartaId == id);

        public virtual void Clear1() => collection1.Clear();

        public virtual IEnumerable<AkNotaDebitKreditBelian1> Lines1 => collection1;
        //Nota Debit Kredit 1 end

        //Nota Debit Kredit 2
        private List<AkNotaDebitKreditBelian2> collection2 = new List<AkNotaDebitKreditBelian2>();

        public virtual void AddItem2(
            int akNotaDebitKreditBelianId,
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
            collection2.Add(new AkNotaDebitKreditBelian2
            {
                AkNotaDebitKreditBelianId = akNotaDebitKreditBelianId,
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

        public virtual void RemoveItem2(int id) =>
            collection2.RemoveAll(l => l.Indek == id);

        public virtual void Clear2() => collection2.Clear();

        public virtual IEnumerable<AkNotaDebitKreditBelian2> Lines2 => collection2;

        //Nota Debit Kredit 2 End
    }
}

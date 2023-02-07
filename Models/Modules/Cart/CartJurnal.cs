using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.Cart
{
    public class CartJurnal
    {
        private List<AkJurnal1> collection1 = new List<AkJurnal1>();
        public virtual void AddItem1(
            int AkJurnalId,
            int Indeks,
            int JBahagianId,
            int AkCartaId,
            decimal Debit,
            decimal Kredit
            )
        {
            AkJurnal1 line = collection1
            .Where(p => p.AkCartaId == AkCartaId && p.Indeks == Indeks)
            .FirstOrDefault();

            if (line == null)
            {
                collection1.Add(new AkJurnal1
                {
                    AkJurnalId = AkJurnalId,
                    Indeks = Indeks,
                    JBahagianId = JBahagianId,
                    AkCartaId = AkCartaId,
                    Debit = Debit,
                    Kredit = Kredit
                });
            }
        }

        public virtual void RemoveItem1(int AkCartaId, int IndeksLama) =>
            collection1.RemoveAll(l => l.AkCartaId == AkCartaId&& l.Indeks==IndeksLama);    
        //public virtual void RemoveItem1(int id) =>
        //    collection1.RemoveAll(l => l.AkCartaId == id);

        public virtual void Clear1() => collection1.Clear();

        public virtual IEnumerable<AkJurnal1> Lines1 => collection1;
    }
}

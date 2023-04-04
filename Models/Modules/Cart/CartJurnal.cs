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
            int JBahagianDebitId,
            int AkCartaDebitId,
            int JBahagianKreditId,
            int AkCartaKreditId,
            decimal Amaun
            )
        {
            AkJurnal1 line = collection1
            .Where(p => p.JBahagianDebitId == p.JBahagianDebitId 
                    && p.AkCartaDebitId == AkCartaDebitId
                    && p.JBahagianKreditId == JBahagianKreditId
                    && p.AkCartaKreditId == AkCartaKreditId 
                    && p.Indeks == Indeks)
            .FirstOrDefault();

            if (line == null)
            {
                collection1.Add(new AkJurnal1
                {
                    AkJurnalId = AkJurnalId,
                    Indeks = Indeks,
                    JBahagianDebitId = JBahagianDebitId,
                    AkCartaDebitId = AkCartaDebitId,
                    JBahagianKreditId = JBahagianKreditId,
                    AkCartaKreditId = AkCartaKreditId,
                    Amaun = Amaun
                });
            }
        }

        public virtual void RemoveItem1(int JBahagianDebitId, int AkCartaDebitId, int JBahagianKreditId, int AkCartaKreditId, int IndeksLama) =>
            collection1.RemoveAll(l => l.JBahagianDebitId == JBahagianDebitId && l.AkCartaDebitId == AkCartaDebitId 
                                    && l.JBahagianKreditId == JBahagianKreditId && l.AkCartaKreditId == AkCartaKreditId 
                                    && l.Indeks==IndeksLama);    
        //public virtual void RemoveItem1(int id) =>
        //    collection1.RemoveAll(l => l.AkCartaId == id);

        public virtual void Clear1() => collection1.Clear();

        public virtual IEnumerable<AkJurnal1> Lines1 => collection1;
    }
}

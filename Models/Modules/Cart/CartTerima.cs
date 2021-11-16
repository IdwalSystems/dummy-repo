using System;
using System.Collections.Generic;
using System.Linq;

namespace MSNK.Models.Modules.Cart
{
    public class CartTerima
    {
        //Terima 1
        
        private List<AkTerima1> collection1 = new List<AkTerima1>();

        public virtual void AddItem1(
            decimal amaun,
            AkCarta akCarta
            )
        {
            AkTerima1 line = collection1
            .Where(p => p.AkCarta.Id == akCarta.Id)
            .FirstOrDefault();

            if (line == null)
            {
                collection1.Add(new AkTerima1
                {
                    Amaun = amaun,
                    AkCarta = akCarta
                });
            }
        }

        public virtual void RemoveItem1(int id) =>
            collection1.RemoveAll(l => l.Id == id);


        public virtual void Clear1() => collection1.Clear();

        public virtual IEnumerable<AkTerima1> Lines1 => collection1;
        // Terima1 End

        //Terima 2
        private List<AkTerima2> collection2 = new List<AkTerima2>();

        public virtual void AddItem2(
            JCaraBayar jCaraBayar,
            decimal amaun, string noCek,
            string jenisCek, string kodBankCek,
            string tempatCek, string noSlip,
            DateTime tarSlip
            )
        {
            AkTerima2 line = collection2
            .Where(p => p.JCaraBayar == jCaraBayar)
            .FirstOrDefault();

            if (line == null)
            {
                collection2.Add(new AkTerima2
                {
                    JCaraBayar = jCaraBayar,
                    Amaun = amaun,
                    NoCek = noCek,
                    JenisCek = jenisCek,
                    KodBankCek = kodBankCek,
                    TempatCek = tempatCek,
                    NoSlip = noSlip,
                    TarSlip = tarSlip
                });
            }
        }

        public virtual void RemoveItem2(int id) =>
            collection2.RemoveAll(l => l.Id == id);


        public virtual void Clear2() => collection2.Clear();

        public virtual IEnumerable<AkTerima2> Lines2 => collection2;
    }
}

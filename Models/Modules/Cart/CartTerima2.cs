using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.Cart
{
    public class CartTerima2
    {

        //Terima 2
        private List<AkTerima2> collection2 = new List<AkTerima2>();

        public virtual void AddItem(
            int akTerimaId, int caraBayarId,
            decimal amaun, string noCek,
            string jenisCek, string kodBankCek,
            string tempatCek, string noSlip,
            DateTime tarSlip
            )
        {
            AkTerima2 line = new AkTerima2();

            if (line == null)
            {
                collection2.Add(new AkTerima2
                {
                    AkTerimaId = akTerimaId,
                    JCaraBayarId = caraBayarId,
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

        public virtual void RemoveItem(int id) =>
            collection2.RemoveAll(l => l.Id == id);


        public virtual void Clear() => collection2.Clear();

        public virtual IEnumerable<AkTerima2> Lines2 => collection2;
    }
}
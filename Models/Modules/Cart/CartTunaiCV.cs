using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.Cart
{
    public class CartTunaiCV
    {
        //TunaiCV1

        private List<AkTerima1> collection1 = new List<AkTerima1>();

        public virtual void AddItem1(
            int akTerimaId,
            decimal amaun,
            int akCartaId
            )
        {
            AkTerima1 line = collection1
            .Where(p => p.AkCartaId == akCartaId)
            .FirstOrDefault();

            if (line == null)
            {
                collection1.Add(new AkTerima1
                {
                    AkTerimaId = akTerimaId,
                    Amaun = amaun,
                    AkCartaId = akCartaId

                });
            }
        }

        public virtual void RemoveItem1(int id) =>
            collection1.RemoveAll(l => l.AkCartaId == id);


        public virtual void Clear1() => collection1.Clear();

        public virtual IEnumerable<AkTerima1> Lines1 => collection1;
        // TunaiCV1 End
    }
}

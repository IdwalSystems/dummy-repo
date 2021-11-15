using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.Cart
{
    public class CartTerima1
    {
        //Terima 1
        private List<AkTerima1> collection1 = new List<AkTerima1>();

        public virtual void AddItem(
            int akTerimaId, int akAkaunId,
            decimal amaun
            )
        {
            AkTerima1 line = collection1
            .Where(p => p.AkAkaun.Id == akAkaunId)
            .FirstOrDefault();

            if (line == null)
            {
                collection1.Add(new AkTerima1
                {
                    AkTerimaId = akTerimaId,
                    AkAkaunId = akAkaunId,
                    Amaun = amaun
                });
            }
        }

        public virtual void RemoveItem(int id) =>
            collection1.RemoveAll(l => l.Id == id);


        public virtual void Clear() => collection1.Clear();

        public virtual IEnumerable<AkTerima1> Lines1 => collection1;

    }
}

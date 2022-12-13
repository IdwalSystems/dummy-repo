using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.Cart
{
    public class CartWaran
    {
        //Waran 1

        private List<AbWaran1> collection1 = new List<AbWaran1>();

        public virtual void AddItem1(
            int abWaranId,
            decimal amaun,
            int akCartaId,
            int? jBahagianId,
            string tk
            )
        {
            AbWaran1 line = collection1
            .Where(p => p.AkCartaId == akCartaId && p.JBahagianId == jBahagianId)
            .FirstOrDefault();

            if (line == null)
            {
                collection1.Add(new AbWaran1
                {
                    AbWaranId = abWaranId,
                    Amaun = amaun,
                    AkCartaId = akCartaId,
                    JBahagianId = jBahagianId,
                    TK = tk
                });
            }
        }

        public virtual void RemoveItem1(int id, int id2) =>
            collection1.RemoveAll(l => l.AkCartaId == id && l.JBahagianId == id2);


        public virtual void Clear1() => collection1.Clear();

        public virtual IEnumerable<AbWaran1> Lines1 => collection1;
        // Waran 1 End
    }
}

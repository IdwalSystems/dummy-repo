using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.Cart
{
    public class CartPV
    {
        //Belian 1

        private List<AkPV1> collection1 = new List<AkPV1>();

        public virtual void AddItem1(
            int akPVId,
            decimal amaun,
            int akCartaId
            )
        {
            AkPV1 line = collection1
            .Where(p => p.AkCartaId == akCartaId)
            .FirstOrDefault();

            if (line == null)
            {
                collection1.Add(new AkPV1
                {
                    AkPVId = akPVId,
                    Amaun = amaun,
                    AkCartaId = akCartaId
                });
            }
        }

        public virtual void RemoveItem1(int id) =>
            collection1.RemoveAll(l => l.AkCartaId == id);


        public virtual void Clear1() => collection1.Clear();

        public virtual IEnumerable<AkPV1> Lines1 => collection1;
        // Belian1 End

        //Belian 2
        private List<AkPV2> collection2 = new List<AkPV2>();

        public virtual void AddItem2(
            int akPVId,
            int? akBelianId,
            decimal amaun,
            bool havePO
            )
        {
            AkPV2 line = collection2
            .Where(p => p.AkBelianId == akBelianId)
            .FirstOrDefault();

            collection2.Add(new AkPV2
            {
                AkPVId = akPVId,
                AkBelianId = akBelianId,
                Amaun = amaun,
                HavePO = havePO
            });
        }

        public virtual void RemoveItem2(int? id) =>
            collection2.RemoveAll(l => l.AkBelianId == id);


        public virtual void Clear2() => collection2.Clear();

        public virtual IEnumerable<AkPV2> Lines2 => collection2;
        // Belian 2 end
    }
}

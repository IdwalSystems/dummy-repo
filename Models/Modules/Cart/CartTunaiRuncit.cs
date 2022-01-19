using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.Cart
{
    public class CartTunaiRuncit
    {
        //Tunai Pemegang

        private List<AkTunaiPemegang> collection1 = new List<AkTunaiPemegang>();

        public virtual void AddItem1(
            int akTunaiRuncitId,
            int suPekerjaId
            )
        {
            AkTunaiPemegang line = collection1
            .Where(p => p.SuPekerjaId == suPekerjaId)
            .FirstOrDefault();

            if (line == null)
            {
                collection1.Add(new AkTunaiPemegang
                {
                    AkTunaiRuncitId = akTunaiRuncitId,
                    SuPekerjaId = suPekerjaId

                });
            }
        }

        public virtual void RemoveItem1(int id) =>
            collection1.RemoveAll(l => l.SuPekerjaId == id);


        public virtual void Clear1() => collection1.Clear();

        public virtual IEnumerable<AkTunaiPemegang> Lines1 => collection1;
        // Tunai Pemegang End
    }
}

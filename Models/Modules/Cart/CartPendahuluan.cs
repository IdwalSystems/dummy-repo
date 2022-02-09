using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.Cart
{
    public class CartPendahuluan
    {
        //SPPENDAHULUANPELBAGAI1

        private List<SpPendahuluanPelbagai1> collection1 = new List<SpPendahuluanPelbagai1>();

        public virtual void AddItem1(
            int SpPendahuluanPelbagaiId,
            int JJantinaId,
            int BilAtl,
            int BilJul,
            int BilPeg,
            int BilTek,
            int BilUru)
        {
            SpPendahuluanPelbagai1 line = collection1
            //.Where(p => p.AkCartaId == akCartaId)
            .FirstOrDefault();

            if (line == null)
            {
                collection1.Add(new SpPendahuluanPelbagai1
                {
                    SpPendahuluanPelbagaiId = SpPendahuluanPelbagaiId,
                    JJantinaId = JJantinaId,
                    BilAtl = BilAtl,
                    BilJul = BilJul,
                    BilPeg = BilPeg,
                    BilTek = BilTek,
                    BilUru = BilUru,
                });
            }
        }

        public virtual void RemoveItem1(int id) =>
            collection1.RemoveAll(l => l.JJantinaId == id);


        public virtual void Clear1() => collection1.Clear();

        public virtual IEnumerable<SpPendahuluanPelbagai1> Lines1 => collection1;
        //SPPENDAHULUANPELBAGAI1 END

        //SPPENDAHULUANPELBAGAI2
        //private List<SpPendahuluanPelbagai2> collection2 = new List<SpPendahuluanPelbagai2>();

        //public virtual void AddItem2(
        //    int akPOId,
        //    int Indek,
        //    int Baris,
        //    string Bil,
        //    string NoStok,
        //    string Perihal,
        //    decimal Kuantiti,
        //    string Unit,
        //    decimal Harga,
        //    decimal Amaun
        //    )
        //{

        //    {
        //        collection2.Add(new SpPendahuluanPelbagai2
        //        {
        //            AkPOId = akPOId,
        //            Indek = Indek,
        //            Baris = Baris,
        //            Bil = Bil,
        //            NoStok = NoStok,
        //            Perihal = Perihal,
        //            Kuantiti = Kuantiti,
        //            Unit = Unit,
        //            Harga = Harga,
        //            Amaun = Amaun
        //        });
        //    }
        //}

        //public virtual void RemoveItem2(int id) =>
        //    collection2.RemoveAll(l => l.Indek == id);


        //public virtual void Clear2() => collection2.Clear();

        //public virtual IEnumerable<SpPendahuluanPelbagai2> Lines2 => collection2;
        //SPPENDAHULUANPELBAGAI2 END
    }
}

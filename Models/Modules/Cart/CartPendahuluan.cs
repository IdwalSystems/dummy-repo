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
            int BilUru,
            int Jumlah)
        {
            SpPendahuluanPelbagai1 line = collection1
            .Where(p => p.JJantinaId == JJantinaId)
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
                    Jumlah = Jumlah
                });
            }
        }

        public virtual void RemoveItem1(int id) =>
            collection1.RemoveAll(l => l.JJantinaId == id);


        public virtual void Clear1() => collection1.Clear();

        public virtual IEnumerable<SpPendahuluanPelbagai1> Lines1 => collection1;
        //SPPENDAHULUANPELBAGAI1 END

        //SPPENDAHULUANPELBAGAI2
        private List<SpPendahuluanPelbagai2> collection2 = new List<SpPendahuluanPelbagai2>();

        public virtual void AddItem2(
            int SpPendahuluanPelbagaiId,
            int Indek,
            decimal Baris,
            string Perihal,
            decimal Kadar,
            decimal Bil,
            decimal Bulan,
            decimal Jumlah)
        {

            {
                collection2.Add(new SpPendahuluanPelbagai2
                {
                    SpPendahuluanPelbagaiId = SpPendahuluanPelbagaiId,
                    Indek = Indek,
                    Baris = Baris,
                    Perihal = Perihal,
                    Kadar = Kadar,
                    Bil = Bil,
                    Bulan = Bulan,
                    Jumlah = Jumlah
                });
            }
        }

        public virtual void RemoveItem2(int id) =>
            collection2.RemoveAll(l => l.Indek == id);


        public virtual void Clear2() => collection2.Clear();

        public virtual IEnumerable<SpPendahuluanPelbagai2> Lines2 => collection2;
        //SPPENDAHULUANPELBAGAI2 END
    }
}

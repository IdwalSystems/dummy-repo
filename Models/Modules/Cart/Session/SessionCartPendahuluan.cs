using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using MSNK.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.Cart.Session
{
    public class SessionCartPendahuluan : CartPendahuluan
    {
        public static CartPendahuluan GetCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?
                .HttpContext.Session;
            SessionCartPendahuluan cart = session?.GetJson<SessionCartPendahuluan>("CartPendahuluan") ??
                new SessionCartPendahuluan();
            cart.Session = session;
            return cart;
        }
        private ISession Session { get; set; }

        //SPPENDAHULUANPELBAGAI1
        public override void AddItem1(
                    int SpPendahuluanPelbagaiId,
                    int JJantinaId,
                    int BilAtl,
                    int BilJul,
                    int BilPeg,
                    int BilTek,
                    int BilUru)
        {
            base.AddItem1(SpPendahuluanPelbagaiId,
            JJantinaId,
            BilAtl,
            BilJul,
            BilPeg,
            BilTek,
            BilUru);

            Session.SetJson("CartPendahuluan", this);
        }
        public override void RemoveItem1(int id)
        {
            base.RemoveItem1(id);
            Session.SetJson("CartPendahuluan", this);
        }
        public override void Clear1()
        {
            base.Clear1();
            Session.Remove("CartPendahuluan");
        }
        //SPPENDAHULUANPELBAGAI1 END

        //SPPENDAHULUANPELBAGAI2
        //public override void AddItem2(
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
        //    base.AddItem2( akPOId,
        //            Indek,
        //            Baris,
        //            Bil,
        //            NoStok,
        //            Perihal,
        //            Kuantiti,
        //            Unit,
        //            Harga,
        //            Amaun);

        //    Session.SetJson("CartPendahuluan", this);
        //}
        //public override void RemoveItem2(int id)
        //{
        //    base.RemoveItem2(id);
        //    Session.SetJson("CartPendahuluan", this);
        //}
        //public override void Clear2()
        //{
        //    base.Clear2();
        //    Session.Remove("CartPendahuluan");
        //}
        //SPPENDAHULUANPELBAGAI2 END
    }
}


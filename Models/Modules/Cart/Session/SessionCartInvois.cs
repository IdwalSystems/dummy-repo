using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using MSNK.Infrastructure;
using System;

namespace MSNK.Models.Modules.Cart.Session
{
    public class SessionCartInvois : CartInvois
    {
        public static CartInvois GetCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?
                .HttpContext.Session;
            SessionCartInvois cart = session?.GetJson<SessionCartInvois>("CartInvois") ??
                new SessionCartInvois();
            cart.Session = session;
            return cart;
        }
        private ISession Session { get; set; }

        //Invois1
        public override void AddItem1(
            int akInvoisId,
            decimal amaun,
            int akCartaId
           )
        {
            base.AddItem1(akInvoisId,
                          amaun,
                          akCartaId);

            Session.SetJson("CartInvois", this);
        }



        public override void RemoveItem1(int id)
        {
            base.RemoveItem1(id);
            Session.SetJson("CartInvois", this);
        }
        public override void Clear1()
        {
            base.Clear1();
            Session.Remove("CartInvois");
        }
        //Belian1 End

        //Belian2
        public override void AddItem2(
            int akInvoisId,
            int Indek,
            int Baris,
            string Bil,
            string NoStok,
            string Perihal,
            decimal Kuantiti,
            string Unit,
            decimal Harga,
            decimal Amaun
            )
        {
            base.AddItem2(akInvoisId,
                    Indek,
                    Baris,
                    Bil,
                    NoStok,
                    Perihal,
                    Kuantiti,
                    Unit,
                    Harga,
                    Amaun);

            Session.SetJson("CartInvois", this);
        }
        public override void RemoveItem2(int id)
        {
            base.RemoveItem2(id);
            Session.SetJson("CartInvois", this);
        }
        public override void Clear2()
        {
            base.Clear2();
            Session.Remove("CartInvois");
        }
        //Belian2 End
    }
}

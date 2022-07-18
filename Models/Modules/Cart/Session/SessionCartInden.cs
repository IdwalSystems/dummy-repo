using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using MSNK.Infrastructure;
using System;

namespace MSNK.Models.Modules.Cart.Session
{
    public class SessionCartInden : CartInden
    {
        public static CartInden GetCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?
                .HttpContext.Session;
            SessionCartInden cart = session?.GetJson<SessionCartInden>("CartInden") ??
                new SessionCartInden();
            cart.Session = session;
            return cart;
        }
        private ISession Session { get; set; }

        //Inden1
        public override void AddItem1(
                int AkIndenId,
                int akCartaId,
                decimal Amaun
            )
        {
            base.AddItem1(AkIndenId,
                          akCartaId,
                          Amaun
                          );

            Session.SetJson("CartInden", this);
        }
        public override void RemoveItem1(int id)
        {
            base.RemoveItem1(id);
            Session.SetJson("CartInden", this);
        }
        public override void Clear1()
        {
            base.Clear1();
            Session.Remove("CartInden");
        }
        //Inden1 End

        //Inden2
        public override void AddItem2(
            int akIndenId,
            int Indek,
            decimal Bil,
            string NoStok,
            string Perihal,
            decimal Kuantiti,
            string Unit,
            decimal Harga,
            decimal Amaun
            )
        {
            base.AddItem2(akIndenId,
                    Indek,
                    Bil,
                    NoStok,
                    Perihal,
                    Kuantiti,
                    Unit,
                    Harga,
                    Amaun);

            Session.SetJson("CartInden", this);
        }
        public override void RemoveItem2(int id)
        {
            base.RemoveItem2(id);
            Session.SetJson("CartInden", this);
        }
        public override void Clear2()
        {
            base.Clear2();
            Session.Remove("CartInden");
        }
        //Inden2 End
    }
}

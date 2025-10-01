using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using MSNK.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.Cart.Session
{
    public class SessionCartIndenLaras : CartIndenLaras
    {
        public static CartIndenLaras GetCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?
                .HttpContext.Session;
            SessionCartIndenLaras cart = session?.GetJson<SessionCartIndenLaras>("CartIndenLaras") ??
                new SessionCartIndenLaras();
            cart.Session = session;
            return cart;
        }
        private ISession Session { get; set; }

        //IndenLaras1
        public override void AddItem1(
                int AkIndenLarasId,
                int akCartaId,
                decimal Amaun
            )
        {
            base.AddItem1(AkIndenLarasId,
                          akCartaId,
                          Amaun
                          );

            Session.SetJson("CartIndenLaras", this);
        }
        public override void RemoveItem1(int id)
        {
            base.RemoveItem1(id);
            Session.SetJson("CartIndenLaras", this);
        }
        public override void Clear1()
        {
            base.Clear1();
            Session.Remove("CartIndenLaras");
        }
        //IndenLaras1 End

        //IndenLaras2
        public override void AddItem2(
            int akIndenLarasId,
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
            base.AddItem2(akIndenLarasId,
                    Indek,
                    Bil,
                    NoStok,
                    Perihal,
                    Kuantiti,
                    Unit,
                    Harga,
                    Amaun);

            Session.SetJson("CartIndenLaras", this);
        }
        public override void RemoveItem2(int id)
        {
            base.RemoveItem2(id);
            Session.SetJson("CartIndenLaras", this);
        }
        public override void Clear2()
        {
            base.Clear2();
            Session.Remove("CartIndenLaras");
        }
        //IndenLaras2 End
    }
}

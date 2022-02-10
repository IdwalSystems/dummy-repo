using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using MSNK.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.Cart.Session
{
    public class SessionCartPOLaras : CartPOLaras
    {
        public static CartPOLaras GetCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?
                .HttpContext.Session;
            SessionCartPOLaras cart = session?.GetJson<SessionCartPOLaras>("CartPOLaras") ??
                new SessionCartPOLaras();
            cart.Session = session;
            return cart;
        }
        private ISession Session { get; set; }

        //POLaras1
        public override void AddItem1(
                int AkPOLarasId,
                int akCartaId,
                decimal Amaun
            )
        {
            base.AddItem1(AkPOLarasId,
                          akCartaId,
                          Amaun
                          );

            Session.SetJson("CartPOLaras", this);
        }
        public override void RemoveItem1(int id)
        {
            base.RemoveItem1(id);
            Session.SetJson("CartPOLaras", this);
        }
        public override void Clear1()
        {
            base.Clear1();
            Session.Remove("CartPOLaras");
        }
        //POLaras1 End

        //POLaras2
        public override void AddItem2(
            int akPOLarasId,
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
            base.AddItem2(akPOLarasId,
                    Indek,
                    Baris,
                    Bil,
                    NoStok,
                    Perihal,
                    Kuantiti,
                    Unit,
                    Harga,
                    Amaun);

            Session.SetJson("CartPOLaras", this);
        }
        public override void RemoveItem2(int id)
        {
            base.RemoveItem2(id);
            Session.SetJson("CartPOLaras", this);
        }
        public override void Clear2()
        {
            base.Clear2();
            Session.Remove("CartPOLaras");
        }
        //POLaras2 End
    }
}

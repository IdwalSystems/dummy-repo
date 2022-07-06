using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using MSNK.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.Cart.Session
{
    public class SessionCartNotaMinta : CartNotaMinta
    {
        public static CartNotaMinta GetCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?
                .HttpContext.Session;
            SessionCartNotaMinta cart = session?.GetJson<SessionCartNotaMinta>("CartNotaMinta") ??
                new SessionCartNotaMinta();
            cart.Session = session;
            return cart;
        }
        private ISession Session { get; set; }

        //Terima1
        public override void AddItem1(
                int AkNotaMintaId,
                int akCartaId,
                decimal Amaun
            )
        {
            base.AddItem1(AkNotaMintaId,
                          akCartaId,
                          Amaun
                          );

            Session.SetJson("CartNotaMinta", this);
        }
        public override void RemoveItem1(int id)
        {
            base.RemoveItem1(id);
            Session.SetJson("CartNotaMinta", this);
        }
        public override void Clear1()
        {
            base.Clear1();
            Session.Remove("CartNotaMinta");
        }
        //Terima1 End

        //Terima2
        public override void AddItem2(
            int akNotaMintaId,
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
            base.AddItem2(akNotaMintaId,
                    Indek,
                    Bil,
                    NoStok,
                    Perihal,
                    Kuantiti,
                    Unit,
                    Harga,
                    Amaun);

            Session.SetJson("CartNotaMinta", this);
        }
        public override void RemoveItem2(int id)
        {
            base.RemoveItem2(id);
            Session.SetJson("CartNotaMinta", this);
        }
        public override void Clear2()
        {
            base.Clear2();
            Session.Remove("CartNotaMinta");
        }
        //Terima2 End
    }
}

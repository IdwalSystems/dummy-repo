using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using MSNK.Infrastructure;
using System;

namespace MSNK.Models.Modules.Cart.Session
{
    public class SessionCartAtlet : CartAtlet
    {
        public static CartAtlet GetCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?
                .HttpContext.Session;
            SessionCartAtlet cart = session?.GetJson<SessionCartAtlet>("CartAtlet") ??
                new SessionCartAtlet();
            cart.Session = session;
            return cart;
        }
        private ISession Session { get; set; }

        //Atlet
        public override void AddItem1(
            int suProfilId,
            decimal amaun,
            decimal amaunsebelum,
            decimal tunggakan,
            decimal jumlah
           )
        {
            base.AddItem1(suProfilId,
            amaun,
            amaunsebelum,
            tunggakan,
            jumlah
            );

            Session.SetJson("CartAtlet", this);
        }

        public override void RemoveItem1(int id)
        {
            base.RemoveItem1(id);
            Session.SetJson("CartAtlet", this);
        }
        public override void Clear1()
        {
            base.Clear1();
            Session.Remove("CartAtlet");
        }
        //Atlet End
    }
}

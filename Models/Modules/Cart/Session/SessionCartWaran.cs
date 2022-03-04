using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using MSNK.Infrastructure;
using System;

namespace MSNK.Models.Modules.Cart.Session
{
    public class SessionCartWaran : CartWaran
    {
        public static CartWaran GetCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?
                .HttpContext.Session;
            SessionCartWaran cart = session?.GetJson<SessionCartWaran>("CartWaran") ??
                new SessionCartWaran();
            cart.Session = session;
            return cart;
        }
        private ISession Session { get; set; }

        //Belian1
        public override void AddItem1(
            int abWaranId,
            decimal amaun,
            int akCartaId,
            string tk
           )
        {
            base.AddItem1(abWaranId,
                          amaun,
                          akCartaId,
                          tk);

            Session.SetJson("CartWaran", this);
        }

        public override void RemoveItem1(int id)
        {
            base.RemoveItem1(id);
            Session.SetJson("CartWaran", this);
        }
        public override void Clear1()
        {
            base.Clear1();
            Session.Remove("CartWaran");
        }
        //Belian1 End
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using MSNK.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.Cart.Session
{
    public class SessionCartTunaiCV : CartTunaiCV
    {
        public static CartTunaiCV GetCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?
                .HttpContext.Session;
            SessionCartTunaiCV cart = session?.GetJson<SessionCartTunaiCV>("CartTunaiCV") ??
                new SessionCartTunaiCV();
            cart.Session = session;
            return cart;
        }
        private ISession Session { get; set; }

        //Terima1
        public override void AddItem1(
            int akTunaiCVId,
            decimal amaun,
            int akCartaId
           )
        {
            base.AddItem1(akTunaiCVId,
                          amaun,
                          akCartaId);

            Session.SetJson("CartTunaiCV", this);
        }
        public override void RemoveItem1(int id)
        {
            base.RemoveItem1(id);
            Session.SetJson("CartTunaiCV", this);
        }
        public override void Clear1()
        {
            base.Clear1();
            Session.Remove("CartTunaiCV");
        }
        //Terima1 End
    }
}

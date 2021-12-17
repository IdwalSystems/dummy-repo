using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using MSNK.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.Cart.Session
{
    public class SessionCartBelian : CartBelian
    {
        public static CartBelian GetCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?
                .HttpContext.Session;
            SessionCartBelian cart = session?.GetJson<SessionCartBelian>("CartBelian") ??
                new SessionCartBelian();
            cart.Session = session;
            return cart;
        }
        private ISession Session { get; set; }

        //Belian1
        public override void AddItem1(
            int akBelianId,
            decimal amaun,
            int akCartaId
           )
        {
            base.AddItem1(akBelianId, amaun, akCartaId);
            Session.SetJson("CartBelian", this);
        }
        public override void RemoveItem1(int id)
        {
            base.RemoveItem1(id);
            Session.SetJson("CartBelian", this);
        }
        public override void Clear1()
        {
            base.Clear1();
            Session.Remove("CartBelian");
        }
        //Belian1 End
    }
}

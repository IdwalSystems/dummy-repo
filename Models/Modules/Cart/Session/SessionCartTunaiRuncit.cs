using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using MSNK.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.Cart.Session
{
    public class SessionCartTunaiRuncit : CartTunaiRuncit
    {
        public static CartTunaiRuncit GetCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?
                .HttpContext.Session;
            SessionCartTunaiRuncit cart = session?.GetJson<SessionCartTunaiRuncit>("CartTunaiRuncit") ??
                new SessionCartTunaiRuncit();
            cart.Session = session;
            return cart;
        }
        private ISession Session { get; set; }

        //Terima1
        public override void AddItem1(
            int akTunaiRuncitId,
            int suPekerjaId
           )
        {
            base.AddItem1(akTunaiRuncitId,
                          suPekerjaId);

            Session.SetJson("CartTunaiRuncit", this);
        }
        public override void RemoveItem1(int id)
        {
            base.RemoveItem1(id);
            Session.SetJson("CartTunaiRuncit", this);
        }
        public override void Clear1()
        {
            base.Clear1();
            Session.Remove("CartTunaiRuncit");
        }
        //Terima1 End
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using MSNK.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.Cart.Session
{
    public class SessionCartPV : CartPV
    {
        public static CartPV GetCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?
                .HttpContext.Session;
            SessionCartPV cart = session?.GetJson<SessionCartPV>("CartPV") ??
                new SessionCartPV();
            cart.Session = session;
            return cart;
        }
        private ISession Session { get; set; }

        //Belian1
        public override void AddItem1(
            int akPVId,
            decimal amaun,
            int akCartaId
           )
        {
            base.AddItem1(akPVId,
                          amaun,
                          akCartaId);

            Session.SetJson("CartPV", this);
        }



        public override void RemoveItem1(int id)
        {
            base.RemoveItem1(id);
            Session.SetJson("CartPV", this);
        }
        public override void Clear1()
        {
            base.Clear1();
            Session.Remove("CartPV");
        }
        //Belian1 End

        //Belian2
        public override void AddItem2(
            int akPVId,
            int? akBelianId,
            decimal amaun,
            bool havePO
            )
        {
            base.AddItem2(
                    akPVId,
                    akBelianId,
                    amaun,
                    havePO);

            Session.SetJson("CartPV", this);
        }
        public override void RemoveItem2(int? id)
        {
            base.RemoveItem2(id);
            Session.SetJson("CartPV", this);
        }
        public override void Clear2()
        {
            base.Clear2();
            Session.Remove("CartPV");
        }
        //Belian2 End
    }
}

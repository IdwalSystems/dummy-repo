using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using MSNK.Infrastructure;
using System;

namespace MSNK.Models.Modules.Cart.Session
{
    public class SessionCartJurulatih : CartJurulatih
    {
        public static CartJurulatih GetCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?
                .HttpContext.Session;
            SessionCartJurulatih cart = session?.GetJson<SessionCartJurulatih>("CartJurulatih") ??
                new SessionCartJurulatih();
            cart.Session = session;
            return cart;
        }
        private ISession Session { get; set; }

        //Atlet
        public override void AddItem1(
            int suProfilId,
            int? suJurulatihId,
            int jSukanId,
            decimal amaun,
            decimal amaunsebelum,
            decimal tunggakan,
            decimal jumlah
           )
        {
            base.AddItem1(suProfilId,
            suJurulatihId,
            jSukanId,
            amaun,
            amaunsebelum,
            tunggakan,
            jumlah
            );

            Session.SetJson("CartJurulatih", this);
        }

        public override void RemoveItem1(int? id)
        {
            base.RemoveItem1(id);
            Session.SetJson("CartJurulatih", this);
        }
        public override void Clear1()
        {
            base.Clear1();
            Session.Remove("CartJurulatih");
        }
        //Atlet End
    }
}

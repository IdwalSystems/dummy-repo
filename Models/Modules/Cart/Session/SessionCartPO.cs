using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using MSNK.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.Cart.Session
{
    public class SessionCartPO : CartPO
    {
        public static CartPO GetCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?
                .HttpContext.Session;
            SessionCartPO cart = session?.GetJson<SessionCartPO>("CartPO") ??
                new SessionCartPO();
            cart.Session = session;
            return cart;
        }
        private ISession Session { get; set; }

        //Terima1
        public override void AddItem1(
                int AkPOId,
                int akCartaId,
                decimal Amaun
            )
        {
            base.AddItem1(AkPOId, 
                          akCartaId, 
                          Amaun
                          );

            Session.SetJson("CartPO", this);
        }
        public override void RemoveItem1(int id)
        {
            base.RemoveItem1(id);
            Session.SetJson("CartPO", this);
        }
        public override void Clear1()
        {
            base.Clear1();
            Session.Remove("CartPO");
        }
        //Terima1 End

        //Terima2
        public override void AddItem2(
            int akPOId,
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
            base.AddItem2( akPOId,
                    Indek,
                    Baris,
                    Bil,
                    NoStok,
                    Perihal,
                    Kuantiti,
                    Unit,
                    Harga,
                    Amaun);

            Session.SetJson("CartPO", this);
        }
        public override void RemoveItem2(int id)
        {
            base.RemoveItem2(id);
            Session.SetJson("CartPO", this);
        }
        public override void Clear2()
        {
            base.Clear2();
            Session.Remove("CartPO");
        }
        //Terima2 End
    }
}


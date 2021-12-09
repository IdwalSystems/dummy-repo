using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using MSNK.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.Cart.Session
{
    public class SessionCartJurnal : CartJurnal
    {
        public static CartJurnal GetCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;
            SessionCartJurnal cart = session?.GetJson<SessionCartJurnal>("CartJurnal") ?? new SessionCartJurnal();
            cart.Session = session;
            return cart;
        }
        private ISession Session { get; set; }

        public override void AddItem1(
            int AkJurnalId,
            string NoRujukan,
            int Indeks,
            int AkCartaId,
            decimal Debit,
            decimal Kredit
           )
        {
            base.AddItem1(AkJurnalId, NoRujukan, Indeks, AkCartaId, Debit, Kredit);
            Session.SetJson("CartJurnal", this);
        }
        public override void RemoveItem1(int id)
        {
            base.RemoveItem1(id);
            Session.SetJson("CartJurnal", this);
        }
        public override void Clear1()
        {
            base.Clear1();
            Session.Remove("CartJurnal");
        }
    }
}

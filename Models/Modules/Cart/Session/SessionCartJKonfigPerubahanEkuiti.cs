using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using MSNK.Infrastructure;
using MSNK.Models.Operations;
using System;

namespace MSNK.Models.Modules.Cart.Session
{
    public class SessionCartJKonfigPerubahanEkuiti : CartJKonfigPerubahanEkuiti
    {
        public static CartJKonfigPerubahanEkuiti GetCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session!;

            SessionCartJKonfigPerubahanEkuiti cart = session?.GetJson<SessionCartJKonfigPerubahanEkuiti>("CartJKonfigPerubahanEkuiti") ?? new SessionCartJKonfigPerubahanEkuiti();

            cart.Session = session;
            return cart;
        }
        private ISession Session { get; set; }

        //KonfigPerubahanEkuitiBaris
        public override void AddItemBaris(int jKonfigPerubahanEkuitiId, EnBarisPerubahanEkuiti enBaris, EnJenisOperasi enJenisOperasi, bool isPukal, string enJenisCartaList, bool isKecuali, string kodList, string setKodList)
        {
            base.AddItemBaris(jKonfigPerubahanEkuitiId, enBaris, enJenisOperasi, isPukal, enJenisCartaList, isKecuali, kodList, setKodList);
            Session?.SetJson("CartJKonfigPerubahanEkuiti", this);
        }

        public override void RemoveItemBaris(EnBarisPerubahanEkuiti enBaris, EnJenisOperasi enJenisOperasi)
        {
            base.RemoveItemBaris(enBaris, enJenisOperasi);
            Session?.SetJson("CartJKonfigPerubahanEkuiti", this);

        }

        public override void ClearBaris()
        {
            base.ClearBaris();
            Session?.Remove("CartJKonfigPerubahanEkuiti");

        }
    }
}

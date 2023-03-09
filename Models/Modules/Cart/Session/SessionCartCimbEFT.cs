using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using MSNK.Infrastructure;
using MSNK.Models.Operations;
using System;

namespace MSNK.Models.Modules.Cart.Session
{
    public class SessionCartCimbEFT : CartCimbEFT
    {
        public static CartCimbEFT GetCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?
                .HttpContext.Session;
            SessionCartCimbEFT cart = session?.GetJson<SessionCartCimbEFT>("CartCimbEFT") ??
                new SessionCartCimbEFT();
            cart.Session = session;
            return cart;
        }
        private ISession Session { get; set; }

        //CimbEFT1
        public override void AddItem1(
            int akCimbEFTId,
            int Indek,
            int akPVId,
            KategoriPenerima flPenerimaEFT,
            int? akPembekalId,
            int? suPekerjaId,
            int? suAtletId,
            int? suJurulatihId,
            decimal amaun,
            string noCek,
            string catatan,
            int? jBankId,
            int? flStatus
            )
        {
            base.AddItem1(akCimbEFTId,
                    Indek,
                    akPVId,
                    flPenerimaEFT,
                    akPembekalId,
                    suPekerjaId,
                    suAtletId,
                    suJurulatihId,
                    amaun,
                    noCek,
                    catatan,
                    jBankId,
                    flStatus);

            Session.SetJson("CartCimbEFT", this);
        }
        public override void RemoveItem1(int id)
        {
            base.RemoveItem1(id);
            Session.SetJson("CartCimbEFT", this);
        }
        public override void Clear1()
        {
            base.Clear1();
            Session.Remove("CartCimbEFT");
        }
        //CimbEFT1 End
    }
}

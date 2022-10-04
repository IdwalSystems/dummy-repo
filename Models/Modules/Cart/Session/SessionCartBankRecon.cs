using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using MSNK.Infrastructure;
using System;

namespace MSNK.Models.Modules.Cart.Session
{
    public class SessionCartBankRecon : CartBankRecon
    {
        public static CartBankRecon GetCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?
                .HttpContext.Session;
            SessionCartBankRecon cart = session?.GetJson<SessionCartBankRecon>("CartBankRecon") ??
                new SessionCartBankRecon();
            cart.Session = session;
            return cart;
        }
        private ISession Session { get; set; }
        // BankRecon
        public override void AddItem1(
            int id,
            int indek,
            int akBankReconId,
            string noAkaunBank,
            DateTime tarikh,
            string kodTransaksi,
            string perihalTransaksi,
            string noDokumen,
            decimal debit,
            decimal kredit,
            decimal baki,
            bool isPadan
            )
        {
            base.AddItem1(id,
                indek,
                akBankReconId,
                noAkaunBank,
                tarikh,
                kodTransaksi,
                perihalTransaksi,
                noDokumen,
                debit,
                kredit,
                baki,
                isPadan);

            Session.SetJson("CartBankRecon", this);
        }
        public override void RemoveItem1(int id)
        {
            base.RemoveItem1(id);
            Session.SetJson("CartBankRecon", this);
        }
        public override void Clear1()
        {
            base.Clear1();
            Session.Remove("CartBankRecon");
        }
        // BankRecon End
    }
}

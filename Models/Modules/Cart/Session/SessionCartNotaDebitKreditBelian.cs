using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using MSNK.Infrastructure;
using System;

namespace MSNK.Models.Modules.Cart.Session
{
    public class SessionCartNotaDebitKreditBelian : CartNotaDebitKreditBelian
    {
        public static CartNotaDebitKreditBelian GetCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?
                .HttpContext.Session;
            SessionCartNotaDebitKreditBelian cart = session?.GetJson<SessionCartNotaDebitKreditBelian>("CartNotaDebitKreditBelian") ??
                new SessionCartNotaDebitKreditBelian();
            cart.Session = session;
            return cart;
        }

        private ISession Session { get; set; }

        //Nota Debit Kredit 1
        public override void AddItem1(
            int akNotaDebitKreditBelianId, 
            decimal amaun, 
            int akCartaId)
        {
            base.AddItem1(
                akNotaDebitKreditBelianId, 
                amaun, 
                akCartaId);

            Session.SetJson("CartNotaDebitKreditBelian", this);
        }

        public override void RemoveItem1(int id)
        {
            base.RemoveItem1(id);
            Session.SetJson("CartNotaDebitKreditBelian", this);
        }
        public override void Clear1()
        {
            base.Clear1();
            Session.Remove("CartNotaDebitKreditBelian");
        }
        //Nota Debit Kredit 1 End

        //Nota Debit Kredit 2
        public override void AddItem2(
            int akNotaDebitKreditBelianId, 
            int Indek, 
            decimal Bil,
            string NoStok, 
            string Perihal, 
            decimal Kuantiti, 
            string Unit, 
            decimal Harga, 
            decimal Amaun)
        {
            base.AddItem2(
                akNotaDebitKreditBelianId, 
                Indek, 
                Bil, 
                NoStok, 
                Perihal, 
                Kuantiti, 
                Unit, 
                Harga, 
                Amaun);

            Session.SetJson("CartNotaDebitKreditBelian", this);
        }
        

        public override void RemoveItem2(int id)
        {
            base.RemoveItem2(id);
            Session.SetJson("CartNotaDebitKreditBelian", this);
        }

        public override void Clear2()
        {
            base.Clear2();
            Session.Remove("CartNotaDebitKreditBelian");
        }
        //Nota Debit Kredit 2 End
    }
}

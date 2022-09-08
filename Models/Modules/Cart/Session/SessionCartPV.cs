using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using MSNK.Infrastructure;
using System;

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

        // PV 1
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
        // PV 1 End

        // PV 2
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
        // PV 2 End

        // PV Ganda
        public override void AddItemGanda(
            int akPVId, 
            int indek, 
            int flKategoriPenerima, 
            int? suPekerjaId, 
            int? suAtletId, 
            int? suJurulatihId, 
            string nama, 
            string noKP, 
            string noAkaun, 
            int? jBankId, 
            JBank jBank,
            decimal amaun, 
            string noCekAtauEFT, 
            DateTime? tarCekAtauEFT, 
            int? jCaraBayarId,
            JCaraBayar jCaraBayar)
        {
            base.AddItemGanda(
                akPVId, 
                indek, 
                flKategoriPenerima, 
                suPekerjaId, 
                suAtletId, 
                suJurulatihId, 
                nama, 
                noKP, 
                noAkaun, 
                jBankId, 
                jBank,
                amaun, 
                noCekAtauEFT, 
                tarCekAtauEFT, 
                jCaraBayarId,
                jCaraBayar);

            Session.SetJson("CartPV", this);
        }
        public override void RemoveItemGanda(int id)
        {
            base.RemoveItemGanda(id);
            Session.SetJson("CartPV", this);
        }
        public override void ClearGanda()
        {
            base.ClearGanda();
            Session.Remove("CartPV");
        }
        // PV 2 End
    }
}

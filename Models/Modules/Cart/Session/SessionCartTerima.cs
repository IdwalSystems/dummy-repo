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
    public class SessionCartTerima : CartTerima
    {
        public static CartTerima GetCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?
                .HttpContext.Session;
            SessionCartTerima cart = session?.GetJson<SessionCartTerima>("CartTerima") ??
                new SessionCartTerima();
            cart.Session = session;
            return cart;
        }
        private ISession Session { get; set; }

        //Terima1
        public override void AddItem1(
            int akTerimaId,
            decimal amaun,
            int akCartaId
           )
        {
            base.AddItem1(akTerimaId,
                          amaun,
                          akCartaId);

            Session.SetJson("CartTerima", this);
        }
        public override void RemoveItem1(int id)
        {
            base.RemoveItem1(id);
            Session.SetJson("CartTerima", this);
        }
        public override void Clear1()
        {
            base.Clear1();
            Session.Remove("CartTerima");
        }
        //Terima1 End
        //Terima2
        public override void AddItem2(
            int akTerimaId,
            int jCaraBayarId,
            decimal amaun, string noCek,
            int jenisCek, string kodBankCek,
            string tempatCek, string noSlip,
            DateTime? tarSlip,
            int? AkPenyataPemungutId
            )
        {
            base.AddItem2(akTerimaId,
                          jCaraBayarId,
                          amaun,
                          noCek,
                          jenisCek,
                          kodBankCek,
                          tempatCek,
                          noSlip,
                          tarSlip,
                          AkPenyataPemungutId);

            Session.SetJson("CartTerima", this);
        }
        public override void RemoveItem2(int id)
        {
            base.RemoveItem2(id);
            Session.SetJson("CartTerima", this);
        }
        public override void Clear2()
        {
            base.Clear2();
            Session.Remove("CartTerima");
        }
        //Terima2 End

        //Terima3
        public override void AddItem3(
            int akTerimaId,
            int? akInvoisId,
            decimal amaun
           )
        {
            base.AddItem3(akTerimaId,
                          akInvoisId,
                          amaun);

            Session.SetJson("CartTerima", this);
        }
        public override void RemoveItem3(int id)
        {
            base.RemoveItem3(id);
            Session.SetJson("CartTerima", this);
        }
        public override void Clear3()
        {
            base.Clear3();
            Session.Remove("CartTerima");
        }
        //Terima3 End
    }
}

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
            int akCartaId,
            string userId,
            DateTime tarMasuk,
            string userIdKemasikini,
            DateTime tarKemaskini
           )
        {
            base.AddItem1(akTerimaId,
                          amaun,
                          akCartaId,
                          userId,
                          tarMasuk,
                          userIdKemasikini,
                          tarKemaskini);
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
            string jenisCek, string kodBankCek,
            string tempatCek, string noSlip,
            DateTime tarSlip,
            string userId,
            DateTime tarMasuk,
            string userIdKemasikini,
            DateTime tarKemaskini
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
                          userId, 
                          tarMasuk, 
                          userIdKemasikini, 
                          tarKemaskini);

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
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using MSNK.Infrastructure;
using MSNK.Models.Operations;
using System;
using System.Collections.Generic;

namespace MSNK.Models.Modules.Cart.Session
{
    public class SessionCartJKonfigPenyata : CartJKonfigPenyata
    {
        public static CartJKonfigPenyata GetCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session!;

            SessionCartJKonfigPenyata cart = session?.GetJson<SessionCartJKonfigPenyata>("CartJKonfigPenyata") ?? new SessionCartJKonfigPenyata();

            cart.Session = session;
            return cart;
        }

        private ISession Session { get; set; }

        // Baris
        public override void AddItemBaris(int id, int bil, int jKonfigPenyataId, EnKategoriTajuk enKategoriTajuk, string perihal, int susunan, bool isFormula, EnKategoriJumlah enKategoriJumlah, string jumlahSusunanList, List<JKonfigPenyataBarisFormula> jKonfigPenyataBarisFormulas)
        {
            base.AddItemBaris(id, bil, jKonfigPenyataId, enKategoriTajuk, perihal, susunan, isFormula, enKategoriJumlah, jumlahSusunanList, jKonfigPenyataBarisFormulas);
            Session?.SetJson("CartJKonfigPenyata", this);
        }

        public override void RemoveItemBaris(int bil)
        {
            base.RemoveItemBaris(bil);
            Session?.SetJson("CartJKonfigPenyata", this);

        }

        public override void ClearBaris()
        {
            base.ClearBaris();
            Session?.Remove("CartJKonfigPenyata");
        }
        // Baris end

        // Baris Formula
        public override void AddItemBarisFormula(int id, int barisBil, int jKonfigPenyataBarisId, EnJenisOperasi enJenisOperasi, bool isPukal, string enJenisCartaList, bool isKecuali, string kodList, string setKodList, decimal amaunTetap, bool isLastYear, bool isUntilYear)
        {
            base.AddItemBarisFormula(id, barisBil, jKonfigPenyataBarisId, enJenisOperasi, isPukal, enJenisCartaList, isKecuali, kodList, setKodList, amaunTetap, isLastYear, isUntilYear);
            Session?.SetJson("CartJKonfigPenyata", this);
        }

        public override void RemoveItemBarisFormula(int id, int barisBil)
        {
            base.RemoveItemBarisFormula(id, barisBil);
            Session?.SetJson("CartJKonfigPenyata", this);
        }

        public override void ClearBarisFormulaByBarisBil()
        {
            base.ClearBarisFormulaByBarisBil();
            Session?.SetJson("CartJKonfigPenyata", this);

        }
        public override void ClearBarisFormula()
        {
            base.ClearBarisFormula();
            Session?.Remove("CartJKonfigPenyata");

        }
        // Baris Formula end
    }
}

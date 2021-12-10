using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MSNK.Data
{
    public static class ClaimStore
    {
        public static List<Claim> claimsList = new List<Claim>()
        {
            new Claim("PR001","PR001 Penerimaan"),
            new Claim("PR001C","PR001 Penerimaan - Tambah"),
            new Claim("PR001E","PR001 Penerimaan - Ubah"),
            new Claim("PR001D","PR001 Penerimaan - Hapus"),
            new Claim("PR001C","PR001 Penerimaan - Cetak"),
            new Claim("PR001B","PR001 Penerimaan - Batal"),
            new Claim("PR001T","PR001 Penerimaan - Posting"),
            new Claim("PR001UT","PR001 Penerimaan - UnPosting")
            //new Claim("Delete","Hapus")
        };
    }
}

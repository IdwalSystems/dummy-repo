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
            //Menu Terimaan
            //Resit Rasmi
            new Claim("PR001","PR001 Penerimaan"),
            new Claim("PR001C","PR001 Penerimaan - Tambah"),
            new Claim("PR001E","PR001 Penerimaan - Ubah"),
            new Claim("PR001D","PR001 Penerimaan - Hapus"),
            new Claim("PR001P","PR001 Penerimaan - Cetak"),
            new Claim("PR001B","PR001 Penerimaan - Batal"),
            new Claim("PR001T","PR001 Penerimaan - Posting"),
            new Claim("PR001UT","PR001 Penerimaan - UnPosting"),
            //Resit Rasmi end
            //Menu Tanggungan
            //Pesanan Tempatan
            new Claim("TG001","TG001 Pesanan Tempatan"),
            new Claim("TG001C","TG001 Pesanan Tempatan - Tambah"),
            new Claim("TG001E","TG001 Pesanan Tempatan - Ubah"),
            new Claim("TG001D","TG001 Pesanan Tempatan - Hapus"),
            new Claim("TG001P","TG001 Pesanan Tempatan - Cetak"),
            new Claim("TG001B","TG001 Pesanan Tempatan - Batal"),
            new Claim("TG001T","TG001 Pesanan Tempatan - Posting"),
            new Claim("TG001UT","TG001 Pesanan Tempatan - UnPosting"),
            //Pesanan Tempatan end
            //Invois Pembekal
            new Claim("TG002","TG002 Invois Pembekal"),
            new Claim("TG002C","TG002 Invois Pembekal - Tambah"),
            new Claim("TG002E","TG002 Invois Pembekal - Ubah"),
            new Claim("TG002D","TG002 Invois Pembekal - Hapus"),
            //new Claim("TG002P","TG002 Invois Pembekal - Cetak"),
            new Claim("TG002B","TG002 Invois Pembekal - Batal"),
            new Claim("TG002T","TG002 Invois Pembekal - Posting"),
            new Claim("TG002UT","TG002 Invois Pembekal - UnPosting"),
            //Invois Pembekal end
            //Menu Baucer
            //Baucer Pembayaran
            new Claim("PV001","PV001 Baucer Pembayaran"),
            new Claim("PV001C","PV001 Baucer Pembayaran - Tambah"),
            new Claim("PV001E","PV001 Baucer Pembayaran - Ubah"),
            new Claim("PV001D","PV001 Baucer Pembayaran - Hapus"),
            new Claim("PV001P","PV001 Baucer Pembayaran - Cetak"),
            new Claim("PV001B","PV001 Baucer Pembayaran - Batal"),
            new Claim("PV001T","PV001 Baucer Pembayaran - Posting"),
            new Claim("PV001UT","PV001 Baucer Pembayaran - UnPosting"),
            //Baucer Pembayaran end
            //Baucer Jurnal
            new Claim("JU001","JU001 Baucer Jurnal"),
            new Claim("JU001C","JU001 Baucer Jurnal - Tambah"),
            new Claim("JU001E","JU001 Baucer Jurnal - Ubah"),
            new Claim("JU001D","JU001 Baucer Jurnal - Hapus"),
            new Claim("JU001P","JU001 Baucer Jurnal - Cetak"),
            new Claim("JU001B","JU001 Baucer Jurnal - Batal"),
            new Claim("JU001T","JU001 Baucer Jurnal - Posting"),
            new Claim("JU001UT","JU001 Baucer Jurnal - UnPosting"),
            //Baucer Jurnal end
            //Menu Tunai Runcit
            //Pemegang Tunai Runcit
            new Claim("TR001","TR001 Pemegang Tunai Runcit"),
            new Claim("TR001C","TR001 Pemegang Tunai Runcit - Tambah"),
            new Claim("TR001E","TR001 Pemegang Tunai Runcit - Ubah"),
            new Claim("TR001D","TR001 Pemegang Tunai Runcit - Hapus"),
            new Claim("TR001P","TR001 Pemegang Tunai Runcit - Cetak"),
            //new Claim("TR001B","TR001 Pemegang Tunai Runcit - Batal"),
            //new Claim("TR001T","TR001 Pemegang Tunai Runcit - Posting"),
            //new Claim("TR001UT","TR001 Pemegang Tunai Runcit - UnPosting"),
            //Pemegang Tunai Runcit end
            //Tunai Keluar
            new Claim("TR002","TR002 Tunai Keluar"),
            new Claim("TR002C","TR002 Tunai Keluar - Tambah"),
            new Claim("TR002E","TR002 Tunai Keluar - Ubah"),
            new Claim("TR002D","TR002 Tunai Keluar - Hapus"),
            new Claim("TR002P","TR002 Tunai Keluar - Cetak"),
            new Claim("TR002B","TR002 Tunai Keluar - Batal"),
            new Claim("TR002T","TR002 Tunai Keluar - Posting"),
            new Claim("TR002UT","TR002 Tunai Keluar - UnPosting"),
            //Tunai Keluar end
            //Nota Minta
            new Claim("NM001","NM001 Nota Minta"),
            new Claim("NM001C","NM001 Nota Minta - Tambah"),
            new Claim("NM001E","NM001 Nota Minta - Ubah"),
            new Claim("NM001E1","NM001 Nota Minta - Ubah Bahagian Kewangan"),
            new Claim("NM001D","NM001 Nota Minta - Hapus"),
            new Claim("NM001P","NM001 Nota Minta - Cetak"),
            new Claim("NM001B","NM001 Nota Minta - Batal"),
            new Claim("NM001T","NM001 Nota Minta - Posting"),
            new Claim("NM001UT","NM001 Baucer Jurnal - UnPosting"),
            //Nota Minta end
        };
    }
}

using System.Collections.Generic;
using System.Security.Claims;

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
            new Claim("PR001R","PR001 Penerimaan - Rollback"),
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
            new Claim("TG001R","TG001 Pesanan Tempatan - Rollback"),
            new Claim("TG001T","TG001 Pesanan Tempatan - Posting"),
            new Claim("TG001UT","TG001 Pesanan Tempatan - UnPosting"),
            //Pesanan Tempatan end
            //Pelarasan Tanggungan
            new Claim("PT001","PT001 Pelarasan Tanggungan"),
            new Claim("PT001C","PT001 Pelarasan Tanggungan - Tambah"),
            new Claim("PT001E","PT001 Pelarasan Tanggungan - Ubah"),
            new Claim("PT001D","PT001 Pelarasan Tanggungan - Hapus"),
            new Claim("PT001P","PT001 Pelarasan Tanggungan - Cetak"),
            new Claim("PT001B","PT001 Pelarasan Tanggungan - Batal"),
            new Claim("PT001R","PT001 Pelarasan Tanggungan - Rollback"),
            new Claim("PT001T","PT001 Pelarasan Tanggungan - Posting"),
            new Claim("PT001UT","PT001 Pelarasan Tanggungan - UnPosting"),
            //Pelarasan Tanggungan end
            //Invois Pembekal
            new Claim("TG002","TG002 Invois Pembekal"),
            new Claim("TG002C","TG002 Invois Pembekal - Tambah"),
            new Claim("TG002E","TG002 Invois Pembekal - Ubah"),
            new Claim("TG002D","TG002 Invois Pembekal - Hapus"),
            //new Claim("TG002P","TG002 Invois Pembekal - Cetak"),
            new Claim("TG002B","TG002 Invois Pembekal - Batal"),
            new Claim("TG002R","TG002 Invois Pembekal - Rollback"),
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
            new Claim("PV001R","PV001 Baucer Pembayaran - Rollback"),
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
            new Claim("JU001R","JU001 Baucer Jurnal - Rollback"),
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
            new Claim("TR001R","TR001 Pemegang Tunai Runcit - Rollback"),
            //new Claim("TR001B","TR001 Pemegang Tunai Runcit - Batal"),
            new Claim("TR001T","TR001 Pemegang Tunai Runcit - Rekup"),
            //new Claim("TR001UT","TR001 Pemegang Tunai Runcit - UnPosting"),
            //Pemegang Tunai Runcit end
            //Tunai Keluar
            new Claim("TR002","TR002 Tunai Keluar"),
            new Claim("TR002C","TR002 Tunai Keluar - Tambah"),
            new Claim("TR002E","TR002 Tunai Keluar - Ubah"),
            new Claim("TR002D","TR002 Tunai Keluar - Hapus"),
            new Claim("TR002P","TR002 Tunai Keluar - Cetak"),
            new Claim("TR002B","TR002 Tunai Keluar - Batal"),
            new Claim("TR002R","TR002 Tunai Keluar - Rollback"),
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
            new Claim("NM001R","NM001 Nota Minta - Rollback"),
            new Claim("NM001T","NM001 Nota Minta - Posting"),
            new Claim("NM001UT","NM001 Nota Minta - UnPosting"),
            //Nota Minta end
            //Pendahuluan Pelbagai
            new Claim("SP001","SP001 Pendahuluan Pelbagai"),
            new Claim("SP001C","SP001 Pendahuluan Pelbagai - Tambah"),
            new Claim("SP001E","SP001 Pendahuluan Pelbagai - Ubah"),
            new Claim("SP001D","SP001 Pendahuluan Pelbagai - Hapus"),
            new Claim("SP001P","SP001 Pendahuluan Pelbagai - Cetak"),
            new Claim("SP001B","SP001 Pendahuluan Pelbagai - Batal"),
            new Claim("SP001R","SP001 Pendahuluan Pelbagai - Rollback"),
            new Claim("SP001T","SP001 Pendahuluan Pelbagai - Posting"),
            new Claim("SP001UT","SP001 Pendahuluan Pelbagai - UnPosting"),
            //Pendahuluan Pelbagai end
            //Menu Belanjawan
            //Waran
            new Claim("BJ001","BJ001 Waran"),
            new Claim("BJ001C","BJ001 Waran - Tambah"),
            new Claim("BJ001E","BJ001 Waran - Ubah"),
            new Claim("BJ001D","BJ001 Waran - Hapus"),
            new Claim("BJ001P","BJ001 Waran - Cetak"),
            new Claim("BJ001B","BJ001 Waran - Batal"),
            new Claim("BJ001R","BJ001 Waran - Rollback"),
            new Claim("BJ001T","BJ001 Waran - Posting"),
            new Claim("BJ001UT","BJ001 Waran - UnPosting"),
            //Waran end
            //Menu Profil
            //Profil Atlet
            new Claim("SU001","SU001 Profil Atlet"),
            new Claim("SU001C","SU001 Profil Atlet - Tambah"),
            new Claim("SU001E","SU001 Profil Atlet - Ubah"),
            new Claim("SU001D","SU001 Profil Atlet - Hapus"),
            new Claim("SU001P","SU001 Profil Atlet - Cetak"),
            new Claim("SU001B","SU001 Profil Atlet - Batal"),
            new Claim("SU001R","SU001 Profil Atlet - Rollback"),
            new Claim("SU001T","SU001 Profil Atlet - Posting"),
            new Claim("SU001UT","SU001 Profil Atlet - UnPosting"),
            //Profil Atlet end
            //Profil Jurulatih
            new Claim("SU002","SU002 Profil Jurulatih"),
            new Claim("SU002C","SU002 Profil Jurulatih - Tambah"),
            new Claim("SU002E","SU002 Profil Jurulatih - Ubah"),
            new Claim("SU002D","SU002 Profil Jurulatih - Hapus"),
            new Claim("SU002P","SU002 Profil Jurulatih - Cetak"),
            new Claim("SU002B","SU002 Profil Jurulatih - Batal"),
            new Claim("SU002R","SU002 Profil Jurulatih - Rollback"),
            new Claim("SU002T","SU002 Profil Jurulatih - Posting"),
            new Claim("SU002UT","SU002 Profil Jurulatih - UnPosting"),
            //Profil Jurulatih end
            //Menu EFT
            //Biz Channel
            new Claim("PV002","PV002 Biz Channel"),
            new Claim("PV002C","PV002 Biz Channel - Jana (Tambah)"),
            new Claim("PV002E","PV002 Biz Channel - Ubah Status"),
            new Claim("PV002D","PV002 Biz Channel - Hapus"),
            new Claim("PV002P","PV002 Biz Channel - Cetak"),
            new Claim("PV002B","PV002 Biz Channel - Batal"),
            new Claim("PV002R","PV002 Biz Channel - Rollback"),
            //Profil Atlet end

        };
    }
}

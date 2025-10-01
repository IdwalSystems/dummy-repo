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
            //Penyata Pemungut
            new Claim("PR002","PR002 Penyata Pemungut"),
            new Claim("PR002C","PR002 Penyata Pemungut - Tambah"),
            new Claim("PR002E","PR002 Penyata Pemungut - Ubah"),
            new Claim("PR002D","PR002 Penyata Pemungut - Hapus"),
            new Claim("PR002P","PR002 Penyata Pemungut - Cetak"),
            new Claim("PR002B","PR002 Penyata Pemungut - Batal"),
            new Claim("PR002R","PR002 Penyata Pemungut - Rollback"),
            //Penyata Pemungut end
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
            //Inden
            new Claim("TG003","TG003 Inden Kerja"),
            new Claim("TG003C","TG003 Inden Kerja - Tambah"),
            new Claim("TG003E","TG003 Inden Kerja - Ubah"),
            new Claim("TG003D","TG003 Inden Kerja - Hapus"),
            new Claim("TG003P","TG003 Inden Kerja - Cetak"),
            new Claim("TG003B","TG003 Inden Kerja - Batal"),
            new Claim("TG003R","TG003 Inden Kerja - Rollback"),
            new Claim("TG003T","TG003 Inden Kerja - Posting"),
            new Claim("TG003UT","TG003 Inden Kerja - UnPosting"),
            //Inden end
            //Pelarasan PO
            new Claim("PT001","PT001 Pelarasan Tanggungan"),
            new Claim("PT001C","PT001 Pelarasan Tanggungan - Tambah"),
            new Claim("PT001E","PT001 Pelarasan Tanggungan - Ubah"),
            new Claim("PT001D","PT001 Pelarasan Tanggungan - Hapus"),
            new Claim("PT001P","PT001 Pelarasan Tanggungan - Cetak"),
            new Claim("PT001B","PT001 Pelarasan Tanggungan - Batal"),
            new Claim("PT001R","PT001 Pelarasan Tanggungan - Rollback"),
            new Claim("PT001T","PT001 Pelarasan Tanggungan - Posting"),
            new Claim("PT001UT","PT001 Pelarasan Tanggungan - UnPosting"),
            //Pelarasan PO end
             //Pelarasan PO
            new Claim("PI001","PI001 Pelarasan Inden Kerja"),
            new Claim("PI001C","PI001 Pelarasan Inden Kerja  - Tambah"),
            new Claim("PI001E","PI001 Pelarasan Inden Kerja  - Ubah"),
            new Claim("PI001D","PI001 Pelarasan Inden Kerja  - Hapus"),
            new Claim("PI001P","PI001 Pelarasan Inden Kerja  - Cetak"),
            new Claim("PI001B","PI001 Pelarasan Inden Kerja  - Batal"),
            new Claim("PI001R","PI001 Pelarasan Inden Kerja  - Rollback"),
            new Claim("PI001T","PI001 Pelarasan Inden Kerja  - Posting"),
            new Claim("PI001UT","PI001 Pelarasan Inden Kerja  - UnPosting"),
            //Pelarasan PO end
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
            //Nota Debit Kredit Belian
            new Claim("ND001","ND001 Nota Debit Kredit Belian"),
            new Claim("ND001C","ND001 Nota Debit Kredit Belian - Tambah"),
            new Claim("ND001E","ND001 Nota Debit Kredit Belian - Ubah"),
            new Claim("ND001D","ND001 Nota Debit Kredit Belian - Hapus"),
            new Claim("ND001P","ND001 Nota Debit Kredit Belian - Cetak"),
            new Claim("ND001B","ND001 Nota Debit Kredit Belian - Batal"),
            new Claim("ND001R","ND001 Nota Debit Kredit Belian - Rollback"),
            new Claim("ND001T","ND001 Nota Debit Kredit Belian - Posting"),
            new Claim("ND001UT","ND001 Nota Debit Kredit Belian - UnPosting"),
            //Nota Debit Kredit Belian end
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
            new Claim("DF004","DF004 P. Tunai Runcit"),
            new Claim("DF004C","DF004 P. Tunai Runcit - Tambah"),
            new Claim("DF004E","DF004 P. Tunai Runcit - Ubah"),
            new Claim("DF004D","DF004 P. Tunai Runcit - Hapus"),
            new Claim("DF004P","DF004 P. Tunai Runcit - Cetak"),
            new Claim("DF004R","DF004 P. Tunai Runcit - Rollback"),
            //new Claim("TR001B","TR001 Pemegang Tunai Runcit - Batal"),
            new Claim("DF004T","DF004 P. Tunai Runcit - Rekup"),
            //new Claim("TR001UT","TR001 Pemegang Tunai Runcit - UnPosting"),
            //Pemegang Tunai Runcit end
            //Tunai Keluar
            new Claim("TR001","TR001 Tunai Keluar"),
            new Claim("TR001C","TR001 Tunai Keluar - Tambah"),
            new Claim("TR001E","TR001 Tunai Keluar - Ubah"),
            new Claim("TR001D","TR001 Tunai Keluar - Hapus"),
            new Claim("TR001P","TR001 Tunai Keluar - Cetak"),
            new Claim("TR001B","TR001 Tunai Keluar - Batal"),
            new Claim("TR001R","TR001 Tunai Keluar - Rollback"),
            new Claim("TR001T","TR001 Tunai Keluar - Posting"),
            new Claim("TR001UT","TR001 Tunai Keluar - UnPosting"),
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
            //Belanjawan Semasa
            new Claim("BJ002","BJ002 Belanjawan Semasa"),
            new Claim("BJ002P","BJ002 Belanjawan Semasa - Cetak"),
            //Belanjawan Semasa end
            //Menu Lejar (Akaun)
            //Prosidur Akhir Tahun
            new Claim("AK004","AK004 Prosidur Akhir Tahun"),
            new Claim("AK004T","AK004 Prosidur Akhir Tahun - Posting"),
            new Claim("AK004UT","AK004 Prosidur Akhir Tahun - UnPosting"),
            //Prosidur Akhir Tahun end
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
            //Menu Invois
            //Invois Dikeluarkan
            new Claim("IN001","IN001 Invois Dikeluarkan"),
            new Claim("IN001C","IN001 Invois Dikeluarkan - Tambah"),
            new Claim("IN001E","IN001 Invois Dikeluarkan - Ubah"),
            new Claim("IN001D","IN001 Invois Dikeluarkan - Hapus"),
            new Claim("IN001P","IN001 Invois Dikeluarkan - Cetak"),
            new Claim("IN001B","IN001 Invois Dikeluarkan - Batal"),
            new Claim("IN001R","IN001 Invois Dikeluarkan - Rollback"),
            new Claim("IN001T","IN001 Invois Dikeluarkan - Posting"),
            new Claim("IN001UT","IN001 Invois Dikeluarkan - UnPosting"),
            //Invois Dikeluarkan end
            //Menu Daftar
            // Anggota
            new Claim("DF001","DF001 Daftar Anggota"),
            new Claim("DF001C","DF001 Daftar Anggota - Tambah"),
            new Claim("DF001E","DF001 Daftar Anggota - Ubah"),
            new Claim("DF001D","DF001 Daftar Anggota - Hapus"),
            new Claim("DF001B","DF001 Daftar Anggota - Batal"),
            new Claim("DF001R","DF001 Daftar Anggota - Rollback"),
            // Anggota End
            // Pembekal
            new Claim("DF002","DF002 Daftar Pembekal"),
            new Claim("DF002C","DF002 Daftar Pembekal - Tambah"),
            new Claim("DF002E","DF002 Daftar Pembekal - Ubah"),
            new Claim("DF002D","DF002 Daftar Pembekal - Hapus"),
            new Claim("DF002B","DF002 Daftar Pembekal - Batal"),
            new Claim("DF002R","DF002 Daftar Pembekal - Rollback"),
            // Pembekal End
            // Penghutang
            new Claim("DF003","DF003 Daftar Penghutang"),
            new Claim("DF003C","DF003 Daftar Penghutang - Tambah"),
            new Claim("DF003E","DF003 Daftar Penghutang - Ubah"),
            new Claim("DF003D","DF003 Daftar Penghutang - Hapus"),
            new Claim("DF003B","DF003 Daftar Penghutang - Batal"),
            new Claim("DF003R","DF003 Daftar Penghutang - Rollback"),
            // Penghutang End
            // P. Tunai Runcit
            new Claim("DF004","DF004 Daftar P. Tunai Runcit"),
            new Claim("DF004C","DF004 Daftar P. Tunai Runcit - Tambah"),
            new Claim("DF004E","DF004 Daftar P. Tunai Runcit - Ubah"),
            new Claim("DF004D","DF004 Daftar P. Tunai Runcit - Hapus"),
            new Claim("DF004B","DF004 Daftar P. Tunai Runcit - Batal"),
            new Claim("DF004R","DF004 Daftar P. Tunai Runcit - Rollback"),
            // P. Tunai Runcit End
            // Atlet
            new Claim("DF005","DF005 Daftar Atlet"),
            new Claim("DF005C","DF005 Daftar Atlet - Tambah"),
            new Claim("DF005E","DF005 Daftar Atlet - Ubah"),
            new Claim("DF005D","DF005 Daftar Atlet - Hapus"),
            new Claim("DF005B","DF005 Daftar Atlet - Batal"),
            new Claim("DF005R","DF005 Daftar Atlet - Rollback"),
            // Atlet End
            // Jurulatih
            new Claim("DF006","DF006 Daftar Jurulatih"),
            new Claim("DF006C","DF006 Daftar Jurulatih - Tambah"),
            new Claim("DF006E","DF006 Daftar Jurulatih - Ubah"),
            new Claim("DF006D","DF006 Daftar Jurulatih - Hapus"),
            new Claim("DF006B","DF006 Daftar Jurulatih - Batal"),
            new Claim("DF006R","DF006 Daftar Jurulatih - Rollback"),
            // Jurulatih End
            // Laporan - Laporan
            new Claim("LP001","LP001 Laporan - laporan Daftar"),
            // Laporan - Laporan End
            //Menu Penyesuaian Bank
            //Penyesuaian Bank
            new Claim("PB001","PB001 Penyesuaian Bank"),
            new Claim("PB001C","PB001 Penyesuaian Bank - Tambah"),
            new Claim("PB001E","PB001 Penyesuaian Bank - Ubah"),
            new Claim("PB001D","PB001 Penyesuaian Bank - Hapus"),
            new Claim("PB001P","PB001 Penyesuaian Bank - Cetak"),
            new Claim("PB001R","PB001 Penyesuaian Bank - Rollback"),
            //Penyesuaian Bank end
            // Menu

        };
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MSNK.Data;
using System;
using System.Linq;

namespace MSNK.Models.Modules
{
    public static class SeedData
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            // Look for any movies.
            if (context.JKW.Any())
            {
                    //return;   // DB has been seeded
            }
            else
            {
                context.JKW.AddRange(
                    new JKW
                    {
                        Kod = "100",
                        Perihal = "MAJLIS SUKAN NEGERI KEDAH"
                    }
                );
            }

            if (context.JCaraBayar.Any())
            {
                //return;
            }
            else
            {
                context.JCaraBayar.AddRange(
                    new JCaraBayar
                    {
                        Kod = "TN",
                        Perihal = "TUNAI"
                    },
                    new JCaraBayar
                    {
                        Kod = "CK",
                        Perihal = "CEK"
                    },
                    new JCaraBayar
                    {
                        Kod = "MK",
                        Perihal = "MAKLUMAN KREDIT"
                    },
                    new JCaraBayar
                    {
                        Kod = "EF",
                        Perihal = "EFT"
                    },
                    new JCaraBayar
                    {
                        Kod = "FP",
                        Perihal = "FPX"
                    }
                );
            }

            if (context.SiModul.Any())
            {
                //return;
            }
            else
            {
                context.SiModul.AddRange(
                    new SiModul
                    {
                        FuncId = "SY001",
                        FuncName = "SY001 Pengurusan Pengguna"
                    },
                    new SiModul
                    {
                        FuncId = "SY001A",
                        FuncName = "SY001 Pengurusan Pengguna – Capaian"
                    },
                    new SiModul
                    {
                        FuncId = "SY001C",
                        FuncName = "SY001 Pengurusan Pengguna - Tambah"
                    },
                    new SiModul
                    {
                        FuncId = "SY001D",
                        FuncName = "SY001 Pengurusan Pengguna - Hapus"
                    },
                    new SiModul
                    {
                        FuncId = "SY001E",
                        FuncName = "SY001 Pengurusan Pengguna - Ubah"
                    },
                    new SiModul
                    {
                        FuncId = "SY001R",
                        FuncName = "SY001 Pengurusan Pengguna - Reset Katalauan"
                    },
                    new SiModul
                    {
                        FuncId = "PR001",
                        FuncName = "PR001 Penerimaan"
                    },
                    new SiModul
                    {
                        FuncId = "PR001C",
                        FuncName = "PR001 Penerimaan - Tambah"
                    },
                    new SiModul
                    {
                        FuncId = "PR001D",
                        FuncName = "PR001 Penerimaan - Hapus"
                    },
                    new SiModul
                    {
                        FuncId = "PR001E",
                        FuncName = "PR001 Penerimaan - Ubah"
                    },
                    new SiModul
                    {
                        FuncId = "PR001P",
                        FuncName = "PR001 Penerimaan - Cetak"
                    },
                    new SiModul
                    {
                        FuncId = "PR001T",
                        FuncName = "PR001 Penerimaan - Posting"
                    },
                    new SiModul
                    {
                        FuncId = "PR001UT",
                        FuncName = "PR001 Penerimaan – UnPosting"
                    },
                    new SiModul
                    {
                        FuncId = "PR001B",
                        FuncName = "PR001 Penerimaan – Batal"
                    }
                );
            }

            if (context.JBank.Any())
            {
                //return;   // DB has been seeded
            }
            else
            {
                context.JBank.AddRange(
                    new JBank
                    {
                        Kod = "BIMB",
                        Nama = "BANK ISLAM MALAYSIA BERHAD"
                    },
                    new JBank
                    {
                        Kod = "BMMB",
                        Nama = "BANK MUAMALAT MALAYSIA BERHAD"
                    },
                    new JBank
                    {
                        Kod = "MBB",
                        Nama = "MALAYAN BANKING BERHAD"
                    }
                );
            }

            if (context.JNegeri.Any())
            {
                //return;   // DB has been seeded
            }
            else
            {
                context.JNegeri.AddRange(
                    new JNegeri
                    {
                        Kod = "01",
                        Perihal = "JOHOR"
                    },
                    new JNegeri
                    {
                        Kod = "02",
                        Perihal = "KEDAH"
                    },
                    new JNegeri
                    {
                        Kod = "03",
                        Perihal = "KELANTAN"
                    },
                    new JNegeri
                    {
                        Kod = "04",
                        Perihal = "MELAKA"
                    },
                    new JNegeri
                    {
                        Kod="05",
                        Perihal="NEGERI SEMBILAN"
                    },
                    new JNegeri
                    {
                        Kod = "06",
                        Perihal = "PAHANG"
                    },
                    new JNegeri
                    {
                        Kod = "07",
                        Perihal = "PULAU PINANG"
                    },
                    new JNegeri
                    {
                        Kod = "08",
                        Perihal = "PERAK"
                    },
                    new JNegeri
                    {
                        Kod = "09",
                        Perihal = "PERLIS"
                    },
                    new JNegeri
                    {
                        Kod="10",
                        Perihal="SELANGOR"
                    },
                    new JNegeri
                    {
                        Kod = "11",
                        Perihal = "TERENGGANU"
                    },
                    new JNegeri
                    {
                        Kod = "12",
                        Perihal = "SABAH"
                    },
                    new JNegeri
                    {
                        Kod = "13",
                        Perihal = "SARAWAK"
                    },
                    new JNegeri
                    {
                        Kod = "14",
                        Perihal = "WILAYAH PERSEKUTUAN (KUALA LUMPUR)"
                    },
                    new JNegeri
                    {
                        Kod = "15",
                        Perihal = "WILAYAH PERSEKUTUAN (LABUAN)"
                    },
                    new JNegeri
                    {
                        Kod = "16",
                        Perihal = "WILAYAH PERSEKUTUAN (PUTRAJAYA)"
                    }
                );
            }

            if (context.JJenis.Any())
            {
                //return;   // DB has been seeded
            }
            else
            {
                context.JJenis.AddRange(
                    new JJenis
                    {
                        Kod = "L",
                        Nama = "Liabiliti"
                    },

                    new JJenis
                    {
                        Kod = "E",
                        Nama = "Ekuiti"
                    },
                    
                    new JJenis
                    {
                        Kod = "B",
                        Nama = "BELANJA"
                    },
                    new JJenis
                    {
                        Kod = "A",
                        Nama = "ASET"
                    },
                    new JJenis
                    {
                        Kod = "H",
                        Nama = "Hasil"
                    }

                );
            }

            if (context.JParas.Any())
            {
                //return;   // DB has been seeded
            }
            else
            {
                context.JParas.AddRange(
                    new JParas
                    {
                        Kod = "1"
                    },

                    new JParas
                    {
                        Kod = "2",
                    },

                    new JParas
                    {
                        Kod = "3"
                    },
                    new JParas
                    {
                        Kod = "4"
                    }

                );
            }
            context.SaveChanges();
            
            //Data with foreign key

            if (context.AkCarta.Any())
            {
                //return;   // DB has been seeded
            }
            else
            {
                var kw = context.JKW.Where(b => b.Kod == "100").FirstOrDefault();
                var jenisH = context.JJenis.Where(b => b.Kod == "H").FirstOrDefault();
                var jenisA = context.JJenis.Where(b => b.Kod == "A").FirstOrDefault();
                var paras1 = context.JParas.Where(b => b.Kod == "1").FirstOrDefault();
                var paras2 = context.JParas.Where(b => b.Kod == "2").FirstOrDefault();
                var paras3 = context.JParas.Where(b => b.Kod == "3").FirstOrDefault();
                var paras4 = context.JParas.Where(b => b.Kod == "4").FirstOrDefault();
                context.AkCarta.AddRange( 
                    new AkCarta
                    {
                        JKWId = kw.Id,
                        Kod = "A10000",
                        Nama = "ASET SEMASA",
                        JJenisId = jenisA.Id,
                        JParasId = paras1.Id,
                        DebitKredit = "D",
                        UmumDetail = "U",
                        Catatan1 = "",
                        Catatan2 = ""
                    },
                    new AkCarta
                    {
                        JKWId = kw.Id,
                        Kod = "A11000",
                        Nama = "WANG TUNAI DAN BAKI BANK",
                        JJenisId = jenisA.Id,
                        JParasId = paras2.Id,
                        DebitKredit = "D",
                        UmumDetail = "U",
                        Catatan1 = "",
                        Catatan2 = ""
                    },
                    new AkCarta
                    {
                        JKWId = kw.Id,
                        Kod = "A11100",
                        Nama = "WANG TUNAI DAN BAKI BANK",
                        JJenisId = jenisA.Id,
                        JParasId = paras3.Id,
                        DebitKredit = "D",
                        UmumDetail = "U",
                        Catatan1 = "",
                        Catatan2 = ""
                    },
                    new AkCarta
                    {
                        JKWId = kw.Id,
                        Kod = "A11101",
                        Nama = "BIMB ... ",
                        JJenisId = jenisA.Id,
                        JParasId = paras4.Id,
                        DebitKredit = "D",
                        UmumDetail = "D",
                        Catatan1 = "",
                        Catatan2 = ""
                    },
                    new AkCarta
                    {
                        JKWId = kw.Id,
                        Kod = "A11106",
                        Nama = "BMMB ... ",
                        JJenisId = jenisA.Id,
                        JParasId = paras4.Id,
                        DebitKredit = "D",
                        UmumDetail = "D",
                        Catatan1 = "",
                        Catatan2 = ""
                    },
                    new AkCarta
                    {
                        JKWId = kw.Id,
                        Kod = "A11108",
                        Nama = "MBB ... ",
                        JJenisId = jenisA.Id,
                        JParasId = paras4.Id,
                        DebitKredit = "D",
                        UmumDetail = "D",
                        Catatan1 = "",
                        Catatan2 = ""
                    },
                    new AkCarta
                    {
                        JKWId = kw.Id,
                        Kod = "H10000",
                        Nama = "Hasil Bukan Cukai",
                        JJenisId = jenisH.Id,
                        JParasId = paras1.Id,
                        DebitKredit = "K",
                        UmumDetail = "U",
                        Catatan1 = "",
                        Catatan2 = ""
                    },
                    new AkCarta
                    {
                        JKWId = kw.Id,
                        Kod = "H11000",
                        Nama = "Hasil Bukan Cukai",
                        JJenisId = jenisH.Id,
                        JParasId = paras2.Id,
                        DebitKredit = "K",
                        UmumDetail = "U",
                        Catatan1 = "",
                        Catatan2 = ""
                    },
                    new AkCarta
                    {
                        JKWId = kw.Id,
                        Kod = "H11100",
                        Nama = "Hasil Dokumen",
                        JJenisId = jenisH.Id,
                        JParasId = paras3.Id,
                        DebitKredit = "K",
                        UmumDetail = "U",
                        Catatan1 = "",
                        Catatan2 = ""
                    },
                    new AkCarta
                    {
                        JKWId = kw.Id,
                        Kod = "H11101",
                        Nama = "Hasil Dokumen Tender",
                        JJenisId = jenisH.Id,
                        JParasId = paras4.Id,
                        DebitKredit = "K",
                        UmumDetail = "D",
                        Catatan1 = "",
                        Catatan2 = ""
                    },
                    new AkCarta
                    {
                        JKWId = kw.Id,
                        Kod = "H11102",
                        Nama = "Hasil Dokumen Sebutharga",
                        JJenisId = jenisH.Id,
                        JParasId = paras4.Id,
                        DebitKredit = "K",
                        UmumDetail = "D",
                        Catatan1 = "",
                        Catatan2 = ""
                    }
                );
            }
            context.SaveChanges();

            if (context.AkBank.Any())
            {
                //return;   // DB has been seeded
            }
            else
            {
                var kw = context.JKW.Where(b => b.Kod == "100").FirstOrDefault();
                var bank = context.JBank.Where(b => b.Kod == "BIMB").FirstOrDefault();
                var carta = context.AkCarta.Where(b => b.Kod == "H11102");

                context.AkBank.AddRange(
                    new AkBank
                    {
                        JKWId = kw.Id,
                        JBankId = 1,
                        AkCartaId = 1,
                        Kod = "001",
                        NoAkaun = "1200210005702"
                    }
                );
            }
            context.SaveChanges();

            if (context.AkPembekal.Any())
            {
            }
            else
            {
                var negeri = context.JNegeri.Where(b => b.Kod == "10").FirstOrDefault();
                var jbank = context.JBank.Where(b => b.Kod == "BIMB").FirstOrDefault();

                context.AkPembekal.AddRange(
                    new AkPembekal
                    {
                        KodSykt = "I00001",
                        NamaSykt = "IDWAL SYSTEMS SDN BHD",
                        NoPendaftaran = "187842-T",
                        Alamat1 = "LOT 605G, KOMPLEKS DIAMOND, BANGI BUSINESS PARK, ",
                        Alamat2 = "JALAN MEDAN BANGI, OFF PERSIARAN BANDAR, ",
                        Alamat3 = "",
                        Poskod="43650",
                        Bandar = "BANDAR BARU BANGI ",
                        JNegeriId = negeri.Id,
                        Telefon1 = "+601133272978",
                        Emel = "idwal.com.my",
                        AkaunBank = "",
                        JBankId = jbank.Id
                    }
                );
            }
            context.SaveChanges();

        }
    }
}
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
            if (context.KW.Any())
                {
                    //return;   // DB has been seeded
                }
                else
                {
                    context.KW.AddRange(
                        new KW
                        {
                            Kod = "100",
                            Perihal = "MAJLIS SUKAN NEGERI KEDAH"
                        }
                    );
                }

                if (context.CaraBayar.Any())
                {
                    //return;
                }
                else
                {
                    context.CaraBayar.AddRange(
                        new CaraBayar
                        {
                            Kod = "TN",
                            Perihal = "TUNAI"
                        },
                        new CaraBayar
                        {
                            Kod = "CK",
                            Perihal = "CEK"
                        },
                        new CaraBayar
                        {
                            Kod = "MK",
                            Perihal = "MAKLUMAN KREDIT"
                        },
                        new CaraBayar
                        {
                            Kod = "EF",
                            Perihal = "EFT"
                        },
                        new CaraBayar
                        {
                            Kod = "FP",
                            Perihal = "FPX"
                        }
                    );
                }

                if (context.Modul.Any())
                {
                    //return;
                }
                else
                {
                    context.Modul.AddRange(
                        new Modul
                        {
                            FuncId = "SY001",
                            FuncName = "SY001 Pengurusan Pengguna"
                        },
                        new Modul
                        {
                            FuncId = "SY001A",
                            FuncName = "SY001 Pengurusan Pengguna – Capaian"
                        },
                        new Modul
                        {
                            FuncId = "SY001C",
                            FuncName = "SY001 Pengurusan Pengguna - Tambah"
                        },
                        new Modul
                        {
                            FuncId = "SY001D",
                            FuncName = "SY001 Pengurusan Pengguna - Hapus"
                        },
                        new Modul
                        {
                            FuncId = "SY001E",
                            FuncName = "SY001 Pengurusan Pengguna - Ubah"
                        },
                        new Modul
                        {
                            FuncId = "SY001R",
                            FuncName = "SY001 Pengurusan Pengguna - Reset Katalauan"
                        },
                        new Modul
                        {
                            FuncId = "PR001",
                            FuncName = "PR001 Penerimaan"
                        },
                        new Modul
                        {
                            FuncId = "PR001C",
                            FuncName = "PR001 Penerimaan - Tambah"
                        },
                        new Modul
                        {
                            FuncId = "PR001D",
                            FuncName = "PR001 Penerimaan - Hapus"
                        },
                        new Modul
                        {
                            FuncId = "PR001E",
                            FuncName = "PR001 Penerimaan - Ubah"
                        },
                        new Modul
                        {
                            FuncId = "PR001P",
                            FuncName = "PR001 Penerimaan - Cetak"
                        },
                        new Modul
                        {
                            FuncId = "PR001T",
                            FuncName = "PR001 Penerimaan - Posting"
                        },
                        new Modul
                        {
                            FuncId = "PR001UT",
                            FuncName = "PR001 Penerimaan – UnPosting"
                        },
                        new Modul
                        {
                            FuncId = "PR001B",
                            FuncName = "PR001 Penerimaan – Batal"
                        }
                    );
                }

                if (context.Bank.Any())
                {
                    //return;   // DB has been seeded
                }
                else
                {
                    context.Bank.AddRange(
                        new Bank
                        {
                            Kod = "BIMB",
                            Nama = "BANK ISLAM MALAYSIA BERHAD"
                        },
                        new Bank
                        {
                            Kod = "BMMB",
                            Nama = "BANK MUAMALAT MALAYSIA BERHAD"
                        },
                        new Bank
                        {
                            Kod = "MBB",
                            Nama = "MALAYAN BANKING BERHAD"
                        }
                    );
                }

                if (context.Negeri.Any())
                {
                    //return;   // DB has been seeded
                }
                else
                {
                    context.Negeri.AddRange(
                        new Negeri
                        {
                            Kod = "08",
                            Perihal = "PERAK"
                        },
                        new Negeri
                        {
                            Kod="05",
                            Perihal="NEGERI SEMBILAN"
                        },
                        new Negeri
                        {
                            Kod="10",
                            Perihal="SELANGOR"
                        }
                    );
                }

            if (context.Jenis.Any())
            {
                //return;   // DB has been seeded
            }
            else
            {
                context.Jenis.AddRange(
                    new Jenis
                    {
                        Kod = "L",
                        Nama = "Liabiliti"
                    },

                    new Jenis
                    {
                        Kod = "E",
                        Nama = "Ekuiti"
                    },
                    
                    new Jenis
                    {
                        Kod = "B",
                        Nama = "BELANJA"
                    },
                    new Jenis
                    {
                        Kod = "A",
                        Nama = "ASET"
                    },
                    new Jenis
                    {
                        Kod = "H",
                        Nama = "Hasil"
                    }

                );
            }

            if (context.Jenis.Any())
            {
                //return;   // DB has been seeded
            }
            else
            {
                context.Paras.AddRange(
                    new Paras
                    {
                        Kod = "1"
                    },

                    new Paras
                    {
                        Kod = "2",
                    },

                    new Paras
                    {
                        Kod = "3"
                    },
                    new Paras
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
                    context.AkCarta.AddRange(
                        new AkCarta
                        {
                            KWId = 1,
                            Kod = "H11102",
                            Nama = "Hasil Dokumen Sebutharga",
                            KodJenis = 5,
                            KodParas = 4,
                            DebitKredit = "K",
                            UmumDetail = "D",
                            Catatan1 = "",
                            Catatan2 = ""
                        }
                    );
                }

                if (context.AkBank.Any())
                {
                    //return;   // DB has been seeded
                }
                else
                {
                    context.AkBank.AddRange(
                        new AkBank
                        {
                            KWId = 1,
                            BankId = 1,
                            Kod = "001",
                            NoAkaun = "1200210005702"
                        }
                    );
                }

            context.SaveChanges();

        }
    }
}
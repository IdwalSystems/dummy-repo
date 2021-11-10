using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
//using MSNKTest.Data;
using System;
using System.Linq;

namespace MSNKTest.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new MSNKDBContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<MSNKDBContext>>()))
            {
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
                            KOD = "100",
                            PERIHAL = "MAJLIS SUKAN NEGERI KEDAH"
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
                            KOD = "TN",
                            PERIHAL = "TUNAI"
                        },
                        new CaraBayar
                        {
                            KOD = "CK",
                            PERIHAL = "CEK"
                        },
                        new CaraBayar
                        {
                            KOD = "MK",
                            PERIHAL = "MAKLUMAN KREDIT"
                        },
                        new CaraBayar
                        {
                            KOD = "EF",
                            PERIHAL = "EFT"
                        },
                        new CaraBayar
                        {
                            KOD = "FP",
                            PERIHAL = "FPX"
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
                            FUNCID = "SY001",
                            FUNCNAME = "SY001 Pengurusan Pengguna"
                        },
                        new Modul
                        {
                            FUNCID = "SY001A",
                            FUNCNAME = "SY001 Pengurusan Pengguna – Capaian"
                        },
                        new Modul
                        {
                            FUNCID = "SY001C",
                            FUNCNAME = "SY001 Pengurusan Pengguna - Tambah"
                        },
                        new Modul
                        {
                            FUNCID = "SY001D",
                            FUNCNAME = "SY001 Pengurusan Pengguna - Hapus"
                        },
                        new Modul
                        {
                            FUNCID = "SY001E",
                            FUNCNAME = "SY001 Pengurusan Pengguna - Ubah"
                        },
                        new Modul
                        {
                            FUNCID = "SY001R",
                            FUNCNAME = "SY001 Pengurusan Pengguna - Reset Katalauan"
                        },
                        new Modul
                        {
                            FUNCID = "PR001",
                            FUNCNAME = "PR001 Penerimaan"
                        },
                        new Modul
                        {
                            FUNCID = "PR001C",
                            FUNCNAME = "PR001 Penerimaan - Tambah"
                        },
                        new Modul
                        {
                            FUNCID = "PR001D",
                            FUNCNAME = "PR001 Penerimaan - Hapus"
                        },
                        new Modul
                        {
                            FUNCID = "PR001E",
                            FUNCNAME = "PR001 Penerimaan - Ubah"
                        },
                        new Modul
                        {
                            FUNCID = "PR001P",
                            FUNCNAME = "PR001 Penerimaan - Cetak"
                        },
                        new Modul
                        {
                            FUNCID = "PR001T",
                            FUNCNAME = "PR001 Penerimaan - Posting"
                        },
                        new Modul
                        {
                            FUNCID = "PR001UT",
                            FUNCNAME = "PR001 Penerimaan – UnPosting"
                        },
                        new Modul
                        {
                            FUNCID = "PR001B",
                            FUNCNAME = "PR001 Penerimaan – Batal"
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
                            KOD = "BIMB",
                            NAMA = "BANK ISLAM MALAYSIA BERHAD"
                        },
                        new Bank
                        {
                            KOD = "BMMB",
                            NAMA = "BANK MUAMALAT MALAYSIA BERHAD"
                        },
                        new Bank
                        {
                            KOD = "MBB",
                            NAMA = "MALAYAN BANKING BERHAD"
                        }
                    );
                }

                context.SaveChanges();
            }
        }
    }
}
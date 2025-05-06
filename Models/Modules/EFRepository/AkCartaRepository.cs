using Microsoft.EntityFrameworkCore;
using MSNK.Data;
using MSNK.Models.Helper;
using MSNK.Models.Modules.IRepository;
using MSNK.Models.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.EFRepository
{
    public class AkCartaRepository : IRepository<AkCarta, int, string>
    {
        public readonly ApplicationDbContext context;

        public AkCartaRepository(ApplicationDbContext context) => this.context = context;
        public async Task Delete(int id)
        {
            var akCarta = await context.AkCarta.FirstOrDefaultAsync(b => b.Id == id);
            if(akCarta != null)
            {
                context.Remove(akCarta);
            }
        }

        public string FormulaInSentence(EnJenisOperasi jenisOperasi, string jenisCarta, bool isKecuali, string kodList, decimal amaunTetap, bool IsLastYear)
        {
            string txtexcept = "";
            string txtcode = "";
            if (!string.IsNullOrEmpty(jenisCarta))
            {
                string[] jenisCartaArray = jenisCarta.Split(",");
                List<string> txtcodeList = new List<string>();
                foreach (var arr in jenisCartaArray)
                {
                    switch (arr[0])
                    {
                        case '1':
                            txtcodeList.Add(EnJenisCarta.LIABILITI.GetDisplayName());
                            break;
                        case '2':
                            txtcodeList.Add(EnJenisCarta.EKUITI.GetDisplayName());
                            break;
                        case '3':
                            txtcodeList.Add(EnJenisCarta.BELANJA.GetDisplayName());
                            break;
                        case '4':
                            txtcodeList.Add(EnJenisCarta.ASET.GetDisplayName());
                            break;
                        case '5':
                            txtcodeList.Add(EnJenisCarta.HASIL.GetDisplayName());
                            break;
                    }
                }
                txtcode = string.Join(",", txtcodeList);
                if (isKecuali && !string.IsNullOrEmpty(kodList))
                {

                    string[] kodListArray = kodList.Split(",");
                    List<string> txtexceptcodeList = new List<string>();
                    foreach (var arr in kodListArray)
                    {
                        var kodAkaun = context.AkCarta.Find(int.Parse(arr))?.Kod ?? "";
                        txtexceptcodeList.Add(kodAkaun);
                    }
                    txtexcept = $" kecuali kod - kod({string.Join(",", txtexceptcodeList)})";
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(kodList))
                {
                    string[] kodListArray = kodList.Split(",");
                    List<string> txtcodeList = new List<string>();
                    foreach (var arr in kodListArray)
                    {
                        var kodAkaun = context.AkCarta.Find(int.Parse(arr))?.Kod ?? "";
                        txtcodeList.Add(kodAkaun);
                    }
                    txtcode = string.Join(",", txtcodeList);
                }

            }

            string sentences = "";

            if (kodList != null || jenisCarta != null)
            {
                if (jenisOperasi == EnJenisOperasi.Tambah)
                {
                    sentences = $"Jumlah bagi kod - kod ({txtcode}){txtexcept}";
                }
                else if(jenisOperasi == EnJenisOperasi.Tolak) 
                {
                    sentences = $"ditolak dengan jumlah bagi kod - kod ({txtcode}){txtexcept}";
                }
                else
                {
                    sentences = $"Jumlah Tetap RM {Convert.ToDecimal(amaunTetap).ToString("#,##0.00")}";
                }
            }
            else
            {
                if (jenisOperasi == EnJenisOperasi.Tambah)
                {
                    sentences = "Tiada formula operasi tambah";
                }
                else if (jenisOperasi == EnJenisOperasi.Tolak)
                {
                    sentences = "Tiada formula operasi tolak";
                }
                else
                {
                    sentences = $"Jumlah Tetap RM {Convert.ToDecimal(amaunTetap).ToString("#,##0.00")}";
                }
            }

            if (IsLastYear)
            {
                sentences += " (kiraan tahun sebelum)";
            }

            return sentences;
        }

        public async Task<IEnumerable<AkCarta>> GetAll(string filter)
        {
            var result = new List<AkCarta>();

            if (string.IsNullOrWhiteSpace(filter))
            {
                result = await context.AkCarta
                .Include(b => b.JKW)
                .Include(b => b.JParas)
                .Include(b => b.JJenis)
                .OrderBy(b => b.Kod)
                .ToListAsync();
            }
            else
            {
                result = await context.AkCarta
                .Include(b => b.JKW)
                .Include(b => b.JParas)
                .Include(b => b.JJenis)
                .OrderBy(b => b.Kod)
                .Where(b => b.Kod == filter)
                .ToListAsync();
            }

            return result;
        }

        public JKonfigPerubahanEkuiti GetAllDetailsByTahunOrJenisEkuiti(string tahun, EnJenisLajurJadualPerubahanEkuiti? enJenisEkuiti)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AkCarta>> GetAllFiltered(string filter, string filterDate1, string filterDate2, string filterType)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<AkCarta>> GetAllIncludeDeletedItems()
        {
            return await context.AkCarta
                .IgnoreQueryFilters()
                .Include(b => b.JKW)
                .Include(b => b.JParas)
                .Include(b => b.JJenis)
                .OrderBy(b => b.Kod)
                .ToListAsync();
        }

        public Task<IEnumerable<AkCarta>> GetAllIncludeDeletedItemsFiltered(string filter, string filterDate1, string filterDate2, string filterType)
        {
            throw new NotImplementedException();
        }

        public async Task<AkCarta> GetById(int id)
        {
            return await context.AkCarta
                .Include(b => b.JKW)
                .Include(b => b.JParas)
                .Include(b => b.JJenis)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<AkCarta> GetByIdIncludeDeletedItems(int id)
        {
            return await context.AkCarta
                .IgnoreQueryFilters()
                .Include(b => b.JKW)
                .Include(b => b.JParas)
                .Include(b => b.JJenis)
                .Where(x=> x.Id == id)
                .FirstOrDefaultAsync();
        }

        public Task<AkCarta> GetByString(string id)
        {
            throw new NotImplementedException();
        }

        public string GetSetOfCartaList(EnBarisPerubahanEkuiti enBaris, EnJenisOperasi enOperasi, bool isPukal, string enJenisCartaList, bool isKecuali, string kodList)
        {
            throw new NotImplementedException();
        }

        public string GetSetOfCartaStringList(bool isPukal, string enJenisCartaList, bool isKecuali, string kodList)
        {
            List<string> setKodList = new List<string>();

            List<string> arrKodList = kodList?.Split(',').ToList() ?? new List<string>();

            if (isPukal)
            {
                List<string> arrJenisCartaList = enJenisCartaList?.Split(',').ToList() ?? new List<string>();
                foreach (var jenisCarta in arrJenisCartaList)
                {

                    var akCartaList = GetCartaListByJenisCarta((EnJenisCarta)int.Parse(jenisCarta), isKecuali, arrKodList);
                    setKodList = akCartaList;
                }
            }
            else
            {
                setKodList = arrKodList;
            }

            return string.Join(',', setKodList);
        }

        private List<string> GetCartaListByJenisCarta(EnJenisCarta jenisCartaId, bool isKecuali, List<string> arrKodList)
        {
            var cartaList = context.AkCarta
                .Include(c => c.JParas)
                .Where(a => a.JJenis != null && a.JJenis.Equals(jenisCartaId) && (!isKecuali || !arrKodList!.Contains(a.Id.ToString())))
                .Select(c => c.Id.ToString())
                .ToList();

            return cartaList ?? new List<string>();
        }

        public async Task<AkCarta> Insert(AkCarta entity)
        {
            await context.AkCarta.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(AkCarta entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();

        }
    }
}

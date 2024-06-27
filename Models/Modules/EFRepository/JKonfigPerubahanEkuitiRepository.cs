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
    public class JKonfigPerubahanEkuitiRepository : IRepository<JKonfigPerubahanEkuiti, int, string>
    {
        public readonly ApplicationDbContext context;

        public JKonfigPerubahanEkuitiRepository(ApplicationDbContext context) => this.context = context;
        public async Task Delete(int id)
        {
            var model = await context.JKonfigPerubahanEkuiti.FirstOrDefaultAsync(b => b.Id == id);
            if (model != null)
            {
                context.Remove(model);
            }
        }

        public async Task<IEnumerable<JKonfigPerubahanEkuiti>> GetAll(string filter)
        {
            return await context.JKonfigPerubahanEkuiti.Include(pe => pe.JKW).ToListAsync();
        }

        public async Task<IEnumerable<JKonfigPerubahanEkuiti>> GetAllIncludeDeletedItems()
        {
            return await context.JKonfigPerubahanEkuiti
                .IgnoreQueryFilters().Include(pe => pe.JKW)
                .ToListAsync();
        }

        public async Task<JKonfigPerubahanEkuiti> GetById(int id)
        {
            var result = await context.JKonfigPerubahanEkuiti.Include(pe => pe.JKW).Include(pe => pe.JKonfigPerubahanEkuitiBaris).FirstOrDefaultAsync(pe => pe.Id == id);

            if (result != null && result.JKonfigPerubahanEkuitiBaris != null && result.JKonfigPerubahanEkuitiBaris.Count > 0)
            {
                string barisBefore = "";

                foreach (var baris in result.JKonfigPerubahanEkuitiBaris.OrderBy(b => b.EnBaris).ThenBy(b => b.EnJenisOperasi))
                {
                    string barisSentences = baris.EnBaris.GetDisplayName();
                    if (barisSentences == barisBefore)
                    {
                        barisSentences = "";
                    }
                    string sentence = FormulaInSentence(baris.EnJenisOperasi, baris.EnJenisCartaList, baris.IsKecuali, baris.KodList);

                    baris.BarisDescription = barisSentences;
                    baris.FormulaDescription = sentence;

                    barisBefore = baris.EnBaris.GetDisplayName();

                }

                result.JKonfigPerubahanEkuitiBaris = result.JKonfigPerubahanEkuitiBaris.OrderBy(b => b.EnBaris).ThenBy(b => b.EnJenisOperasi).ToList();
            }

            return result ?? new JKonfigPerubahanEkuiti();

        }

        public string FormulaInSentence(EnJenisOperasi jenisOperasi, string jenisCarta, bool isKecuali, string kodList)
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
                        case '5':
                            txtcodeList.Add(EnJenisCarta.LIABILITI.GetDisplayName());
                            break;
                        case '4':
                            txtcodeList.Add(EnJenisCarta.EKUITI.GetDisplayName());
                            break;
                        case '3':
                            txtcodeList.Add(EnJenisCarta.BELANJA.GetDisplayName());
                            break;
                        case '2':
                            txtcodeList.Add(EnJenisCarta.ASET.GetDisplayName());
                            break;
                        case '1':
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
                else
                {
                    sentences = $"ditolak dengan jumlah bagi kod - kod ({txtcode}){txtexcept}";
                }
            }
            else
            {
                if (jenisOperasi == EnJenisOperasi.Tambah)
                {
                    sentences = "Tiada formula operasi tambah";
                }
                else
                {
                    sentences = "Tiada formula operasi tolak";
                }
            }


            return sentences;
        }

        public async Task<JKonfigPerubahanEkuiti> GetByIdIncludeDeletedItems(int id)
        {
            var result = await context.JKonfigPerubahanEkuiti
                .IgnoreQueryFilters().Include(pe => pe.JKW).Include(pe => pe.JKonfigPerubahanEkuitiBaris).FirstOrDefaultAsync(pe => pe.Id == id);

            if (result != null && result.JKonfigPerubahanEkuitiBaris != null && result.JKonfigPerubahanEkuitiBaris.Count > 0)
            {
                string barisBefore = "";

                foreach (var baris in result.JKonfigPerubahanEkuitiBaris.OrderBy(b => b.EnBaris).ThenBy(b => b.EnJenisOperasi))
                {
                    string barisSentences = baris.EnBaris.GetDisplayName();
                    if (barisSentences == barisBefore)
                    {
                        barisSentences = "";
                    }
                    string sentence = FormulaInSentence(baris.EnJenisOperasi, baris.EnJenisCartaList, baris.IsKecuali, baris.KodList);

                    baris.BarisDescription = barisSentences;
                    baris.FormulaDescription = sentence;

                    barisBefore = baris.EnBaris.GetDisplayName();

                }

                result.JKonfigPerubahanEkuitiBaris = result.JKonfigPerubahanEkuitiBaris.OrderBy(b => b.EnBaris).ThenBy(b => b.EnJenisOperasi).ToList();
            }

            return result ?? new JKonfigPerubahanEkuiti();
        }

        public JKonfigPerubahanEkuiti GetAllDetailsByTahunOrJenisEkuiti(string tahun, EnJenisLajurJadualPerubahanEkuiti? enJenisEkuiti)
        {
            var result = new JKonfigPerubahanEkuiti();
            result = context.JKonfigPerubahanEkuiti.Include(pe => pe.JKW).Include(pe => pe.JKonfigPerubahanEkuitiBaris).FirstOrDefault(pe => pe.Tahun == tahun);

            if (enJenisEkuiti != null)
            {
                result = context.JKonfigPerubahanEkuiti.Include(pe => pe.JKW).Include(pe => pe.JKonfigPerubahanEkuitiBaris).FirstOrDefault(pe => pe.Tahun == tahun && pe.EnLajurJadual == enJenisEkuiti);
            }

            if (result != null && result.JKonfigPerubahanEkuitiBaris != null && result.JKonfigPerubahanEkuitiBaris.Count > 0)
            {
                string barisBefore = "";

                foreach (var baris in result.JKonfigPerubahanEkuitiBaris.OrderBy(b => b.EnBaris).ThenBy(b => b.EnJenisOperasi))
                {
                    string barisSentences = baris.EnBaris.GetDisplayName();
                    if (barisSentences == barisBefore)
                    {
                        barisSentences = "";
                    }
                    string sentence = FormulaInSentence(baris.EnJenisOperasi, baris.EnJenisCartaList, baris.IsKecuali, baris.KodList);

                    baris.BarisDescription = barisSentences;
                    baris.FormulaDescription = sentence;

                    barisBefore = baris.EnBaris.GetDisplayName();

                }

                result.JKonfigPerubahanEkuitiBaris = result.JKonfigPerubahanEkuitiBaris.OrderBy(b => b.EnBaris).ThenBy(b => b.EnJenisOperasi).ToList();
            }

            return result ?? new JKonfigPerubahanEkuiti();
        }

        public string GetSetOfCartaList(EnBarisPerubahanEkuiti enBaris, EnJenisOperasi enOperasi, bool isPukal, string enJenisCartaList, bool isKecuali, string kodList)
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
                .Where(a => a.JJenis.Nama.Equals(jenisCartaId.GetDisplayName()) && (!isKecuali || !arrKodList!.Contains(a.Id.ToString())))
                .Select(c => c.Id.ToString())
                .ToList();

            return cartaList ?? new List<string>();
        }


        public Task<JKonfigPerubahanEkuiti> GetByString(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<JKonfigPerubahanEkuiti> Insert(JKonfigPerubahanEkuiti entity)
        {
            await context.JKonfigPerubahanEkuiti.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(JKonfigPerubahanEkuiti entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }

        public Task<IEnumerable<JKonfigPerubahanEkuiti>> GetAllFiltered(string filter, string filterDate1, string filterDate2, string filterType)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<JKonfigPerubahanEkuiti>> GetAllIncludeDeletedItemsFiltered(string filter, string filterDate1, string filterDate2, string filterType)
        {
            throw new NotImplementedException();
        }
    }
}

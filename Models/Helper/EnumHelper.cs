using MSNK.Models.Modules.ViewModel;
using System.Collections.Generic;
using System;
using System.Linq;

namespace MSNK.Models.Helper
{
    public static class EnumHelper<T> where T : struct, Enum
    {
        public static List<ListItemViewModel> GetList()
        {
            List<T> values = Enum.GetValues(typeof(T)).Cast<T>().ToList();


            var resultList = new List<ListItemViewModel>();

            foreach (var item in values)
            {
                resultList.Add(new ListItemViewModel()
                {
                    id = item.GetDisplayCode(),
                    indek = item.GetDisplayCode(),
                    perihal = item.GetDisplayName().ToUpper()
                });
            }
            return resultList;
        }
    }
}

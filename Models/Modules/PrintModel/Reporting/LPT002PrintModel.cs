using MSNK.Models.Administration;
using System.Collections.Generic;

namespace MSNK.Models.Modules.PrintModel.Reporting
{
    public class LPT002PrintModel : PrintModel
    {
        public IEnumerable<AkBelian> AkBelian { get; set; }
    }
}

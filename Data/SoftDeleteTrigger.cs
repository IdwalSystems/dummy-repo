using EntityFrameworkCore.Triggered;
using Microsoft.EntityFrameworkCore;
using MSNK.Models.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MSNK.Data
{
    public class SoftDeleteTrigger : IBeforeSaveTrigger<ISoftDelete>
    {
        readonly ApplicationDbContext _dataContext;
        public SoftDeleteTrigger(ApplicationDbContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task BeforeSave(ITriggerContext<ISoftDelete> context, CancellationToken cancellationToken)
        {
            if (context.ChangeType == ChangeType.Deleted)
            {
                var entry = _dataContext.Entry(context.Entity);
                context.Entity.TarHapus = DateTime.UtcNow;
                context.Entity.FlHapus = true;
                entry.State = EntityState.Modified;
            }
            await Task.CompletedTask;
        }
    }
}

using KoalaBlog.Entity;
using KoalaBlog.Entity.Models;
using KoalaBlog.Principal;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoalaBlog.DAL
{
    public partial class KoalaBlogDbContext
    {
        private KoalaBlogIdentityObject CurrentIdentityObject
        {
            get
            {
                if(System.Threading.Thread.CurrentPrincipal.Identity is KoalaBlogIdentity)
                {
                    return (System.Threading.Thread.CurrentPrincipal.Identity as KoalaBlogIdentity).IdentityObject;
                }
                return null;
            }
        }

        public override int SaveChanges()
        {
            OnBeforeSave();

            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync()
        {
            OnBeforeSave();

            return base.SaveChangesAsync();
        }

        private void OnBeforeSave()
        {
            var addedAuditedEntities = ChangeTracker.Entries<EntityBase>().Where(x => x.State == EntityState.Added).Select(x => x.Entity);
            var modifiedAuditedEntities = ChangeTracker.Entries<EntityBase>().Where(x => x.State == EntityState.Modified).Select(x => x.Entity);

            foreach (var addedEntity in addedAuditedEntities)
            {
                addedEntity.CreatedBy = CurrentIdentityObject != null ? CurrentIdentityObject.UserID : 0;
                addedEntity.CreatedDate = DateTime.Now;
                addedEntity.LastModifiedBy = CurrentIdentityObject != null ? CurrentIdentityObject.UserID : 0;
                addedEntity.LastModifiedDate = DateTime.Now;
            }

            foreach (var modifiedEntity in modifiedAuditedEntities)
            {
                //exclude field
                Entry(modifiedEntity).Property(x => x.CreatedBy).IsModified = false;
                Entry(modifiedEntity).Property(x => x.CreatedDate).IsModified = false;

                modifiedEntity.LastModifiedBy = CurrentIdentityObject != null ? CurrentIdentityObject.UserID : 0;
                modifiedEntity.LastModifiedDate = DateTime.Now;
            }
        }
    }
}

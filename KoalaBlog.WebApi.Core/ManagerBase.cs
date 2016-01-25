using KoalaBlog.BLL.Handlers;
using KoalaBlog.Entity.Models;
using KoalaBlog.Principal;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoalaBlog.WebApi.Core
{
    public abstract class ManagerBase
    {
        /// <summary>
        /// Current Thread UserAccount Object
        /// </summary>
        protected KoalaBlogIdentityObject CurrentThreadIdentityObject
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

        /// <summary>
        /// Get the current UserProfile object from the UserAccount in the CurrentPrinciple
        /// </summary>
        /// <param name="dbContext">Must be KoalaBlogDbContext</param>
        /// <returns></returns>
        protected async Task<Person> CurrentUserPersonRecord(DbContext dbContext)
        {
            PersonHandler perHandler = new PersonHandler(dbContext);

            if (CurrentThreadIdentityObject != null)
            {
                return await perHandler.LoadByUserAccountIDAsync(CurrentThreadIdentityObject.UserID);
            }

            return null;
        }
    }
}

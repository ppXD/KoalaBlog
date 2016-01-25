using System;
using System.Security.Principal;

namespace KoalaBlog.Principal
{
    [Serializable]
    public class KoalaBlogPrincipal : IPrincipal
    {
        private IIdentity identity;

        public KoalaBlogPrincipal(IIdentity identity)
        {
            this.identity = identity;
        }

        public IIdentity Identity
        {
            get 
            {
                return identity as KoalaBlogIdentity;
            }
        }

        public bool IsInRole(string role)
        {
            return false;
        }
    }
}

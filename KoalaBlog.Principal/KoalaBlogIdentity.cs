using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using KoalaBlog.Framework.Extensions;
using KoalaBlog.Framework.Common;

namespace KoalaBlog.Principal
{
    [Serializable]
    public class KoalaBlogIdentity : IIdentity
    {
        private const string STATUS_ACTIVE = "Active";
        private const string STATUS_SUSPENDED = "Suspend";

        private readonly KoalaBlogIdentityObject _identityObject;

        public KoalaBlogIdentity(KoalaBlogIdentityObject identityObject)
        {
            this._identityObject = identityObject;
        }

        public string Name
        {
            get
            {
                return this._identityObject.UserName;
            }
        }

        public string AuthenticationType
        {
            get { return "KoalaBlogIdentity"; }
        }

        public bool IsAuthenticated
        {
            get
            {
                if (_identityObject != null)
                {
                    if (_identityObject.Status == STATUS_ACTIVE)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public KoalaBlogIdentityObject IdentityObject
        {
            get
            {
                return _identityObject;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoalaBlog.Framework.Filters
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class RequireRolesOrPermissionsAttribute : Attribute
    {
        public string[] RolesOrPermissionsName { get; set; }

        public RequireRolesOrPermissionsAttribute(params string[] rolesOrPermissionsName)
        {
            this.RolesOrPermissionsName = rolesOrPermissionsName;
        }
    }
}

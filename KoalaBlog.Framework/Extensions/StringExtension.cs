using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoalaBlog.Framework.Extensions
{
    public static class StringExtension
    {
        public static string Link(this string baseUrlPath, string additionalNode)
        {
            if (baseUrlPath == null)
            {
                return additionalNode;
            }
            if (additionalNode == null)
            {
                return baseUrlPath;
            }
            if (baseUrlPath.EndsWith("/", StringComparison.OrdinalIgnoreCase))
            {
                if (additionalNode.StartsWith("/", StringComparison.OrdinalIgnoreCase))
                {
                    baseUrlPath = baseUrlPath.TrimEnd(new char[] { '/' });
                }
                return baseUrlPath + additionalNode;
            }
            if (additionalNode.StartsWith("/", StringComparison.OrdinalIgnoreCase))
            {
                return baseUrlPath + additionalNode;
            }
            return baseUrlPath + "/" + additionalNode;
        }

        public static string Link(this string baseUrlPath, params object[] additionalNodes)
        {
            var temp = baseUrlPath;
            foreach (var item in additionalNodes)
            {
                if (item != null)
                {
                    temp = temp.Link(item.ToString());
                }
            }
            return temp;
        }
    }
}

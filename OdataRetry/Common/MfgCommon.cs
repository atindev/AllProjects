using System;
using System.Linq;
using System.Reflection;

namespace OdataRetry.Common
{
    public class MfgCommon
    {
        protected MfgCommon()
        {
        }

        public static Type[] GetTypesInNamespace(Assembly assembly = null, string nameSpace = "West.Manufacturing.Model")
        {
            if (assembly == null)
            {
                assembly = Assembly.Load("West.Manufacturing.Model");
            }
            return assembly.GetTypes()
             .Where(t => String.Equals(t.Namespace, nameSpace, StringComparison.Ordinal))
             .ToArray();
        }
    }
}

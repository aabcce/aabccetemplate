using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace AabcceTemplate.Engine
{
    /// <summary>
    /// Assembl集合
    /// </summary>
    [Serializable]
    public class AssemblyCollection : List<Assembly>
    {
        /// <summary>
        /// AssemblyCollection
        /// </summary>
        public AssemblyCollection()
        {
        }

        public Assembly Find(string strAssemblyName)
        {
            foreach (Assembly a in this)
            {
                if (a.Location == strAssemblyName)
                {
                    return (a);
                }
            }

            return (null);
        }

        public bool Exists(string strAssemblyName)
        {
            return (Find(strAssemblyName) != null);
        }
    }
}

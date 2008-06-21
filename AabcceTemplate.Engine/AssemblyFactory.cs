using System;
using System.Collections.Generic;
using System.Text;

using System.Reflection;

namespace AabcceTemplate.Engine
{
    public static class AssemblyFactory
    {
        private static AssemblyCollection objAssemblies;

        public static Assembly Get(string strAssembly)
        {
            if(!System.IO.File.Exists(strAssembly))
            {
                return(null);
            }

            if (objAssemblies == null)
            {
                objAssemblies = new AssemblyCollection();
            }

            System.Reflection.Assembly a = objAssemblies.Find(strAssembly);

            if(a == null)
            {
                a = Assembly.LoadFile(strAssembly);

                if(a != null)
                {
                    objAssemblies.Add(a);
                }
            }

            return(a);
        }
    }
}

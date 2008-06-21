using System;
using System.Collections.Generic;
using System.Text;

namespace AabcceTemplate.Engine.Runtime
{
    public class PropertyInfoCollection : List<PropertyInfo>
    {
        public PropertyInfoCollection()
        {
        }

        public PropertyInfo Find(string strName)
        {
            foreach (PropertyInfo info in this)
            {
                if (info.Name == strName)
                {
                    return (info);
                }
            }

            return (null);
        }

        public bool Exists(string strName)
        {
            return (Find(strName) != null);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace AabcceTemplate.Engine.Runtime
{
    /// <summary>
    /// 发现属性 (Property) 的属性 (Attribute) 并提供对属性 (Property) 元数据的访问
    /// </summary>
    public class PropertyInfo : MemberInfo
    {
        public PropertyInfo()
        {
            CanRead = true;
            CanWrite = true;
        }

        public bool CanRead;

        public bool CanWrite;

        private string strValue;

        public string Default;

        public string Value
        {
            get { return strValue; }
            set { strValue = value; }
        }


        public override string ToString()
        {
            string privateVar = " obj" + Name;

            string output = "private " + strDeclaringType + privateVar;

            if (!String.IsNullOrEmpty(Default))
            {
                output += " = " + Default.ToString();
            }

            output += ";\r\n";

            output += "public " + strDeclaringType + " " + Name + "{\r\n";

            if (CanRead)
            {
                output += "\tget {\r\n\t\treturn(" + privateVar + ");\r\n\t}\r\n";
            }

            if (CanWrite)
            {
                output += "\tset {\r\n\t\t" + privateVar + " = value;\r\n\t}\r\n";
            }

            output += "}";

            return output;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace AabcceTemplate.Engine.Runtime
{
    /// <summary>
    /// 字段属性 (Attribute) 并提供对字段元数据的访问权
    /// </summary>
    public class FieldInfo : MemberInfo
    {        
        private Type objFieldType;

        public Type FieldType
        {
            get
            {
                return (objFieldType);
            }
            set
            {
                objFieldType = value;
            }
        }

        private string strValue;

        public string Value
        {
            get { return strValue; }
            set { strValue = value; }
        }

        public override string ToString()
        {
            string output = BindingFlags.ToString().Replace(",", " ").ToLower();

            output += " " + strDeclaringType + " " + Name;

            if (!String.IsNullOrEmpty(strValue))
            {
                output += " = " + strValue;
            }
            output += ";";

            return output;
        }

    }
}

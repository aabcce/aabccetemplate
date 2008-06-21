using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;


namespace AabcceTemplate.Engine.Runtime
{
    /// <summary>
    /// ���ַ��������� (Attribute) ���ṩ�Է���Ԫ���ݵķ��ʡ�
    /// </summary>
    public class MethodInfo : MemberInfo
    {
        public MethodInfo()
        {
            objParameterInfoCollection = new ParameterInfoCollection();
        }

        public MethodInfo(string strCode)
        {
            objParameterInfoCollection = new ParameterInfoCollection();
        }

        private bool blnConstructor = false;
        /// <summary>
        /// �Ƿ��ǹ��캯��
        /// </summary>
        public bool IsConstructor
        {
            get { return blnConstructor; }
            set 
            { 
                blnConstructor = value;
                if (blnConstructor)
                {
                    this.BindingFlags = BindingFlagOption.Public;
                }
            }
        }

        private ParameterInfoCollection objParameterInfoCollection;

        public ParameterInfoCollection ParameterInfos
        {
            get { return objParameterInfoCollection; }
        }

        private string strBody;

        public string Body
        {
            get { return strBody; }
            set { strBody = value; }
        }

        public override string ToString()
        {
            string output = BindingFlags.ToString().Replace(",", " ").ToLower();

            if (!IsConstructor)
            {
                output += " " + strDeclaringType;
            }

            output += " " + Name + "(";

            foreach (ParameterInfo para in objParameterInfoCollection)
            {
                output += para.ToString() + ",";
            }

            if (objParameterInfoCollection.Count > 0)
            {
                output = output.Substring(0, output.Length - 1);
            }

            output += ")\r\n{\r\n\t";

            output += Body;

            output += "\r\n}";

            return output;
        }
    }
}

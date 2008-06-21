using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace AabcceTemplate.Engine.Runtime
{
    /// <summary>
    /// ��ȡ�йس�Ա���Ե���Ϣ���ṩ�Գ�ԱԪ���ݵķ���
    /// </summary>
    public class MemberInfo
    {
        protected string strName;

        public string Name
        {
            get { return strName; }
            set { strName = value; }
        }

        protected string strDeclaringType;

        public string DeclaringType
        {
            get
            {
                return (strDeclaringType);
            }
            set
            {
                strDeclaringType = value;
            }
        }

        protected MemberTypes objMemberType;

        public MemberTypes MemberType
        {
            get { return objMemberType; }
            set { objMemberType = value; }
        }

        protected Runtime.BindingFlagOption objBindingFlags;

        public Runtime.BindingFlagOption BindingFlags
        {
            get { return objBindingFlags; }
            set { objBindingFlags = value; }
        }
    }
}

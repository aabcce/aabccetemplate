using System;
using System.Collections.Generic;
using System.Text;

namespace AabcceTemplate.Engine.Runtime
{
    public class ParameterInfo
    {
        /// <summary>
        /// ����������
        /// </summary>
        public ParameterAttributes AttrsImpl;

        /// <summary>
        /// ������ Type
        /// </summary>
        public string ClassImpl;

        /// <summary>
        /// ������
        /// </summary>
        public string NameImpl;

        /// <summary>
        /// �����б��в������㿪ʼ��λ��
        /// </summary>
        public int PositionImpl;

        public override string ToString()
        {
            string output = "";

            if (AttrsImpl != ParameterAttributes.Default)
            {
                output += AttrsImpl.ToString() + " ";
            }
            output += ClassImpl + " " + NameImpl;

            return output;
        }
    }
}

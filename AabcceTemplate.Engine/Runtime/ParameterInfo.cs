using System;
using System.Collections.Generic;
using System.Text;

namespace AabcceTemplate.Engine.Runtime
{
    public class ParameterInfo
    {
        /// <summary>
        /// 参数的属性
        /// </summary>
        public ParameterAttributes AttrsImpl;

        /// <summary>
        /// 参数的 Type
        /// </summary>
        public string ClassImpl;

        /// <summary>
        /// 参数名
        /// </summary>
        public string NameImpl;

        /// <summary>
        /// 参数列表中参数从零开始的位置
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

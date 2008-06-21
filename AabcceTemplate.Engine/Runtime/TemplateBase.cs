using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace AabcceTemplate.Engine.Runtime
{
    public abstract class TemplateBase : Type
    {
        private StringBuilder __OUTPUT__;

        public TemplateBase()
        {
            Namespace = "AabcceTemplate";

            Name = "Template_" + Net.Zhenlong.Common.Text.StringUtility.GetNewSerial(6);

            FullName = Namespace + "." + Name;
            
            Assemblies = new StringCollection();

            Assemblies.Add(System.Reflection.Assembly.GetAssembly(typeof(System.String)).Location);

            __ConstructMethod = new MethodInfo();

            __ConstructMethod.IsConstructor = true;

            __ConstructMethod.BindingFlags = BindingFlagOption.Public;

            __ConstructMethod.Name = Name;

            this.MethodInfos.Add(__ConstructMethod);
            
            //__OUTPUT__ = new StringBuilder();

            FieldInfo field = new FieldInfo();

            field.BindingFlags = Runtime.BindingFlagOption.Private;

            field.DeclaringType = "System.Text.StringBuilder";

            field.Name = "__OUTPUT__";

            field.Value = "new System.Text.StringBuilder()";

            FieldInfos.Add(field);

            __OutputMethod = new Runtime.MethodInfo();

            __OutputMethod.DeclaringType = "System.Text.StringBuilder";

            __OutputMethod.BindingFlags = Runtime.BindingFlagOption.Public;

            __OutputMethod.Name = "Output";

            __OutputMethod.Body = "return(__OUTPUT__);";

            this.MethodInfos.Add(__OutputMethod);
        }


        public LanguageType Language;

        public TargetLanguageType TargetLanguage;

        public string OutputAssembly;

        public StringCollection Assemblies;

        /// <summary>
        /// 没有语法解析过的函数体
        /// </summary>
        public string UnAnalyseBody;

        /// <summary>
        /// 输出的方法
        /// </summary>
        public MethodInfo __ConstructMethod;

        /// <summary>
        /// 输出的方法
        /// </summary>
        public MethodInfo __OutputMethod;

        public override string ToString()
        {
            string output = base.ToString();

            if (UnAnalyseBody.Length > 0)
            {
                int anchor = output.LastIndexOf("}", output.Length - 2)-1;

                output = output.Substring(0, anchor) + "\r\n" + UnAnalyseBody + "\r\n" + output.Substring(anchor);
            }

            return output;
        }


    }
}

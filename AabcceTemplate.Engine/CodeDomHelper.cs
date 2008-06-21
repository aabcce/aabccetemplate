using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Text;

using System.Reflection;
using System.CodeDom;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using Microsoft.VisualBasic;
using Microsoft.JScript;

namespace AabcceTemplate.Engine
{
    public class CodeDomHelper
    {
        private LanguageType objLanguage;
        private string strSource;

        private StringCollection objAssemblyStrings;

        private AssemblyCollection objAssemblies;

        public CodeDomHelper()
        {
            objAssemblyStrings = new StringCollection();
            objAssemblies = new AssemblyCollection();
        }

        public CodeDomHelper(LanguageType objLanguage, string strSource, StringCollection objAssemblyStrings)
        {
            this.objLanguage = objLanguage;
            this.strSource = strSource;
            this.objAssemblyStrings = objAssemblyStrings;
        }

        public CodeDomHelper(LanguageType objLanguage, string strSource, AssemblyCollection objAssemblies)
        {
            this.objLanguage = objLanguage;
            this.strSource = strSource;
            this.objAssemblies = objAssemblies;
        }

        public LanguageType Language
        {
            get
            {
                return objLanguage;
            }
            set
            {
                objLanguage = value;
            }
        }

        public string Source
        {
            get
            {
                return strSource;
            }
            set
            {
                strSource = value;
            }
        }

        public StringCollection Assemblies
        {
            get
            {
                return objAssemblyStrings;
            }
            set
            {
                objAssemblyStrings = value;
            }
        }

        public void AddAssembly(Assembly assembly)
        {
            objAssemblies.Add(assembly);
        }

        private CodeDomProvider GetCurrentProvider( LanguageType objLanguage)
        {
            CodeDomProvider provider;
            switch (objLanguage)
            {
                case LanguageType.CSharp:
                    provider = new CSharpCodeProvider();
                    break;
                case LanguageType.VBNET:
                    provider = new VBCodeProvider();
                    break;
                case LanguageType.JScript:
                    provider = new JScriptCodeProvider();
                    break;
                default:
                    provider = new CSharpCodeProvider();
                    break;
            }
            return provider;
        }

        public Assembly Execute()
        {
            CodeDomProvider provider = GetCurrentProvider(objLanguage);

            CompilerParameters cp = new CompilerParameters();

            //if (objAssemblies == null)
            //{
            //    objAssemblies = new AssemblyCollection();
            //}

            //if (objAssemblyStrings != null)
            //{
            //    foreach (string s in objAssemblyStrings)
            //    {
            //        if (!objAssemblies.Exists(s))
            //        {
            //            objAssemblies.Add(AssemblyFactory.Get(s));
            //        }
            //    }
            //}

            if (objAssemblyStrings != null)
            {
                foreach (string s in objAssemblyStrings)
                {
                    cp.ReferencedAssemblies.Add(s);
                }
            }

            if (objAssemblies != null)
            {
                foreach (Assembly a in objAssemblies)
                {
                    cp.ReferencedAssemblies.Add(a.Location);
                }
            }

            cp.CompilerOptions = "/t:library";
            cp.GenerateExecutable = false;
            cp.OutputAssembly = "temp_" + Net.Zhenlong.Common.Text.StringUtility.GetNewSerial(16) + ".dll";
            cp.TreatWarningsAsErrors = false;

            CompilerResults compiler_results = provider.CompileAssemblyFromSource(cp,
                (
                 new string[1] { strSource }
                ));

            if (compiler_results.Errors.HasErrors)
            {
                StringBuilder sb = new StringBuilder();
                foreach (CompilerError err in compiler_results.Errors)
                    sb.Append(err.ErrorText + "\n");
                throw new Exception.CompileException("Template Compilation Error.\n" + sb.ToString());
            }
            else
            {
                Assembly assembly = compiler_results.CompiledAssembly;

                return (assembly);
            }

            return (null);
        }
    }
}

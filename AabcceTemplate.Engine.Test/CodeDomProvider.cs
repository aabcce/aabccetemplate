using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Microsoft.CSharp;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Reflection;

namespace AabcceTemplate.Engine.Test
{
    /// <summary>
    /// UnitTest1 的摘要说明
    /// </summary>
    [TestClass]
    public class CodeDomProvider
    {
        public CodeDomProvider()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        #region 其他测试属性
        //
        // 您可以在编写测试时使用下列其他属性:
        //
        // 在运行类中的第一个测试之前使用 ClassInitialize 运行代码
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // 在类中的所有测试都已运行之后使用 ClassCleanup 运行代码
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // 在运行每个测试之前使用 TestInitialize 运行代码 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // 在运行每个测试之后使用 TestCleanup 运行代码
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void CodeDomProvider_FirstTest()
        {
            Microsoft.CSharp.CSharpCodeProvider provider = new Microsoft.CSharp.CSharpCodeProvider();

            CompilerParameters cp = new CompilerParameters();
                        
            //cp.ReferencedAssemblies.Add("system.dll");
            foreach (Assembly a in System.AppDomain.CurrentDomain.GetAssemblies())
            {
                cp.ReferencedAssemblies.Add(a.Location);
            }

            //cp.ReferencedAssemblies.Add(Assembly.GetAssembly(typeof(NTemplate.Driver)).Location);

            cp.CompilerOptions = "/t:library";
            cp.GenerateExecutable = false;
            cp.OutputAssembly = "D:\\test.dll";
            cp.TreatWarningsAsErrors = false;

            CompilerResults compiler_results = provider.CompileAssemblyFromSource(cp,
                (/*compile_units.ToArray() as CodeCompileUnit[]*/

                 new string[1]{@"
using System;

namespace PagesstyleUpdate
{
    public class Program
    {
        public int SomeMethod()
        {
            return(123);
        }
    }
}

"}
                ));

            if (compiler_results.Errors.HasErrors)
            {
                StringBuilder sb = new StringBuilder();
                foreach (CompilerError err in compiler_results.Errors)
                    sb.Append(err.ErrorText + "\n");
                throw new ApplicationException("Template Compilation Error.\n" + sb.ToString());
            }
            else
            {
                //AppDomain domain = AppDomain.CreateDomain("Compiled App");
                //domain.ExecuteAssembly(compiler_results.PathToAssembly);
                Assembly assembly = compiler_results.CompiledAssembly;

                Type certernType = assembly.GetType("PagesstyleUpdate.Program");

                if (certernType == null) return;

                Object myObj = System.Activator.CreateInstance(certernType);

                int i = Convert.ToInt32(certernType.InvokeMember("SomeMethod", 
                    BindingFlags.InvokeMethod, null, myObj/*null*/
                , null, System.Globalization.CultureInfo.CurrentCulture));


            }

            //Assembly templates_assembly = compiler_results.CompiledAssembly;
            //return templates_assembly;
        }
    }
}

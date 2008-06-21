using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AabcceTemplate.Engine.Test.Execute
{
    /// <summary>
    /// UnitTest1 的摘要说明
    /// </summary>
    [TestClass]
    public class UnitTest1
    {
        public UnitTest1()
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
        public void Execute_Template()
        {
            AabcceEngine engione = new AabcceEngine(System.Text.UTF8Encoding.UTF8.GetString(Resource_Execute.SimpleTpl));

            engione.SetProperty("SourceTable", "1111111111111111111111111111111111111111111111");

            string s = engione.Execute();
            
            Console.WriteLine(s);
        }

        [TestMethod]
        public void Execute_Template_Sample_With_Property()
        {
            AabcceEngine engione = new AabcceEngine(System.Text.UTF8Encoding.UTF8.GetString(Resource_Execute.Sample_With_Property));

            engione.SetProperty("SourceTable", "1111111111111111111111111111111111111111111111");

            List<string> lst = new List<string>();

            lst.Add("sssssssssss1");
            lst.Add("sssssssssss2");
            lst.Add("sssssssssss3");
            lst.Add("sssssssssss4");
            lst.Add("sssssssssss5");
            lst.Add("sssssssssss61");
            lst.Add("sssssssssss7");
            lst.Add("sssssssssss8");
            lst.Add("lst9");

            engione.SetProperty("lstString", lst);

            string s = engione.Execute();

            Console.WriteLine(s);
        }
    }
}

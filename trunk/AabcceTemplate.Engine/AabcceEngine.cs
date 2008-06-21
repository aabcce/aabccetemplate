using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace AabcceTemplate.Engine
{
    /// <summary>
    /// 解析的引擎
    /// </summary>
    [Serializable]
    public class AabcceEngine
    {
        private Template template;

        private Dictionary<string, object> objProperties;

        /// <summary>
        /// AabcceEngine
        /// </summary>
        public AabcceEngine()
        {
            objProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// AabcceEngine
        /// </summary>
        /// <param name="strSource"></param>
        public AabcceEngine(string strSource)
        {
            template = new Template(strSource);

            objProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Load
        /// </summary>
        /// <param name="strSource"></param>
        public void Load(string strSource)
        {
            template = new Template(strSource);

            objProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// LoadFile
        /// </summary>
        /// <param name="strSource"></param>
        public void LoadFile(string strFile)
        {
            template = Template.LoadFromFile(strFile);

            objProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// LoadFromFile
        /// </summary>
        /// <param name="strFile"></param>
        /// <returns></returns>
        public void LoadFromFile(string strFile)
        {
            template = Template.LoadFromFile(strFile);

            objProperties = new Dictionary<string, object>();
        }

        public void SetProperty(string key, object value)
        {
            objProperties.Add(key, value);
        }

        private string tempCode;

        private string tempParse;
       
        /// <summary>
        /// Execute
        /// </summary>
        /// <returns></returns>
        public string Execute()
        {
            if (template == null)
            {
                return ("");
            }

            if (String.IsNullOrEmpty(tempCode))
            {
                tempCode = template.ToString();
            }

            if (String.IsNullOrEmpty(tempParse))
            {
                CompileSource();
            }

            return (tempParse);
        }

        /// <summary>
        /// CompileSource
        /// </summary>
        /// <returns></returns>
        private void CompileSource()
        {
            if (String.IsNullOrEmpty(tempCode))
            {
                return;
            }

            CodeDomHelper helper = new CodeDomHelper(template.Language,tempCode, template.Assemblies);

            Assembly assembly = helper.Execute();

            Type outputType = assembly.GetType(template.FullName);

            Object templateObject = System.Activator.CreateInstance(outputType);

            foreach (string key in objProperties.Keys)
            {
                Runtime.PropertyInfo info = template.PropertyInfos.Find(key);
                if (info != null && info.CanWrite)
                {
                    outputType.InvokeMember(key,BindingFlags.SetProperty,null,templateObject,
                        new object[1]{objProperties[key]}, System.Globalization.CultureInfo.CurrentCulture);
                }
            }

            Object objOutput = outputType.InvokeMember("Output",
                    BindingFlags.InvokeMethod, null, templateObject
                , null, System.Globalization.CultureInfo.CurrentCulture);

            if (objOutput != null)
            {
                tempParse = Convert.ToString(objOutput);
            }
            else
            {
                tempParse = "";
            }
        }
    }
}

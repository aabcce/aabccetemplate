using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Text;

namespace AabcceTemplate.Engine.Runtime
{
    /// <summary>
    /// Type
    /// </summary>
    public class Type
    {
        public Type()
        {
            Imports = new StringCollection();

            Imports.Add("System");
            Imports.Add("System.Collections");
            Imports.Add("System.Collections.Generic");
            Imports.Add("System.Text");
            //Imports.Add("System.Xml");
            //Imports.Add("System.Data");
            //Imports.Add("System.Data.Common");

            BindingFlags = Runtime.BindingFlagOption.Public;

            objFieldInfoCollection = new FieldInfoCollection();

            objPropertyInfoCollection = new PropertyInfoCollection();

            objMethodInfoCollection = new MethodInfoCollection();
        }
        
        public StringCollection Imports;

        public Runtime.BindingFlagOption BindingFlags;

        public string FullName;

        public string Namespace;

        public string Name;
                
        private FieldInfoCollection objFieldInfoCollection;

        public FieldInfoCollection FieldInfos
        {
            get { return objFieldInfoCollection; }
            set { objFieldInfoCollection = value; }
        }

        private PropertyInfoCollection objPropertyInfoCollection;
        /// <summary>
        /// 
        /// </summary>
        public PropertyInfoCollection PropertyInfos
        {
            get { return objPropertyInfoCollection; }
            set { objPropertyInfoCollection = value; }
        }

        private MethodInfoCollection objMethodInfoCollection;

        public MethodInfoCollection MethodInfos
        {
            get { return objMethodInfoCollection; }
            set { objMethodInfoCollection = value; }
        }

        public override string ToString()
        {
            StringBuilder outputBuilder = new StringBuilder();

            foreach (string a in Imports)
            {
                outputBuilder.Append("using " + a + ";\r\n");
            }

            outputBuilder.Append("namespace " + Namespace + "\r\n{\r\n\t");

            outputBuilder.Append(BindingFlags.ToString().ToLower().Replace(","," "));

            outputBuilder.Append(" class " + Name + "\r\n\t\t{\r\n\t\t\t");

            foreach (FieldInfo field in FieldInfos)
            {
                outputBuilder.Append(field.ToString() + "\r\n");
            }

            foreach (PropertyInfo property in PropertyInfos)
            {
                outputBuilder.Append(property.ToString() + "\r\n");
            }

            foreach (MethodInfo member in MethodInfos)
            {
                outputBuilder.Append(member.ToString() + "\r\n");
            }

            outputBuilder.Append("\t}\r\n");

            outputBuilder.Append("}");


            return outputBuilder.ToString();
        }
	
    }
}

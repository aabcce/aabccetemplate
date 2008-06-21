using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace AabcceTemplate.Engine
{
    /// <summary>
    /// 模板文件
    /// </summary>
    public class Template : Runtime.TemplateBase
    {
        public Template()
        {
        }

        public Template(string strSource)
        {

            if (String.IsNullOrEmpty(strSource))
            {
                return;
            }

            //Filter Comment <%--  --%>
            strSource = Regex.Replace(strSource, @"<%--[^(--%>)]*?--%>", "", 
                RegexOptions.Multiline | RegexOptions.IgnoreCase);

            Dictionary<int, int> objDistance = new Dictionary<int, int>();

            #region Filter <%@ CodeTemplate
            {
                strSource = Regex.Replace(strSource, @"<%@[\s]* [\s]*CodeTemplate[\s]* ([^(%>)]*?)%>",
                   delegate(Match match)
                   {
                       Dictionary<string, string> dicAttributes = this.AnalyseAttributes(match.Groups[1].Value);

                       AnalyseCodeTemplateTag(dicAttributes);

                       return ("");
                   },
                RegexOptions.Multiline | RegexOptions.IgnoreCase);

                if(this.Language == LanguageType.Unsupported)
                {
                    throw (new Exception.ParseException("Template Parse Error:No Language Spcificed"));
                }
            }
            #endregion

            #region Filter <%@ 预编译指令
            {
                strSource = Regex.Replace(strSource, @"<%@[\s]* [\s]*(\w+)[\s]* [\s]*([\s\S]*?)(?=%>)%>",
                   delegate(Match match)
                   {
                       string strPrecompiledHeader = match.Groups[1].Value.Trim();

                       Dictionary<string, string> dicAttributes = this.AnalyseAttributes(match.Groups[2].Value.Trim());

                       if (strPrecompiledHeader == "Property")
                       {
                           AnalyseProperty(dicAttributes);
                       }
                       else if (strPrecompiledHeader == "Assembly")
                       {
                           AnalyseAssembly(dicAttributes);
                       }
                       else if (strPrecompiledHeader == "Import")
                       {
                           AnalyseImport(dicAttributes);
                       }

                       return ("");
                   },
                RegexOptions.Multiline | RegexOptions.IgnoreCase);

                if(this.Language == LanguageType.Unsupported)
                {
                    throw (new Exception.ParseException("Template Parse Error:No Language Spcificed"));
                }
            }
            #endregion
            
            #region Filter <script runat="template"> <script[\s]* [^(>)]*runat[\s ]*=[\s ]*(?:"template"|'template'|template)[^(>)]*>[\s\S]*?(?=</script>)</script>
            {
                strSource = Regex.Replace(strSource, @"<script[\s]* [^(>)]*runat[\s ]*=[\s ]*(?:""template""|'template'|template)[^(>)]*>([\s\S]*?)(?=</script>)</script>",
                   delegate(Match match)
                   {
                       string strNestSection = match.Groups[1].Value.Trim();

                       return (AnalyseNestSection(strNestSection));
                   },
                RegexOptions.Multiline | RegexOptions.IgnoreCase);
            }
            #endregion

            Dictionary<string, string> dicConst;
            
            strSource = FilterConst(strSource, out dicConst); //以后的分析不能被常量里的数据破坏

            strSource = TransOutputSyntax(strSource);

            #region Filter <%=
            {
                strSource = Regex.Replace(strSource, @"<%=([\s\S]*?)(?=%>)%>",
                   delegate(Match match)
                   {
                       string strNestSection = match.Groups[1].Value.Trim();

                       while (strNestSection.EndsWith(";"))
                       {
                           strNestSection = strNestSection.Substring(0, strNestSection.Length - 1).Trim();
                       }

                       return (String.Concat("<% __OUTPUT__.Append(", strNestSection, "); %>"));
                   },
                RegexOptions.Multiline | RegexOptions.IgnoreCase);
            }
            #endregion

            //只剩下 <%  %>　了
            strSource = AnalyseAspTagContent(strSource, dicConst);
            
            strSource = FillConst(strSource, dicConst);

            __OutputMethod.Body = strSource + "\r\n" + __OutputMethod.Body;

        }

        private string AnalyseNestSection(string strNestSection)
        {
            //提取出公共方法和变量

            Dictionary<string, string> dicConst;

            strNestSection = FilterConst(strNestSection, out dicConst);

            strNestSection = TransOutputSyntax(strNestSection);

            while (true)
            {
                Regex objRegex = new Regex(@"\)\s*\{",RegexOptions.IgnoreCase | RegexOptions.Multiline);

                Match match = objRegex.Match(strNestSection);

                if (!match.Success)
                {
                    break;
                }

                int intStart = 0;
                int intEnd = 0;

                intStart = strNestSection.LastIndexOf(";", match.Index) + 1;

                intEnd = strNestSection.IndexOf("}",match.Index);

                while (intEnd > intStart)
                {
                    int b = strNestSection.LastIndexOf("{",intEnd) + 1;
                    if (b > match.Index + match.Length)  //说明有嵌套
                    {
                        intEnd = strNestSection.IndexOf("}", intEnd);

                        if (intEnd == -1)
                        {
                            throw (new Exception.ParseException("Syntax Parse Error"));
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                if (intEnd < intStart)
                {
                    throw (new Exception.ParseException("Syntax Parse Error"));
                }

                string strMethodSection = strNestSection.Substring(intStart, intEnd + 1 - intStart).Trim();

                strMethodSection = FillConst(strMethodSection, dicConst);

                this.UnAnalyseBody += FillConst(strMethodSection,dicConst);

                //this.MemberInfos.Add(new AabcceTemplate.Engine.Runtime.MethodInfo(strMethodSection));

                strNestSection = strNestSection.Substring(0, intStart) + strNestSection.Substring(intEnd  + 1);
            }

            return("<% " + strNestSection + " %>");

            //strNestSection = strNestSection.Trim();

            //Dictionary<string, string> dicConst;

            //strNestSection = FilterConst(strNestSection, out dicConst);
            
            //strNestSection = TransformResponse(strNestSection);

            //strNestSection = FillConst(strNestSection, dicConst);
            
            //UnAnalyseBody += strNestSection;
        }

        private string TransOutputSyntax(string strSection)
        {
             strSection = Regex.Replace(strSection, @"Response\.Write",
                delegate(Match match)
                {
                    return ("__OUTPUT__.Append");
                },
                System.Text.RegularExpressions.RegexOptions.Multiline | System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            return(strSection);
        }

        //private string TransformResponse(string strSection)
        //{
        //    Dictionary<string, string> dicConst;

        //    strSection = FilterConst(strSection, out dicConst);

        //    strSection = Regex.Replace(strSection, @"Response\.Write",
        //        delegate(Match match)
        //        {
        //            return ("__OUTPUT__.Append");
        //        },
        //        System.Text.RegularExpressions.RegexOptions.Multiline | System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        //    strSection = FillConst(strSection, dicConst);
        //}


        /// <summary>
        /// 分析&lg;% %&gt;标签内的内容
        /// </summary>
        /// <param name="strSource"></param>
        /// <returns></returns>
        private string AnalyseAspTagContent(string strSource)
        {
            return (AnalyseAspTagContent(strSource, null));
        }

        /// <summary>
        /// 分析&lg;% %&gt;标签内的内容
        /// </summary>
        /// <param name="strSource"></param>
        /// <returns></returns>
        private string AnalyseAspTagContent(string strSource,Dictionary<string,string> dicConst)
        {

            strSource = Regex.Replace(strSource, @"%>([\s\S]*?)(?=<%)<%",
               delegate(Match match)
               {
                   string htmlContent = match.Groups[1].Value.Trim();

                   if (!String.IsNullOrEmpty(htmlContent))
                   {
                       return ("\r\n__OUTPUT__.Append(@\"" + 
                           (dicConst == null?htmlContent:FillConst(htmlContent, dicConst)).Replace("\"", "\"\"") + 
                           "\");");
                   }
                   else
                   {
                       return ("");
                   }
               },
            RegexOptions.Multiline | RegexOptions.IgnoreCase);

            int startTag = strSource.IndexOf("<%");

            if (startTag > -1)
            {
                strSource = String.Concat("\r\n__OUTPUT__.Append(@\"", strSource.Substring(0, startTag).Trim().Replace("\"", "\"\""), "\");", strSource.Substring(startTag + 2));
            }

            int endTag = strSource.LastIndexOf("%>");

            if (endTag > -1)
            {
                strSource = String.Concat(strSource.Substring(0, endTag), "\r\n__OUTPUT__.Append(@\"", strSource.Substring(endTag + 2).Trim().Replace("\"", "\"\""), "\");");
            }

            return (strSource);
        }

        /// <summary>
        /// 提取常量
        /// </summary>
        /// <param name="strSesction"></param>
        /// <param name="dicConst"></param>
        /// <returns></returns>
        private string FilterConst(string strSesction, out Dictionary<string, string> dicConst)
        {
            int intCount = 0;
            Dictionary<string, string> t = new Dictionary<string, string>();;

            strSesction = System.Text.RegularExpressions.Regex.Replace(strSesction, @"("""")|(""[\s\S]*?[^\\]"")",
            delegate(System.Text.RegularExpressions.Match match)
            {
                t.Add("__CONST" + intCount + "__", match.Groups[0].Value.Trim());

                return ("__CONST" + intCount++ + "__");
            },
            System.Text.RegularExpressions.RegexOptions.Multiline | System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            dicConst = t;

            return (strSesction);
        }

        /// <summary>
        /// 回写常量
        /// </summary>
        /// <param name="strSesction"></param>
        /// <param name="dicConst"></param>
        /// <returns></returns>
        private string FillConst(string strSesction, Dictionary<string, string> dicConst)
        {
            foreach (string key in dicConst.Keys)
            {
                strSesction = strSesction.Replace(key, dicConst[key]);
            }

            return (strSesction);
        }

        private void AnalyseImport(Dictionary<string, string> dicAttributes)
        {
            foreach (string s in dicAttributes.Keys)
            {
                if (s == "Namespace")
                {
                    string attribute = TrimAttribute(dicAttributes[s].Trim());
                    bool blnExists = false;
                    foreach (string t in Imports)
                    {
                        if (t == attribute)
                        {
                            blnExists = true;
                            break;
                        }
                    }

                    if (!blnExists)
                    {
                        Imports.Add(attribute);
                    }
                }
            }
        }

        private void AnalyseAssembly(Dictionary<string, string> dicAttributes)
        {
            foreach (string s in dicAttributes.Keys)
            {
                if (s == "Name")
                {
                    string attribute = TrimAttribute(dicAttributes[s].Trim());
                    bool blnExists = false;
                    foreach (string t in Assemblies)
                    {
                        if (t == attribute)
                        {
                            blnExists = true;
                            break;
                        }
                    }

                    if (!blnExists)
                    {
                        Assemblies.Add(attribute);
                    }
                }
            }
        }

        private string TrimAttribute(string a)
        {
            a = a.Trim();
            if (a.StartsWith("\"") && a.EndsWith("\""))
            {
                a = a.Substring(1, a.Length - 2);
            }
            if (a.StartsWith("'") && a.EndsWith("'"))
            {
                a = a.Substring(1, a.Length - 2);
            }

            return (a);

        }

        private void AnalyseProperty(Dictionary<string, string> dicAttributes)
        {
            Runtime.PropertyInfo pi = new AabcceTemplate.Engine.Runtime.PropertyInfo();


            foreach (string key in dicAttributes.Keys)
            {
                if (key == "Name")
                {
                    pi.Name = TrimAttribute(dicAttributes[key]);
                }
                else if (key == "Type")
                {
                    pi.DeclaringType = TrimAttribute(dicAttributes[key]);
                }
                else if(key == "Default")
                {
                    pi.Default = TrimAttribute(dicAttributes[key]);
                }
            }

            if (String.IsNullOrEmpty(pi.Name) || String.IsNullOrEmpty(pi.DeclaringType))
            {
                throw (new Exception.ParseException("Property Syntax Error"));
            }

            if (pi.DeclaringType == "System.String" || pi.DeclaringType == "string")
            {
                pi.Default = "\"" + pi.Default + "\"";
            }

            PropertyInfos.Add(pi);
        }

        /// <summary>
        /// 分析 AnalyseCodeTemplateTag 属性
        /// </summary>
        /// <param name="dicAttributes"></param>
        private void AnalyseCodeTemplateTag(Dictionary<string, string> dicAttributes)
        {
            foreach (string key in dicAttributes.Keys)
            {
                if (key == "Language")
                {
                    string lang = dicAttributes[key].ToLower();
                    if (lang != "c#" && lang != "csharp")
                    {
                        Language = LanguageType.CSharp;
                    }
                    else if (lang != "vb.net" && lang != "vbnet" && lang != "vbdotnet" && lang != "visualbasic")
                    {
                        Language = LanguageType.CSharp;
                    }
                    else if (lang != "jscript" && lang != "jscript.net")
                    {
                        Language = LanguageType.CSharp;
                    }
                    else if (lang != "j#" && lang != "jsharp")
                    {
                        Language = LanguageType.JScript;
                    }
                }
                else if (key == "TargetLanguage")
                {
                    TargetLanguage = TargetLanguageType.Text;
                }
            }
        }

        /// <summary>
        /// 分析属性值
        /// </summary>
        /// <param name="attributes"></param>
        /// <returns></returns>
        private Dictionary<string, string> AnalyseAttributes(string attributes)
        {
            attributes = attributes.Trim();

            attributes = Regex.Replace(attributes, @"[\s ]* [\s ]*", " ", 
                RegexOptions.Multiline | RegexOptions.IgnoreCase);

            Dictionary<string, string> dicAttributes = new Dictionary<string, string>();

            foreach (string a in attributes.Split(' '))
            {
                string[] arr = a.Split('=');

                if (arr.Length == 2)
                {
                    dicAttributes.Add(arr[0].Trim(), arr[1].Trim());
                }
            }

            return (dicAttributes);
        }

        public static Template LoadFromFile(string strFile)
        {
            if (!File.Exists(strFile))
            {
                throw(new FileNotFoundException());
            }

            return (new Template(File.ReadAllText(strFile)));
        }

        public override string ToString()
        {
            return (base.ToString());
        }
    }
}

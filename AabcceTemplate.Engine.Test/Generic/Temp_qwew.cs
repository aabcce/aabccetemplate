﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
namespace AabcceTemplate
{
    public class Template_871122
    {
        private System.Text.StringBuilder __OUTPUT__ = new System.Text.StringBuilder();
        private System.String objSourceTable = "SourceTable";
        public System.String SourceTable
        {
            get
            {
                return (objSourceTable);
            }
            set
            {
                objSourceTable = value;
            }
        }
        public Template_871122()
        {

        }
        public System.Text.StringBuilder Output()
        {

            __OUTPUT__.Append(@"// This class generated by CodeSmith on"); __OUTPUT__.Append(DateTime.Now.ToLongDateString());
            __OUTPUT__.Append(@"""This is some static content (just like the static HTML in a ASP page)."""); __OUTPUT__.Append("This is dynamic content using an expression  %> write construct."); __OUTPUT__.Append(SomeStringMethod()); SomeRenderMethod(); __OUTPUT__.Append(SourceTable);
            __OUTPUT__.Append(@"This is more static content."); __OUTPUT__.Append("This is some static content (just like the static HTML in a ASP page).");
            __OUTPUT__.Append(@"<script >
function SomeStringMethod()
{
	alert(""MM/dd/yyyy"");
}

</script>"); __OUTPUT__.Append(SomeStringMethod()); SomeRenderMethod(); int count = 10;

            string a = "safasdfasd";

            if (count != 10)
            {

                __OUTPUT__.Append(@"now the count is 10");
            }
            else
            {

                __OUTPUT__.Append(@"now the count is"); __OUTPUT__.Append(a);
                __OUTPUT__.Append(@";");
            }
            for (int i = 0; i < count; i++)
            {

                __OUTPUT__.Append(@"output line"); __OUTPUT__.Append(i);
            }

            __OUTPUT__.Append(@"");
            return (__OUTPUT__);
        }

        string SomeStringMethod()
        {
            return "Today's date: " + DateTime.Now.ToString("MM/dd/yyyy");
        }
        void SomeRenderMethod()
        {
            __OUTPUT__.Append("This is 'more dynamic c\'ontent usi'ng a render method.");
            __OUTPUT__.Append("Hello " + System.Environment.UserName + "!");
        }
    }
}
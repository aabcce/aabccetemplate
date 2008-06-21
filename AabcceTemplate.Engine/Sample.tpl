<%@ CodeTemplate Language="C#" TargetLanguage="Text" Description="This template demonstrates using template script blocks." %>
<%-- 
This template demonstrates using template script blocks.
--%>

<%@ Property Name="SourceTable" Type="System.String" Default="SourceTable" Description="The table to use for this sample." %>

<%@ Property Name="SampleClass" Type="System.Sample.SampleClass" Default="Null" Description="The table to use for this sample." %>

<%@ Assembly Name="SchemaExplorer" %>
<%@ Import Namespace="SchemaExplorer" %>

// This class generated by CodeSmith on <%= DateTime.Now.ToLongDateString() %>

"This is some static content (just like the static HTML in a ASP page)."
<%-- 
This template demonstrates using template script blocks.
--%>
<%= "This is dynamic content using an expression  %> write construct." %>

<%= SomeStringMethod() %>

<% SomeRenderMethod(); %>

<%= SourceTable%>

<%= SampleClass.SampleMethod("a",SourceTable)  %>

This is more static content.

<script runat="template">

Response.Write("This is 'more dynamic c\'ontent usi'ng a render method.");

string SomeStringMethod()
{
	return "Today's date: " + DateTime.Now.ToString("MM/dd/yyyy");
}

void SomeRenderMethod()
{
	Response.Write("This is 'more dynamic c\'ontent usi'ng a render method.");
	Response.Write("Hello " + System.Environment.UserName + "!");
}
</script>

<script >
function SomeStringMethod()
{
	alert("MM/dd/yyyy");
}

</script>

<%= SomeStringMethod() %>

<% SomeRenderMethod(); %>

<%  int count = 10; %>

<%  

string a = "safasdfasd";

if (i=10) 
{
%>
now the count is 10
<%  
}else {
%>
now the count is unknown
<%  
}
%>



<%  for (int i=0;i< count ;i++) 
{
%>
output line <%=i%>
<%  
}
%>
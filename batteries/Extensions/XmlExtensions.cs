using System.Xml;

namespace batteries.Extensions;

public static class XmlExtensions
{
    public static string ExtractValueFromXmlQuery(this string xml, string query)
    {
        try
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            var node = xmlDoc.SelectSingleNode(query);
            if(node!=null)
                return node.InnerText;
        }
        catch (Exception _)
        {
            //suppress exceptions here
        }
        return string.Empty;
    }
}
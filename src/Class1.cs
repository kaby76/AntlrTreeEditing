using System;
using System.Collections.Generic;
using System.Text;
using java.util;

namespace java.net
{
    public class URI
    {
        private string @string;

        public URI(string @string)
        {
            this.@string = @string;
        }

        public static URI create(string uri)
        {
            throw new NotImplementedException();
        }

        public bool Absolute { get; set; }

        public URI resolve(string uri)
        {
            throw new NotImplementedException();
        }
    }

    public class MalformedURLException : Exception
    {
    }

}

namespace java.time
{
    public class Duration { }
}

namespace java.util
{
    public class GregorianCalendar : Calendar
    {
        public static int BC;

        public GregorianCalendar(object getTimeZone)
        {
            throw new NotImplementedException();
        }
    }

    public abstract class Calendar
    {
        public static int YEAR { get; internal set; }
        public static int ERA { get; set; }
        public static int MONTH { get; set; }
        public static int DAY_OF_MONTH { get; set; }
        public static int MILLISECOND { get; set; }

        public Calendar clone()
        {
            throw new NotImplementedException();
        }

        public void AddHours(int hours)
        {
            throw new NotImplementedException();
        }

        public void AddMinutes(int minutes)
        {
            throw new NotImplementedException();
        }

        public int get(int year)
        {
            throw new NotImplementedException();
        }

        public long getTimeInMillis()
        {
            throw new NotImplementedException();
        }

        public void add(int month, int monthValue)
        {
            throw new NotImplementedException();
        }
    }

    public class TimeZone
    {
        public static object getTimeZone(string gmt)
        {
            throw new NotImplementedException();
        }
    }
}

namespace java.xml
{
    public class XMLConstants
    {
        public static string NULL_NS_URI { get; set; }
        public static string DEFAULT_NS_PREFIX { get; set; }
    }
}

namespace javax.xml.datatype
{
    public class Duration
    {
        public Duration negate()
        {
            throw new NotImplementedException();
        }
    }

    public class XMLGregorianCalendar
    {
        public void add(Duration negate)
        {
            throw new NotImplementedException();
        }

        public Calendar toGregorianCalendar()
        {
            throw new NotImplementedException();
        }
    }
}
namespace javax.xml.@namespace
{
    public class NamespaceContext
    {
        public string getNamespaceURI(string prefix)
        {
            throw new NotImplementedException();
        }
    }

    public class QName
    {
        private string v1;
        private string v2;
        private string v3;

        public QName(string v1, string v2, string v3)
        {
            this.v1 = v1;
            this.v2 = v2;
            this.v3 = v3;
        }

        public string NamespaceURI { get; set; }
        public string LocalPart { get; set; }
        public string Prefix { get; set; }
    }

}

namespace org.apache.xerces.xs
{
    public interface XSModel
    {
    }
}

namespace org.apache.xerces.dom
{
    public class PSVIElementNSImpl { }
}

namespace org.w3c.dom
{
    
    public class NodeConstants
    {
        public const short TEXT_NODE = 1;
        public const short ELEMENT_NODE = 2;
        public const short DOCUMENT_NODE = 3;
        public const short COMMENT_NODE = 4;
        public const short ATTRIBUTE_NODE = 5;
        public const short CDATA_SECTION_NODE = 6;
        public const short PROCESSING_INSTRUCTION_NODE = 7;
        public const short DOCUMENT_POSITION_PRECEDING = 8;
        public const short DOCUMENT_POSITION_FOLLOWING = 9;
    }

    public interface Node
    {
        short ELEMENT_NODE { get; set; }
        short NodeType { get; set; }
        string LocalName { get; set; }
        short DOCUMENT_NODE { get; set; }
    }

    public interface Attr : Node
    {
    }

    public interface Document : Node
    {
        string DocumentURI { get; set; }
    }

    public interface Element : Node
    {
        object getAttributeNS(string sCHEMA_INSTANCE, string nIL_ATTRIBUTE);
    }

    interface NodeList { }

    interface Text { }

    interface TypeInfo { }

}



using System;
using System.Collections.Generic;
using System.Text;

namespace java.net
{
    public class URI
    {
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
        public GregorianCalendar(object getTimeZone)
        {
            throw new NotImplementedException();
        }
    }

    public class Calendar
    {
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
    public class Duration { }

    public class XMLGregorianCalendar { }
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
    public interface Node
    {
        short TEXT_NODE { get; set; }
        short ELEMENT_NODE { get; set; }
        short NodeType { get; set; }
        string LocalName { get; set; }
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



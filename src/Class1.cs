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

namespace javax.xml.datatype
{
    public class Duration { }

    public class XMLGregorianCalendar { }
}
namespace javax.xml.@namespace
{
    public class NamespaceContext { }

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
    public class Node
    {
        internal static short TEXT_NODE;

        public short NodeType { get; internal set; }
    }

    public class Document
    {
        public string DocumentURI;
    }

    public class Attr { }

    public class Element : Node
    {
        internal object getAttributeNS(string sCHEMA_INSTANCE, string nIL_ATTRIBUTE)
        {
            throw new NotImplementedException();
        }
    }

    interface NodeList { }

    interface Text { }

    interface TypeInfo { }

}



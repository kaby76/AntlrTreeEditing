﻿namespace AntlrTreeEditing.AntlrDOM
{
    using org.w3c.dom;
    using System;

    public class AntlrDocument : AntlrNode, Document
    {
        public string DocumentURI { get; set; }
        public NodeList getElementsByTagNameNS(string ns, string local)
        {
            throw new NotImplementedException();
        }

        public bool isSupported(string core, string s)
        {
            throw new NotImplementedException();
        }
    }
}

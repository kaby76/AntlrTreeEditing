# AntlrTreeEditing

This library contains a collection of routines for editing
Antlr parse trees. As noted by Parr, Antlr4 does not have
methods to do tree transformations. Parr felt that editing
trees was not necessary because the focus was parsing, information
extraction, and translation. For information extraction, Antlr
includes a rudimentary XPath engine, but it's a farcry from
XPath1 let alone XPath3. For translation, Parr assumes you would
be using string templates, and something like ASF+SDF in the future.
Antlr does itself perform some tree rewriting, but it does so
"manually".

This library tries to fill in some practical methods for
searching and editing parse trees. It contains:

* a repurposed and clean up port of the Eclipse XPath2 for use with
Antlr parse trees;
* a tree construction method from a s-expression abbreviation;
* node replace and delete methods.
* an observer parse tree node so as to allow notification of observers
when a parse tree changes.

grammar TextAdvLang; // NOTE: This is a default ANTLR4 file that has yet to be edited as ANTLR is not currently used.
r  : 'hello' ID ;         // match keyword hello followed by an identifier
ID : [a-z]+ ;             // match lower-case identifiers
WS : [ \t\r\n]+ -> skip ; // skip spaces, tabs, newlines

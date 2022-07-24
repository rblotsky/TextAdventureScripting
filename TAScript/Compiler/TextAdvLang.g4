grammar TextAdvLang; // NOTE: This is a default ANTLR4 file that has yet to be edited as ANTLR is not currently used.

/*
 * PARSER RULES
 */
blocktext: (WORD*) ('['WORD*']')? (WORD*);
prompt: PROMPT_CHAR+ blocktext RETURN_CHAR?;
option: OPTION_CHAR+ blocktext RETURN_CHAR?;

/*
 * LEXER RULES
 */
fragment LOWERCASE: [a-z];
fragment UPPERCASE: [A-Z];
WHITESPACE : [ \t\r]+ -> skip;
NEWLINE : ('\r'? '\n' | '\r')+;
PROMPT_CHAR : '-';
OPTION_CHAR : '>';
RETURN_CHAR : '~';
BLOCK_SEP_START: '[';
BLOCK_SEP_END: ']';
WORD: '[^[]@$#->~]*';


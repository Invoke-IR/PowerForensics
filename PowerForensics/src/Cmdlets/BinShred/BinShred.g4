grammar BinShred;

options {
    language = CSharp;
}

template
    :   templateEntry templateEntry* EOF
    ;

templateEntry
    :   parseRule
    |   lookupTable
    ;

parseRule
    :   label COLON ruleBody ruleBody* SEMI
    ;

ruleBody
    :   (   DOC_COMMENT?
            (   (label LPAREN sizeReference ITEMS RPAREN)
            |   (label (LPAREN ruleOptions RPAREN)?)
            )
        )
    |   (LPAREN ADDITIONAL PROPERTIES IDENTIFIED BY propertyName FROM lookupTableName RPAREN)
    ;

propertyName    : label;
lookupTableName : label;

ruleOptions
    :   byteOption
    ;

byteOption
    :   sizeReference BYTES (AS byteFormat (DESCRIBED BY label)?)?
    ;

sizeReference
    :   label
    |   INT
    ;

byteFormat
    :   ASCII
    |   UNICODE
    |   UTF8
    |   UINT64
    |   UINT32
    |   UINT16
    |   INT64
    |   INT32
    |   INT16
    |   SINGLE
    |   FLOAT
    |   DOUBLE
    ;

lookupTable
    :   lookupTableName COLON lookupTableEntry lookupTableEntry* SEMI
    ;

lookupTableEntry
    :   DOC_COMMENT?
        (    lookupTableEntryKey (COMMA lookupTableEntryKey)* COLON QUOTEDVALUE
        |    lookupTableEntryKey (COMMA lookupTableEntryKey)* COLON label
        )
    ;

lookupTableEntryKey
    :   label
    |   INT
    |   HEXADECIMAL
    ;

label
    : LABEL
    | BYTES
    | ITEMS
    | AS
    | DESCRIBED
    | BY
    | ASCII
    | UNICODE
    | UTF8
    | UINT64
    | UINT32
    | UINT16
    | INT64
    | INT32
    | INT16
    | SINGLE
    | FLOAT
    | DOUBLE
    | ADDITIONAL
    | PROPERTIES
    | IDENTIFIED
    | FROM
    ;

QUOTEDVALUE  : QUOTE .+? QUOTE          ;

COMMA        : ','                      ;
QUOTE        : '"'                      ;
COLON        : ':'                      ;
SEMI         : ';'                      ;
LPAREN       : '('                      ;
RPAREN       : ')'                      ;
BYTES        : B Y T E S?               ;
ITEMS        : I T E M S                ;
AS           : A S                      ;
DESCRIBED    : D E S C R I B E D        ;
BY           : B Y                      ;
ASCII        : A S C I I                ;
UNICODE      : U N I C O D E            ;
UTF8         : U T F '8'                ;
UINT64       : U I N T '64'             ;
UINT32       : U I N T '32'             ;
UINT16       : U I N T '16'             ;
INT64        : I N T '64'               ;
INT32        : I N T '32'               ;
INT16        : I N T '16'               ;
SINGLE       : S I N G L E              ;
FLOAT        : F L O A T                ;
DOUBLE       : D O U B L E              ;
ADDITIONAL   : A D D I T I O N A L      ;
PROPERTIES   : P R O P E R T I E S      ;
IDENTIFIED   : I D E N T I F I E D      ;
FROM         : F R O M                  ;

WS  :    [ \t\r\n\f]+ -> channel(HIDDEN)    ;

DOC_COMMENT
    :    '/**' .*? '*/'
    ;

BLOCK_COMMENT
    :    '/*' .*? '*/'  -> channel(HIDDEN)
    ;

LINE_COMMENT
    :    '//' ~[\r\n]*  -> channel(HIDDEN)
    ;

INT          : [0-9]+                   ;
HEXADECIMAL  : '0'('x'|'X')[0-9a-fA-F]+ ;

LABEL
    : [a-zA-Z_][.a-zA-Z0-9_]*
    ;

fragment A:('a'|'A');
fragment B:('b'|'B');
fragment C:('c'|'C');
fragment D:('d'|'D');
fragment E:('e'|'E');
fragment F:('f'|'F');
fragment G:('g'|'G');
fragment H:('h'|'H');
fragment I:('i'|'I');
fragment J:('j'|'J');
fragment K:('k'|'K');
fragment L:('l'|'L');
fragment M:('m'|'M');
fragment N:('n'|'N');
fragment O:('o'|'O');
fragment P:('p'|'P');
fragment Q:('q'|'Q');
fragment R:('r'|'R');
fragment S:('s'|'S');
fragment T:('t'|'T');
fragment U:('u'|'U');
fragment V:('v'|'V');
fragment W:('w'|'W');
fragment X:('x'|'X');
fragment Y:('y'|'Y');
fragment Z:('z'|'Z');
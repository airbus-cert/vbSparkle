/* Copyright Sylvain Bruyere - Airbus CERT */

grammar VBPreprocessors; 

// module ----------------------------------

startRule
   : NEWLINE* codeBlock* EOF?
   ;

macroConst:
   MACRO_CONST IDENTIFIER WS? EQ WS? valueStmt
;

codeBlock: lineLabel* codeBlockBody commentBlock? (NEWLINE+|EOF) ;
 
codeBlockBody
   : macroIfThenElseStmt   #vmacroIf
   | macroConst            #vmacroConst
   | commentBlock          #vcommentBlock 
   | nonMacroCodeBlockX    #vcodeBlock
   | lineLabel             #vlineLabel
;

nonMacroCodeBlockX
   : nonMacroCodeBlockLine+
;
 
moduleInfo:
	moduleReferences? NEWLINE* 
	controlProperties? NEWLINE* 
	moduleConfig? NEWLINE* 
	moduleAttributes? NEWLINE* 
   ;


moduleReferences
   : moduleReference+
   ;

moduleReference
   : OBJECT WS? EQ WS? moduleReferenceValue (SEMICOLON WS? moduleReferenceComponent)? NEWLINE*
   ;

moduleReferenceValue
   : STRINGLITERAL
   ;

moduleReferenceComponent
   : STRINGLITERAL
   ;

moduleHeader
   : VERSION WS DOUBLELITERAL (WS CLASS)?
   ;

moduleConfig
   : BEGIN NEWLINE + moduleConfigElement + END NEWLINE +
   ;

moduleConfigElement
   : ambiguousIdentifier WS? EQ WS? literal NEWLINE
   ;
ambiguousIdentifier
   : (IDENTIFIER ) +
   | L_SQUARE_BRACKET (IDENTIFIER) + R_SQUARE_BRACKET
   ;

moduleAttributes
   : (attributeStmt NEWLINE +) +
   ;


macroIfThenElseStmt
   : macroIfBlockStmt macroElseIfBlockStmt* macroElseBlockStmt? MACRO_END_IF
   ;

macroIfBlockCondStmt:
   MACRO_IF WS ifConditionStmt WS THEN
;

macroIfBlockStmt
   : macroIfBlockCondStmt WS? commentBlock* NEWLINE + 
      codeBlock*
   ;

macroElseIfBlockStmt
   : MACRO_ELSEIF WS ifConditionStmt WS THEN WS? commentBlock* NEWLINE + 
      codeBlock*
   ;

macroElseBlockStmt
   : MACRO_ELSE WS? commentBlock* NEWLINE +
      codeBlock*
   ;

commentBlock:
   COMMENT+;// NEWLINE*;

lineLabel
     : LABEL_L
     ; 

nonMacroCodeBlockLine:
   nonMacroCodeStmt COMMENT?;

nonMacroCodeStmt:
   ~(MACRO_IF|MACRO_ELSE|MACRO_ELSEIF|MACRO_END_IF|MACRO_CONST|COMMENT|NEWLINE)+;
 
// xNoMacroBlock:
//    anytoken+ NL?
// ;

// controls ----------------------------------

controlProperties
	: WS? BEGIN WS cp_ControlType WS cp_ControlIdentifier WS? NEWLINE+ cp_Properties+ END NEWLINE*
	;

cp_Properties
	// : cp_SingleProperty
	: cp_NestedProperty
	| controlProperties;

// cp_SingleProperty
// 	: WS? implicitCallStmt_InStmt WS? EQ WS? '$'? cp_PropertyValue FRX_OFFSET? NEWLINE+
// 	;


cp_NestedProperty
	: WS? BEGINPROPERTY WS ambiguousIdentifier (LPAREN INTEGERLITERAL RPAREN)? (WS GUID)? NEWLINE+ (cp_Properties+)? ENDPROPERTY NEWLINE+
	;

cp_ControlType
	: complexType
	;
complexType
   : ambiguousIdentifier (DOT ambiguousIdentifier)*
   ;

cp_ControlIdentifier
	: ambiguousIdentifier
	;

// block ----------------------------------

attributeStmt
   : ATTRIBUTE WS ambiguousIdentifier WS? EQ WS? literal (WS? COMMA WS? literal)*
   ;



ifConditionStmt
   : valueStmt
   ;


// operator precedence is represented by rule order
//https://stackoverflow.com/questions/25413460/what-should-the-correct-grammar-be-for-correct-precedence-evaluation-of
valueStmt
   : valUnary=literal                                                                     # vsLiteral 
   | valUnary=ambiguousIdentifier                                                         # vsConstant
   | LPAREN WS? valUnary=valueStmt (WS? COMMA WS? valueStmt)* WS? RPAREN                  # vsStruct
   | operator=PLUS WS? valUnary=valueStmt                                                 # vsUnaryOperation
   | operator=MINUS WS? valUnary=valueStmt                                                # vsUnaryOperation

   | left=valueStmt WS? operator=POW WS? right=valueStmt                                 # vsDualOperation

   | left=valueStmt WS? operator=(MULT | DIV | MOD) WS? right=valueStmt                  # vsDualOperation
   | left=valueStmt WS? operator=(PLUS | MINUS) WS? right=valueStmt                      # vsDualOperation 

   | left=valueStmt WS? operator=AMPERSAND WS? right=valueStmt                           # vsDualOperation
   | left=valueStmt WS? operator=EQ WS? right=valueStmt                                  # vsDualOperation
   | left=valueStmt WS? operator=NEQ WS? right=valueStmt                                 # vsDualOperation
   | left=valueStmt WS? operator=LT WS? right=valueStmt                                  # vsDualOperation
   | left=valueStmt WS? operator=GT WS? right=valueStmt                                  # vsDualOperation
   | left=valueStmt WS? operator=LEQ WS? right=valueStmt                                 # vsDualOperation
   | left=valueStmt WS? operator=GEQ WS? right=valueStmt                                 # vsDualOperation
   | left=valueStmt WS? operator=AND WS? right=valueStmt                                 # vsDualOperation
   | left=valueStmt WS? operator=OR WS? right=valueStmt                                  # vsDualOperation
   | left=valueStmt WS? operator=XOR WS? right=valueStmt                                 # vsDualOperation
   | left=valueStmt WS? operator=EQV WS? right=valueStmt                                 # vsDualOperation
   | left=valueStmt WS? operator=IMP WS? right=valueStmt                                 # vsDualOperation
   | left=valueStmt WS operator=LIKE WS right=valueStmt                                  # vsDualOperation
   | left=valueStmt WS operator=IS WS right=valueStmt                                    # vsDualOperation
   | operator=NOT (WS valUnary=valueStmt | LPAREN WS? valUnary=valueStmt WS? RPAREN)	 # vsUnaryOperation
   ;
 

// atomic rules ----------------------------------


delimitedLiteral
   : HEXLITERAL         #ltColor
   | OCTALLITERAL       #ltOctal
   | DATELITERAL        #ltDate
   | STRINGLITERAL      #ltString
   ;

literal
   : DOUBLELITERAL      #ltDouble
   | delimitedLiteral   #ltDelimited
  // | FILENUMBER         #ltFilenumber
   | INTEGERLITERAL     #ltInteger
   | TRUE               #ltBoolean
   | FALSE              #ltBoolean
 //  | NOTHING            #ltNothing
 //  | NULL               #ltNull
   ;

//block
//   : noMacroBlock+ (NEWLINE+ WS? noMacroBlock)*
//  ;

//noMacroBlock
//   : noMacroLine WS* (NEWLINE+ WS? noMacroLine NEWLINE *)* NEWLINE *
//;


anytoken:
   ~NEWLINE;


MACRO_CONST:
   HASH CONST WS
   ;

fragment DOUBLE_DOT:
   ':'
;

fragment NL      
   : '\r'? '\n' 
   | '\r'
   | EOF;

AND
   : A N D
   ;


ATTRIBUTE
   : A T T R I B U T E
   ;



BEGIN
   : B E G I N
   ;


BEGINPROPERTY
   : B E G I N P R O P E R T Y
   ;


CLASS
   : C L A S S
   ;


CONST
   : C O N S T
   ;

ELSE
   : E L S E
   ;


ELSEIF
   : E L S E I F
   ;


END_IF
   : E N D ' ' I F
   ;



END
   : E N D
   ;


ENDPROPERTY
   : E N D P R O P E R T Y
   ;


EQV
   : E Q V
   ;


FALSE
   : F A L S E
   ;


IF
   : I F
   ;


IMP
   : I M P
   ;



IS
   : I S
   ;


LIKE
   : L I K E
   ;

MACRO_IF
   : HASH IF
   ;


MACRO_ELSEIF
   : HASH ELSEIF
   ;


MACRO_ELSE
   : HASH ELSE
   ;


MACRO_END_IF
   : HASH END_IF
   ;

MOD
   : M O D
   ;

NOT
   : N O T
   ;


NOTHING
   : N O T H I N G
   ;


NULL
   : N U L L
   ;

OBJECT
   : O B J E C T
   ;

OR
   : O R
   ;

THEN
   : T H E N
   ;


TRUE
   : T R U E
   ;


VERSION
   : V E R S I O N
   ;


XOR
   : X O R
   ;

// symbols

AMPERSAND
   : '&'
   ;


AT
   : '@'
   ;

 
// COLON
//    : ':'
//    ;


COMMA
   : ','
   ;


DIV
   : '\\' | '/'
   ;

DOT
   : '.'
   ;


EQ
   : '='
   ;


EXCLAMATIONMARK
   : '!'
   ;


GEQ
   : '>='
   ;


GT
   : '>'
   ;


HASH
   : '#'
   ;


LEQ
   : '<='
   ;

LBRACE
	: '{'
	;


LPAREN
   : '('
   ;


LT
   : '<'
   ;


MINUS
   : '-'
   ;



MULT
   : '*'
   ;


NEQ
   : '<>'
   ;



PLUS
   : '+'
   ;


POW
   : '^'
   ;


RBRACE
	: '}'
	;


RPAREN
   : ')'
   ;


SEMICOLON
   : ';'
   ;


L_SQUARE_BRACKET
   : '['
   ;


R_SQUARE_BRACKET
   : ']'
   ;

// literals

STRINGLITERAL
   : '"' (~ ["\r\n] | '""')* '"'
   ;


DATELITERAL
   : HASH (~ [#\r\n])* HASH
   ;


HEXLITERAL
   : ('&H' | '&h') [0-9A-F] + AMPERSAND?
   ;


INTEGERLITERAL
   : ('0' .. '9') + (('e' | 'E') INTEGERLITERAL)* (HASH | AMPERSAND | EXCLAMATIONMARK | AT)?
   //: (PLUS | MINUS)? ('0' .. '9') + (('e' | 'E') INTEGERLITERAL)* (HASH | AMPERSAND | EXCLAMATIONMARK | AT)?
   ;


DOUBLELITERAL
   //: (PLUS | MINUS)? ('0' .. '9')* DOT ('0' .. '9') + (('e' | 'E') (PLUS | MINUS)? ('0' .. '9') +)* (HASH | AMPERSAND | EXCLAMATIONMARK | AT)?
   : ('0' .. '9')* DOT ('0' .. '9') + (('e' | 'E') (PLUS | MINUS)? ('0' .. '9') +)* (HASH | AMPERSAND | EXCLAMATIONMARK | AT)?
   
;



FILENUMBER
   : HASH LETTERORDIGIT +
   ;

OCTALLITERAL
   //: (PLUS | MINUS)? '&O' [0-7] + AMPERSAND?
   : '&O' [0-7] + AMPERSAND?
   ;
   
// misc
// FRX_OFFSET
// 	: COLON [0-9A-F]+
// 	;

GUID
	: LBRACE [0-9A-F]+ MINUS [0-9A-F]+ MINUS [0-9A-F]+ MINUS [0-9A-F]+ MINUS [0-9A-F]+ RBRACE
	;

// identifier


NEWLINE
   : WS? NL WS?
   //: WS? ('\r'? '\n' | COLON ' '?) WS?
   ;


IDENTIFIER
   : LETTER LETTERORDIGIT*
   ;

// whitespace, line breaks, comments, ...


COMMENT
   : WS? ('\'' | ':'? REM ' ') (LINE_CONTINUATION | ~ ('\n' | '\r'))*
   ;

LINE_CONTINUATION
   : ' ' '_' '\r'? '\n' -> channel(HIDDEN)
   ;


WS
   : [ \t] +
   ;

// letters

// blockline:
//  fullline+
// // ;
// fullline:
//    ~( '\r' | '\n' | EOF )+;

// aNOMACROLINE:
//     NL ~'#' anytoken*? NL;

ANYCHAR:
    .;

REM
   : R E M
   ;
LABEL_L:
    WS? IDENTIFIER WS? DOUBLE_DOT WS?;

fragment LETTER
   : [a-zA-Z_äöüÄÖÜáéíóúÁÉÍÓÚâêîôûÂÊÎÔÛàèìòùÀÈÌÒÙãẽĩõũÃẼĨÕŨçÇ]
   ;


fragment LETTERORDIGIT
   : [a-zA-Z0-9_äöüÄÖÜáéíóúÁÉÍÓÚâêîôûÂÊÎÔÛàèìòùÀÈÌÒÙãẽĩõũÃẼĨÕŨçÇ]
   ;

// case insensitive chars

fragment A
   : ('a' | 'A')
   ;


fragment B
   : ('b' | 'B')
   ;


fragment C
   : ('c' | 'C')
   ;


fragment D
   : ('d' | 'D')
   ;


fragment E
   : ('e' | 'E')
   ;


fragment F
   : ('f' | 'F')
   ;


fragment G
   : ('g' | 'G')
   ;


fragment H
   : ('h' | 'H')
   ;


fragment I
   : ('i' | 'I')
   ;


fragment J
   : ('j' | 'J')
   ;


fragment K
   : ('k' | 'K')
   ;


fragment L
   : ('l' | 'L')
   ;


fragment M
   : ('m' | 'M')
   ;


fragment N
   : ('n' | 'N')
   ;


fragment O
   : ('o' | 'O')
   ;


fragment P
   : ('p' | 'P')
   ;


fragment Q
   : ('q' | 'Q')
   ;


fragment R
   : ('r' | 'R')
   ;


fragment S
   : ('s' | 'S')
   ;


fragment T
   : ('t' | 'T')
   ;


fragment U
   : ('u' | 'U')
   ;


fragment V
   : ('v' | 'V')
   ;


fragment W
   : ('w' | 'W')
   ;


fragment X
   : ('x' | 'X')
   ;


fragment Y
   : ('y' | 'Y')
   ;


fragment Z
   : ('z' | 'Z')
   ;
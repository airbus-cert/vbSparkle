/*
* Copyright (C) 2017, Ulrich Wolffgang <ulrich.wolffgang@proleap.io>
* All rights reserved.
*
* This software may be modified and distributed under the terms
* of the MIT license. See the LICENSE file for details.
*/
 
/* 
* Visual Basic 6.0 Grammar for ANTLR4
*
* This is a Visual Basic 6.0 grammar, which is part of the Visual Basic 6.0
* parser at https://github.com/uwol/vb6parser.
*
* The grammar is derived from the Visual Basic 6.0 language reference
* http://msdn.microsoft.com/en-us/library/aa338033%28v=vs.60%29.aspx
* and has been tested with MSDN VB6 statements as well as several Visual
* Basic 6.0 code repositories.
*/   
 
grammar VBScript;
// module ----------------------------------

startRule
   : module  EOF
   //:implicitCallStmt_InBlock EOF
   ;  

module:  //VbModule
	WS? 
	NEWLINE* 
	(moduleHeader NEWLINE +)?
	moduleReferences? NEWLINE* 
	controlProperties? NEWLINE* 
	moduleConfig? NEWLINE* 
	moduleAttributes? NEWLINE* 
	moduleOptions? NEWLINE* 
	moduleBody? NEWLINE* WS?
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

moduleAttributes
   : (attributeStmt NEWLINE +) +
   ;

moduleOptions
   : (moduleOption NEWLINE +) +
   ;

moduleOption 
   : OPTION_BASE WS INTEGERLITERAL # optionBaseStmt
   | OPTION_COMPARE WS (BINARY | TEXT) # optionCompareStmt
   | OPTION_EXPLICIT # optionExplicitStmt
   | OPTION_PRIVATE_MODULE # optionPrivateModuleStmt
   ;

moduleBody //VbModule
   : moduleBodyElement WS? ((NEWLINE+ | INLINE_SEP+) WS? moduleBodyElement)*
   ;
  
moduleBodyElement // foreach (var moduleBodyElement in propertiesContext.moduleBodyElement())
   : moduleBlock     // AddGlobalCode(..)
   | moduleOption    // TODO: AddOption(..)
   | declareStmt     // AddDeclare(..)
   | classStmt       // AddClass(..)
   | enumerationStmt // AddEnumeration(..)
   | eventStmt       // TODO: AddEvent(..)
   | functionStmt    // AddFunction(..)
   | propertyGetStmt // AddPropertyGet(..)
   | propertySetStmt // AddPropertyLet(..)
   | propertyLetStmt // AddPropertySet(..)
   | subStmt         // AddSub(..)
   | typeStmt        // AddType(..)
   ;

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

cp_PropertyName
	: (OBJECT DOT)? ambiguousIdentifier (LPAREN literal RPAREN)? (DOT ambiguousIdentifier (LPAREN literal RPAREN)?)*
	;

cp_PropertyValue
    : DOLLAR? (literal | (LBRACE ambiguousIdentifier RBRACE) | POW ambiguousIdentifier)
    ;

cp_NestedProperty
	: WS? BEGINPROPERTY WS ambiguousIdentifier (LPAREN INTEGERLITERAL RPAREN)? (WS GUID)? NEWLINE+ (cp_Properties+)? ENDPROPERTY NEWLINE+
	;

cp_ControlType
	: complexType
	;

cp_ControlIdentifier
	: ambiguousIdentifier
	;

// block ----------------------------------

  
blockSwitch
   :  lineLabel | (lineLabel? (inlineBlock | blockStmt))
   ;

block
   : blockSwitch (NEWLINE+ WS? blockSwitch)*
   ;
moduleBlock
   : block
   ;
 
attributeStmt
   : ATTRIBUTE WS implicitCallStmt_InStmt WS? EQ WS? literal (WS? COMMA WS? literal)*
   ;

lineLabel
    : ambiguousIdentifier WS? INLINE_SEP WS?
    ; 


inlineBlock
   //:  (inlineBlockStmt (INLINE_SEP inlineBlockStmt | INLINE_SEP)*  INLINE_SEP?) 
   : inlineBlockStmt | (inlineBlockStmt? (INLINE_SEP inlineBlockStmt | INLINE_SEP)+  INLINE_SEP?)
   //:  (inlineBlockStmt INLINE_SEP)? ((INLINE_SEP inlineBlockStmt?)+) 
   //:  INLINE_SEP? ((INLINE_SEP inlineBlockStmt?)+) 
   ; 

// block
//    : (inlineBlock | blockStmt) (NEWLINE+ WS? (inlineBlock | blockStmt))*
//    ;


inlineBlockStmt
   : appActivateStmt
   | attributeStmt
   | beepStmt
   | chDirStmt
   | chDriveStmt
   | closeStmt
   | constStmt
   | dateStmt
   | deleteSettingStmt
   | deftypeStmt
   | doLoopStmt
   | endStmt
   | eraseStmt
   | errorStmt
   | exitStmt
   | explicitCallStmt
   | filecopyStmt
   | forEachStmt
   | forNextStmt
   | getStmt
   | goSubStmt
   | goToStmt
   | inlineIfThenElseStmt
   | implementsStmt
   | inputStmt
   | killStmt
   | midStmt
   | letStmt
   | lineInputStmt 
   | loadStmt
   | lockStmt
   | lsetStmt
   | macroIfThenElseStmt
   | mkdirStmt
   | nameStmt
   | onErrorStmt
   | onGoToStmt
   | onGoSubStmt
   | openStmt
   | printStmt
   | putStmt
   | raiseEventStmt
   | randomizeStmt
   | redimStmt
   | resetStmt
   | resumeStmt
   | returnStmt
   | rmdirStmt
   | rsetStmt
   | savepictureStmt
   | saveSettingStmt
   | seekStmt
   | selectCaseStmt
   | sendkeysStmt
   | setattrStmt
   | setStmt
   | stopStmt
   | timeStmt
   | unloadStmt
   | unlockStmt
   | variableStmt
   | whileWendStmt
   | widthStmt
   | withStmt
   | writeStmt
   | implicitCallStmt_InStmt
   | implicitCallStmt_InBlock
   ;

blockStmt
   : appActivateStmt
   | attributeStmt
   | beepStmt
   | chDirStmt
   | chDriveStmt
   | closeStmt
   | constStmt
   | dateStmt
   | deleteSettingStmt
   | deftypeStmt
   | doLoopStmt
   | endStmt
   | eraseStmt
   | errorStmt
   | exitStmt
   | explicitCallStmt
   | filecopyStmt
   | forEachStmt
   | forNextStmt
   | getStmt
   | goSubStmt
   | goToStmt
   | inlineIfThenElseStmt
   | ifThenElseStmt
   | implementsStmt
   | inputStmt
   | killStmt
   | midStmt
   | letStmt
   | lineInputStmt 
   | loadStmt
   | lockStmt
   | lsetStmt
   | macroIfThenElseStmt
   | mkdirStmt
   | nameStmt
   | onErrorStmt
   | onGoToStmt
   | onGoSubStmt
   | openStmt
   | printStmt
   | putStmt
   | raiseEventStmt
   | randomizeStmt
   | redimStmt
   | resetStmt
   | resumeStmt
   | returnStmt
   | rmdirStmt
   | rsetStmt
   | savepictureStmt
   | saveSettingStmt
   | seekStmt
   | selectCaseStmt
   | sendkeysStmt
   | setattrStmt
   | setStmt
   | stopStmt
   | timeStmt
   | unloadStmt
   | unlockStmt
   | variableStmt
   | whileWendStmt
   | widthStmt
   | withStmt
   | writeStmt
   | implicitCallStmt_InStmt
   | implicitCallStmt_InBlock
   ;

// statements ----------------------------------

appActivateStmt
   : APPACTIVATE WS valueStmt (WS? COMMA WS? valueStmt)?
   ;

beepStmt
   : BEEP
   ;

chDirStmt
   : CHDIR WS valueStmt
   ;

chDriveStmt
   : CHDRIVE WS valueStmt
   ;

closeStmt
   : CLOSE (WS valueStmt (WS? COMMA WS? valueStmt)*)?
   ;

constStmt
   : (publicPrivateGlobalVisibility WS)? CONST WS constSubStmt (WS? COMMA WS? constSubStmt)*
   ;

constSubStmt
   : ambiguousIdentifier typeHint? (WS asTypeClause)? WS? EQ WS? valueStmt
   ;

dateStmt
   : DATE WS? EQ WS? valueStmt
   ;

declareStmt
   : (visibility WS)? DECLARE WS (PTRSAFE WS)? (FUNCTION typeHint? | SUB) WS ambiguousIdentifier typeHint? WS LIB WS STRINGLITERAL (WS ALIAS WS STRINGLITERAL)? (WS? argList)? (WS asTypeClause)?
   ;

deftypeStmt
   : (DEFBOOL | DEFBYTE | DEFINT | DEFLNG | DEFCUR | DEFSNG | DEFDBL | DEFDEC | DEFDATE | DEFSTR | DEFOBJ | DEFVAR) WS letterrange (WS? COMMA WS? letterrange)*
   ;

deleteSettingStmt
   : DELETESETTING WS valueStmt WS? COMMA WS? valueStmt (WS? COMMA WS? valueStmt)?
   ;

doLoopStmt
   : DO (NEWLINE|INLINE_SEP)+ (block (NEWLINE|INLINE_SEP)+)? LOOP                                     #dlStatement
   | DO WS (WHILE | UNTIL) WS? valueStmt (NEWLINE|INLINE_SEP)+ (block (NEWLINE|INLINE_SEP)+)? LOOP     #dwlStatement
   | DO (NEWLINE|INLINE_SEP)+ (block (NEWLINE|INLINE_SEP)+) LOOP WS (WHILE | UNTIL) WS? valueStmt      #dlwStatement
   ;

endStmt
   : END
   ;

enumerationStmt
   : (publicPrivateVisibility WS)? ENUM WS ambiguousIdentifier (NEWLINE|INLINE_SEP)+ (enumerationStmt_Constant)* END_ENUM
   ;
	
classStmt
   : (publicPrivateVisibility WS)? CLASS WS ambiguousIdentifier (NEWLINE|INLINE_SEP)+ (moduleBody (NEWLINE|INLINE_SEP)+)? END_CLASS
   ;

enumerationStmt_Constant
   : ambiguousIdentifier (WS? EQ WS? valueStmt)? (NEWLINE|INLINE_SEP)+
   ;

eraseStmt
   : ERASE WS valueStmt (WS? COMMA WS? valueStmt)*
   ;

errorStmt
   : ERROR WS valueStmt
   ;

eventStmt
   : (visibility WS)? EVENT WS ambiguousIdentifier WS? argList
   ;

exitStmt
   : EXIT_DO
   | EXIT_FOR
   | EXIT_FUNCTION
   | EXIT_PROPERTY
   | EXIT_SUB
   ;

filecopyStmt
   : FILECOPY WS valueStmt WS? COMMA WS? valueStmt
   ;

forEachStmt
   : FOR WS EACH WS? ambiguousIdentifier typeHint? WS? IN WS? valueStmt (NEWLINE | INLINE_SEP)+ (block (NEWLINE|INLINE_SEP)+)? NEXT (WS? ambiguousIdentifier)?
;

forNextStmt
   : FOR WS iCS_S_VariableOrProcedureCall typeHint? (WS asTypeClause)? WS? EQ WS? valueStmt WS TO WS valueStmt (WS? STEP WS valueStmt)? (NEWLINE|INLINE_SEP)+ (block (NEWLINE|INLINE_SEP)+)? NEXT (WS ambiguousIdentifier typeHint?)?
   ;

functionStmt
   : (visibility WS)? (DEFAULT WS)? (STATIC WS)? FUNCTION WS ambiguousIdentifier (WS? argList)? (WS asTypeClause)? WS? (NEWLINE | INLINE_SEP | WS )+ WS? (block WS* (NEWLINE | INLINE_SEP)+)? WS* END_FUNCTION
   ;
   
getStmt
   : GET WS valueStmt WS? COMMA WS? valueStmt? WS? COMMA WS? valueStmt
   ;

goSubStmt
   : GOSUB WS valueStmt
   ;

goToStmt
   : GOTO WS valueStmt
   ;

ifThenElseStmt
   //: IF WS ifConditionStmt WS THEN WS blockStmt (WS ELSE WS blockStmt)? # inlineIfThenElse
   : ifBlockStmt ifElseIfBlockStmt* ifElseBlockStmt? END_IF # blockIfThenElse
   ;

inlineIfThenElseStmt
   //: IF WS ifConditionStmt WS THEN WS blockStmt (WS ELSE WS blockStmt)? # inlineIfThenElse
   : inlineIfBlockStmt inlineElseBlockStmt? (INLINE_SEP* END_IF?)  # inlineIfThenElse
   ;

inlineIfBlockStmt
   : IF WS? ifConditionStmt WS? THEN (WS|INLINE_SEP)+ inlineBlock
   ; 


inlineElseBlockStmt
   : (WS|INLINE_SEP)? ELSE (WS|INLINE_SEP) inlineBlock
   ;

ifBlockStmt
   : IF WS? ifConditionStmt WS? THEN NEWLINE + (block NEWLINE +)?
   ;

ifConditionStmt
   : valueStmt
   ;

ifElseIfBlockStmt
   : ELSEIF WS ifConditionStmt WS THEN (NEWLINE | INLINE_SEP) + (block (NEWLINE | INLINE_SEP) +)?
   ;

ifElseBlockStmt
   : ELSE (NEWLINE | INLINE_SEP) + (block (NEWLINE | INLINE_SEP) +)?
   ;

implementsStmt
   : IMPLEMENTS WS ambiguousIdentifier
   ;

inputStmt
   : INPUT WS valueStmt (WS? COMMA WS? valueStmt) +
   ;

killStmt
   : KILL WS valueStmt
   ;


midStmt
   : MID WS? LPAREN WS? ambiguousIdentifier WS? COMMA WS? argCall WS? COMMA WS? argCall (WS? COMMA WS? argCall)? WS? RPAREN WS? EQ WS? valueStmt
   ;


letStmt
   : (LET WS)? implicitCallStmt_InStmt WS? (EQ | PLUS_EQ | MINUS_EQ) WS? valueStmt
   ;

lineInputStmt
   : LINE_INPUT WS valueStmt WS? COMMA WS? valueStmt
   ;

loadStmt
   : LOAD WS valueStmt
   ;

lockStmt
   : LOCK WS valueStmt (WS? COMMA WS? valueStmt (WS TO WS valueStmt)?)?
   ;

lsetStmt
   : LSET WS implicitCallStmt_InStmt WS? EQ WS? valueStmt
   ;

macroIfThenElseStmt
   : macroIfBlockStmt macroElseIfBlockStmt* macroElseBlockStmt? MACRO_END_IF
   ;

macroIfBlockStmt
   : MACRO_IF WS ifConditionStmt WS THEN NEWLINE + (moduleBody NEWLINE +)?
   ;

macroElseIfBlockStmt
   : MACRO_ELSEIF WS ifConditionStmt WS THEN NEWLINE + (moduleBody NEWLINE +)?
   ;

macroElseBlockStmt
   : MACRO_ELSE NEWLINE + (moduleBody NEWLINE +)?
   ;


mkdirStmt
   : MKDIR WS valueStmt
   ;

nameStmt
   : NAME WS valueStmt WS AS WS valueStmt
   ;

onErrorStmt
   : (ON_ERROR | ON_LOCAL_ERROR) WS (GOTO WS valueStmt | RESUME WS NEXT)
   ;

onGoToStmt
   : ON WS valueStmt WS GOTO WS valueStmt (WS? COMMA WS? valueStmt)*
   ;

onGoSubStmt
   : ON WS valueStmt WS GOSUB WS valueStmt (WS? COMMA WS? valueStmt)*
   ;

openStmt
   : OPEN WS valueStmt WS FOR WS (APPEND | BINARY | INPUT | OUTPUT | RANDOM) (WS ACCESS WS (READ | WRITE | READ_WRITE))? (WS (SHARED | LOCK_READ | LOCK_WRITE | LOCK_READ_WRITE))? WS AS WS valueStmt (WS LEN WS? EQ WS? valueStmt)?
   ;

outputList
   : outputList_Expression (WS? (SEMICOLON | COMMA) WS? outputList_Expression?)*
   | outputList_Expression? (WS? (SEMICOLON | COMMA) WS? outputList_Expression?) +
   ;

outputList_Expression
   : (SPC | TAB) (WS? LPAREN WS? argsCall WS? RPAREN)?
   | valueStmt
   ;

printStmt
   : PRINT WS valueStmt (WS? COMMA (WS? outputList)?)?
   ;

propertyGetStmt
   : (visibility WS)? (STATIC WS)? PROPERTY_GET WS ambiguousIdentifier typeHint? (WS? argList)? (WS asTypeClause)? (NEWLINE|INLINE_SEP)+ (block (NEWLINE|INLINE_SEP)+)? END_PROPERTY
   ;

propertySetStmt
   : (visibility WS)? (STATIC WS)? PROPERTY_SET WS ambiguousIdentifier (WS? argList)? (NEWLINE|INLINE_SEP)+ (block (NEWLINE|INLINE_SEP)+)? END_PROPERTY
   ;

propertyLetStmt
   : (visibility WS)? (STATIC WS)? PROPERTY_LET WS ambiguousIdentifier (WS? argList)? (NEWLINE|INLINE_SEP)+ (block (NEWLINE|INLINE_SEP)+)? END_PROPERTY
   ;

putStmt
   : PUT WS valueStmt WS? COMMA WS? valueStmt? WS? COMMA WS? valueStmt
   ;

raiseEventStmt
   : RAISEEVENT WS ambiguousIdentifier (WS? LPAREN WS? (argsCall WS?)? RPAREN)?
   ;

randomizeStmt
   : RANDOMIZE (WS valueStmt)?
   ;

redimStmt
   : REDIM WS (PRESERVE WS)? redimSubStmt (WS? COMMA WS? redimSubStmt)*
   ;

// redimSubStmt
//    : implicitCallStmt_InStmt WS? LPAREN WS? subscripts WS? RPAREN (WS asTypeClause)?
//    ;

redimSubStmt
   : ambiguousIdentifier WS? LPAREN WS? subscripts WS? RPAREN (WS asTypeClause)?
   ;

resetStmt
   : RESET
   ;

resumeStmt
   : RESUME (WS (NEXT | ambiguousIdentifier))?
   ;

returnStmt
   : RETURN
   ;

rmdirStmt
   : RMDIR WS valueStmt
   ;

rsetStmt
   : RSET WS implicitCallStmt_InStmt WS? EQ WS? valueStmt
   ;

savepictureStmt
   : SAVEPICTURE WS valueStmt WS? COMMA WS? valueStmt
   ;

saveSettingStmt
   : SAVESETTING WS valueStmt WS? COMMA WS? valueStmt WS? COMMA WS? valueStmt WS? COMMA WS? valueStmt
   ;

seekStmt
   : SEEK WS valueStmt WS? COMMA WS? valueStmt
   ;

selectCaseStmt
   : SELECT WS CASE WS valueStmt (NEWLINE|INLINE_SEP)+ sC_Case* WS? END_SELECT
   ;

sC_Case
   : CASE WS? sC_Cond WS? (NEWLINE|INLINE_SEP)* (block (NEWLINE|INLINE_SEP) +)?
   ;

// ELSE first, so that it is not interpreted as a variable call
sC_Cond
   : ELSE # caseCondElse
   | sC_CondExpr (WS? COMMA WS? sC_CondExpr)* #caseCondExpr
   ;

sC_CondExpr
   : IS WS? comparisonOperator WS? valueStmt # caseCondExprIs
   | valueStmt # caseCondExprValue
   | valueStmt WS TO WS valueStmt # caseCondExprTo
   ;

sendkeysStmt
   : SENDKEYS WS valueStmt (WS? COMMA WS? valueStmt)?
   ;

setattrStmt
   : SETATTR WS valueStmt WS? COMMA WS? valueStmt
   ;

setStmt
   : SET WS implicitCallStmt_InStmt WS? EQ WS? valueStmt
   ;

stopStmt
   : STOP
   ;

subStmt
   : (visibility WS)? (DEFAULT WS)? (STATIC WS)? SUB WS ambiguousIdentifier (WS? argList)? (NEWLINE|INLINE_SEP)+ (block (NEWLINE|INLINE_SEP)+)? END_SUB
   ;
  
timeStmt
   : TIME WS? EQ WS? valueStmt
   ;

typeStmt
   : (visibility WS)? TYPE WS ambiguousIdentifier (NEWLINE|INLINE_SEP)+ (typeStmt_Element)* END_TYPE
   ;

typeStmt_Element
   : ambiguousIdentifier (WS? LPAREN (WS? subscripts)? WS? RPAREN)? (WS asTypeClause)? (NEWLINE|INLINE_SEP)+
   ;

typeOfStmt
   : TYPEOF WS valueStmt (WS IS WS type)?
   ;

unloadStmt
   : UNLOAD WS valueStmt
   ;

unlockStmt
   : UNLOCK WS valueStmt (WS? COMMA WS? valueStmt (WS TO WS valueStmt)?)?
   ;

// operator precedence is represented by rule order
//https://stackoverflow.com/questions/25413460/what-should-the-correct-grammar-be-for-correct-precedence-evaluation-of
valueStmt
   : valUnary=literal																	# vsLiteral 
   | implicitCallStmt_InStmt															# vsICS
   | LPAREN WS? valUnary=valueStmt (WS? COMMA WS? valueStmt)* WS? RPAREN				# vsStruct
   | NEW WS type																	# vsNew
   | typeOfStmt																			# vsTypeOf
   | ADDRESSOF WS valueStmt																# vsAddressOf
   | implicitCallStmt_InStmt WS? ASSIGN WS? valueStmt									# vsAssign
   | operator=PLUS WS? valUnary=valueStmt												# vsUnaryOperation
   | operator=MINUS WS? valUnary=valueStmt                                              # vsUnaryOperation

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
   | midStmt																			 # vsMid
   ;
 

variableStmt
   : (DIM | STATIC | visibility) WS (WITHEVENTS WS)? variableListStmt
   ;

variableListStmt
   : variableSubStmt (WS? COMMA WS? variableSubStmt)*
   ;
   
variableSubStmt
   : ambiguousIdentifier typeHint? (WS? LPAREN WS? (subscripts WS?)? RPAREN WS?)? (WS asTypeClause)?
   ;

whileWendStmt
   : WHILE WS valueStmt (NEWLINE | INLINE_SEP) + block* (NEWLINE | INLINE_SEP)* WEND
   ;

widthStmt
   : WIDTH WS valueStmt WS? COMMA WS? valueStmt
   ;

withStmt
   : WITH WS (NEW WS)? implicitCallStmt_InStmt (NEWLINE | INLINE_SEP) + (block (NEWLINE | INLINE_SEP) +)? END_WITH
   ;

writeStmt
   : WRITE WS valueStmt WS? COMMA (WS? outputList)?
   ;

// complex call statements ----------------------------------

explicitCallStmt
   : eCS_MemberProcedureCall
   | eCS_ProcedureCall
   ;

// parantheses are required in case of args -> empty parantheses are removed
eCS_ProcedureCall
   : CALL WS ambiguousIdentifier typeHint? (WS? LPAREN WS? argsCall WS? RPAREN)?
   ;

// parantheses are required in case of args -> empty parantheses are removed
eCS_MemberProcedureCall
   : CALL WS implicitCallStmt_InStmt? DOT WS? ambiguousIdentifier typeHint? (WS? LPAREN WS? argsCall WS? RPAREN)?
   ;

implicitCallStmt_InBlock
   : iCS_B_MemberProcedureCall
   | iCS_B_ProcedureCall
   ;
  
noParenthesisArgs
   : ((delimitedLiteral? WS? (COMMA | SEMICOLON) WS?) | WS)? (argsCall)?
   ;

// parantheses are forbidden in case of args
// variables cannot be called in blocks
// certainIdentifier instead of ambiguousIdentifier for preventing ambiguity with statement keywords
iCS_B_ProcedureCall
   : certainIdentifier noParenthesisArgs
   ;

iCS_B_MemberProcedureCall
   : implicitCallStmt_InStmt? DOT ambiguousIdentifier typeHint? noParenthesisArgs? defaultMemberAccess?
   ;

// iCS_S_MembersCall first, so that member calls are not resolved as separate iCS_S_VariableOrProcedureCalls
implicitCallStmt_InStmt
   : iCS_S_MembersCall
   | iCS_S_VariableOrProcedureCall
   | iCS_S_ProcedureOrArrayCall
   | iCS_S_DefaultMemberAccess
   ;

iCS_S_VariableOrProcedureCall
   : ambiguousIdentifier typeHint? defaultMemberAccess?
   ;

iCS_S_ProcedureOrArrayCall
   : (ambiguousIdentifier | baseType | iCS_S_NestedProcedureCall) typeHint? WS? (LPAREN WS? (argsCall WS?)? RPAREN)+ defaultMemberAccess?
   ;

iCS_S_NestedProcedureCall
	: ambiguousIdentifier typeHint? WS? LPAREN WS? (argsCall WS?)? RPAREN
	;


iCS_S_MembersCall
   : (iCS_S_VariableOrProcedureCall | iCS_S_ProcedureOrArrayCall)? iCS_S_MemberCall + defaultMemberAccess?
   ;

iCS_S_MemberCall
   : WS? DOT WS? (iCS_S_VariableOrProcedureCall | iCS_S_ProcedureOrArrayCall)
   ;

iCS_S_DefaultMemberAccess
   : defaultMemberAccess
   ;

// atomic call statements ----------------------------------

argsCall
   : (argCall? WS? (COMMA | SEMICOLON) WS?)* argCall (WS? (COMMA | SEMICOLON) WS? argCall?)*
   ;

argCall
   : ((BYVAL | BYREF | PARAMARRAY) WS)? valueStmt
   ;

defaultMemberAccess
   : EXCLAMATIONMARK ambiguousIdentifier typeHint?
   ;

// atomic rules for statements
argList
   : LPAREN (WS? arg (WS? COMMA WS? arg)*)? WS? RPAREN
   ;

arg
   : (OPTIONAL WS)? ((BYVAL | BYREF) WS)? (PARAMARRAY WS)? ambiguousIdentifier typeHint? (WS? LPAREN WS? RPAREN)? (WS asTypeClause)? (WS? argDefaultValue)?
   ;

argDefaultValue
   : EQ WS? valueStmt
   ;

subscripts
   : subscript (WS? COMMA WS? subscript)*
   ;

subscript
   : (valueStmt WS TO WS)? valueStmt
   ;

// atomic rules ----------------------------------

ambiguousIdentifier
   : (IDENTIFIER | ambiguousKeyword) +
   | L_SQUARE_BRACKET (IDENTIFIER | ambiguousKeyword) + R_SQUARE_BRACKET
   ;

asTypeClause
   : AS WS (NEW WS)? type (WS fieldLength)?
   ;

baseType
   : BOOLEAN
   | BYTE
   | COLLECTION
   | DATE
   | DOUBLE
   | INTEGER
   | LONG
   | OBJECT
   | SINGLE
   | STRING
   | VARIANT
   ;

certainIdentifier
   : IDENTIFIER (ambiguousKeyword | IDENTIFIER)*
   | ambiguousKeyword (ambiguousKeyword | IDENTIFIER) +
   ;

comparisonOperator
   : LT
   | LEQ
   | GT
   | GEQ
   | EQ
   | NEQ
   | IS
   | LIKE
   ;

complexType
   : ambiguousIdentifier (DOT ambiguousIdentifier)*
   ;

fieldLength
   : MULT WS? (INTEGERLITERAL | ambiguousIdentifier)
   ;

letterrange
   : certainIdentifier (WS? MINUS WS? certainIdentifier)?
   ;

delimitedLiteral
   : HEXLITERAL         #ltColor
   | OCTALLITERAL       #ltOctal
   | DATELITERAL        #ltDate
   | STRINGLITERAL      #ltString
   ;

literal
   : DOUBLELITERAL      #ltDouble
   | delimitedLiteral   #ltDelimited
   | FILENUMBER         #ltFilenumber
   | INTEGERLITERAL     #ltInteger
   | TRUE               #ltBoolean
   | FALSE              #ltBoolean
   | NOTHING            #ltNothing
   | NULL               #ltNull
   ;

publicPrivateVisibility
	: PRIVATE
	| PUBLIC
	;

publicPrivateGlobalVisibility
	: PRIVATE
	| PUBLIC
	| GLOBAL
	;

type
   : (baseType | complexType) (WS? LPAREN WS? RPAREN)?
   ;

typeHint
   : AMPERSAND
   | AT
   | DOLLAR
   | EXCLAMATIONMARK
   | HASH
   | PERCENT
   ;

visibility
   : PRIVATE
   | PUBLIC
   | FRIEND
   | GLOBAL
   ;

// ambiguous keywords
ambiguousKeyword
   : ACCESS
   | ADDRESSOF
   | ALIAS
   | AND
   | ATTRIBUTE
   | APPACTIVATE
   | APPEND
   | AS
   | BEEP
   | BEGIN
   | BINARY
   | BOOLEAN
   | BYVAL
   | BYREF
   | BYTE
   | CALL
   | CASE
   | CLASS
   | CLOSE
   | CHDIR
   | CHDRIVE
   | COLLECTION
   | CONST
   | DATE
   | DECLARE
   | DEFBOOL
   | DEFBYTE
   | DEFCUR
   | DEFDBL
   | DEFDATE
   | DEFDEC
   | DEFINT
   | DEFLNG
   | DEFOBJ
   | DEFSNG
   | DEFSTR
   | DEFVAR
   | DELETESETTING
   | DIM
   | DO
   | DOUBLE
   | EACH
   //| ELSE
   | ELSEIF
   | END
   | ENUM
   | EQV
   | ERASE
   | ERROR
   | EVENT
   | FALSE
   | FILECOPY
   | FRIEND
   | FOR
   | FUNCTION
   | GET
   | GLOBAL
   | GOSUB
   | GOTO
   | IF
   | IMP
   | IMPLEMENTS
   | IN
   | INPUT
   | IS
   | INTEGER
   | KILL
   | LOAD
   | LOCK
   | LONG
   | LOOP
   | LEN
   | LET
   | LIB
   | LIKE
   | LSET
   | ME
   | MID
   | MKDIR
   | MOD
   | NAME
   | NEXT
   | NEW
   | NOT
   | NOTHING
   | NULL
   | OBJECT
   | ON
   | OPEN
   | OPTIONAL
   | OR
   | OUTPUT
   | PARAMARRAY
   | PRESERVE
   | PRINT
   | PRIVATE
   | PUBLIC
   | PUT
   | RANDOM
   | RANDOMIZE
   | RAISEEVENT
   | READ
   | REDIM
   | REM
   | RESET
   | RESUME
   | RETURN
   | RMDIR
   | RSET
   | SAVEPICTURE
   | SAVESETTING
   | SEEK
   | SELECT
   | SENDKEYS
   | SET
   | SETATTR
   | SHARED
   | SINGLE
   | SPC
   | STATIC
   | STEP
   | STOP
   | STRING
   | SUB
   | TAB
   | TEXT
   | THEN
   | TIME
   | TO
   | TRUE
   | TYPE
   | TYPEOF
   | UNLOAD
   | UNLOCK
   | UNTIL
   | VARIANT
   | VERSION
   | WEND
   | WHILE
   | WIDTH
   | WITH
   | WITHEVENTS
   | WRITE
   | XOR
   ;

// lexer rules --------------------------------------------------------------------------------

// keywords

ACCESS
   : A C C E S S
   ;


ADDRESSOF
   : A D D R E S S O F
   ;


ALIAS
   : A L I A S
   ;


AND
   : A N D
   ;


ATTRIBUTE
   : A T T R I B U T E
   ;


APPACTIVATE
   : A P P A C T I V A T E
   ;


APPEND
   : A P P E N D
   ;


AS
   : A S
   ;


BEEP
   : B E E P
   ;


BEGIN
   : B E G I N
   ;


BEGINPROPERTY
   : B E G I N P R O P E R T Y
   ;


BINARY
   : B I N A R Y
   ;


BOOLEAN
   : B O O L E A N
   ;


BYVAL
   : B Y V A L
   ;


BYREF
   : B Y R E F
   ;


BYTE
   : B Y T E
   ;


CALL
   : C A L L
   ;


CASE
   : C A S E
   ;


CHDIR
   : C H D I R
   ;


CHDRIVE
   : C H D R I V E
   ;


CLASS
   : C L A S S
   ;


CLOSE
   : C L O S E
   ;


COLLECTION
   : C O L L E C T I O N
   ;


CONST
   : C O N S T
   ;


DATE
   : D A T E
   ;

PTRSAFE
   : P T R S A F E
   ;

DECLARE
   : D E C L A R E
   ;


DEFBOOL
   : D E F B O O L
   ;


DEFBYTE
   : D E F B Y T E
   ;


DEFDATE
   : D E F D A T E
   ;


DEFDBL
   : D E F D B L
   ;


DEFDEC
   : D E F D E C
   ;


DEFCUR
   : D E F C U R
   ;


DEFINT
   : D E F I N T
   ;


DEFLNG
   : D E F L N G
   ;


DEFOBJ
   : D E F O B J
   ;


DEFSNG
   : D E F S N G
   ;


DEFSTR
   : D E F S T R
   ;


DEFVAR
   : D E F V A R
   ;


DELETESETTING
   : D E L E T E S E T T I N G
   ;


DIM
   : D I M
   ;


DO
   : D O
   ;


DOUBLE
   : D O U B L E
   ;


EACH
   : E A C H
   ;


ELSE
   : E L S E
   ;


ELSEIF
   : E L S E I F
   ;


END_ENUM
   : E N D ' ' E N U M
   ;


END_CLASS
   : E N D ' ' C L A S S
   ;


END_FUNCTION
   : E N D ' ' F U N C T I O N
   ;


END_IF
   : E N D ' ' I F
   ;


END_PROPERTY
   : E N D ' ' P R O P E R T Y
   ;


END_SELECT
   : E N D ' ' S E L E C T
   ;


END_SUB
   : E N D ' ' S U B
   ;


END_TYPE
   : E N D ' ' T Y P E
   ;


END_WITH
   : E N D ' ' W I T H
   ;


END
   : E N D
   ;


ENDPROPERTY
   : E N D P R O P E R T Y
   ;


ENUM
   : E N U M
   ;


EQV
   : E Q V
   ;


ERASE
   : E R A S E
   ;


ERROR
   : E R R O R
   ;


EVENT
   : E V E N T
   ;


EXIT_DO
   : E X I T ' ' D O
   ;


EXIT_FOR
   : E X I T ' ' F O R
   ;


EXIT_FUNCTION
   : E X I T ' ' F U N C T I O N
   ;


EXIT_PROPERTY
   : E X I T ' ' P R O P E R T Y
   ;


EXIT_SUB
   : E X I T ' ' S U B
   ;


FALSE
   : F A L S E
   ;


FILECOPY
   : F I L E C O P Y
   ;


FRIEND
   : F R I E N D
   ;


FOR
   : F O R
   ;


FUNCTION
   : F U N C T I O N
   ;


GET
   : G E T
   ;


GLOBAL
   : G L O B A L
   ;


GOSUB
   : G O S U B
   ;


GOTO
   : G O T O
   ;


IF
   : I F
   ;


IMP
   : I M P
   ;


IMPLEMENTS
   : I M P L E M E N T S
   ;


IN
   : I N
   ;


INPUT
   : I N P U T
   ;


IS
   : I S
   ;


INTEGER
   : I N T E G E R
   ;


KILL
   : K I L L
   ;


LOAD
   : L O A D
   ;


LOCK
   : L O C K
   ;


LONG
   : L O N G
   ;


LOOP
   : L O O P
   ;


LEN
   : L E N
   ;


LET
   : L E T
   ;


LIB
   : L I B
   ;


LIKE
   : L I K E
   ;


LINE_INPUT
   : L I N E ' ' I N P U T
   ;


LOCK_READ
   : L O C K ' ' R E A D
   ;


LOCK_WRITE
   : L O C K ' ' W R I T E
   ;


LOCK_READ_WRITE
   : L O C K ' ' R E A D ' ' W R I T E
   ;


LSET
   : L S E T
   ;


MACRO_IF
   : HASH I F
   ;


MACRO_ELSEIF
   : HASH E L S E I F
   ;


MACRO_ELSE
   : HASH E L S E
   ;


MACRO_END_IF
   : HASH E N D ' ' I F
   ;


ME
   : M E
   ;


MID
   : M I D
   ;


MKDIR
   : M K D I R
   ;


MOD
   : M O D
   ;


NAME
   : N A M E
   ;


NEXT
   : N E X T
   ;


NEW
   : N E W
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

ON
   : O N
   ;


ON_ERROR
   : O N ' ' E R R O R
   ;


ON_LOCAL_ERROR
   : O N ' ' L O C A L ' ' E R R O R
   ;


OPEN
   : O P E N
   ;


OPTIONAL
   : O P T I O N A L
   ;


OPTION_BASE
   : O P T I O N ' ' B A S E
   ;


OPTION_EXPLICIT
   : O P T I O N ' ' E X P L I C I T
   ;


OPTION_COMPARE
   : O P T I O N ' ' C O M P A R E
   ;


OPTION_PRIVATE_MODULE
   : O P T I O N ' ' P R I V A T E ' ' M O D U L E
   ;


OR
   : O R
   ;


OUTPUT
   : O U T P U T
   ;


PARAMARRAY
   : P A R A M A R R A Y
   ;


PRESERVE
   : P R E S E R V E
   ;


PRINT
   : P R I N T
   ;


PRIVATE
   : P R I V A T E
   ;


PROPERTY_GET
   : P R O P E R T Y ' ' G E T
   ;


PROPERTY_LET
   : P R O P E R T Y ' ' L E T
   ;


PROPERTY_SET
   : P R O P E R T Y ' ' S E T
   ;


PUBLIC
   : P U B L I C
   ;


PUT
   : P U T
   ;


RANDOM
   : R A N D O M
   ;


RANDOMIZE
   : R A N D O M I Z E
   ;


RAISEEVENT
   : R A I S E E V E N T
   ;


READ
   : R E A D
   ;


READ_WRITE
   : R E A D ' ' W R I T E
   ;


REDIM
   : R E D I M
   ;


REM
   : R E M
   ;


RESET
   : R E S E T
   ;


RESUME
   : R E S U M E
   ;


RETURN
   : R E T U R N
   ;


RMDIR
   : R M D I R
   ;


RSET
   : R S E T
   ;


SAVEPICTURE
   : S A V E P I C T U R E
   ;


SAVESETTING
   : S A V E S E T T I N G
   ;


SEEK
   : S E E K
   ;


SELECT
   : S E L E C T
   ;


SENDKEYS
   : S E N D K E Y S
   ;


SET
   : S E T
   ;


SETATTR
   : S E T A T T R
   ;


SHARED
   : S H A R E D
   ;


SINGLE
   : S I N G L E
   ;


SPC
   : S P C
   ;


STATIC
   : S T A T I C
   ;

DEFAULT
   : D E F A U L T
   ;


STEP
   : S T E P
   ;


STOP
   : S T O P
   ;


STRING
   : S T R I N G
   ;


SUB
   : S U B
   ;


TAB
   : T A B
   ;


TEXT
   : T E X T
   ;


THEN
   : T H E N
   ;


TIME
   : T I M E
   ;


TO
   : T O
   ;


TRUE
   : T R U E
   ;


TYPE
   : T Y P E
   ;


TYPEOF
   : T Y P E O F
   ;


UNLOAD
   : U N L O A D
   ;


UNLOCK
   : U N L O C K
   ;


UNTIL
   : U N T I L
   ;


VARIANT
   : V A R I A N T
   ;


VERSION
   : V E R S I O N
   ;


WEND
   : W E N D
   ;


WHILE
   : W H I L E
   ;


WIDTH
   : W I D T H
   ;


WITH
   : W I T H
   ;


WITHEVENTS
   : W I T H E V E N T S
   ;


WRITE
   : W R I T E
   ;


XOR
   : X O R
   ;

// symbols

AMPERSAND
   : '&'
   ;


ASSIGN
   : ':='
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


DOLLAR
   : '$'
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


MINUS_EQ
   : '-='
   ;


MULT
   : '*'
   ;


NEQ
   : '<>'
   ;


PERCENT
   : '%'
   ;


PLUS
   : '+'
   ;


PLUS_EQ
   : '+='
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
   : ('&H' | '&h') [0-9A-Fa-f] + AMPERSAND?
   ;


INTEGERLITERAL
   :  ('0' .. '9') + (('e' | 'E') INTEGERLITERAL)* 
      //(HASH | AMPERSAND | EXCLAMATIONMARK | AT)? // VBA
      (HASH | EXCLAMATIONMARK | AT)? // VBSCRIPT
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

IDENTIFIER
   : LETTER LETTERORDIGIT*
   ;

// whitespace, line breaks, comments, ...

//LINE_CONTINUATION
//   : ' ' '_' '\r'? '\n' -> channel(HIDDEN)
//   ;   
// NEWLINE
//    : WS? NL WS?
//    //: WS? ('\r'? '\n' | COLON ' '?) WS?
//    ;

NEWLINE
   : WS? NL WS?
   //: WS? ('\r'? '\n' | COLON ' '?) WS?
   ;
  
INLINE_SEP:
   WS? (INLN WS?)+
   ;

//COMMENT
//   : WS? ('\'' | ':'? REM ' ') (LINE_CONTINUATION | ~ ('\n' | '\r'))* -> channel(HIDDEN)
//   ;

INLN
   : ':'
   ;

COMMENT
   : WS? ('\'' | ':'? REM ' ') (~ ('\n' | '\r'))* -> channel(HIDDEN)
   ;

  

WS
   : [ \u2002\t] +
   ;

LINE_CONTINUATION
   : ' ' '_' ' '* '\r'? '\n' -> channel(HIDDEN)
   ;   

// letters
fragment NL      
   : '\r'? '\n' 
   | '\r'
   | EOF;

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


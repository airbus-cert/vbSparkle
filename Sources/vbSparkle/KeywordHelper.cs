using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace vbSparkle
{
    public static class KeywordHelper
    {
        private static string[] _keywords = "Base;Characters;Character;Char;Chr;Bits;Bytes;Words;Byte;Word;Power;Str;String;Strip;Addr;Address;Addresses;Path;Dir;Directory;Black;File;White;Log;Work;Condition;Cond;Const;Ip;MD5;SHA1;SHA256;Write;Line;Ok;Signed;Unsigned;Rotate;Left;Right;Unicode;UTF;Ansi;Content;Var;Hex;Low;High;Group;Binary;Decode;Array;To;Len;Length;InStrB;Stream;Get;Put;Set;Add;Remove;Shift;Conv;Convert;Message;Msg;Number;Pos;Position;Count;Cnt;Text;Obj;FSO;Write;Close;Hash;Line;Data;Read;Print;Close;Exit;Chr;ChrB;ChrW;ChrA;Result;Mid;MidB;Value;Result;Not;And;Xor;Of;Asc;Server;Variables;Variable;Clear;Open;Create;At;End;Case;LCase;UCase;UBound;LBound;Bound;Form;At;Date;Time;IpAddr;InStr;Response;Expires;Buffer;Res;LenB;Err;Info;Os;Bit;Replace;Request;Now;Type;Terminate;Upload;Name;Begin;As;Wide;InStrRev;Alert;Elem;Col;Column;Element;Index;Save;Disk;Folder;Mode;Flush;Copy;Script;Timeout;Out;Key;Enc;Flush;Pass;Password;Session;Schema;Table;List;Move;Next;Is;Do;Down;Download;State;Status;And;Or;Xor;Equal;Equals;Strip;Resume;Next;Previous;Msg;Box;Compt;Attr;Attribute;Arr;Comp;Comput;Computer;DSE;LDAP;Int;Root;Service;Opt;Option;Ds;DN;NT;Echo;WScript;RID;Infr;Inf;Infra;Infrastructure;Master;Part;Name;Naming;PDC;Emulator;Img;Hyp;Angle;Cos;Sin;Fill;Poly;Draw;Current;Shell;Run;Auth;Authenticate;Authentication;Query;Edit;Delete;Remove;JScript;Java;Modif;Last;Access;Accessed;Main;Make;Primary;Cmd;Net;Scan;Url;Db;Use;Usuable;Usable;CStr;Size;Page;pg;Num;EOF;Field;Fields;Diff;Def;Define;SMTP;DNS;Doc;Exec;Ext;Extension;Drop;Dump;Exist;Exists;In;oFS;First;Attach;Attachment;Head;Header;Path;Name;New;Sys;Item;Items;Exist;Exists;Parent;Parse;Description;Source;Temp;Get;Write;Writeable;Html;Null;By;Cmd;Update;Insert;Flag;Defined;Precision;Scale;Attachment;Absolute;Abs;Body;Port;Template;Bin;Binary;Cook;Cookie;Cookies;Randomize;Random;Rnd;RC4;Str;CInt;Cipher;Swap;Pwd;Cb;Change;Reg;Registry;Regul;Regular;Regularize;Rslt;Exe;Execute;Class;StdOut;All;Str;Out;Env;Envir;Environ;Environment;WS;Space;Map;Sub;Func;Function;Drive;Driver;Drives;Letter;Raise;CBool;CInt"
                                            .Split(';').OrderBy(v => v.Length).ToArray();

        public static string RenameVar(string name)
        {
            TextInfo ti = CultureInfo.CurrentCulture.TextInfo;
            string newName = name.ToLower();

            if (newName.Contains("_"))
                newName = string.Join("_", newName.Split('_').Select(v => ti.ToTitleCase(v)));

            foreach(var kw in _keywords)
            {
                newName = Regex.Replace(newName, kw, kw, RegexOptions.IgnoreCase);
            }

            return newName;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SofaCompiler.Compiler {

    public class Token {
        public byte kind;

        public string tok;

        public Token(byte kind, string tok) {
            this.kind = kind;
            this.tok = tok;

            if (kind == IDENTIFIER)
                for (byte i = 0; i < TOKENS.Length; i++) {
                    if (tok.ToLower().Equals(TOKENS[i].ToLower())) {
                        this.kind = i;

                        break;
                    }
                }
        }

        private static string[] TOKENS = {
            "sofa",
            "identifier",
            "integer",
            "operator",
            "dust",
            "nop",
            "spot",

            "list<dust>",
            "list<nop>",
            "tv.input",
            "tv.output",

            "if",
            "then",
            "elseif",
            "else",
            "fi",

            "while",
            "do",
            "od",

            "foreach",
            "in",
            "fe",

            "out",
            "=>",

            "void",

            ",",
            ";",
            ":",
            "::",
            "{",
            "}",

            "eot",
            "error"
        };

        public const byte SOFA = 0;

        public const byte IDENTIFIER = 1;

        public const byte INTEGER = 2;

        public const byte OPERATOR = 3;

        public const byte DUST = 4;

        public const byte NOP = 5;

        public const byte SPOT = 6;

        public const byte LIST_DUST = 7;

        public const byte LIST_NOP = 8;

        public const byte TV_INPUT = 9;

        public const byte TV_OUTPUT = 10;

        public const byte IF = 11;

        public const byte THEN = 12;

        public const byte ELSEIF = 13;

        public const byte ELSE = 14;

        public const byte FI = 15;

        public const byte WHILE = 16;

        public const byte DO = 17;

        public const byte OD = 18;

        public const byte FOREACH = 19;

        public const byte IN = 20;

        public const byte FE = 21;

        public const byte OUT = 22;

        public const byte OUT_LAMBDA = 23;

        public const byte VOID = 24;

        public const byte COMMA = 25;

        public const byte SEMICOLON = 26;

        public const byte COLON = 28;

        public const byte DOUBLE_COLON = 27;

        public const byte LEFT_BRAKET = 29;

        public const byte RIGHT_BRAKET = 30;

        public static byte EOT = 31;

        public const byte ERROR = 50;


        private static string[] ADD_OPER = {"+"};

        private static string SUB_OPER = "-";

        private static string[] MUL_OPER = {"*"};

        private static string DIV_OPER = "/";

        private static string[] ASS_OPER = {"="};

        private bool containsOperator(string spelling, string[] ops) {
            for (int i = 0; i < ops.Length; ++i)
                if (spelling.Equals(ops[i]))
                    return true;

            return false;
        }

        public bool IsAssignOperator() {
            if (kind == OPERATOR)
                return containsOperator(tok, ASS_OPER);
            else
                return false;
        }

        public bool IsAddOperator() {
          if( kind == OPERATOR )
                    return containsOperator( tok, ADD_OPER );
                else
                    return false;
            
        }

        public bool IsMulOperator() {
            if( kind == OPERATOR )
                return containsOperator( tok, MUL_OPER);
            else
                return false;
        }
    }

}
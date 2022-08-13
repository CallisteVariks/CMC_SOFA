using System;

namespace SofaCompiler.Compiler.AST {

    public class IntegerLiteral : Terminal {


        public IntegerLiteral(String spelling) {
            this.spelling = spelling;
        }


        public override object Pop(IPopping popping, Object arg) {
            return popping.PopInteger(this, arg);
        }

    }

}
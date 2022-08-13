using System;
using SofaCompiler.Compiler.AST;

namespace SofaCompiler.Compiler.AST {

    public class VariableExpression : Expression {
        private Identifier name;

        public Identifier Name => name;
        
        public VariableDeclaration VarDeclaration { get; set; }

        public VariableExpression(Identifier name) {
            this.name = name;
        }

        public override object Pop(IPopping popping, object o) {
            return popping.PopVariableExpression(this, o);
        }
    }

}
using System.Collections;

namespace SofaCompiler.Compiler.AST {

    public class Statements : AST {

        private ArrayList _statements = new ArrayList();

        public ArrayList StatementsList => _statements;

        public override object Pop(IPopping popping, object o) {
            return popping.PopStatements(this, o);
        }

    }

}
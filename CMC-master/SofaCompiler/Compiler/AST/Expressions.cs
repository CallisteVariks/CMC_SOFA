using System.Collections;

namespace SofaCompiler.Compiler.AST {

    public class Expressions :AST {
        
        private ArrayList _expressions = new ArrayList();

        public ArrayList ExpressionList => _expressions;

        public override object Pop(IPopping popping, object o) {
            return popping.PopExpressions(this, o);

        }
    }

}
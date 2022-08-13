namespace SofaCompiler.Compiler.AST{

    public class ExpressionStatement : Statement {

        private Expression _expression;

        public Expression Expression => _expression;

        public ExpressionStatement(Expression expression) {
            _expression = expression;
        }

        public override object Pop(IPopping popping, object o) {
            return popping.PopExpressionStatement(this, o);
        }
    }

}
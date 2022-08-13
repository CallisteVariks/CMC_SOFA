namespace SofaCompiler.Compiler.AST {

    public class UnaryExpression : Expression {
        public Operator @operator;
        public Expression operand;
        
        public UnaryExpression(Operator op, Expression exp) {
            @operator = op;
            operand = exp;
        }

        public override object Pop(IPopping popping, object o) {
            return popping.PopUnaryExpression(this,o);
        }
    }

}
namespace SofaCompiler.Compiler.AST {

    public class BinaryExpression : Expression {
        public readonly Operator @operator;

        public readonly Expression operand1;

        public readonly Expression operand2;

        public BinaryExpression(Operator op, Expression res, Expression tmp) {
            @operator = op;
            operand1 = res;
            operand2 = tmp;

        }

        public override object Pop(IPopping popping, object o) {
            return popping.PopBinaryExpression(this, o);
        }
    }

}
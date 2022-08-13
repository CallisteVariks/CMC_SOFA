namespace SofaCompiler.Compiler.AST {

    public interface IPopping {
        object PopSofa(SOFA sofa, object o);
        object PopProgram(Program program, object o);
        object PopOperator(Operator @operator, object o);
        object PopStatements(Statements statements, object o);
        object PopIdentifier(Identifier identifier, object o);
        object PopExpressions(Expressions expressions, object o);
        object PopDeclarations(Declarations declarations, object o);
        object PopUnaryExpression(UnaryExpression unaryExpression, object o);
        object PopBinaryExpression(BinaryExpression unaryExpression, object o);
        object PopExpressionStatement(ExpressionStatement statement, object o);
        object PopVariableExpression(VariableExpression variableExpression, object o);
        object PopVariableDeclaration(VariableDeclaration variableDeclaration, object o);
        object PopFunctionDeclaration(FunctionDeclaration functionDeclaration, object o);
        object PopInteger(IntegerLiteral integerLiteral, object o);
    }

}
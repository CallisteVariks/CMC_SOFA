using System;
using System.Collections;
using SofaCompiler.Compiler.AST;

namespace SofaCompiler.Compiler {

    internal class Checker : IPopping {

        private Storage storage = new Storage();

        public void Check(Compiler.AST.Program program) {
            program.Pop(this, null);
        }

        public object PopSofa(SOFA sofa, object o) {
            sofa.Decs.Pop(this, null);
            sofa.Statements.Pop(this, null);

            return null;
        }

        public object PopProgram(AST.Program program, object o) {
            storage.Open();

            program.Sofa.Pop(this, null);

            storage.Close();

            return null;
        }

        public object PopOperator(Operator @operator, object o) {
            return @operator.spelling;
        }

        public object PopIdentifier(Identifier identifier, object o) {
            return identifier.spelling;
        }

        public object PopStatements(Statements statements, object o) {
            ArrayList statList = new ArrayList();
            foreach (Statement stat in statements.StatementsList)
                statList.Add((Type) stat.Pop(this, null));

            return statList;
        }

        public object PopExpressions(Expressions expressions, object o) {
            ArrayList types = new ArrayList();

            foreach (Expression exp in expressions.ExpressionList)
                types.Add((Type) exp.Pop(this, null));

            return types;
        }

        public object PopDeclarations(Declarations declarations, object o) {
            foreach (Declaration declaration in declarations.declarations) {
                declaration.Pop(this, null);
            }

            return null;
        }

        public object PopUnaryExpression(UnaryExpression unaryExpression, object o) {
            unaryExpression.operand.Pop(this, null);
            string @operator = (string) unaryExpression.@operator.Pop(this, null);

            if (!@operator.Equals("+") && !@operator.Equals("-"))
                Console.WriteLine("Only + or - is allowed as unary operator");

            return new Type(true);
        }


        public object PopBinaryExpression(BinaryExpression b, object arg) {
            Type t1 = (Type) b.operand1.Pop(this, null);
            Type t2 = (Type) b.operand2.Pop(this, null);
            string @operator = (string) b.@operator.Pop(this, null);

            if (@operator.Equals(":=") && t1.rvalueOnly)
                Console.WriteLine("Left-hand side of := must be a variable");

            return new Type(true);
        }

        public object PopExpressionStatement(ExpressionStatement statement, object o) {
            statement.Expression.Pop(this, null);

            return null;
        }

        public object PopVariableExpression(VariableExpression variableExpression, object o) {
            string id = (string) variableExpression.Name.Pop(this, null);

            Declaration d = storage.Get(id);
            if (d == null)
                Console.WriteLine(id + " is not declared");
            else if (!(d.GetType() == typeof(VariableDeclaration)))
                Console.WriteLine(id + " is not a variable");
            else
                variableExpression.VarDeclaration = (VariableDeclaration) d;

            return new Type(false);
        }

        public object PopVariableDeclaration(VariableDeclaration variableDeclaration, object o) {
            string id = (string) variableDeclaration.Id.Pop(this, null);

            storage.Add(id, variableDeclaration);

            return null;
        }

        public object PopFunctionDeclaration(FunctionDeclaration functionDeclaration, object o) {
            string id = (string) functionDeclaration.name.Pop(this, null);

            storage.Add(id, functionDeclaration);
            storage.Open();

            functionDeclaration.decs.Pop(this, null);
            functionDeclaration.sofa.Pop(this, null);
            functionDeclaration.retExp.Pop(this, null);

            storage.Close();

            return null;
        }

        public object PopInteger(IntegerLiteral integerLiteral, object o) {
            return null;
        }
    }

}
using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using SofaCompiler.Compiler.AST;
using SofaCompiler.TAM;

namespace SofaCompiler.Compiler {

    public class Encoder : IPopping {

        private int nextAddress = Machine.CB;

        private int currentLevel = 0;

        private void Emit(int op, int n, int r, int d) {
            if (n > 255) {
                Console.WriteLine("Operand too long");
                n = 255;
            }

            Instruction instruction = new Instruction {op = op, n = n, r = r, d = d};

            if (nextAddress >= Machine.PB)
                Console.WriteLine("Program too large");
            else
                Machine.code[nextAddress++] = instruction;
        }

        private void Patch(int adr, int d) {
            Machine.code[adr].d = d;
        }

        private int DisplayRegister(int currentLevel, int entityLevel) {
            if (entityLevel == 0)
                return Machine.SBr;
            else if (currentLevel - entityLevel <= 6)
                return Machine.LBr + currentLevel - entityLevel;
            else {
                Console.WriteLine("Accessing across to many levels");

                return Machine.L6r;
            }
        }

        public void SaveTargetProgram(String fileName) {
            try {
                BinaryWriter reader = new BinaryWriter(new FileStream(fileName, FileMode.Open));

                for (int i = Machine.CB; i < nextAddress; ++i)
                    Machine.code[i].Write(reader);
                reader.Close();
            }
            catch (Exception ex) {
                Console.Write(ex.StackTrace);
                Console.Write("Trouble writing " + fileName);
            }
        }

        public void Encode(AST.Program program) {
            program.Pop(this, null);
        }

        public object PopSofa(SOFA sofa, object o) {
            int before = nextAddress;
            Emit(Machine.JUMPop, 0, Machine.CB, 0);

            int size = (int) sofa.Statements.Pop(this, o);

            Patch(before, nextAddress);

            if (size > 0)
                Emit(Machine.PUSHop, 0, 0, size);

            sofa.Statements.Pop(this, null);

            return size;
        }

        public object PopProgram(AST.Program program, object o) {
            currentLevel = 0;

            program.Sofa.Pop(this, new Address());

            Emit(Machine.HALTop, 0, 0, 0);

            return null;
        }

        public object PopOperator(Operator @operator, object o) {
            return @operator.spelling;
        }

        public object PopIdentifier(Identifier identifier, object o) {
            return null;
        }

        public object PopStatements(Statements statements, object o) {
            foreach (Statement statement in statements.StatementsList) {
                statement.Pop(this, null);
            }

            return null;
        }

        public object PopExpressions(Expressions expressions, object o) {
            foreach (Expression expression in expressions.ExpressionList) {
                expression.Pop(this, true);
            }

            return null;
        }

        public object PopDeclarations(Declarations declarations, object o) {
            int startDisplacement = ((Address) o).displacement;

            foreach (Declaration dec in declarations.declarations)
                o = dec.Pop(this, o);

            Address adr = (Address) o;
            int size = adr.displacement - startDisplacement;

            return size;
        }

        public object PopUnaryExpression(UnaryExpression unaryExpression, object o) {
            bool valueNeeded = (bool) o;

            string op = (string) unaryExpression.@operator.Pop(this, null);
            unaryExpression.operand.Pop(this, o);

            if (valueNeeded && op.Equals("-"))
                Emit(Machine.CALLop, 0, Machine.PBr, Machine.negDisplacement);

            return null;
        }

        public object PopBinaryExpression(BinaryExpression binaryExpression, object o) {
            bool value = (bool) o;

            string op = (string) binaryExpression.@operator.Pop(this, null);

            if (!op.Equals("=")) return null;
            Address adr = (Address) binaryExpression.operand1.Pop(this, false);
            binaryExpression.operand2.Pop(this, true);

            int register = DisplayRegister(currentLevel, adr.level);
            Emit(Machine.STOREop, 1, register, adr.displacement);

            if (value)
                Emit(Machine.LOADop, 1, register, adr.displacement);

            return null;
        }


        public object PopExpressionStatement(ExpressionStatement statement, object o) {
            statement.Pop(this, false);

            return null;
        }

        public object PopVariableExpression(VariableExpression variableExpression, object o) {
            bool valueNeeded = (bool) o;

            Address adr = variableExpression.VarDeclaration.Address;
            int register = DisplayRegister(currentLevel, adr.level);

            if (valueNeeded)
                Emit(Machine.LOADop, 1, register, adr.displacement);

            return adr;
        }

        public object PopVariableDeclaration(VariableDeclaration variableDeclaration, object o) {
            variableDeclaration.Address = (Address) o;

            return new Address((Address) o, 1);
        }

        public object PopFunctionDeclaration(FunctionDeclaration functionDeclaration, object o) {
            throw new NotImplementedException();
        }

        public object PopInteger(IntegerLiteral integerLiteral, object o) {
            return integerLiteral.spelling;
        }
    }

}
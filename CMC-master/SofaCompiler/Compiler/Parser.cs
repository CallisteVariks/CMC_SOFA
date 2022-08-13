using System;
using SofaCompiler.Compiler.AST;

namespace SofaCompiler.Compiler {

    public class Parser {
        private readonly Scanner _scan;

        private Token _currentTerminal;

        public Parser(Scanner scan) {
            _scan = scan;
            _currentTerminal = _scan.Scan();
        }

        private void Accept(byte expected) {
            if (_currentTerminal.kind == expected)
                _currentTerminal = _scan.Scan();
            else
                Console.WriteLine("Expected token of kind " + expected);
        }

        public object ParseProgram() {
            SOFA sofa = ParseSofa();

            if (_currentTerminal.kind != Token.EOT)
                Console.WriteLine("Tokens found after end of program");

            return new AST.Program(sofa);
        }

        private SOFA ParseSofa() {
            Accept(Token.SOFA);
            Declarations decs = ParseDeclarations();
          
            Statements statements = ParseStatements();

            return new SOFA(decs, statements);
        }

        private object Address;
        
        private Declarations ParseDeclarations() {
            Declarations decs = new Declarations();
            while (_currentTerminal.kind == Token.DUST ||
                   _currentTerminal.kind == Token.NOP ||
                   _currentTerminal.kind == Token.SPOT ||
                   _currentTerminal.kind == Token.VOID) {
                decs.declarations.Add(ParseOneDeclaration());
            }
            
            return decs;
        }

        private Declaration ParseOneDeclaration() {
            switch (_currentTerminal.kind) {
                case Token.DUST:
                    Accept(Token.DUST);
                    Identifier idDust = ParseIdentifier();
                    Accept(Token.OPERATOR);

                    return new VariableDeclaration(idDust);
                case Token.NOP:
                    Accept(Token.NOP);
                    Identifier idNop = ParseIdentifier();
                    Accept(Token.SEMICOLON);

                    return new VariableDeclaration(idNop);
                case Token.SPOT:
                    Accept(Token.SPOT);
                    Identifier idSpot = ParseIdentifier();
                    Accept(Token.SEMICOLON);

                    return new VariableDeclaration(idSpot);
                case Token.VOID:
                    Accept(Token.VOID);
                    Identifier name = ParseIdentifier();
                    Accept(Token.COLON);
                    Declarations decs;
                    decs = _currentTerminal.kind == Token.IDENTIFIER ? ParseIdList() : new Declarations();

                    Accept(Token.LEFT_BRAKET);
                    SOFA sofa = ParseSofa();
                    Accept(Token.OUT);
                    Expression retExp = ParseExpression();
                    Accept(Token.RIGHT_BRAKET);

                    return new FunctionDeclaration(name, decs, sofa, retExp);

                default:
                    Console.WriteLine("var or func expected");

                    return null;
            }
        }

        private Declarations ParseIdList() {
            Declarations list = new Declarations();

            list.declarations.Add(new VariableDeclaration(ParseIdentifier()));

            while (_currentTerminal.kind == Token.COMMA) {
                Accept(Token.COMMA);
                list.declarations.Add(new VariableDeclaration(ParseIdentifier()));
            }

            return list;
        }

        private Statements ParseStatements() {
            Statements stats = new Statements();
            while (_currentTerminal.kind == Token.IDENTIFIER ||
                   _currentTerminal.kind == Token.OPERATOR ||
                   _currentTerminal.kind == Token.COLON) {
                stats.StatementsList.Add(ParseOneStatement());
            }

            return stats;
        }

        private Statement ParseOneStatement() {
            switch (_currentTerminal.kind) {
                case Token.IDENTIFIER:
                case Token.OPERATOR:
                case Token.COLON:
                    
                    Expression exp = ParseExpression();
                    Accept(Token.SEMICOLON);

                    return new ExpressionStatement(exp);

                default:
                    Console.WriteLine("Error in statement");

                    return null;
            }
        }

        private Expression ParseExpression() {
            Expression res = ParseExpression1();

            if (!_currentTerminal.IsAssignOperator()) return res;

            Operator op = ParseOperator();
            Expression tmp = ParseExpression();

            res = new BinaryExpression(op, res, tmp);

            return res;
        }

        private Expression ParseExpression1() {
            Expression res = ParseExpression2();

            while( _currentTerminal.IsAddOperator() ) {
                Operator op = ParseOperator();
                Expression tmp = ParseExpression2();
          
                res = new BinaryExpression( op, res, tmp );
            }

            return res;
        }

        private Expression ParseExpression2() {
            Expression res = ParsePrimary();
            
            while( _currentTerminal.IsMulOperator() ) {
                Operator op =ParseOperator();
                Expression tmp = ParsePrimary();
          
                res = new BinaryExpression( op, res, tmp );
            }

            return res;
        }

        private Expression ParsePrimary() {
            switch (_currentTerminal.kind) {
                case Token.IDENTIFIER:
                    Identifier name = ParseIdentifier();

                    if (_currentTerminal.kind != Token.LEFT_BRAKET) return new VariableExpression(name);

                    /*
                    Accept(Token.LEFT_BRAKET);
                        Declaration args;

                        if (_currentTerminal.kind == Token.IDENTIFIER ||
                            _currentTerminal.kind == Token.INTEGER ||
                            _currentTerminal.kind == Token.OPERATOR ||
                            _currentTerminal.kind == Token.LEFT_BRAKET)

                            args = ParseExpressionList();
                        else
                            args = new Expressions();


                        Accept(Token.RIGHT_BRAKET);

                        return new CallExpression(args, name);*/

                    return null;
                case Token.DUST:
                    Identifier nameDust = ParseIdentifier();

                    if (_currentTerminal.kind != Token.LEFT_BRAKET) return new VariableExpression(nameDust);

                    return null;

                case Token.SPOT:
                    Identifier nameSpot = ParseIdentifier();

                    if (_currentTerminal.kind != Token.LEFT_BRAKET) return new VariableExpression(nameSpot);

                    return null;
                case Token.NOP:
                    Identifier nameNop = ParseIdentifier();

                    if (_currentTerminal.kind != Token.LEFT_BRAKET) return new VariableExpression(nameNop);

                    return null;
                case Token.COLON:
                    Accept(Token.COLON);
                    Expression res = ParseExpression();

                    //do something if needed
                    return res;
                case Token.OPERATOR:
                    Operator op = ParseOperator();
                    Expression exp = ParsePrimary();

                    return new UnaryExpression(op, exp);
                /*
                case Token.INTEGERLITERAL:
                    IntegerLiteral lit = parseIntegerLiteral();
    
                    return new IntLitExpression(lit);
    
                case Token.LEFTPARAN:
                    accept(Token.LEFTPARAN);
                    Expression res = parseExpression();
                    accept(Token.RIGHTPARAN);
    
                    return res;
    */
                default:
                    Console.WriteLine("Error in primary");

                    return null;
            }
        }

        private Operator ParseOperator() {
            if (_currentTerminal.kind == Token.OPERATOR) {
                Operator res = new Operator(_currentTerminal.tok);
                _currentTerminal = _scan.Scan();

                return res;
            }
            else {
                Console.WriteLine("Operator expected");

                return new Operator("???");
            }
        }

        private Identifier ParseIdentifier() {
            if (_currentTerminal.kind == Token.IDENTIFIER) {
                Identifier res = new Identifier(_currentTerminal.tok);
                _currentTerminal = _scan.Scan();

                return res;
            }

            Console.WriteLine("Identifier expected");

            return new Identifier("???");
        }

        private Expressions ParseExpressionList() {
            Expressions expressions = new Expressions();

            expressions.ExpressionList.Add(ParseExpression());
            while (_currentTerminal.kind == Token.COMMA) {
                Accept(Token.COMMA);
                expressions.ExpressionList.Add(ParseExpression());
            }

            return expressions;
        }
    }

}
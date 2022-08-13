namespace SofaCompiler.Compiler.AST {

    public class SOFA : AST {
        private Declarations _declarations;

        public Declarations Decs => _declarations;

        private readonly Statements _statements;

        public Statements Statements => _statements;

        public SOFA(Declarations decs, Statements statements) {
            _declarations = decs;
            _statements = statements;
        }

        public override object Pop(IPopping popping, object o) {
            return popping.PopSofa(this, o);
        }
    }

}
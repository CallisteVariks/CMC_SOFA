namespace SofaCompiler.Compiler.AST {

    public class FunctionDeclaration : Declaration {

        public Identifier name;

        public Declarations decs;

        public SOFA sofa;

        public Expression retExp;

        public Address Address { get; set; }


        public FunctionDeclaration(Identifier name, Declarations decs, SOFA sofa, Expression retExp) {
            this.name = name;
            this.decs = decs;
            this.sofa = sofa;
            this.retExp = retExp;
        }

        public override object Pop(IPopping popping, object o) {
            return popping.PopFunctionDeclaration(this, o);
        }
    }

}
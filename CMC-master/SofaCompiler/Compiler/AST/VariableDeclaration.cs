namespace SofaCompiler.Compiler.AST {

    public class VariableDeclaration : Declaration {

        public Identifier Id { get; set; }
        public Address Address { get; set; }

        public VariableDeclaration(Identifier id) {
            Id = id;
        }

        public override object Pop(IPopping popping, object o) {
            return popping.PopVariableDeclaration(this, o);
        }
    }

}
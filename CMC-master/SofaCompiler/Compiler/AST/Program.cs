namespace SofaCompiler.Compiler.AST {

    public class Program : AST {
        private readonly SOFA _sofa;

        public SOFA Sofa => _sofa;

        public Program(SOFA sofa) {
            _sofa = sofa;
        }

        public override object Pop(IPopping popping, object o) {
            return popping.PopProgram(this, o);
        }

    }

}
using System.Collections;

namespace SofaCompiler.Compiler.AST {

    public class Declarations : AST {

        public ArrayList declarations = new ArrayList();


        public override object Pop(IPopping popping, object arg) {
            return popping.PopDeclarations(this, arg);
        }
    }

}
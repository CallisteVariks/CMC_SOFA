
using SofaCompiler.Compiler.AST;

namespace SofaCompiler.Compiler {

    public class Operator : Terminal{
        
        public Operator( string spelling )
        {
            this.spelling = spelling;
        }
        
        public override object Pop(IPopping popping, object o) {
            return popping.PopOperator( this, o);
        }
    }

}
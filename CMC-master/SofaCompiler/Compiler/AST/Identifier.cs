using System;

namespace SofaCompiler.Compiler.AST {

    public class Identifier : Terminal {
        
        public Declaration declaration;
	
        public Identifier( String spelling )
        {
            this.spelling = spelling;
        }
	
        public override object Pop( IPopping popping, object o)
        {
            return popping.PopIdentifier( this, o );
        }
    }

}
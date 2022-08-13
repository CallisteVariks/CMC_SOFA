namespace SofaCompiler.Compiler {

    public class Address {
        public int level;
        public int displacement;
	
	
        public Address()
        {
            level = 0;
            displacement = 0;
        }
	
	
        public Address( int level, int displacement )
        {
            this.level = level;
            this.displacement = displacement;
        }
	
	
        public Address( Address a, int increment )
        {
            level = a.level;
            displacement = a.displacement + increment;
        }
	
	
        public Address( Address a )
        {
            level = a.level + 1;
            displacement = 0;
        }
	
	
        public string toString()
        {
            return "level=" + level + " displacement=" + displacement;
        }
    }

}
using SofaCompiler.Compiler.AST;

namespace SofaCompiler.Compiler {

    internal class Pillow {

        public int Level { get; }
        public string Id { get; }
        public Declaration Declaration { get; }

        public Pillow(int level, string id, Declaration declaration) {
            Level = level;
            Id = id;
            Declaration = declaration;
        }
        public string ToString()
        {
            return Level + "," + Id;
        }
    }

}
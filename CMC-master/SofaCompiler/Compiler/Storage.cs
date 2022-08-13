using System;
using System.Collections;
using SofaCompiler.Compiler.AST;

namespace SofaCompiler.Compiler {

    internal class Storage {

        private readonly ArrayList storage = new ArrayList();

        private int level = 0;

        public Storage() {
        }

        public void Add(string id, Declaration declaration) {
            Pillow pillow = Find(id);

            if (pillow != null && pillow.Level == level)
                Console.WriteLine(id + " declared twice");
            else
                storage.Add(new Pillow(level, id, declaration));
        }


        public Declaration Get(string id) {
            Pillow pillow = Find(id);

            return pillow?.Declaration;
        }

        public void Open() {
            ++level;
        }


        public void Close() {
            int pos = storage.Count - 1;
            while (pos >= 0 && ((Pillow) storage[pos]).Level == level) {
                storage.Remove(pos);
                pos--;
            }

            level--;
        }


        private Pillow Find(string id) {
            for (int i = storage.Count - 1; i >= 0; i--)
                if (((Pillow) storage[i]).Id.Equals(id))
                    return (Pillow) storage[i];

            return null;
        }
    }

}
using System;
using System.IO;

namespace SofaCompiler.TAM {


    public class Instruction {

        public Instruction() {
            op = 0;
            r = 0;
            n = 0;
            d = 0;
        }

        // Java has no type synonyms, so the following representations are
        // assumed:
        //
        //  type
        //    OpCode = 0..15;  {4 bits unsigned}
        //    Length = 0..255;  {8 bits unsigned}
        //    Operand = -32767..+32767;  {16 bits signed}

        // Represents TAM instructions.
        public int op; // OpCode

        public int r; // RegisterNumber

        public int n; // Length

        public int d; // Operand

        public void Write(BinaryWriter output) {
            output.Write(op);
            output.Write(r);
            output.Write(n);
            output.Write(d);
        }

        public static Instruction read(BinaryReader input) {
            Instruction inst = new Instruction();
            try {
                inst.op = input.ReadInt32();
                inst.r = input.ReadInt32();
                inst.n = input.ReadInt32();
                inst.d = input.ReadInt32();

                return inst;
            }
            catch (Exception s) {
                return null;
            }
        }
    }

}
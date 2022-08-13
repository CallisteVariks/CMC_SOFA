using System;
using SofaCompiler.Compiler;
using System.Windows.Forms;
using SofaCompiler.Compiler.AST;

namespace SofaCompiler {

    internal class Program {
        [STAThread]
        public static void Main(string[] args) {
            OpenFileDialog dialog = new OpenFileDialog();
            DialogResult result = dialog.ShowDialog();

            if (result != DialogResult.Cancel) {
                string sourceName = dialog.FileName;

                SourceFile inS = new SourceFile(sourceName);
                Scanner s = new Scanner(inS);
                Parser p = new Parser(s);
                Checker c = new Checker();
                Encoder e = new Encoder();

                Compiler.AST.Program program = (Compiler.AST.Program) p.ParseProgram();
                c.Check(program);
                e.Encode(program);
                string targetName;
                if (sourceName.EndsWith(".txt"))
                    targetName = sourceName.Substring(0, sourceName.Length - 4) + ".tam";
                else
                    targetName = sourceName + ".tam";

                e.SaveTargetProgram(targetName);
            }
        }
    }

}
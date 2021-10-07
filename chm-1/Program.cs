using System;
using System.IO;

namespace chm_1
{
    class Program
    {
        private const string InputTextFile = "input.txt";
        private const string InputAnswerTextFile = "answer_input.txt";
        private const string OutputTextFile = "output.txt";

        static void Main(string[] args)
        {
            using var inputFile = new StreamReader(InputTextFile);
            using var inputAnswerFile = new StreamReader(InputAnswerTextFile);

            using var outputFile = new StreamWriter(OutputTextFile);

            var matrixA = Utils.MatrixFromFile(inputFile);
            var vectorB = Utils.VectorFromFile(inputFile);
            var exactVectorX = Utils.VectorFromFile(inputAnswerFile);

            Utils.Pprint(matrixA);

            Utils.Pprint(vectorB);

            matrixA.LuDecomposition();

            Utils.Pprint(matrixA);

            matrixA.CheckDecomposition();

            var vectorX = Linalg.Solve(matrixA, vectorB);

            Console.WriteLine("\nResult:");
            Utils.Pprint(vectorX);

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Utils.Pprint(Linalg.Abs(vectorX, exactVectorX));
            Console.ResetColor();

            Utils.ExportToFile(outputFile, vectorX, Linalg.Abs(vectorX, exactVectorX));
        }
    }
}
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

            var matrixA1 = new float[matrixA.Size, matrixA.Size];

            for (var i = 0; i < matrixA.Size; i++)
            {
                for (var j = 0; j < matrixA.Size; j++)
                {
                    matrixA1[i, j] = matrixA[i, j];
                }
            }

            var vectorB = Utils.VectorFromFile(inputFile);

            var vectorB1 = new float[vectorB.Length];
            vectorB.AsSpan().CopyTo(vectorB1);

            var exactVectorX = Utils.VectorFromFile(inputAnswerFile);

            Utils.Pprint(matrixA);

            Utils.Pprint(vectorB);

            matrixA.LuDecomposition();

            Utils.Pprint(matrixA);

            matrixA.CheckDecomposition();

            var vectorX = LinAlg.Solve(matrixA, vectorB);
            var vectorX1 = LinAlg.Gauss.Solve(matrixA1, vectorB1);

            Console.Write("\nResult:");
            Utils.Pprint(vectorX);

            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.Write("\nBy Gaussian elimination:");
            Utils.Pprint(vectorX1);

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write("\nError measure:");
            Utils.Pprint(LinAlg.Abs(vectorX, exactVectorX));
            Console.ResetColor();

            Utils.ExportToFile(outputFile, vectorX, LinAlg.Abs(vectorX, exactVectorX));
        }
    }
}
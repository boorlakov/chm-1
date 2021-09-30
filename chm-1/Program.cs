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
            // TODO Add try-catch construction for reading from the file 
            using var inputFile = new StreamReader(InputTextFile);
            using var inputAnswerFile = new StreamReader(InputAnswerTextFile);
            using var outputFile = new StreamReader(OutputTextFile);

            var matrixA = Utils.MatrixFromFile(inputFile);
            var b = Utils.VectorFromFile(inputFile);
            var exactVectorX = Utils.VectorFromFile(inputAnswerFile);

            Console.WriteLine(matrixA);

            Utils.Pprint(matrixA);

            matrixA.LU_decomposition();

            Utils.Pprint(matrixA);

            matrixA.check_decomposition();

            var vectorX = Linalg.Solve(matrixA, b);

            Utils.ExportToFile(outputFile, vectorX, Linalg.Abs(vectorX, exactVectorX));
        }
    }
}
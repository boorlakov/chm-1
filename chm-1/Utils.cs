using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace chm_1
{
    public static class Utils
    {
        /// <param name="file"> input file for initialise matrix vectorX</param>
        /// <returns>Complete matrix</returns>
        public static Matrix MatrixFromFile(StreamReader file)
        {
            var ln = file.ReadLine();
            var size = int.Parse(ln!);

            ln = file.ReadLine();
            var di = ln!.Split(' ').Select(double.Parse).ToArray();

            ln = file.ReadLine();
            var ia = ln!.Split(' ').Select(int.Parse).ToArray();

            ln = file.ReadLine();
            var au = ln!.Split(' ').Select(double.Parse).ToArray();

            ln = file.ReadLine();
            var al = ln!.Split(' ').Select(double.Parse).ToArray();

            return new Matrix(size, di, ia, au, al);
        }

        public static double[] VectorFromFile(StreamReader file)
        {
            var ln = file.ReadLine();
            var b = ln!.Split(' ').Select(double.Parse).ToArray();
            return b;
        }

        public static void ExportToFile(StreamWriter outputFile, double[] vectorX, double[] absVector)
        {
            var sb = new StringBuilder();

            foreach (var item in vectorX)
            {
                sb.AppendFormat("{0} ", item);
            }

            sb.Append('\n');

            foreach (var item in absVector)
            {
                sb.AppendFormat("{0} ", item);
            }

            var text = sb.ToString();

            outputFile.Write(text);
        }

        /// <summary>
        ///     Perfect print is using for debugging profile format.
        ///     Prints as matrix is in default format.
        /// </summary>
        public static void Pprint(Matrix matrixA)
        {
            Console.WriteLine("\nMatrix PPRINT:\n");

            if (!matrixA.Decomposed)
            {
                Console.WriteLine("Undecomposed:");

                for (var i = 0; i < matrixA.Size; i++)
                {
                    for (var j = 0; j < matrixA.Size; j++)
                    {
                        Console.Write($"{matrixA[i, j]} ");
                    }

                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("Decomposed:");

                Console.WriteLine("L:");

                for (var i = 0; i < matrixA.Size; i++)
                {
                    for (var j = 0; j < matrixA.Size; j++)
                    {
                        Console.Write($"{matrixA.L(i, j)} ");
                    }

                    Console.WriteLine();
                }

                Console.WriteLine("U:");

                for (var i = 0; i < matrixA.Size; i++)
                {
                    for (var j = 0; j < matrixA.Size; j++)
                    {
                        Console.Write($"{matrixA.U(i, j)} ");
                    }

                    Console.WriteLine();
                }
            }
        }

        public static void Pprint(IEnumerable<double> vectorX)
        {
            Console.WriteLine("\nVector PPRINT:\n");

            foreach (var item in vectorX)
            {
                Console.Write("{0} ", item);
            }

            Console.WriteLine();
        }
    }
}
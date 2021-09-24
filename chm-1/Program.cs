using System;
using System.IO;

namespace chm_1
{
    class Program
    {
        private const string TextFile = "input.txt";

        static void Main(string[] args)
        {
            // TODO Add try-catch construction for reading from the file 
            using var file = new StreamReader(TextFile);

            var matrixA = Utils.LoadFrom(file);

            Console.WriteLine(matrixA);
        }
    }
}
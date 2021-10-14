using System;

namespace chm_1
{
    public static class LinAlg
    {
        public static double[] Solve(Matrix matrixA, double[] vectorB)
        {
            var vectorY = new double[matrixA.Size];

            // Forward Substitution
            for (var i = 0; i < matrixA.Size; i++)
            {
                vectorY[i] = vectorB[i];
                var indAl = matrixA.Ia[i + 1] - 1 - i;

                for (var j = i - (matrixA.Ia[i + 1] - matrixA.Ia[i]); j < i; j++)
                {
                    vectorY[i] -= matrixA.Al[indAl + j] * vectorY[j];
                }

                if (!matrixA.Di[i].Equals(0.0))
                {
                    vectorY[i] /= matrixA.Di[i];
                }
            }

            // DEBUG INFO
            Console.WriteLine("y:");

            foreach (var item in vectorY)
            {
                Console.Write($"{item:G15} ");
            }

            Console.WriteLine();

            // Backward Substitution
            // We can store elements in vectorB, because now vectorB is useless.
            for (var i = matrixA.Size - 1; i >= 0; i--)
            {
                vectorB[i] = vectorY[i];

                for (var j = i + 1; j < matrixA.Size; j++)
                {
                    if (i + 1 > j - (matrixA.Ia[j + 1] - matrixA.Ia[j]))
                    {
                        vectorB[i] -= matrixA.Au[matrixA.Ia[j + 1] + i - j - 1] * vectorB[j];
                    }
                }
            }

            return vectorB;
        }

        public static double[] Abs(double[] lhs, double[] rhs)
        {
            var res = new double[lhs.Length];

            for (var i = 0; i < res.Length; i++)
            {
                res[i] = Math.Abs(lhs[i] - rhs[i]);
            }

            return res;
        }

        public abstract class Gauss
        {
            public static double[] Solve(double[,] matrixA, double[] vectorB)
            {
                const int axisX = 0;

                var extendedMatrixRowSize = matrixA.GetLength(axisX);
                var extendedMatrixColSize = extendedMatrixRowSize + 1;

                var extendedMatrix = ExtendMatrix(matrixA, vectorB);

                var pivotRow = 0;
                var pivotCol = 0;

                while (pivotRow < extendedMatrixRowSize && pivotCol < extendedMatrixColSize)
                {
                    var iMax = ArgMax(pivotRow, extendedMatrixRowSize, pivotCol, extendedMatrix);

                    if (extendedMatrix[iMax, pivotCol] == 0.0)
                    {
                        pivotCol++;
                    }
                    else
                    {
                        SwapRows(pivotRow, iMax, extendedMatrix);

                        for (var i = pivotRow + 1; i < extendedMatrixRowSize; i++)
                        {
                            var f = extendedMatrix[i, pivotCol] / extendedMatrix[pivotRow, pivotCol];
                            extendedMatrix[i, pivotCol] = 0.0;

                            for (var j = pivotCol + 1; j < extendedMatrixColSize; j++)
                            {
                                extendedMatrix[i, j] -= extendedMatrix[pivotRow, j] * f;
                            }
                        }

                        pivotRow++;
                        pivotCol++;
                    }
                }

                return vectorB;
            }

            private static void SwapRows(int src, int dst, double[,] matrixA)
            {
                const int axisY = 1;

                for (var j = 0; j < matrixA.GetLength(axisY); j++)
                {
                    Swap(ref matrixA[src, j], ref matrixA[dst, j]);
                }
            }

            private static double[,] ExtendMatrix(double[,] matrixA, double[] vectorB)
            {
                var extendedMatrixRowSize = matrixA.GetLength(0);
                var extendedMatrixColSize = extendedMatrixRowSize + 1;

                var extendedMatrix = new double[extendedMatrixRowSize, extendedMatrixColSize];

                for (var i = 0; i < extendedMatrixRowSize; i++)
                {
                    for (var j = 0; j < extendedMatrixRowSize; j++)
                    {
                        extendedMatrix[i, j] = matrixA[i, j];
                    }
                }

                for (var i = 0; i < extendedMatrixRowSize; i++)
                {
                    extendedMatrix[i, extendedMatrixColSize - 1] = vectorB[i];
                }

                return extendedMatrix;
            }

            private static void Swap(ref double src, ref double dst) => (src, dst) = (dst, src);

            private static int ArgMax(int src, int dst, int pivotCol, double[,] matrixA)
            {
                var argmax = -1;

                for (var i = src; i < dst; i++)
                {
                    if (argmax <= Math.Abs(matrixA[i, pivotCol]))
                    {
                        argmax = i;
                    }
                }

                return argmax;
            }
        }
    }
}
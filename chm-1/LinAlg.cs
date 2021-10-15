using System;

namespace chm_1
{
    public static class LinAlg
    {
        public static double[] Solve(Matrix matrixA, double[] vectorB)
        {
            var vectorY = ForwardSubstitution(matrixA, vectorB);

            vectorB = BackSubstitution(matrixA, vectorB, vectorY);

            return vectorB;
        }

        private static double[] ForwardSubstitution(Matrix matrixA, double[] vectorB)
        {
            var vectorY = new double[matrixA.Size];

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

            return vectorY;
        }

        private static double[] BackSubstitution(Matrix matrixA, double[] vectorB, double[] vectorY)
        {
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
                var extendedMatrix = Elimination(matrixA, vectorB);

                var vectorX = BackSubstitution(extendedMatrix);

                return vectorX;
            }

            private static double[] BackSubstitution(double[,] extendedMatrix)
            {
                var rowSize = extendedMatrix.GetLength(Axis.X);
                var colSize = extendedMatrix.GetLength(Axis.Y);

                var vectorX = new double[rowSize];

                for (var row = rowSize - 1; row >= 0; row--)
                {
                    vectorX[row] = extendedMatrix[row, colSize - 1];

                    for (var col = row + 1; col < rowSize; col++)
                    {
                        vectorX[row] -= extendedMatrix[row, col] * extendedMatrix[col, colSize - 1];
                    }

                    if (!extendedMatrix[row, row].Equals(0.0))
                    {
                        vectorX[row] /= extendedMatrix[row, row];
                    }
                }

                return vectorX;
            }

            private static double[,] Elimination(double[,] matrixA, double[] vectorB)
            {
                var rowSize = matrixA.GetLength(Axis.X);
                var colSize = rowSize + 1;

                var extendedMatrix = ExtendMatrix(matrixA, vectorB);

                var pivotRow = 0;
                var pivotCol = 0;

                while (pivotRow < rowSize && pivotCol < colSize)
                {
                    var maxRow = ArgMax(pivotRow, rowSize, pivotCol, extendedMatrix);

                    if (extendedMatrix[maxRow, pivotCol] == 0.0)
                    {
                        pivotCol++;
                    }
                    else
                    {
                        SwapRows(pivotRow, maxRow, extendedMatrix);

                        for (var row = pivotRow + 1; row < rowSize; row++)
                        {
                            var f = extendedMatrix[row, pivotCol] / extendedMatrix[pivotRow, pivotCol];
                            extendedMatrix[row, pivotCol] = 0.0;

                            for (var col = pivotCol + 1; col < colSize; col++)
                            {
                                extendedMatrix[row, col] -= extendedMatrix[pivotRow, col] * f;
                            }
                        }

                        pivotRow++;
                        pivotCol++;
                    }
                }

                return extendedMatrix;
            }

            private static void SwapRows(int src, int dst, double[,] matrixA)
            {
                for (var j = 0; j < matrixA.GetLength(Axis.Y); j++)
                {
                    Swap(ref matrixA[src, j], ref matrixA[dst, j]);
                }
            }

            private static double[,] ExtendMatrix(double[,] matrixA, double[] vectorB)
            {
                var extendedMatrixRowSize = matrixA.GetLength(Axis.X);
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

            private abstract class Axis
            {
                public const int X = 0;
                public const int Y = 1;
            }
        }
    }
}
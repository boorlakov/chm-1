using System;

namespace chm_1
{
    public static class Linalg
    {
        public static double[] Solve(Matrix matrixA, double[] vectorB)
        {
            var vectorY = new double[matrixA.Size];
            // Code for normal format matrix
            // TODO: Use profile format matrix
            vectorY[0] = vectorB[0];

            // Forward prop
            for (var i = 1; i < matrixA.Size; i++)
            {
                vectorY[i] = vectorB[i];

                for (var j = 0; j < i; j++)
                {
                    vectorY[i] -= matrixA.L(i, j) * vectorY[j];
                }
            }

            // Backward prop
            // we can store elements in b
            for (var i = matrixA.Size - 1; i >= 0; i--)
            {
                vectorB[i] = vectorY[i];

                for (var j = i + 1; j < matrixA.Size; j++)
                {
                    vectorB[i] -= matrixA.U(i, j) * vectorB[j];
                }

                vectorB[i] /= matrixA.U(i, i);
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
    }
}
using System;

namespace chm_1
{
    public static class Linalg
    {
        public static double[] Solve(Matrix A, double[] b)
        {
            var y = new double[A.Size];
            // Code for normal format matrix
            // TODO: Use profile format matrix
            y[0] = b[0];

            // Forward prop
            for (var i = 1; i < A.Size; i++)
            {
                y[i] = b[i];

                for (var j = 0; j < i; j++)
                {
                    y[i] -= A.L(i, j) * y[j];
                    y[i] -= A.Al[A.Ia[i + 1] + j - 1 - i] * y[j];
                }

                y[i] /= A.L(i, i);
                //y[i] /= A.Di[i];
            }

            // Backward prop
            // we can store elements in b
            for (var i = A.Size - 1; i >= 0; i--)
            {
                b[i] = y[i];

                for (var j = i + 1; j < A.Size; j++)
                {
                    b[i] -= A.U(i, j) * b[j];
                    // b[i] -= A.Au[A.Ia[j + 1] + i - j - 1] * b[j];
                }
            }

            return b;
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
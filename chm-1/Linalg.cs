using System;

namespace chm_1
{
    public static class Linalg
    {
        public static double[] Solve(Matrix A, double[] b)
        {
            var y = new double[A.Size];

            // Forward Substitution
            for (var i = 0; i < A.Size; i++)
            {
                y[i] = b[i];
                var indAl = A.Ia[i + 1] - 1 - i;

                for (var j = i - (A.Ia[i + 1] - A.Ia[i]); j < i; j++)
                {
                    y[i] -= A.Al[indAl + j] * y[j];
                }

                if (!A.Di[i].Equals(0.0))
                {
                    y[i] /= A.Di[i];
                }
            }

            // DEBUG INFO
            Console.WriteLine("y:");

            foreach (var item in y)
            {
                Console.Write($"{item:G15} ");
            }

            Console.WriteLine();

            // Backward Substitution
            // We can store elements in b, because now b is useless.
            for (var i = A.Size - 1; i >= 0; i--)
            {
                b[i] = y[i];

                for (var j = i + 1; j < A.Size; j++)
                {
                    if (i + 1 > j - (A.Ia[j + 1] - A.Ia[j]))
                    {
                        b[i] -= A.Au[A.Ia[j + 1] + i - j - 1] * b[j];
                    }
                }
            }

            return b;
        }

        public static double[] SolveGauss(Matrix A, double[] b)
        {
            for (var sourceRow = 0; sourceRow + 1 < A.Size; sourceRow++)
            {
                for (var destRow = sourceRow + 1; destRow < A.Size; destRow++)
                {
                    var df = A[sourceRow, sourceRow];
                    var sf = A[destRow, sourceRow];

                    for (var i = 0; i < A.Size; i++)
                    {
                        A[destRow, i] = A[destRow, i] * df - A[sourceRow, i] * sf;
                    }
                }
            }

            for (var row = A.Size - 1; row >= 0; row--)
            {
                var f = A[row, row];

                if (f == 0)
                {
                    throw new Exception("No solution");
                }

                for (var i = 0; i < A.Size; i++)
                {
                    A[row, i] /= f;
                }

                for (var destRow = 0; destRow < row; destRow++)
                {
                    A[destRow, A.Size - 1] -= A[destRow, row] * A[row, A.Size - 1];
                    A[destRow, row] = 0;
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
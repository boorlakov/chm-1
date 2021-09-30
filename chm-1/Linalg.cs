using System;

namespace chm_1
{
    public static class Linalg
    {
        public static double[] Solve(Matrix A, double[] b)
        {
            var y = new double[A.Size];
            y[0] = b[0];

            // Forward Substitution
            for (var i = 1; i < A.Size; i++)
            {
                y[i] = b[i];

                for (var j = i - (A.Ia[i + 1] - A.Ia[i]); j < i; j++)
                {
                    y[i] -= A.Al[A.Ia[i + 1] + j - 1 - i] * y[j];
                }

                y[i] /= A.Di[i];
            }

            Console.WriteLine("y:");

            foreach (var item in y)
            {
                Console.Write($"{item} ");
            }

            Console.WriteLine();

            b[A.Size - 1] = y[A.Size - 1];

            // Backward Substitution
            // We can store elements in b, because now b is useless
            for (var i = A.Size - 2; i >= 0; i--)
            {
                b[i] = y[i];

                for (var j = i + 1; j < A.Size; j++)
                {
                    if (!(i + 1 <= j - (A.Ia[j + 1] - A.Ia[j])))
                    {
                        b[i] -= A.Au[A.Ia[j + 1] + i - j - 1] * b[j];
                    }
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
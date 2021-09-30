using System;
using System.Text;
using static System.Math;

namespace chm_1
{
    /// <summary>
    ///     Matrix as in math. Data is stored in profile format
    /// </summary>
    public class Matrix
    {
        /// <summary>
        ///     al is for elements of lower triangular part of matrix
        /// </summary>
        /// <returns></returns>
        private readonly double[] _al;

        /// <summary>
        ///     au is for elements of upper triangular part of matrix
        /// </summary>
        private readonly double[] _au;

        /// di is for diagonal elements
        private readonly double[] _di;

        /// <summary>
        ///     ia is for profile matrix. i.e. by manipulating ia elements we can use our matrix
        ///     much more time and memory efficient
        /// </summary>
        private readonly int[] _ia;

        public Matrix()
        {
            _di = Array.Empty<double>();
            _au = Array.Empty<double>();
            _al = Array.Empty<double>();
            _ia = Array.Empty<int>();
            Decomposed = false;
        }

        public Matrix(int size, double[] di, int[] ia, double[] au, double[] al)
        {
            Size = size;
            _di = di ?? throw new ArgumentNullException(nameof(di));
            _ia = ia ?? throw new ArgumentNullException(nameof(ia));
            _au = au ?? throw new ArgumentNullException(nameof(au));
            _al = al ?? throw new ArgumentNullException(nameof(al));
            Decomposed = false;
        }

        public bool Decomposed { get; private set; }

        /// <summary>
        ///     Warning: accessing the data in that way is not fast
        /// </summary>
        /// <param name="i"> row </param>
        /// <param name="j"> column </param>
        public double this[int i, int j] => GetElement(i, j);

        public int Size { get; }

        /// <summary>
        ///     LU-decomposition with value=1 in diagonal elements of U matrix.
        ///     Corrupts base object. To access data as one matrix you need to build it from L and U.
        /// </summary>
        /// <exception cref="DivideByZeroException"> If diagonal element is zero </exception>
        public void LU_decomposition()
        {
            for (var i = 1; i < Size; i++)
            {
                var sumDi = 0.0;
                var j0 = i - (_ia[i + 1] - _ia[i]);

                for (var ii = _ia[i] - 1; ii < _ia[i + 1] - 1; ii++)
                {
                    var j = ii - _ia[i] + j0 + 1;
                    var jBeg = _ia[j];
                    var jEnd = _ia[j + 1];

                    if (jBeg < jEnd)
                    {
                        var j0J = j - (jEnd - jBeg);
                        var jjBeg = Max(j0, j0J);
                        var jjEnd = Max(j, i - 1);
                        var cL = 0.0;

                        for (var k = 0; k <= jjEnd - jjBeg - 1; k++)
                        {
                            var indAu = _ia[j] + jjBeg - j0J + k - 1;
                            var indAl = _ia[i] + jjBeg - j0 + k - 1;
                            cL += _au[indAu] * _al[indAl];
                        }

                        _al[ii] -= cL;
                        var cU = 0.0;

                        for (var k = 0; k <= jjEnd - jjBeg - 1; k++)
                        {
                            var indAl = _ia[j] + jjBeg - j0J + k - 1;
                            var indAu = _ia[i] + jjBeg - j0 + k - 1;
                            cU += _au[indAu] * _al[indAl];
                        }

                        _au[ii] -= cU;
                    }

                    if (_di[j] == 0.0)
                    {
                        throw new DivideByZeroException($"No dividing by zero. DEBUG INFO: [i:{i}; j:{j}]");
                    }

                    _au[ii] /= _di[j];
                    sumDi += _al[ii] * _au[ii];
                }

                _di[i] -= sumDi;
            }

            Decomposed = true;
        }

        /// <summary>
        ///     Was made for debugging LU-decomposition.
        /// </summary>
        /// <returns></returns>
        public void check_decomposition()
        {
            Console.WriteLine("\nLU-check:");

            for (var i = 0; i < Size; i++)
            {
                for (var j = 0; j < Size; j++)
                {
                    var c = 0.0;

                    for (var k = 0; k < Size; k++)
                    {
                        c += L(i, k) * U(k, j);
                    }

                    Console.Write($"{c} ");
                }

                Console.WriteLine();
            }
        }

        /// <summary>
        ///     u[i][j] of Upper triangular matrix U
        /// </summary>
        /// <param name="i"> rows </param>
        /// <param name="j"> columns</param>
        /// <exception cref="FieldAccessException"> If matrix is not decomposed </exception>
        /// <returns></returns>
        public double U(int i, int j)
        {
            if (!Decomposed)
            {
                throw new FieldAccessException("Matrix must be decomposed.");
            }

            if (i == j)
            {
                return 1.0;
            }

            return i < j ? this[i, j] : 0.0;
        }

        /// <summary>
        ///     l[i][j] of Lower triangular matrix L
        /// </summary>
        /// <param name="i"> rows </param>
        /// <param name="j"> columns</param>
        /// <exception cref="FieldAccessException"> If matrix is not decomposed </exception>
        /// <returns></returns>
        public double L(int i, int j)
        {
            if (!Decomposed)
            {
                throw new FieldAccessException("Matrix must be decomposed.");
            }

            return i >= j ? this[i, j] : 0.0;
        }

        /// <summary>
        ///     WARNING: Accessing data this way is not efficient
        ///     Because of profile format we need to refer A[i][j] special way. 
        ///     We have that method for accessing data more naturally.    
        /// </summary>
        /// <param name="i"> rows </param>
        /// <param name="j"> columns </param>
        /// <returns></returns>
        private double GetElement(int i, int j)
        {
            if (i == j)
            {
                return _di[i];
            }

            if (i > j)
            {
                return j + 1 <= i - (_ia[i + 1] - _ia[i]) ? 0.0 : _al[_ia[i + 1] + j - 1 - i];
            }

            return i + 1 <= j - (_ia[j + 1] - _ia[j]) ? 0.0 : _au[_ia[j + 1] + i - j - 1];
        }

        public override string ToString()
        {
            var sb = new StringBuilder($"{nameof(Matrix)}:\ndi:\n");

            foreach (var item in _di)
            {
                sb.AppendFormat("{0} ", item);
            }

            sb.Append("\nia:\n");

            foreach (var item in _ia)
            {
                sb.AppendFormat("{0} ", item);
            }

            sb.Append("\nau:\n");

            foreach (var item in _au)
            {
                sb.AppendFormat("{0} ", item);
            }

            sb.Append("\nal:\n");

            foreach (var item in _al)
            {
                sb.AppendFormat("{0} ", item);
            }

            return sb.ToString();
        }
    }
}
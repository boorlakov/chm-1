using System;
using System.Text;

namespace chm_1
{
    /// <summary>
    /// Matrix as in math. Data is stored in profile format
    /// </summary>
    public class Matrix
    {
        /// al is for elements of lower triangular part of matrix
        private readonly double[] _al;

        /// au is for elements of upper triangular part of matrix
        private readonly double[] _au;

        /// di is for diagonal elements
        private readonly double[] _di;

        /// ia is for profile matrix. i.e. by manipulating ia elements we can use our matrix
        /// much more time and memory efficient
        private readonly int[] _ia;

        private bool _decomposed = false;

        public Matrix()
        {
            _di = Array.Empty<double>();
            _au = Array.Empty<double>();
            _al = Array.Empty<double>();
            _ia = Array.Empty<int>();
        }

        public Matrix(uint size, double[] di, int[] ia, double[] au, double[] al)
        {
            Size = size;
            _di = di ?? throw new ArgumentNullException(nameof(di));
            _ia = ia ?? throw new ArgumentNullException(nameof(ia));
            _au = au ?? throw new ArgumentNullException(nameof(au));
            _al = al ?? throw new ArgumentNullException(nameof(al));
        }

        /// <summary>
        /// Warning: accessing the data in that way is not fast
        /// </summary>
        /// <param name="i"> row </param>
        /// <param name="j"> column </param>
        public double this[uint i, uint j] => GetElement(i, j);

        private uint Size { get; }

        public void LU_decomposition()
        {
            for (int i = 1; i < Size; i++)
            {
                double sumDi = 0;
                int j0 = i - (_ia[i + 1] - _ia[i]);
                for (int ii = _ia[i]; ii < _ia[i + 1]; ii++)
                {
                    int j = ii - _ia[i] + j0;
                    int jBeg = _ia[j];
                    int jEnd = _ia[j + 1];
                    if (jBeg < jEnd)
                    {
                        int j0j = j - (jEnd - jBeg);
                        int jjBeg = Math.Max(j0, j0j);
                        int jjEnd = Math.Max(j, i - 1);
                        double cL = 0;
                        for (int k = 0; k < jjEnd - jjBeg; k++)
                        {
                            int indAu = _ia[j] + jjBeg - j0j + k;
                            int indAl = _ia[i] + jjBeg - j0 + k;
                            cL += _au[indAu] * _al[indAl];
                        }

                        _al[ii - 1] -= cL;
                        double cU = 0;
                        for (int k = 0; k < jjEnd - jjBeg; k++)
                        {
                            int indAl = _ia[j] + jjBeg - j0j + k;
                            int indAu = _ia[i] + jjBeg - j0 + k;
                            cU += _au[indAu] * _al[indAl];
                        }

                        _au[ii - 1] = _al[ii - 1] - cU;
                    }

                    _au[ii - 1] /= _di[j + 1];
                    sumDi += _al[ii - 1] * _au[ii - 1];
                }

                _di[i] -= sumDi;
            }

            _decomposed = true;
        }

        public void Pprint()
        {
            Console.WriteLine("Matrix PPRINT:");

            if (_decomposed == false)
            {
                Console.WriteLine("undecomposed:");

                for (uint i = 0; i < Size; i++)
                {
                    for (uint j = 0; j < Size; j++)
                    {
                        Console.Write(this[i, j]);
                    }

                    Console.WriteLine("");
                }
            }
            else
            {
                Console.WriteLine("decomposed:");

                Console.WriteLine("L:");

                for (uint i = 0; i < Size; i++)
                {
                    for (uint j = 0; j < Size; j++)
                    {
                        Console.Write(L(i, j));
                    }

                    Console.WriteLine();
                }

                Console.WriteLine("U:");

                for (uint i = 0; i < Size; i++)
                {
                    for (uint j = 0; j < Size; j++)
                    {
                        Console.Write(U(i, j));
                    }

                    Console.WriteLine();
                }
            }
        }

        public double U(uint i, uint j)
        {
            if (i == j)
            {
                return 1.0;
            }

            return i > j ? this[i, j] : 0.0;
        }

        public double L(uint i, uint j)
        {
            return i <= j ? this[i, j] : 0.0;
        }

        private double GetElement(uint i, uint j)
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
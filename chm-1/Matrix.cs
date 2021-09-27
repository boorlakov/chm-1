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

        //Сюда не смотреть. Список людей, кому можно смотреть: Полина. 🤓
        public void LU_decomposition()
        {
            for (var i = 0; i < Size; i++)
            {
                var startPos = i - (_ia[i + 1] - _ia[i]); //first non-zero element of i-line
                double sumDi = 0;

                for (var k = _ia[i]; (k < _ia[i + 1]); startPos++, k++)
                {
                    double sumAl = 0;
                    double sumAu = 0;
                    var tL = _ia[i];
                    var tU = _ia[startPos];
                    var shift = k - _ia[i] - (_ia[startPos + 1] - _ia[startPos]);

                    if (shift < 0)
                    {
                        tU += Math.Abs(shift);
                    }
                    else
                    {
                        tL += shift;
                    }

                    while (tL < k)
                    {
                        sumAl += _au[tU] * _al[tL];
                        sumAu += _au[tL] * _al[tU];
                        tL++;
                        tU++;
                    }

                    // Less nesting == good!
                    if (k >= _al.Length) continue;
                    _al[k] = (_al[k] - sumAl) / _di[startPos];
                    _au[k] = (_au[k] - sumAu) / _di[startPos];
                    sumDi += _al[k] * _au[k];
                }

                _di[i] = Math.Sqrt(_di[i] - sumDi);
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
                        Console.Write(l(i, j));
                    }

                    Console.WriteLine();
                }

                Console.WriteLine("U:");

                for (uint i = 0; i < Size; i++)
                {
                    for (uint j = 0; j < Size; j++)
                    {
                        Console.Write(u(i, j));
                    }

                    Console.WriteLine();
                }
            }
        }

        public double u(uint i, uint j)
        {
            if (i == j)
            {
                return 1.0;
            }

            return i > j ? this[i, j] : 0.0;
        }

        public double l(uint i, uint j)
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
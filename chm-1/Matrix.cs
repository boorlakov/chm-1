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
        private double[] _al;

        /// au is for elements of upper triangular part of matrix
        private double[] _au;

        /// di is for diagonal elements
        private double[] _di;

        /// ia is for profile matrix. i.e. by manipulating ia elements we can use our matrix
        /// much more time and memory efficient
        private int[] _ia;

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
        
        //Сюда не смотреть. Список людей, кому можно смотреть: Полина.
        public void LU_decomposition()
        {
            for (int i = 0; i < Size; i++)
            {
                int j = i - (_ia[i + 1] - _ia[i]); //first non-zero element of i-line
                double sumdi = 0;
                for (int k = _ia[i]; (k < _ia[i + 1]); j++, k++)
                {
                    double sumal = 0;
                    double sumau = 0;
                    int tl = _ia[i];
                    int tu = _ia[j];
                    int a = k - _ia[i] - (_ia[j + 1] - _ia[j]); //shift
                    if (a < 0)
                    {
                        tu += Math.Abs(a);
                    }
                    else
                    {
                        tl += a;
                    }

                    while (tl < k)
                    {
                        sumal += _au[tu] * _al[tl];
                        sumau += _au[tl] * _al[tu];
                        tl++;
                        tu++;
                    }

                    if (k < _al.Length) //неппавильный фрагмент начинается
                    {
                        _al[k] = (_al[k] - sumal) / _di[j];
                        _au[k] = (_au[k] - sumau) / _di[j];
                        sumdi += _al[k] * _au[k];
                    } //неправильный фрагмент заканчивается
                }

                _di[i] = Math.Sqrt(_di[i] - sumdi);
            }
        }

        private uint Size { get; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder($"{nameof(Matrix)}:\ndi:\n");

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
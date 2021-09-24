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
        private uint[] _ia;

        public Matrix()
        {
            _di = Array.Empty<double>();
            _au = Array.Empty<double>();
            _al = Array.Empty<double>();
            _ia = Array.Empty<uint>();
        }

        public Matrix(uint size, double[] di, uint[] ia, double[] au, double[] al)
        {
            Size = size;
            _di = di ?? throw new ArgumentNullException(nameof(di));
            _ia = ia ?? throw new ArgumentNullException(nameof(ia));
            _au = au ?? throw new ArgumentNullException(nameof(au));
            _al = al ?? throw new ArgumentNullException(nameof(al));
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
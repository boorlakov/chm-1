using System;

namespace chm_1
{
    public class Matrix
    {
        private double[] _al;
        private double[] _au;
        private double[] _di;
        private uint[] _ia;
        private uint _size;

        public Matrix()
        {
            _di = Array.Empty<double>();
            _au = Array.Empty<double>();
            _al = Array.Empty<double>();
            _ia = Array.Empty<uint>();
        }

        public Matrix(double[] di, double[] au, double[] al, uint[] ia, uint size)
        {
            _di = di ?? throw new ArgumentNullException(nameof(di));
            _au = au ?? throw new ArgumentNullException(nameof(au));
            _al = al ?? throw new ArgumentNullException(nameof(al));
            _ia = ia ?? throw new ArgumentNullException(nameof(ia));
            _size = size;
        }
    }
}
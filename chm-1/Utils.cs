using System.IO;
using System.Linq;

namespace chm_1
{
    public static class Utils
    {
        /// <param name="file"> input file for initialise matrix data</param>
        /// <returns>Complete matrix</returns>
        public static Matrix LoadFrom(StreamReader file)
        {
            var ln = file.ReadLine();
            var size = uint.Parse(ln!);

            ln = file.ReadLine();
            var di = ln!.Split(' ').Select(double.Parse).ToArray();

            ln = file.ReadLine();
            var ia = ln!.Split(' ').Select(uint.Parse).ToArray();

            ln = file.ReadLine();
            var au = ln!.Split(' ').Select(double.Parse).ToArray();

            ln = file.ReadLine();
            var al = ln!.Split(' ').Select(double.Parse).ToArray();

            return new Matrix(size, di, ia, au, al);
        }
    }
}
namespace MVVMToolKit.Comparers
{
    using System;
    using System.Collections.Generic;

    public static class DoubleUtil
    {
        /// <summary>The default tolerance</summary>
        public const double DefaultTolerance = 1e-10;
        /// <summary>Equalses the specified source.</summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <param name="precision">The precision.</param>
        /// <returns>
        ///   <c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool Equals(double source, double target, double precision = DefaultTolerance)
        {
            var diff = Math.Abs(source - target);
            return
                diff <= precision ||
                diff <= Math.Max(Math.Abs(source), Math.Abs(target)) * precision;
        }

        /// <summary>Compares the specified x.</summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="tolerance">The tolerance.</param>
        /// <returns>System.Int32.</returns>
        public static int Compare(double x, double y, double tolerance = DefaultTolerance)
        {

            if (Equals(x, y, tolerance))
            {
                if (x < y)
                {
                    return 0;
                }
                else
                {
                    return Equals(x, y, tolerance) ? 0 : 1;
                }
            }
            else
            {
                if (Equals(x, y, tolerance))
                {
                    return x < y ? -1 : 0;
                }
                else
                {
                    return x < y ? -1 : 1;
                }
            }
        }
    }

    /// <summary>
    /// Class DoubleComparer.
    ///</summary>
    public class DoubleComparer : EqualityComparer<double>, IComparer<double>
    {
        private readonly double _tolerance;

        /// <inheritdoc />
        public DoubleComparer(double tolerance = DoubleUtil.DefaultTolerance) => _tolerance = tolerance;

        /// <inheritdoc/>

        public int Compare(double x, double y) => DoubleUtil.Compare(x, y, _tolerance);
        /// <inheritdoc/>

        public override bool Equals(double x, double y) => DoubleUtil.Equals(x, y, _tolerance);
        /// <inheritdoc/>

        public override int GetHashCode(double obj) => obj.GetHashCode();
    }
}

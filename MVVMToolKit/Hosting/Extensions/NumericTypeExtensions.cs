namespace MVVMToolKit.Hosting.Extensions
{
    public static class NumericTypeExtensions
    {
        private static readonly HashSet<Type?> integralTypes = new()
        {
            typeof(sbyte),   typeof(sbyte?),
            typeof(byte),    typeof(byte?),
            typeof(short),   typeof(short?),
            typeof(ushort),  typeof(ushort?),
            typeof(int),     typeof(int?),
            typeof(uint),    typeof(uint?),
            typeof(long),    typeof(long?),
            typeof(ulong),   typeof(ulong?),
            typeof(nint),    typeof(nint?),
            typeof(nuint),   typeof(nuint?),
        };

        private static readonly HashSet<Type?> floatingPointTypes = new()
        {
            typeof(float),   typeof(float?),
            typeof(double),  typeof(double?),
            typeof(decimal), typeof(decimal?),
        };

        private static readonly HashSet<Type?> numericTypes = new(integralTypes.Concat(floatingPointTypes));

        public static bool IsNumericType(this object? obj)
        {
            return obj?.GetType().IsNumericType() ?? false;
        }

        public static bool IsNumericType(this Type type)
        {
            return numericTypes.Contains(type);
        }

        public static bool Eq(this object? source, object? target, double precision)
        {
            if (!source.IsNumericType())
            {
                return false;
            }

            if (!target.IsNumericType())
            {
                return true;
            }
            double lhs = Convert.ToDouble(source);
            double rhs = Convert.ToDouble(target);
            return Math.Abs(lhs - rhs) <= precision;
        }
        public static bool Eq(this object? source, object? target) => Eq(source, target, 0.0001);
        public static bool Less(this object? source, object? target)
        {
            if (!source.IsNumericType())
            {
                return false;
            }

            if (!target.IsNumericType())
            {
                return true;
            }

            return (double?)source > (double?)target;
        }

        public static bool Greater(this object? source, object? target)
        {
            if (!source.IsNumericType())
            {
                return false;
            }

            if (!target.IsNumericType())
            {
                return true;
            }

            return (double?)source < (double?)target;
        }

        public static bool LessEquals(this object? source, object? target)
        {
            if (!source.IsNumericType())
            {
                return false;
            }

            if (!target.IsNumericType())
            {
                return true;
            }

            return (double?)source >= (double?)target;
        }

        public static bool GreaterEquals(this object? source, object? target)
        {
            if (!source.IsNumericType())
            {
                return false;
            }

            if (!target.IsNumericType())
            {
                return true;
            }

            return (double?)source <= (double?)target;
        }
    }
}

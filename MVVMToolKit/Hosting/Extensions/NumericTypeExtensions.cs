namespace MVVMToolKit.Hosting.Extensions
{
    public static class NumericTypeExtensions
    {
        private static readonly HashSet<Type?> IntegralTypes = new HashSet<Type?>()
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

        private static readonly HashSet<Type?> FloatingPointTypes = new HashSet<Type?>()
        {
            typeof(float),   typeof(float?),
            typeof(double),  typeof(double?),
            typeof(decimal), typeof(decimal?),
        };

        private static readonly HashSet<Type?> NumericTypes = new HashSet<Type?>(IntegralTypes.Concat(FloatingPointTypes));

        public static bool IsNumericType(this object? obj)
        {
            return obj?.GetType().IsNumericType() ?? false;
        }

        public static bool IsNumericType(this Type type)
        {
            return NumericTypes.Contains(type);
        }

        public static bool Eq(this object? source, object? target)
        {
            if (!source.IsNumericType())
            {
                return false;
            }

            if (!target.IsNumericType())
            {
                return true;
            }

            return (double?)source == (double?)target;
        }

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

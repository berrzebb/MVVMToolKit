
namespace EnChart.Enc.Common
{
    using System.Text;
    /// <summary>
    /// 타입에 대한 확장 메서드
    /// </summary>
    public static class TypeExtensions
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

        private static readonly Type[] primitiveTypes = {
            typeof(string),
            typeof(decimal),
            typeof(DateTime),
            typeof(DateTimeOffset),
            typeof(TimeSpan),
            typeof(Guid)
        };

        private static readonly HashSet<Type?> numericTypes = new(integralTypes.Concat(floatingPointTypes));

        public static IEnumerable<Type> NumericTypes()
        {
            foreach (var type in numericTypes)
            {
                yield return type;
            }
        }
        /// <summary>
        /// 입력된 타입이 Primitive타입인지 체크합니다.
        /// </summary>
        /// <param name="type">체크할 타입</param>
        /// <param name="includeArray">배열 타입 포함 여부</param>
        /// <returns>Primitive 타입 여부</returns>
        public static bool CheckPrimitive(this Type? type, bool includeArray = true)
        {
            if (type == null) return false;
            if (type.IsArray && includeArray)
            {
                var elementType = type.GetElementType();
                return elementType!.CheckPrimitive();
            }

            return
                type.IsValueType ||
                type.IsPrimitive || primitiveTypes.Contains(type) ||
                Convert.GetTypeCode(type) != TypeCode.Object;
        }
        /// <summary>
        /// 배열의 사이즈를 문자로 변환합니다.(ex : n * m) 
        /// </summary>
        /// <param name="array">배열</param>
        /// <returns>배열의 사이즈 문자열</returns>
        public static string ToStringArraySize(this Array array)
        {
            var builder = new StringBuilder();

            for (int i = 0; i < array.Rank; i++)
            {
                if (i == 0) builder.Append(array.GetLength(i));
                else builder.Append($" * {array.GetLength(i)}");
            }

            return builder.ToString();
        }
        /// <summary>
        /// 입력받은 객체가 Numeric Type 인지 체크합니다.
        /// </summary>
        /// <param name="obj">체크할 객체.</param>
        /// <returns>Numeric Type 여부</returns>
        public static bool IsNumericType(this object? obj)
        {
            return obj?.GetType().IsNumericType() ?? false;
        }

        /// <summary>
        /// 입력받은 타입이 Numeric Type 인지 체크합니다.
        /// </summary>
        /// <param name="type">체크할 타입.</param>
        /// <returns>Numeric Type 여부.</returns>
        public static bool IsNumericType(this Type type)
        {
            return numericTypes.Contains(type);
        }
        /// <summary>
        /// 입력받은 값과 타겟 값이 같은지 확인합니다.
        /// </summary>
        /// <param name="source">소스</param>
        /// <param name="target">대상</param>
        /// <param name="precision">정밀도</param>
        /// <returns>대상과 소스가 같은지(source equals target)</returns>
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
        /// <summary>
        /// 대상과 소스가 같은지 확인합니다.
        /// </summary>
        /// <param name="source">소스</param>
        /// <param name="target">대상</param>
        /// <returns>대상과 소스가 같은지(source equals target)</returns>
        public static bool Eq(this object? source, object? target) => source.Eq(target, 0.0001);
        /// <summary>
        /// 대상이 소스보다 작은지 확인합니다.
        /// </summary>
        /// <param name="source">소스</param>
        /// <param name="target">대상</param>
        /// <returns>대상이 소스보다 작은지(source less then target)</returns>
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
        /// <summary>
        /// 대상이 소스보다 큰지 확인합니다.
        /// </summary>
        /// <param name="source">소스</param>
        /// <param name="target">대상</param>
        /// <returns>대상이 소스보다 큰지(source greater then target) </returns>
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
        /// <summary>
        /// 대상이 소스보다 작거나 같은지 확인합니다.
        /// </summary>
        /// <param name="source">소스</param>
        /// <param name="target">대상</param>
        /// <returns>대상이 소스보다 작거나 같은지(source less then equals target)</returns>
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
        /// <summary>
        /// 대상이 소스보다 크거나 같은지 확인합니다.
        /// </summary>
        /// <param name="source">소스</param>
        /// <param name="target">대상</param>
        /// <returns>대상이 소스보다 크거나 같은지(source greater then equals target)</returns>
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

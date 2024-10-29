namespace MVVMToolKit.Ioc
{
    using System;
    /// <summary>
    /// 객체 정보
    /// </summary>
    public class TypeInfo
    {
        /// <summary>
        /// 객체의 명칭
        /// </summary>
        public string? Name { get; }
        /// <summary>
        /// 객체의 전체 명칭
        /// </summary>
        public string? FullName { get; }
        /// <summary>
        /// 객체의 실제 타입
        /// </summary>
        public Type ActualType { get; }

        /// <summary>
        /// 타입 정보 생성자
        /// </summary>
        /// <param name="name">객체의 명칭</param>
        /// <param name="fullName">객체의 전체 명칭</param>
        /// <param name="actualType">객체의 실제 타입</param>
        public TypeInfo(string? name, string? fullName, Type actualType)
        {
            Name = name;
            FullName = fullName;
            ActualType = actualType;
        }

        /// <inheritdoc />
        public override string ToString() => $"{FullName}({Name})";
    }

    /// <summary>
    /// 객체 공급자 클래스
    /// </summary>
    public static class TypeProvider
    {
        /// <summary>
        /// 객체 정보 셋
        /// </summary>
        private static readonly HashSet<TypeInfo> types = new();

        /// <summary>
        /// 객체의 이름을 통해 객체 타입을 얻어옵니다.
        /// </summary>
        /// <param name="name">객체의 명칭</param>
        /// <param name="type">객체의 타입</param>
        /// <returns>객체 존재 여부</returns>
        public static bool Resolve(string? name, out Type? type)
        {
            type = null;
            if (string.IsNullOrEmpty(name))
                return false;
            TypeInfo? typeInfo = types.FirstOrDefault(v => v.FullName == name || v.Name == name);
            type = typeInfo?.ActualType;
            return type != null;
        }

        internal static void RegisterType(Type type)
        {
            types.Add(new TypeInfo(type.Name, type.FullName, type));
        }

        internal static IEnumerable<TypeInfo> Types => types;
    }
}

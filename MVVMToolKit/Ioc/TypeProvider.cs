
namespace MVVMToolKit.Ioc
{
    public class TypeInfo
    {
        public string? Name { get; }
        public string? FullName { get; }
        public Type ActualType { get; }

        public TypeInfo(string? name, string? fullName, Type actualType)
        {
            Name = name;
            FullName = fullName;
            ActualType = actualType;
        }

        /// <inheritdoc />
        public override string ToString() => $"{FullName}({Name})";
    }

    public static class TypeProvider
    {
        private static readonly HashSet<TypeInfo> _types = new();
        public static bool Resolve(string? name, out Type? type)
        {
            type = null;
            if (string.IsNullOrEmpty(name)) return false;
            var typeInfo = _types.FirstOrDefault(v => v.FullName == name || v.Name == name);
            type = typeInfo?.ActualType;
            return type != null;
        }

        internal static void RegisterType(Type type)
        {
            _types.Add(new TypeInfo(type.Name, type.FullName, type));
        }
    }
}

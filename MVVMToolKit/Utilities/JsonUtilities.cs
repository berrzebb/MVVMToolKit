namespace MVVMToolKit.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Text;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using System.Threading;
    using System.Threading.Tasks;

    internal static class ArrayExtensions
    {
        internal static T[,] To2D<T>(this List<List<T>> source)
        {
            if (source.Count == 0 || source[0].Count == 0)
            {
                throw new ArgumentException("Source list cannot be empty.");
            }

            var firstDim = source.Count;
            var secondDim = source[0].Count;

            var result = new T[firstDim, secondDim];
            for (int i = 0; i < firstDim; i++)
            {
                for (int j = 0, count = source[i].Count; j < count; j++)
                {
                    result[i, j] = source[i][j];
                }
            }
            return result;
        }
        internal static T[,,] To3D<T>(this List<List<List<T>>> source)
        {
            if (source.Count == 0 || source[0].Count == 0 || source[0][0].Count == 0)
            {
                throw new ArgumentException("Source list cannot be empty.");
            }

            var firstDim = source.Count;
            var secondDim = source[0].Count;
            var thirdDim = source[0][0].Count;

            var result = new T[firstDim, secondDim, thirdDim];
            for (int i = 0; i < firstDim; i++)
            {
                for (int j = 0, secondCount = source[i].Count; j < secondCount; j++)
                {
                    for (int k = 0, thirdCount = source[i][j].Count; k < thirdCount; k++)
                    {
                        result[i, j, k] = source[i][j][k];
                    }
                }
            }

            return result;
        }
        internal static T[,,,] To4D<T>(this List<List<List<List<T>>>> source)
        {
            if (source.Count == 0 || source[0].Count == 0 || source[0][0].Count == 0 || source[0][0][0].Count == 0)
            {
                throw new ArgumentException("Source list cannot be empty.");
            }

            var firstDim = source.Count;
            var secondDim = source[0].Count;
            var thirdDim = source[0][0].Count;
            var quadDim = source[0][0][0].Count;

            var result = new T[firstDim, secondDim, thirdDim, quadDim];
            for (int i = 0; i < firstDim; i++)
            {
                for (int j = 0, secondCount = source[i].Count; j < secondCount; j++)
                {
                    for (int k = 0, thirdCount = source[i][j].Count; k < thirdCount; k++)
                    {
                        for (int m = 0, quadCount = source[i][j][k].Count; m < quadCount; m++)
                        {

                            result[i, j, k, m] = source[i][j][k][m];
                        }
                    }
                }
            }
            return result;
        }
    }
    public class ArrayConverter : JsonConverterFactory
    {
        private static Dictionary<(Type, Type), JsonConverter> _converters = new();
        public override bool CanConvert(Type typeToConvert) => typeToConvert.IsArray;

        public override JsonConverter CreateConverter(Type type, JsonSerializerOptions options)
        {
            var key = (type.GetElementType(), type);
            if (!_converters.TryGetValue(key, out var converter))
            {
                converter = (JsonConverter)Activator.CreateInstance(
                    typeof(ArrayConverterInner<,>).MakeGenericType(new[] { type.GetElementType(), type }),
                    BindingFlags.Instance | BindingFlags.Public,
                    binder: null,
                    args: new object[] { options },
                    culture: null);
                _converters.Add(key, converter);
            }
            return converter;
        }

        class ArrayConverterInner<TType, TArray> : JsonConverter<Array>
        {
            private int maxRank = typeof(TArray).GetArrayRank();
            public override bool CanConvert(Type typeToConvert)
            {
                return typeToConvert == typeof(TArray);
            }
            readonly JsonConverter<TType> _valueConverter;


            public ArrayConverterInner(JsonSerializerOptions options) =>
                this._valueConverter = (typeof(TType) == typeof(object) ? null : (JsonConverter<TType>)options.GetConverter(typeof(TType))); // Encountered a bug using the builtin ObjectConverter 
            private void WriteArray(Utf8JsonWriter writer, Array array, JsonSerializerOptions options, int rank, int maxRank, params int[] dimensions)
            {
                var firstIndex = array.GetLowerBound(rank - 1);
                var lastIndex = array.GetUpperBound(rank - 1);

                writer.WriteStartArray();
                for (var index = firstIndex; index <= lastIndex; index++)
                {
                    var currentDimensions = dimensions.Concat(new int[] { index }).ToArray();
                    if (rank == maxRank)
                    {
                        TType? value;
                        if (array.GetType().GetElementType() == typeof(TType))
                        {
                            value = (TType?)array.GetValue(currentDimensions);
                        }
                        else
                        {
                            value = (TType?)Convert.ChangeType(array.GetValue(currentDimensions), typeof(TType));
                        }
                        _valueConverter.Write(writer, value, options);
                    }
                    else
                    {
                        WriteArray(writer, array, options, rank + 1, maxRank, currentDimensions);
                    }
                }
                writer.WriteEndArray();
            }
            public override void Write(Utf8JsonWriter writer, Array data, JsonSerializerOptions options)
            {
                if (data == null)
                {
                    writer.WriteNull("");
                    return;
                }
                WriteArray(writer, data, options, 1, maxRank);
            }

            public override Array Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return maxRank switch
                {
                    1 => (Array)JsonSerializer.Deserialize<List<TType>>(ref reader, options).ToArray(),
                    2 => (Array)JsonSerializer.Deserialize<List<List<TType>>>(ref reader, options).To2D(),
                    3 => (Array)JsonSerializer.Deserialize<List<List<List<TType>>>>(ref reader, options).To3D(),
                    4 => (Array)JsonSerializer.Deserialize<List<List<List<List<TType>>>>>(ref reader, options).To4D(),
                    _ => null
                };

            }
        }
    }
    public static class Json
    {
        private static JsonSerializerOptions _defaultOptions = new JsonSerializerOptions()
        {
            WriteIndented = true,
            IncludeFields = true,
            Converters =
            {
                new ArrayConverter()
            }
        };
        public static async Task<string> SerializeObjectAsync(object obj, JsonSerializerOptions? options = null, CancellationToken token = default)
        {
            options ??= _defaultOptions;
            TaskCompletionSource<string> tcs = new TaskCompletionSource<string>();
            try
            {
                using (var stream = new MemoryStream())
                {
                    await JsonSerializer.SerializeAsync(stream, obj, options, cancellationToken: token);
                    if (token.IsCancellationRequested)
                    {
                        tcs.SetCanceled(token);
                        return string.Empty;
                    }
                    stream.Position = 0;
                    using var reader = new StreamReader(stream);
                    tcs.SetResult(await reader.ReadToEndAsync());
                }
            }
            catch (Exception ex)
            {
                tcs.SetException(ex);
            }
            return await tcs.Task;
        }
        public static async Task SaveObjectAsync(object obj, string path, JsonSerializerOptions? options = null, CancellationToken token = default)
        {
            var jsonString = await SerializeObjectAsync(obj, options, token).ConfigureAwait(false);
            await File.WriteAllTextAsync(path, jsonString, cancellationToken: token).ConfigureAwait(false);
        }
        public static async Task<T?> LoadObjectAsync<T>(string path, JsonSerializerOptions? options = null, CancellationToken token = default)
        {
            var jsonString = await File.ReadAllTextAsync(path, cancellationToken: token).ConfigureAwait(false);
            return await DeserializeObjectAsync<T>(jsonString, options, token).ConfigureAwait(false);
        }
        public static async Task<object?> LoadObjectAsync(string path, Type type, JsonSerializerOptions? options = null, CancellationToken token = default)
        {
            var jsonString = await File.ReadAllTextAsync(path, cancellationToken: token).ConfigureAwait(false);
            return await DeserializeObjectAsync(jsonString, type, options, token).ConfigureAwait(false);
        }
        public static async Task<T?> DeserializeObjectAsync<T>(string jsonData, JsonSerializerOptions? options = null, CancellationToken token = default)
        {
            options ??= _defaultOptions;

            TaskCompletionSource<T> tcs = new TaskCompletionSource<T>();
            try
            {
                using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonData)))
                {
                    T? item = await JsonSerializer.DeserializeAsync<T>(stream, options, cancellationToken: token);

                    if (token.IsCancellationRequested)
                    {
                        tcs.SetCanceled(token);
                        return default(T);
                    }
                    tcs.SetResult(item);
                }

            }
            catch (Exception ex)
            {
                tcs.SetException(ex);
            }
            return await tcs.Task;
        }
        public static async Task<object?> DeserializeObjectAsync(string jsonData, Type type, JsonSerializerOptions? options = null, CancellationToken token = default)
        {
            options ??= _defaultOptions;

            TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();
            try
            {
                using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonData)))
                {
                    object? item = await JsonSerializer.DeserializeAsync(stream, type, options, cancellationToken: token);

                    if (token.IsCancellationRequested)
                    {
                        tcs.SetCanceled(token);
                        return default;
                    }
                    tcs.SetResult(item);
                }

            }
            catch (Exception ex)
            {
                tcs.SetException(ex);
            }
            return await tcs.Task;
        }
    }
}

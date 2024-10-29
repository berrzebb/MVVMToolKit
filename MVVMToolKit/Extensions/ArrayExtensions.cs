namespace EnChart.Enc.Common
{
    using System.Collections.Immutable;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// 배열의 기능을 확장합니다.
    /// </summary>
    public static class ArrayExtensions
    {
        /// <summary>
        /// 미리 정의되어 있는 비어있는 byte 배열
        /// </summary>
        public static readonly ImmutableArray<byte> ZeroBytes = ImmutableArray.Create<byte>();
        /// <summary>
        /// 배열을 특정길이 만큼 추출합니다.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">대상 배열</param>
        /// <param name="length">길이</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static T[] Slice<T>(this T[] array, int length)
        {
            Contract.Requires(array != null);

            return length > array?.Length
                ? throw new ArgumentOutOfRangeException(nameof(length), $"length({length}) cannot be longer than Array.length({array.Length})")
                : Slice(array!, 0, length);
        }
        /// <summary>
        /// 배열을 특정 지점부터 특정 길이 만큼 추출합니다.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">대상 배열</param>
        /// <param name="index">시작 지점</param>
        /// <param name="length">길이</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static T[] Slice<T>(this T[] array, int index, int length)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            if (index + length > array.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(length), $"index: ({index}), length({length}) index + length cannot be longer than Array.length({array.Length})");
            }

            var result = new T[length];
            Array.Copy(array!, index, result, 0, length);
            return result;
        }
        /// <summary>
        /// 입력된 배열의 특정 인덱스부터 대상 배열에 할당합니다.
        /// </summary>
        /// <typeparam name="T">배열의 타입.</typeparam>
        /// <param name="array">대상 배열</param>
        /// <param name="index">시작 위치</param>
        /// <param name="src">입력된 배열</param>
        public static void SetRange<T>(this T[] array, int index, T[] src) => SetRange(array, index, src, 0, src.Length);
        /// <summary>
        /// 입력된 배열의 특정 인덱스부터 특정 길이 만큼 대상 배열에 할당합니다.
        /// </summary>
        /// <typeparam name="T">배열의 타입.</typeparam>
        /// <param name="array">대상 배열</param>
        /// <param name="index">시작 위치</param>
        /// <param name="src">입력된 배열</param>
        /// <param name="srcIndex">입력된 배열의 시작 위치</param>
        /// <param name="srcLength">할당할 길이</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static void SetRange<T>(this T[] array, int index, T[] src, int srcIndex, int srcLength)
        {
            Contract.Requires(array != null);
            Contract.Requires(src != null);
            if (index + srcLength > array?.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(srcLength), $"index: ({index}), srcLength({srcLength}) index + length cannot be longer than Array.length({array.Length})");
            }

            if (srcIndex + srcLength > src?.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(srcLength), $"index: ({srcIndex}), srcLength({srcLength}) index + length cannot be longer than src.length({src.Length})");
            }

            Array.Copy(src!, srcIndex, array!, index, srcLength);
        }
        /// <summary>
        /// 배열에 특정 값을 채웁니다.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">대상 배열</param>
        /// <param name="value">입력 값</param>
        public static void Fill<T>(this T[] array, T value)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = value;
            }
        }/// <summary>
         /// 배열의 특정 지점부터 원하는 갯수 만큼 특정 값을 채웁니다.
         /// </summary>
         /// <typeparam name="T"></typeparam>
         /// <param name="array">대상 배열</param>
         /// <param name="offset">시작 지점</param>
         /// <param name="count">갯수</param>
         /// <param name="value">입력 값</param>
        public static void Fill<T>(this T[] array, int offset, int count, T value)
        {
            Contract.Requires(count + offset <= array.Length);

            for (int i = offset; i < count + offset; i++)
            {
                array[i] = value;
            }
        }

        /// <summary>
        ///     Merge the byte arrays into one byte array.
        /// </summary>
        public static byte[] CombineBytes(this byte[][] arrays)
        {
            long newlength = 0;
            foreach (byte[] array in arrays)
            {
                newlength += array.Length;
            }

            var mergedArray = new byte[newlength];
            int offset = 0;
            foreach (byte[] array in arrays)
            {
                Buffer.BlockCopy(array, 0, mergedArray, offset, array.Length);
                offset += array.Length;
            }

            return mergedArray;
        }
        /// <summary>
        /// 입력된 최대치만큼의 인덱스 목록을 반환합니다.
        /// </summary>
        /// <param name="s1">최대치</param>
        /// <returns>인덱스 목록</returns>
        public static IEnumerable<int[]> Indices(int s1)
        {
            for (int i1 = 0; i1 < s1; i1++)
            {
                yield return new int[] { i1 };
            }
        }
        /// <summary>
        /// 입력된 최대치 만큼의 인덱스 목록을 반환합니다.
        /// </summary>
        /// <param name="s1">1차원 최대치</param>
        /// <param name="s2">2차원 최대치</param>
        /// <returns>인덱스 목록</returns>
        public static IEnumerable<int[]> Indices(int s1, int s2)
        {
            for (int i1 = 0; i1 < s1; i1++)
            {
                for (int i2 = 0; i2 < s2; i2++)
                {
                    yield return new int[] { i1, i2 };
                }
            }
        }
        /// <summary>
        /// 입력된 최대치 만큼의 인덱스 목록을 반환합니다.
        /// </summary>
        /// <param name="s1">1차원 최대치</param>
        /// <param name="s2">2차원 최대치</param>
        /// <param name="s3">3차원 최대치</param>
        /// <returns>인덱스 목록</returns>
        public static IEnumerable<int[]> Indices(int s1, int s2, int s3)
        {
            for (int i1 = 0; i1 < s1; i1++)
            {
                for (int i2 = 0; i2 < s2; i2++)
                {
                    for (int i3 = 0; i3 < s3; i3++)
                    {
                        yield return new int[] { i1, i2, i3 };
                    }
                }
            }
        }
        /// <summary>
        /// 입력된 최대치 만큼의 인덱스 목록을 반환합니다.
        /// </summary>
        /// <param name="s1">1차원 최대치</param>
        /// <param name="s2">2차원 최대치</param>
        /// <param name="s3">3차원 최대치</param>
        /// <param name="s4">4차원 최대치</param>
        /// <returns>인덱스 목록</returns>
        public static IEnumerable<int[]> Indices(int s1, int s2, int s3, int s4)
        {
            for (int i1 = 0; i1 < s1; i1++)
            {
                for (int i2 = 0; i2 < s2; i2++)
                {
                    for (int i3 = 0; i3 < s3; i3++)
                    {
                        for (int i4 = 0; i4 < s4; i4++)
                        {
                            yield return new int[] { i1, i2, i3, i4 };
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 배열의 최소 랭크를 얻습니다.
        /// </summary>
        /// <param name="array">대상 배열</param>
        /// <returns>최소 랭크</returns>
        public static int[] MinIndex(this Array array) => array.Rank switch
        {
            1 => new int[] { 0 },
            2 => new int[] { 0, 0 },
            3 => new int[] { 0, 0, 0 },
            4 => new int[] { 0, 0, 0, 0 },
            _ => Array.Empty<int>()
        };

        /// <summary>
        /// 배열의 최대 크기를 얻습니다.
        /// </summary>
        /// <param name="array">대상 배열</param>
        /// <returns>최대 크기 인덱스</returns>
        public static int[] MaxIndex(this Array array) => array.Rank switch
        {
            1 => new int[] { array.GetLength(0) },
            2 => new int[] { array.GetLength(0), array.GetLength(1) },
            3 => new int[] { array.GetLength(0), array.GetLength(1), array.GetLength(2) },
            4 => new int[] { array.GetLength(0), array.GetLength(1), array.GetLength(2), array.GetLength(3) },
            _ => Array.Empty<int>()
        };

        /// <summary>Nexts the index.</summary>
        /// <param name="array">The array.</param>
        /// <param name="index">The index.</param>
        /// <returns>System.Int32[].</returns>
        /// <exception cref="System.ArgumentNullException">array</exception>
        /// <exception cref="System.InvalidOperationException">index</exception>
        public static int[] NextIndex(this Array array, int[] index)
        {
            if (array == default || index == default)
            {
                throw new ArgumentNullException(nameof(array));
            }

            var rank = array.Rank;
            if (rank != index.Length)
            {
                throw new InvalidOperationException(nameof(index));
            }

            var ret = index.ToArray();

            for (int i = rank - 1; i >= 0; i--)
            {
                if (ret[i] + 1 < array.GetLength(i))
                {
                    ret[i]++;
                    break;
                }
                else
                {
                    ret[i] = i > 0 ? 0 : index[i] + 1;
                }
            }

            return ret;
        }

        /// <summary>Returns a <see cref="System.String" /> that represents this instance.</summary>
        /// <param name="index">The index.</param>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        /// <exception cref="System.ArgumentNullException">index</exception>
        public static string ToString(int[] index) => index == default || !index.Any() ? throw new ArgumentNullException(nameof(index)) : string.Join(",", index);

        /// <summary>Determines whether this instance contains the object.</summary>
        /// <param name="source">The source.</param>
        /// <param name="index">The index.</param>
        /// <returns>
        ///   <c>true</c> if [contains] [the specified index]; otherwise, <c>false</c>.</returns>
        public static bool Contains(this Array source, int[] index)
        {
            if (source == default || index == default || source.Rank != index.Length)
            {
                return false;
            }

            for (int i = 0; i < source.Rank; i++)
            {
                if (index[i] < 0 || index[i] >= source.GetLength(i))
                {
                    return false;
                }
            }

            return true;
        }
    }
}

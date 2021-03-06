﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;

namespace Stellaris
{
    public static class Helper
    {
        static int seed = 0;
        /// <summary>
        /// 读取IList中索引为index那一项的值，如果越界则返回第一项或最后一项的值
        /// </summary>
        public static T TryGetValue<T>(this IList<T> array, int index)
        {
            if (array == null) return default;
            if (array.Count == 0) return default;
            if (index < 0) return array[0];
            if (index > array.Count - 1) return array[array.Count - 1];
            return array[index];
        }
        /// <summary>
        /// 将每一项转换成字符串然后相加
        /// </summary>
        public static string ToStringAlt<T>(this IList<T> array)
        {
            string result = "";
            for (int i = 0; i < array.Count - 1; i++)
            {
                result += array[i].ToString() + ",";
            }
            result += array[array.Count - 1].ToString();
            return result;
        }
        /// <summary>
        /// 将每一项转换成字符串然后保存到新数组中
        /// </summary>
        public static string[] ToStringArray<T>(this IList<T> array)
        {
            string[] result = new string[array.Count];
            for (int i = 0; i < array.Count; i++)
            {
                result[i] = array[i].ToString();
            }
            return result;
        }
        public static byte[] ToByteArray(this Stream stream)
        {
            if (!stream.CanSeek) return new byte[] { 0 };
            byte[] bytes;
            stream.Position = 0;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                bytes = memoryStream.ToArray();
            }
            return bytes;
        }
        public static int[] ToCodePointArray(this IList<char> charArray)
        {
            int[] result = new int[charArray.Count];
            for(int i = 0; i < charArray.Count; i++)
            {
                result[i] = (int)charArray[i];
            }
            return result;
        }
        public static int[] ToCodePointArray(this string str)
        {
            int[] result = new int[str.Length];
            for (int i = 0; i < str.Length; i++)
            {
                result[i] = (int)str[i];
            }
            return result;
        }
        public static T[] InitializeArrayFromValue<T>(T value, int length)
        {
            T[] result = new T[length];
            for(int i = 0; i < length; i++)
            {
                result[i] = value;
            }
            return result;
        }
        public static T[] InitializeArrayFromValue<T>(Func<int, T> func, int length)
        {
            T[] result = new T[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = func(i);
            }
            return result;
        }
        public static T[] CutOut<T>(this T[] array, int start, int end)
        {
            T[] result = new T[end - start + 1];
            for (int i = start; i < end + 1; i++)
            {
                result[i - start] = array.TryGetValue(i);
            }
            return result;
        }
        public static void PlusAll(this short[] array, short num)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] += num;
            }
        }
        public static short[] FromAToB(short a, short b)
        {
            short[] result = new short[b - a];
            for (short i = a; i < b; i++)
            {
                result[i - a] = i;
            }
            return result;
        }
        public static Vector2 CenterTypeToVector2(CenterType centerType)
        {
            switch(centerType)
            {
                case CenterType.TopLeft:
                    return new Vector2(0f, 0f);
                case CenterType.TopCenter:
                    return new Vector2(0f, 0.5f);
                case CenterType.TopRight:
                    return new Vector2(0f, 1f);
                case CenterType.MiddleLeft:
                    return new Vector2(0.5f, 0f);
                case CenterType.MiddleCenter:
                    return new Vector2(0.5f, 0.5f);
                case CenterType.MiddleRight:
                    return new Vector2(0.5f, 1f);
                case CenterType.BottomLeft:
                    return new Vector2(1f, 0f);
                case CenterType.BottomCenter:
                    return new Vector2(1f, 0.5f);
                case CenterType.BottomRight:
                    return new Vector2(1f, 1f);
                default:
                    return default;
            }
        }
        /// <summary>
        /// 随机角度的向量
        /// </summary>
        public static Vector2 RandomAngleVec(float length, Vector2 center = default, float min = 0f, float max = 6.283f)
        {
            Random random = new Random(seed);
            float radian = min.LinearTo(max, (float)random.NextDouble(), 1f);
            float c = (float)Math.Cos(radian);
            float s = (float)Math.Sin(radian);
            seed = random.Next();
            return new Vector2(c * length, s * length) + center;
        }
        public static byte[] IntToByteArray(int i)
        {
            byte[] result = new byte[4];
            result[0] = (byte)((i >> 24) & 0xFF);
            result[1] = (byte)((i >> 16) & 0xFF);
            result[2] = (byte)((i >> 8) & 0xFF);
            result[3] = (byte)(i & 0xFF);
            return result;
        }
        public static int ByteArrayToInt(byte[] bytes)
        {
            int value = 0;
            for (int i = 0; i < 4; i++)
            {
                int shift = (3 - i) * 8;
                value += (bytes[i] & 0x000000FF) << shift;
            }
            return value;
        }
        /// <summary>
        /// 求两向量角度平均值
        /// </summary>
        public static float AngleBetween(Vector2 a, Vector2 b)
        {
            return (a + b).Angle();
        }
        public static Color ToColor(this Vector4 vec)
        {
            return new Color(vec.X, vec.Y, vec.Z, vec.W);
        }
        /// <summary>
        /// 线性插值
        /// </summary>
        public static float Linear(float a, float b, float progress, float max)
        {
            return progress / max * b + (max - progress) / max * a;
        }
        /// <summary>
        /// 线性插值
        /// </summary>
        public static float LinearTo(this float a, float b, float progress, float max)
        {
            return progress / max * b + (max - progress) / max * a;
        }
        /// <summary>
        /// 线性插值
        /// </summary>
        public static Vector2 Linear(Vector2 a, Vector2 b, float progress, float max)
        {
            return progress / max * b + (max - progress) / max * a;
        }
        /// <summary>
        /// 拉格朗日插值
        /// </summary>
        public static float Lagrange(float[] x, float[] y, float n)
        {
            float result = 0;
            for (int i = 0; i < x.Length; i++)
            {
                float item = 1;
                for (int j = 0; j < x.Length; j++)
                {
                    if (j == i) continue;
                    item *= (n - x[j]) / (x[i] - x[j]);
                }
                result += item * y[i];
            }
            return result;
        }
        /// <summary>
        /// 拉格朗日插值
        /// </summary>
        /// <param name="data">样本数据</param>
        /// <param name="precision">精度</param>
        /// <returns>插值过后的数据</returns>
        public static Vector2[] Lagrange(Vector2[] data, int precision)
        {
            if (precision < 2) return new Vector2[1] { Vector2.Zero };
            Vector2[] result = new Vector2[data.Length * (precision + 1) - precision];
            float[] x = new float[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                x[i] = data[i].X;
            }
            float[] y = new float[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                y[i] = data[i].Y;
            }
            for (int i = 0; i < data.Length - 1; i++)
            {
                for (int j = 0; j < precision + 1; j++)
                {
                    float n = Linear(x[i], x[i + 1], j, precision + 1);
                    result[i * (precision + 1) + j] = new Vector2(n, Lagrange(x, y, n));
                }
            }
            result[result.Length - 1] = data[data.Length - 1];
            return result;
        }
        public static Vector2[] CatmullRom(Vector2[] data, int precision)
        {
            Vector2[] result = new Vector2[data.Length * (precision + 1) - precision];
            float delta = 1f / precision;
            for (int i = 0; i < data.Length; i++)
            {
                Vector2 v1 = data.TryGetValue(i - 1);
                Vector2 v2 = data[i];
                Vector2 v3 = data.TryGetValue(i + 1);
                Vector2 v4 = data.TryGetValue(i + 2);
                for (int j = 0; j < precision; j++)
                {
                    result[i * precision + j] = CatmullRom(v1, v2, v3, v4, delta * j);
                }
            }
            return result;
        }
        public static Vector2 CatmullRom(Vector2 value1, Vector2 value2, Vector2 value3, Vector2 value4, float amount)
        {
            float num = amount * amount;
            float num2 = amount * num;
            Vector2 result = default;
            result.X = 0.5f * (2f * value2.X + (0f - value1.X + value3.X) * amount + (2f * value1.X - 5f * value2.X + 4f * value3.X - value4.X) * num + (0f - value1.X + 3f * value2.X - 3f * value3.X + value4.X) * num2);
            result.Y = 0.5f * (2f * value2.Y + (0f - value1.Y + value3.Y) * amount + (2f * value1.Y - 5f * value2.Y + 4f * value3.Y - value4.Y) * num + (0f - value1.Y + 3f * value2.Y - 3f * value3.Y + value4.Y) * num2);
            return result;
        }
    }
}

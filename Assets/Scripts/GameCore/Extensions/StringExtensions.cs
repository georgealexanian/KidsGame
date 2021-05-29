using System.Numerics;
using Newtonsoft.Json;
using UnityEngine.Rendering;

namespace GameCore.Extensions
{
    /// <summary>
    /// All String extensions.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Check Null or Empty string.
        /// </summary>
        /// <param name="string">Source string data.</param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string @string)
        {
            return string.IsNullOrEmpty(@string);
        }

        public static string Format(this string @string, params object[] @params)
        {
            return string.Format(@string, @params);
        }

        public static bool IsNumeric(this string str)
        {
            foreach (var character in str)
            {
                if (!char.IsDigit(character))
                {
                    return false;
                }
            }

            return true;
        }

        public static T FromJson<T>(this string json)
        {
            return JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });
        }

        public static string ToJson<T>(this T obj, TypeNameHandling handling = TypeNameHandling.Auto) where T : class
        {
            return JsonConvert.SerializeObject(obj, Formatting.Indented, new JsonSerializerSettings
            {
                TypeNameHandling = handling
            });
        }
        
        public static BigInteger ParseExpressionBigInteger(this string str, params object[] param)
        {
            str = string.Format(str, param);
            var parser = new ExpressionParser();
            var exp = parser.EvaluateExpression(str);
            return new BigInteger(exp.Value);
        }

        public static int ParseExpressionInt(this string str)
        {
            var parser = new ExpressionParser();
            var exp = parser.EvaluateExpression(str);
            return (int) exp.Value;
        }

        public static float ParseExpressionFloat(this string str)
        {
            var parser = new ExpressionParser();
            var exp = parser.EvaluateExpression(str);
            return exp.Value;
        }
    }
}
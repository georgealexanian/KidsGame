using System;
using System.Numerics;
using UnityEngine;

namespace GameCore.Extensions
{
    public static class BigIntegerExtensions
    {
        public static BigInteger MultiplyByDouble(this BigInteger left, double right)
        {
            var multiplied = (BigInteger) Math.Round(10000 * right);
            return left * multiplied / 10000;
        }

        public static BigInteger Sqrt(this BigInteger n)
        {
            if (n == 0) return 0;
            if (n > 0)
            {
                var bitLength = Convert.ToInt32(Math.Ceiling(BigInteger.Log(n, 2)));
                var root = BigInteger.One << (bitLength / 2);

                while (!IsSqrt(n, root))
                {
                    root += n / root;
                    root /= 2;
                }

                return root;
            }

            Debug.LogError("Couldn't get sqrt");
            return BigInteger.One;
        }

        public static string ToReadableString(this BigInteger n)
        {
            var str = n.ToString();

            for (var i = str.Length; i > 0; i -= 3)
            {
                str = str.Insert(i, " ");
            }

            return str;
        }

        private static bool IsSqrt(BigInteger n, BigInteger root)
        {
            var lowerBound = root * root;
            var upperBound = (root + 1) * (root + 1);

            return (n >= lowerBound && n < upperBound);
        }
    }
}
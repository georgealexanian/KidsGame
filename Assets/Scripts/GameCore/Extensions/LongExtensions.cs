using UnityEngine;

namespace GameCore.Extensions
{
    public static class LongExtensions
    {
        public static string BytesFormat(this long bytes)
        {
            var units = new string[] {"B", "KB", "MB", "GB", "TB", "PB", "EB"};
            var c = 0;
            for (c = 0; c < units.Length; c++)
            {
                var m = (long) 1 << ((c + 1) * 10);
                if (bytes < m)
                    break;
            }

            var n = bytes / (double) ((long) 1 << (c * 10));
            return $"{n:0.##} {units[c]}";
        }
    }
}
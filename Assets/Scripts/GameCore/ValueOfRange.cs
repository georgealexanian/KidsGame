using System;

namespace GameCore
{
    [Serializable]
    public class ValueOfRange : IntRange
    {
        public int value;
        public bool InsideOf(int val)
        {
            return val >= min && val < max;
        }
    }
}
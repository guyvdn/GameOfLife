using System;

namespace GameOfLife
{
    public static class RandomExtensions
    {
        public static bool Bool(this Random random)
        {
            return random.NextDouble() >= 0.5;
        }
    }
}
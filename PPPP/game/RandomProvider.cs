using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniRoguelike
{
    // Отвечает только за генерацию случайных чисел (SRP)
    public static class RandomProvider
    {
        private static readonly Random _rnd = new Random();


        public static int Next(int maxValue) => _rnd.Next(maxValue);
        public static int Next(int minValue, int maxValue) => _rnd.Next(minValue, maxValue);
    }
}

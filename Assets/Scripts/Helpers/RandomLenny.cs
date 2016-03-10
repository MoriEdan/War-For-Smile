using UnityEngine;
using System.Collections;

namespace Helpers
{
    public static class RandomLenny
    {
        private static readonly string[] Lennys =
        {
            @"¯\_¬ ͜ʖ¬_/¯",
            @"( ͡° ͜ʖ ͡°)",
            @"¯\_•v•_/¯",
            @"( ͡°益 ͡°)",
            @"( ͡°╭͜ʖ╮ ͡°)"
        };

        public static string GetRandomLenny()
        {
            var number = Mathf.FloorToInt(Random.Range(0.0f, Lennys.Length));
            return Lennys[number];
        }
    }
}
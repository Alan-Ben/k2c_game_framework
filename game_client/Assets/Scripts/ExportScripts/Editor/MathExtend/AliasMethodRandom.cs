using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MathExtend
{
    public class AliasMethodRandom
    {
        public struct Alias
        {
            public float odds;
            public int alias;

            public Alias(float _odds, int _alias)
            {
                odds = _odds;
                alias = _alias;
            }
        }
        
        private class WeightInfo
        {
            public float weight;
            public int index;

            public WeightInfo(int _index, float _weight)
            {
                index = _index;
                weight = _weight;
            }
        }
        
        public static List<Alias> prepareAliasRandom(List<float> weights)
        {
            int N = weights.Count;
            float sumWeight = 0;
            foreach (var weight in weights)
            {
                sumWeight += weight;
            }

            float avg = sumWeight / N;

            List<Alias> aliases = new List<Alias>();
            for (int i = 0; i < N; i++)
            {
                aliases.Add(new Alias(-1,0));
            }

            List<WeightInfo> large = new List<WeightInfo>();
            List<WeightInfo> small = new List<WeightInfo>();
            for (var i = 0; i < weights.Count; i++)
            {
                var weight = weights[i];
                if (weight >= avg)
                    large.Add(new WeightInfo(i, weight/avg));
                else
                    small.Add(new WeightInfo(i, weight/avg));
            }

            // if (small.Count == 0)
            // {
            //     for (int i = 0; i < N; i++)
            //     {
            //         aliases[i] = new Alias(weights[i], 0);
            //     }
            //     return aliases;
            // }

            while (small.Count > 0 && large.Count > 0)
            {
                int lastSmall = small.Count - 1;
                WeightInfo less = small[lastSmall];
                small.RemoveAt(lastSmall);
                int lastLarge = large.Count - 1;
                WeightInfo more = large[lastLarge];

                Alias alias = aliases[less.index];
                alias.odds = less.weight;
                alias.alias = more.index;
                aliases[less.index] = alias;

                more.weight -= (1 - less.weight);
                if (more.weight < 1)
                {
                    small.Add(more);
                    large.RemoveAt(lastLarge);
                }
            }
            //把最后的等于平均值的加到列表里
            if (large.Count > 0)
            {
                foreach (var more in large)
                {
                    aliases[more.index] = new Alias(more.weight, 0);
                }
            }
            return aliases;
        }

        public static int aliasRandom(List<Alias> aliases)
        {
            float r = Random.value * aliases.Count;
            int i = (int)Mathf.Floor(r);
            Alias alias = aliases[i];
            if (r - i > alias.odds)
                return alias.alias;
            else
                return i;
        }
    }
}
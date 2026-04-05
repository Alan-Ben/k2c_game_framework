using UnityEngine;

namespace MathExtend
{
    public class MathExtend
    {
        #region 生成高斯分布数
        // mean：均值，variance：方差
        // min和max用于去掉不需要的偏差值
        public static float NextGaussian(float mean, float variance, float min, float max) {
            float x;
            do {
                x = NextGaussian(mean, variance);
            } while (x < min || x > max);
            return x;
        }
    
        public static float NextGaussian(float mean, float standard_deviation) {
            return mean + NextGaussian() * standard_deviation;
        }
    
        public static float NextGaussian() {
            float v1, v2, s;
            do {
                v1 = 2.0f * Random.Range(0f, 1f) - 1.0f;
                v2 = 2.0f * Random.Range(0f, 1f) - 1.0f;
                s = v1 * v1 + v2 * v2;
            } while (s >= 1.0f || s == 0f);
            s = Mathf.Sqrt((-2.0f * Mathf.Log(s)) / s);
            return v1 * s;
        }
        #endregion
    }
}
using System;

namespace GOE
{
    /// <summary>
    /// 同步随机数生成器
    /// </summary>
    public class CSSyncRandom
    {
        private long _m_lSeed;          // 玩家专属随机种子
        private long _m_iOperationCount; // 当前操作计数

        /// <summary>
        /// 当前操作计数
        /// </summary>
        public long operationCount { get { return _m_iOperationCount; } }

        public CSSyncRandom(long _seed, long _operationCount)
        {
            // 根据玩家ID生成种子(可加入其他因素)
            _m_lSeed = _seed;
            _m_iOperationCount = _operationCount;
        }
        public CSSyncRandom(long _seed)
        {
            // 根据玩家ID生成种子(可加入其他因素)
            _m_lSeed = _seed;
            _m_iOperationCount = 0;
        }

        // 为特定操作获取种子
        private long GetSeedForOperation(long opCount)
        {
            // 使用乘法散列来增强种子与操作计数的混合效果
            long hash = _m_lSeed ^ opCount;
            hash = (hash ^ (hash >> 32)) * unchecked((long) 0x9E3779B97F4A7C15L);
            return hash;
        }

        // 固定算法生成0-1随机浮点数
        private float GenerateRandomFloat(long randSeed)
        {
            // XorShift实现，与服务端保持完全一致
            randSeed ^= (randSeed << 13);
            randSeed ^= (randSeed >> 17);
            randSeed ^= (randSeed << 5);
            // 转换为0-1浮点数
            return ((randSeed & 0x7FFFFFFF) * 1.0f) / 0x7FFFFFFF;
        }

        public int NextInt(int bound)
        {
            long randSeed = GetSeedForOperation(_m_iOperationCount);
            int result = (int)((GenerateRandomFloat(randSeed)) * bound);
            _m_iOperationCount++;
            return result;
        }
        public int RandomInt(long _operationCount, int bound)
        {
            long randSeed = GetSeedForOperation(_operationCount);
            int result = (int)((GenerateRandomFloat(randSeed)) * bound);
            return result;
        }

        public void addOperationCount()
        {
            _m_iOperationCount++;
        }
    }
} 
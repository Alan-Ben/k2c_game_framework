package NPCommon.Game.CS;

import NPCommon.Util.CommonFunc;

/**
 * 同步随机数生成器
 */
public class CSSyncRandom
{
    private long _m_seed;          // 玩家专属随机种子
    private long _m_operationCount; // 当前操作计数

    public CSSyncRandom(long _id)
    {
        // 根据玩家ID生成种子(可加入其他因素)
        this._m_seed = (_id << 32) | (CommonFunc.getNowTimeMS() & 0xFFFFFFFFL);
        this._m_operationCount = 0;
    }

    public long getSeed()
    {
        return _m_seed;
    }

    public long getOperationCount()
    {
        return _m_operationCount;
    }

    // 为特定操作获取种子
    private long getSeedForOperation(long opCount)
    {
        // 使用乘法散列来增强种子与操作计数的混合效果
        long hash = _m_seed ^ opCount;
        hash = (hash ^ (hash >> 32)) * 0x9E3779B97F4A7C15L;
        return hash;
    }

    // 固定算法生成0-1随机浮点数
    private float generateRandomFloat(long randSeed)
    {
        // XorShift实现，与客户端保持完全一致
        randSeed ^= (randSeed << 13);
        randSeed ^= (randSeed >> 17);
        randSeed ^= (randSeed << 5);
        // 转换为0-1浮点数
        return ((randSeed & 0x7FFFFFFF) * 1.0f) / 0x7FFFFFFF;
    }

    public int nextInt(int _bound)
    {
        long randSeed = getSeedForOperation(_m_operationCount);

        int result = (int) ((generateRandomFloat(randSeed)) * _bound);
        _m_operationCount++;
        return result;
    }

    public int nextInt(long _operationNum, int _bound)
    {
        long randSeed = getSeedForOperation(_operationNum);
        return (int) ((generateRandomFloat(randSeed)) * _bound);
    }
}
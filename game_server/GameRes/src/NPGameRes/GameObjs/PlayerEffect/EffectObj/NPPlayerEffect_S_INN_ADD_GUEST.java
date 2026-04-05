package NPGameRes.GameObjs.PlayerEffect.EffectObj;

import ALServerLog.ALServerLog;
import NPCommon.CommonObj.NPStringReader;
import NPEnum.ENPPlayerEffectType;
import NPGameRes.GameObjs.PlayerEffect._ANPPlayerEffectInfo;

/**
 * 旅店系统：直接添加指定数量的客人（无检查）
 * <p>
 * 效果格式：S_INN_ADD_GUEST:数量
 * <p>
 * 功能说明：
 * - 跳过客人可用性检查
 * - 跳过菜品可用性检查
 * - 跳过队列上限检查
 * - 直接添加指定数量的客人到接待队列
 * <p>
 * 使用场景：
 * - 新手引导（给予初始可结算客人）
 * - GM命令（测试和调试）
 * - 活动奖励（快速进入旅店玩法）
 * @author claude
 */
public class NPPlayerEffect_S_INN_ADD_GUEST extends _ANPPlayerEffectInfo
{
    // 添加的客人数量
    private int _m_count;

    public int getCount()
    {
        return _m_count;
    }

    @Override
    public ENPPlayerEffectType effectType()
    {
        return ENPPlayerEffectType.S_INN_ADD_GUEST;
    }

    public static NPPlayerEffect_S_INN_ADD_GUEST readStr(String _str)
    {
        return new NPPlayerEffect_S_INN_ADD_GUEST();
    }

    /**
     * 从字符串读取效果参数
     * @param _reader 字符串读取器，包含客人数量参数
     * @return 效果对象实例，解析失败返回null
     */
    public static NPPlayerEffect_S_INN_ADD_GUEST readStr(NPStringReader _reader)
    {
        if (_reader.isEmpty())
        {
            ALServerLog.Error("NPPlayerEffect_S_INN_ADD_GUEST.readStr - param validation failed: reader is empty, srcString=" + _reader.getSrcString());
            return null;
        }

        try
        {
            NPPlayerEffect_S_INN_ADD_GUEST obj = new NPPlayerEffect_S_INN_ADD_GUEST();
            obj._m_count = Integer.parseInt(_reader.getSrcString());

            // 参数合法性检查
            if (obj._m_count <= 0)
            {
                ALServerLog.Error("NPPlayerEffect_S_INN_ADD_GUEST.readStr - param validation failed: count must be positive, count=" + obj._m_count);
                return null;
            }

            return obj;
        } catch (Exception e)
        {
            ALServerLog.Error("NPPlayerEffect_S_INN_ADD_GUEST.readStr - parse failed: exception occurred, srcString=" + _reader.getSrcString());
            e.printStackTrace();
            return null;
        }
    }
}
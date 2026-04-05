package NPUSServer.NPUSUserMgr.EffectDealer.Dealer;

import NPEnum.ENPPlayerEffectType;
import NPGameRes.GameObjs.CommonObj.VarInfo.NPVarInfo;
import NPGameRes.GameObjs.PlayerEffect.EffectObj.NPPlayerEffect_S_INN_ADD_GUEST;
import NPGameRes.GameObjs.PlayerEffect._ANPPlayerEffectInfo;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.EffectDealer._ANPPlayerEffectDealer;
import NPUSServer.NPUSUserMgr.NPUSUserData;

/**
 * 旅店系统：直接添加客人效果处理器
 * <p>
 * 功能：跳过所有检查，直接添加指定数量的客人到旅店接待队列
 * <p>
 * 执行流程：
 * 1. 转换效果对象类型
 * 2. 调用旅店组件的直接添加方法
 * 3. 无需检查客人可用性、菜品可用性、队列上限
 * <p>
 * 使用场景：
 * - 新手引导初始化
 * - GM命令快速测试
 * - 活动奖励发放
 * @author claude
 */
public class NPPlayerEffectDealer_S_INN_ADD_GUEST extends _ANPPlayerEffectDealer
{
    @Override
    public ENPPlayerEffectType effectType()
    {
        return ENPPlayerEffectType.S_INN_ADD_GUEST;
    }

    @Override
    public void dealEffect(_ANPPlayerEffectInfo _effectInfo, NPUSUserData _userData, NPVarInfo _varVariableInfo, NPPlayerContext _context)
    {
        // 转换效果对象类型
        NPPlayerEffect_S_INN_ADD_GUEST effect = (NPPlayerEffect_S_INN_ADD_GUEST) _effectInfo;

        // 调用旅店组件的直接添加方法（无检查）
        _userData.getInnComponent().addGuestDirectly(effect.getCount(), _context);
    }
}
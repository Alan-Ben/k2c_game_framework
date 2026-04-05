package NPUSServer.NPUSUserMgr.ConditionDealer.Dealer;

import NPEnum.ENPPlayerConditionType;
import NPGameRes.GameObjs.CommonObj.VarInfo.NPVarInfo;
import NPGameRes.GameObjs.PlayerCondition.ConditionObj.NPPlayerCondition_S_IS_SYSTEM_QUEST_GROUP_DONE;
import NPGameRes.GameObjs.PlayerCondition._ANPBasicPlayerCondition;
import NPUSServer.NPUSUserMgr.ConditionDealer._ANPPlayerConditionDealer;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.SystemQuestComp.SystemQuestComponent;

/**
 * 系统任务组完成状态条件处理器
 * <p>
 * 主要功能：
 * 1. 检查指定系统任务组是否已完成
 * 2. 通过SystemQuestComponent查询任务组状态
 * <p>
 * 条件格式：S_IS_SYSTEM_QUEST_GROUP_DONE:group_id
 * <p>
 * 线程安全：通过玩家级锁保证数据一致性
 */
public class NPPlayerConditionDealer_S_IS_SYSTEM_QUEST_GROUP_DONE extends _ANPPlayerConditionDealer
{
    @Override
    public ENPPlayerConditionType conditionType()
    {
        return ENPPlayerConditionType.S_IS_SYSTEM_QUEST_GROUP_DONE;
    }

    /**
     * 检查系统任务组完成条件是否满足
     * <p>
     * 执行流程：
     * 1. 转换条件对象类型
     * 2. 获取系统任务组件
     * 3. 调用任务组完成检查方法
     * 4. 返回检查结果
     * @param _cond            条件对象
     * @param _userData        玩家数据
     * @param _varVariableInfo 变量信息（本条件中未使用）
     * @return 任务组是否完成
     * <p>
     * 线程安全：由SystemQuestComponent内部保证
     */
    @Override
    public boolean isEnable(_ANPBasicPlayerCondition _cond, NPUSUserData _userData, NPVarInfo _varVariableInfo)
    {
        NPPlayerCondition_S_IS_SYSTEM_QUEST_GROUP_DONE cond = (NPPlayerCondition_S_IS_SYSTEM_QUEST_GROUP_DONE) _cond;

        // 获取系统任务组件
        SystemQuestComponent systemQuestComp = _userData.getSystemQuestComponent();
        if (systemQuestComp == null)
            return false;

        // 检查任务组是否完成
        return systemQuestComp.isSystemQuestGroupDone(cond.getGroupId());
    }
}
package NPGameRes.GameObjs.PlayerCondition.ConditionObj;

import NPCommon.CommonObj.NPStringReader;
import NPCommon.Log.CommLog;
import NPEnum.ENPPlayerConditionType;
import NPGameRes.GameObjs.PlayerCondition._ANPBasicPlayerCondition;

/**
 * 是否完成系统任务组 S_IS_SYSTEM_QUEST_GROUP_DONE:group_id
 * <p>
 * 主要功能：
 * 1. 检查指定的系统任务组是否已完成
 * 2. 支持从字符串解析group_id参数
 * <p>
 * 使用格式：S_IS_SYSTEM_QUEST_GROUP_DONE:group_id
 * 其中group_id为要检查的系统任务组ID
 */
public class NPPlayerCondition_S_IS_SYSTEM_QUEST_GROUP_DONE extends _ANPBasicPlayerCondition
{
    // 系统任务组ID
    private long _m_groupId;

    public long getGroupId()
    {
        return _m_groupId;
    }

    @Override
    public ENPPlayerConditionType conditionType()
    {
        return ENPPlayerConditionType.S_IS_SYSTEM_QUEST_GROUP_DONE;
    }

    /**
     * 从字符串读取条件参数 - S_IS_SYSTEM_QUEST_GROUP_DONE:group_id
     * <p>
     * 执行流程：
     * 1. 读取任务组ID参数
     * 2. 验证参数有效性
     * 3. 创建条件对象
     * @param _reader 字符串读取器
     * @return 条件对象或null（解析失败时）
     */
    public static NPPlayerCondition_S_IS_SYSTEM_QUEST_GROUP_DONE readStr(NPStringReader _reader)
    {
        NPPlayerCondition_S_IS_SYSTEM_QUEST_GROUP_DONE cond = new NPPlayerCondition_S_IS_SYSTEM_QUEST_GROUP_DONE();

        // 读取任务组ID参数
        String rawGroupId = _reader.readItem(':');
        if (null == rawGroupId)
        {
            CommLog.error("NPPlayerCondition_S_IS_SYSTEM_QUEST_GROUP_DONE: Invalid format, missing group_id.");
            return null;
        }

        try
        {
            cond._m_groupId = Long.parseLong(rawGroupId);
        } catch (NumberFormatException e)
        {
            CommLog.error("NPPlayerCondition_S_IS_SYSTEM_QUEST_GROUP_DONE: Invalid group_id format: {}", rawGroupId);
            return null;
        }

        return cond;
    }
}
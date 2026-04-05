package NPGameRes.GameObjs.Battle;

import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import NPGameRes.GameObjs.Battle.Variable.WCGVariableGroupObj;
import WCGCommon.Enum.NPEnum.EWCGBothConditionType;


/**************
 * 状态值范围
 格式如下：枚举:状态值类型:最小值（高级计算公式）:最大值（高级计算公式）

 示例：队伍1的积分小于等于10点 - s_rng:spe_group_res@1@score:-1:10
 **/
public class WCGGroupConditionSRng extends _AWCGBasicBothCondition
{
    /*目标对象高级公式*/
    private WCGVariableGroupObj _m_eSeniorObj;

    private WCGIntRange _m_CountRange;

    public WCGVariableGroupObj seniorObj()
    {
        return _m_eSeniorObj;
    }

    public WCGIntRange Range()
    {
        return _m_CountRange;
    }

    public EWCGBothConditionType conditionType()
    {
        return EWCGBothConditionType.S_RNG;
    }

    /**************
     * ai_value_range:idx:min:max - ai对应索引值的范围(idx索引暂定0-99)，min,max如为-1表示无限制
     **/
    public static WCGGroupConditionSRng readCond(String _infoStr)
    {
        WCGGroupConditionSRng obj = new WCGGroupConditionSRng();
        String[] strs = CommonFunc.charSplit(_infoStr, ':');
        if (strs.length < 3)
        {
            CommLog.error("ai s_rng:spe_group_res@1@score:-1:10 配置错误，_infoStr：" + _infoStr);
            return null;
        }
        try
        {
            obj._m_eSeniorObj = WCGVariableGroupObj.readVariableGroup(strs[0], "s_rng");
            obj._m_CountRange = new WCGIntRange(strs[1], strs[2]);
            return obj;
        } catch (Exception e)
        {
            CommLog.error("ai s_rng:spe_group_res@1@score:-1:10 配置错误，_infoStr：" + _infoStr);
            return null;
        }
    }
}
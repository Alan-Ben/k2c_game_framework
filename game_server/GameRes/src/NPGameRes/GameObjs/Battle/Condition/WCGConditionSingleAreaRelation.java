package NPGameRes.GameObjs.Battle.Condition;

import NPCommon.Log.CommLog;
import NPCommon.Util.WCGResCommon;
import NPGameRes.GameObjs.Battle._AWCGBasicSingleCondition;
import WCGCommon.Enum.NPEnum.EWCGSingleConditionType;


/**************
 * AREA_RELATION - 判断当前所在区域的关系
 *
 * 根据对象当前所在区域判断这个区域的所有者跟这个对象是否符合对应的关系集合
 枚举:关系集合（用@分隔）
 示例 area_relation:self@ally
 **/
public class WCGConditionSingleAreaRelation extends _AWCGBasicSingleCondition
{
    private int _m_iRelationShip;

    public int RelationShip()
    {
        return _m_iRelationShip;
    }

    public EWCGSingleConditionType conditionType()
    {
        return EWCGSingleConditionType.AREA_RELATION;
    }

    /**************
     * 枚举:关系集合（用@分隔） 比如：area_relation:self@ally
     **/
    public static WCGConditionSingleAreaRelation readCond(String _infoStr)
    {
        WCGConditionSingleAreaRelation obj = new WCGConditionSingleAreaRelation();
        try
        {
            obj._m_iRelationShip = WCGResCommon.readRelationBitValue(_infoStr);
        } catch (Exception e)
        {
            CommLog.error("枚举:关系集合（用@分隔） 比如：area_relation:self@ally 配置错误，_infoStr:" + _infoStr, e);
            return null;
        }
        return obj;
    }
}
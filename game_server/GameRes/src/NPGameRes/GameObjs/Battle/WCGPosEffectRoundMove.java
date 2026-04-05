package NPGameRes.GameObjs.Battle;

import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.WCGResCommon;
import WCGCommon.Enum.NPEnum.EWCGPosEffectType;

public class WCGPosEffectRoundMove extends _AWCGPosEffectInfo
{
    //检索对象范围
    private int _m_iSearchTargetRange;
    //关系值
    private int _m_iRelationValue;
    //牵引重量单位
    private int _m_iMoveWeight;
    //牵引距离
    private int _m_iMoveDistance;
    //牵引速度
    private int _m_iMoveSpeed;
    //牵引时间
    private int _m_iMoveTimeMS;
    //可选：检索对象的条件
    private WCGBothConditionGroupObj _m_cgTargetConditionObj;

    //存放临时数据的，最大移动距离
    private int _m_fMaxMovDis;

    public int searchTargetRange()
    {
        return _m_iSearchTargetRange;
    }

    public int relationValue()
    {
        return _m_iRelationValue;
    }

    public int moveWeight()
    {
        return _m_iMoveWeight;
    }

    public int moveDistance()
    {
        return _m_iMoveDistance;
    }

    public int moveSpeed()
    {
        return _m_iMoveSpeed;
    }

    public int moveTimeMS()
    {
        return _m_iMoveTimeMS;
    }

    public WCGBothConditionGroupObj WCGBothConditionGroupObj()
    {
        return _m_cgTargetConditionObj;
    }

    public int maxMovDis()
    {
        return _m_fMaxMovDis;
    }

    protected WCGPosEffectRoundMove()
    {
        _m_iSearchTargetRange = 0;
        _m_iRelationValue = 0;
        _m_iMoveWeight = 0;
        _m_iMoveDistance = 0;
        _m_iMoveSpeed = 0;
        _m_iMoveTimeMS = 0;
        _m_cgTargetConditionObj = null;

        _m_fMaxMovDis = 0;
    }

    public EWCGPosEffectType effectType()
    {
        return EWCGPosEffectType.ROUND_MOVE;
    }

    public static WCGPosEffectRoundMove readVariable(String _str)
    {
        WCGPosEffectRoundMove effectObj = new WCGPosEffectRoundMove();

        //解析字符串
        String[] strs = CommonFunc.charSplit(_str, ':', 7);
        if (strs.length < 6)
        {
            CommLog.error("Error Format for WCGEffectKnockRound -  example: enum:searchRange:relation:weight:distance:speed:time Error Str: " + _str);
            return null;
        }
        effectObj._m_iSearchTargetRange = Integer.parseInt(strs[0].trim());
        effectObj._m_iRelationValue = WCGResCommon.readRelationBitValue(strs[1]);
        effectObj._m_iMoveWeight = Integer.parseInt(strs[2].trim());
        effectObj._m_iMoveDistance = Integer.parseInt(strs[3].trim());
        effectObj._m_iMoveSpeed = Integer.parseInt(strs[4].trim());
        effectObj._m_iMoveTimeMS = Integer.parseInt(strs[5].trim());

        effectObj._m_fMaxMovDis = effectObj._m_iMoveSpeed * effectObj._m_iMoveTimeMS / 1000;

        if (strs.length > 6)
            effectObj._m_cgTargetConditionObj = WCGBothConditionGroupObj.readConditionGroupList(strs[6]);

        return effectObj;
    }
}
package NPGameRes.GameObjs.Battle;

import NPCommon.Log.CommLog;
import WCGCommon.Enum.NPEnum.EWCGTeamConditionType;

public class WCGTeamConditionObj
{
    /**
     * 本对象的条件类型
     */
    public EWCGTeamConditionType conditionType = EWCGTeamConditionType.NONE;
    /**
     * 存储内容的字符串
     */
    public String infoStr = "";
    /**
     * 是否初始化过
     */
    private boolean _m_bInit = false;
    /**
     * xml中对应的具体数据
     */
    private _AWCGBasicTeamCondition _m_cConditionInfo = null;

    public _AWCGBasicTeamCondition condition()
    {
        if (!_m_bInit)
        {

            //还未初始化则进行初始化
            _m_cConditionInfo = _AWCGBasicTeamCondition.readCondition(conditionType, infoStr);
            if (_m_cConditionInfo == null)
                CommLog.error("条件数据读取失败:" + conditionType + ":" + infoStr);
            infoStr = "";
            _m_bInit = true;
        }

        return _m_cConditionInfo;

    }

    /**************
     * 将字符串转化为本对象
     **/
    public static WCGTeamConditionObj readCondition(String _str)
    {
        int splitPos = _str.indexOf(':');

        if (0 >= splitPos)
        {
            CommLog.warn("空的条件字符串3:" + _str);
            return null;
        }

        //读取第一个字段：条件类型
        String conditionType = _str.substring(0, splitPos);
        //读取类型枚举
        EWCGTeamConditionType condType = EWCGTeamConditionType.valueOf(conditionType.toUpperCase().trim());
        //读取剩余字符串
        String conditionInfoStr = _str.substring(splitPos + 1);

        if (condType == EWCGTeamConditionType.NONE)
        {
            CommLog.warn("错误的条件类型:" + _str);
            return null;
        }

        //创建对象
        WCGTeamConditionObj obj = new WCGTeamConditionObj();
        obj.conditionType = condType;
        obj.infoStr = conditionInfoStr;

        return obj;
    }
}
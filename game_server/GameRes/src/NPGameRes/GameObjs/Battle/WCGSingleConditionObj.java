package NPGameRes.GameObjs.Battle;

import NPCommon.Log.CommLog;
import WCGCommon.Enum.NPEnum.EWCGSingleConditionType;

public class WCGSingleConditionObj
{
    /**
     * 本对象的条件类型
     */
    public EWCGSingleConditionType conditionType = EWCGSingleConditionType.NONE;
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

    private _AWCGBasicSingleCondition _m_cConditionInfo = null;

    public _AWCGBasicSingleCondition condition()
    {
        if (!_m_bInit)
        {
            //还未初始化则进行初始化
            _m_cConditionInfo = _AWCGBasicSingleCondition.readCondition(conditionType, infoStr);
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
    //public static List<WCGSingleConditionObj> readConditionList(String _str)
    //{
    //    List<WCGSingleConditionObj> list = new ArrayList<WCGSingleConditionObj>();

    //    String[] condStrs =  WCGCommonFunc.charSplit(_str, '#');

    //    for (int i = 0; i < condStrs.Length; i++)
    //    {
    //        String infoStr = condStrs[i];
    //        //创建对象
    //        WCGSingleConditionObj obj = readCondition(infoStr);
    //        if(null != obj)
    //            list.Add(obj);
    //    }

    //    return list;
    //}

    /**************
     * 将字符串转化为本对象
     **/
    public static WCGSingleConditionObj readCondition(String _str)
    {
        int splitPos = _str.indexOf(':');

        if (0 >= splitPos)
        {
            CommLog.warn("空的条件字符串1:" + _str);
            return null;
        }

        //读取第一个字段：条件类型
        String conditionType = _str.substring(0, splitPos);
        //读取类型枚举
        EWCGSingleConditionType condType = EWCGSingleConditionType.valueOf(conditionType.toUpperCase().trim());
        //读取剩余字符串
        String conditionInfoStr = _str.substring(splitPos + 1);

        if (condType == EWCGSingleConditionType.NONE)
        {
            CommLog.warn("错误的条件类型:" + _str);
            return null;
        }

        //创建对象
        WCGSingleConditionObj obj = new WCGSingleConditionObj();
        obj.conditionType = condType;
        obj.infoStr = conditionInfoStr;

        return obj;
    }
}
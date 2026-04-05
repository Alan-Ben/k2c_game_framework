package NPGameRes.GameObjs.Battle;

import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import WCGCommon.Enum.NPEnum.EWCGTeamTriggerType;

import java.util.ArrayList;
import java.util.List;

public class WCGTeamTriggerObj
{
    /**
     * 本对象的条件类型
     */
    public EWCGTeamTriggerType triggerType;
    /**
     * 存储内容的字符串
     */
    public String infoStr;

    /**
     * 是否初始化过
     */
    private boolean _m_bInit = false;
    /**
     * xml中对应的具体数据
     */
    private _AWCGBasicTeamTrigger _m_cTriggerInfo = null;

    public _AWCGBasicTeamTrigger triggerInfo()
    {
        if (!_m_bInit)
        {

            //还未初始化则进行初始化
            _m_cTriggerInfo = _AWCGBasicTeamTrigger.readAITrigger(triggerType, infoStr);
            if (_m_cTriggerInfo == null)
                CommLog.error("EWCGTeamTriggerType事件数据读取失败3:" + triggerType + ":" + infoStr, new Exception());
            infoStr = "";
            _m_bInit = true;
        }

        return _m_cTriggerInfo;

    }

    /**************
     * 将字符串转化为本对象
     **/
    public static List<WCGTeamTriggerObj> readTriggerList(String _str)
    {
        List<WCGTeamTriggerObj> list = new ArrayList<WCGTeamTriggerObj>();

        String[] condStrs = CommonFunc.charSplit(_str, ';');

        for (int i = 0; i < condStrs.length; i++)
        {
            String infoStr = condStrs[i];
            int splitPos = infoStr.indexOf(":");

            if (0 >= splitPos)
                return null;

            //读取第一个字段：条件类型
            String typeStr = infoStr.substring(0, splitPos);
            //读取类型枚举
            EWCGTeamTriggerType type = EWCGTeamTriggerType.valueOf(typeStr.toUpperCase().trim());
            //读取剩余字符串
            String typeInfoStr = infoStr.substring(splitPos + 1);

            if (type == EWCGTeamTriggerType.NONE)
                return null;

            //读取条件对象
            WCGTeamTriggerObj obj = new WCGTeamTriggerObj();
            obj.triggerType = type;
            obj.infoStr = typeInfoStr;

            list.add(obj);
        }

        return list;
    }
}
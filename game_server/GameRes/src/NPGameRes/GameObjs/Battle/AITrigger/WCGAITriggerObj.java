package NPGameRes.GameObjs.Battle.AITrigger;

import NPCommon.Log.CommLog;
import NPCommon.RefData._IParseFromStringable;
import NPCommon.Util.CommonFunc;
import NPGameRes.GameObjs.Battle._AWCGBasicAITrigger;
import WCGCommon.Enum.NPEnum.EWCGAITriggerType;

import java.util.ArrayList;

public class WCGAITriggerObj implements _IParseFromStringable
{
    /**
     * 本对象的条件类型
     */
    public EWCGAITriggerType triggerType;
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
    private _AWCGBasicAITrigger _m_cTriggerInfo = null;

    public _AWCGBasicAITrigger triggerInfo()
    {

        if (!_m_bInit)
        {
            //还未初始化则进行初始化
            _m_cTriggerInfo = _AWCGBasicAITrigger.readAITrigger(triggerType, infoStr);
            infoStr = "";
            _m_bInit = true;
        }
        if (_m_cTriggerInfo == null)
            CommLog.error("EWCGAITriggerType事件数据读取失败:" + triggerType + ":" + infoStr, new Exception());

        return _m_cTriggerInfo;

    }

    /**************
     * 将字符串转化为本对象
     **/
    public static ArrayList<WCGAITriggerObj> readTriggerList(String _str)
    {

        ArrayList<WCGAITriggerObj> list = new ArrayList<WCGAITriggerObj>();

        if (null == _str || _str.isEmpty())
            return list;
        String[] condStrs = CommonFunc.charSplit(_str, ';');

        for (int i = 0; i < condStrs.length; i++)
        {
            String infoStr = condStrs[i];
            int splitPos = infoStr.indexOf(':');

            if (0 >= splitPos)
                return null;

            //读取第一个字段：条件类型
            String typeStr = infoStr.substring(0, splitPos);
            //读取类型枚举
            EWCGAITriggerType type = EWCGAITriggerType.valueOf(typeStr.toUpperCase().trim());
            //读取剩余字符串
            String typeInfoStr = infoStr.substring(splitPos + 1);

            if (type == EWCGAITriggerType.NONE)
                return null;

            //读取条件对象
            WCGAITriggerObj obj = new WCGAITriggerObj();
            obj.triggerType = type;
            obj.infoStr = typeInfoStr;

            list.add(obj);
        }

        return list;
    }

    @Override
    public boolean parseFromString(String sValue)
    {
        String infoStr = sValue;
        int splitPos = infoStr.indexOf(':');
        if (0 >= splitPos)
            return false;
        //读取第一个字段：条件类型
        String typeStr = infoStr.substring(0, splitPos);
        //读取类型枚举
        EWCGAITriggerType type = EWCGAITriggerType.valueOf(typeStr.toUpperCase().trim());
        //读取剩余字符串
        String typeInfoStr = infoStr.substring(splitPos + 1);
        if (type == EWCGAITriggerType.NONE)
            return false;
        //读取条件对象
        this.triggerType = type;
        this.infoStr = typeInfoStr;
        return true;
    }
}

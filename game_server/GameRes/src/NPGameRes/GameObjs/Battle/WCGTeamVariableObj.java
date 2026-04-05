package NPGameRes.GameObjs.Battle;

import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import WCGCommon.Enum.NPEnum.EWCGTeamVariableType;

import java.util.ArrayList;
import java.util.List;

public class WCGTeamVariableObj
{
    /**
     * 本对象的条件类型
     */
    public EWCGTeamVariableType variableType;
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
    private _AWCGBasicTeamVariableObj _m_cVariableInfo = null;

    public _AWCGBasicTeamVariableObj variableInfo()
    {
        if (!_m_bInit)
        {

            //还未初始化则进行初始化
            _m_cVariableInfo = _AWCGBasicTeamVariableObj.readVariable(variableType, infoStr);
            if (_m_cVariableInfo == null)
                CommLog.error("计算公式数据读取失败:" + variableType + ":" + infoStr);
            infoStr = "";
            _m_bInit = true;
        }

        return _m_cVariableInfo;
    }

    /**************
     * 将字符串转化为本对象
     **/
    public static WCGTeamVariableObj readVariable(String _str)
    {
        int splitPos = _str.indexOf('@');

        if (0 >= splitPos)
            return null;

        //读取第一个字段：条件类型
        String varTypeStr = _str.substring(0, splitPos);
        //读取类型枚举
        EWCGTeamVariableType varType = EWCGTeamVariableType.valueOf(varTypeStr.toUpperCase().trim());
        //读取剩余字符串
        String varInfoStr = _str.substring(splitPos + 1);

        if (varType == EWCGTeamVariableType.NONE)
            return null;

        //创建对象
        WCGTeamVariableObj obj = new WCGTeamVariableObj();
        obj.variableType = varType;
        obj.infoStr = varInfoStr;

        return obj;
    }

    /**************
     * 将字符串转化为本对象
     **/
    public static List<WCGTeamVariableObj> readVariablList(String _str)
    {
        List<WCGTeamVariableObj> groupObj = new ArrayList<WCGTeamVariableObj>();

        String[] variableStrs = CommonFunc.charSplit(_str, '*');

        for (int i = 0; i < variableStrs.length; i++)
        {
            String infoStr = variableStrs[i];
            int splitPos = infoStr.indexOf('@');

            if (0 >= splitPos)
                return null;

            //读取第一个字段：条件类型
            String varTypeStr = infoStr.substring(0, splitPos);
            //读取类型枚举
            EWCGTeamVariableType varType = EWCGTeamVariableType.valueOf(varTypeStr.toUpperCase().trim());
            //读取剩余字符串
            String varInfoStr = infoStr.substring(splitPos + 1);

            if (varType == EWCGTeamVariableType.NONE)
                return null;

            //创建对象
            WCGTeamVariableObj obj = new WCGTeamVariableObj();
            obj.variableType = varType;
            obj.infoStr = varInfoStr;

            groupObj.add(obj);
        }

        return groupObj;
    }
}

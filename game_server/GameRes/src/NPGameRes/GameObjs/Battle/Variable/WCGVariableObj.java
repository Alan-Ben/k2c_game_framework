package NPGameRes.GameObjs.Battle.Variable;

import ALServerLog.ALServerLog;
import NPCommon.Util.CommonFunc;
import NPGameRes.GameObjs.Battle._AWCGBasicVariableObj;
import WCGCommon.Enum.NPEnum.EWCGVariableType;

import java.util.ArrayList;

public class WCGVariableObj
{
    /**
     * 本对象的条件类型
     */
    public EWCGVariableType variableType;
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
    private _AWCGBasicVariableObj _m_cVariableInfo = null;

    public _AWCGBasicVariableObj variableInfo()
    {
        if (!_m_bInit)
        {

            // 还未初始化则进行初始化
            _m_cVariableInfo = _AWCGBasicVariableObj.readVariable(variableType, infoStr);
            infoStr = "";
        }
        if (_m_cVariableInfo == null)
            ALServerLog.Error("计算公式数据读取失败:" + variableType + ":" + infoStr);
        _m_bInit = true;
        return _m_cVariableInfo;
    }

    /**************
     * 将字符串转化为本对象
     **/
    public static WCGVariableObj readVariable(String _str)
    {
        int splitPos = _str.indexOf('@');

        if (0 >= splitPos)
            return null;

        //读取第一个字段：条件类型
        String varTypeStr = _str.substring(0, splitPos);
        //读取类型枚举
        EWCGVariableType varType = EWCGVariableType.valueOf(varTypeStr.toUpperCase().trim());
        //读取剩余字符串
        String varInfoStr = _str.substring(splitPos + 1);

        if (varType == EWCGVariableType.NONE)
            return null;

        //创建对象
        WCGVariableObj obj = new WCGVariableObj();
        obj.variableType = varType;
        obj.infoStr = varInfoStr;

        return obj;
    }

    /**************
     * 将字符串转化为本对象
     **/
    public static ArrayList<WCGVariableObj> readVariablList(String _str)
    {
        ArrayList<WCGVariableObj> groupObj = new ArrayList<WCGVariableObj>();

        String[] variableStrs = CommonFunc.charSplit(_str, '*');

        for (int i = 0; i < variableStrs.length; i++)
        {
            String infoStr = variableStrs[i];
            int splitPos = infoStr.indexOf('@');

            if (0 >= splitPos)
                return null;

            // 读取第一个字段：条件类型
            String varTypeStr = infoStr.substring(0, splitPos);
            // 读取类型枚举
            EWCGVariableType varType = EWCGVariableType.valueOf(varTypeStr.toUpperCase().trim());
            // 读取剩余字符串
            String varInfoStr = infoStr.substring(splitPos + 1);

            if (varType == EWCGVariableType.NONE)
                return null;

            // 创建对象
            WCGVariableObj obj = new WCGVariableObj();
            obj.variableType = varType;
            obj.infoStr = varInfoStr;

            groupObj.add(obj);
        }

        return groupObj;
    }
}
package NPGameRes.GameObjs.PlayerVariable;

import NPCommon.Log.CommLog;
import NPCommon.RefData._IParseFromStringable;
import NPEnum.ENPPlayerVariableType;
import NPGameRes.GameObjs.CommonObj.Variable._ATNPBasicVariableGroupObj;

public class NPPlayerVariableGroupObj extends _ATNPBasicVariableGroupObj<ENPPlayerVariableType, _ANPBasicPlayerVariableObj, NPPlayerVariableGroupObj>
        implements _IParseFromStringable
{

    private String _m_sRowString = "";

    public static NPPlayerVariableGroupObj readVariable(String _str)
    {
        NPPlayerVariableGroupObj obj = new NPPlayerVariableGroupObj();
        obj.readVariableGroup(_str,"");
        return obj;
    }

    /***************
     * 创建对应的条件集合对象
     * @param _str
     * @return
     */
    @Override
    protected NPPlayerVariableGroupObj _createGroupObj()
    {
        return new NPPlayerVariableGroupObj();
    }

    /***************
     * 从字符串读取出对应的条件
     * @param _str
     * @return
     */
    @Override
    protected _ANPBasicPlayerVariableObj _readVariableStr(String _str)
    {
        return _ANPBasicPlayerVariableObj.readVariable(_str);
    }

    @Override
    public boolean parseFromString(String sValue)
    {
        try
        {
            readVariableGroup(sValue, "");
            _m_sRowString = sValue;
            return true;
        } catch (Exception _e)
        {
            CommLog.error("can not parse NPPlayerVariableGroupObj for str:{}", sValue, _e);
            return false;
        }
    }

    @Override
    public String toString()
    {
        return _m_sRowString;
    }
}

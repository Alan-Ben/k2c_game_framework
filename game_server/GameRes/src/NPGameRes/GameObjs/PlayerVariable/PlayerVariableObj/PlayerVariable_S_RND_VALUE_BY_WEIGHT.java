package NPGameRes.GameObjs.PlayerVariable.PlayerVariableObj;

import ALServerLog.ALServerLog;
import NPCommon.CommonObj.NPStringReader;
import NPCommon.Game.WeightLongValueList;
import NPEnum.ENPPlayerVariableType;
import NPGameRes.GameObjs.PlayerVariable._ANPBasicPlayerVariableObj;

public class PlayerVariable_S_RND_VALUE_BY_WEIGHT extends _ANPBasicPlayerVariableObj
{
    private WeightLongValueList _m_weightValueList = new WeightLongValueList();

    public WeightLongValueList getWeightValueList()
    {
        return _m_weightValueList;
    }

    public long random()
    {
        return _m_weightValueList.random();
    }

    /******************
     * 获取条件类型
     */
    @Override
    public ENPPlayerVariableType variableType()
    {
        return ENPPlayerVariableType.S_RND_VALUE_BY_WEIGHT;
    }

    public static PlayerVariable_S_RND_VALUE_BY_WEIGHT readVariable(NPStringReader _reader)
    {
        PlayerVariable_S_RND_VALUE_BY_WEIGHT variableObj = new PlayerVariable_S_RND_VALUE_BY_WEIGHT();

        try
        {
            String weightList = _reader.readItem('@');
            if (null != weightList)
            {
                variableObj._m_weightValueList.parseFromString(weightList);
            }

            return variableObj;
        } catch (Exception e)
        {
            ALServerLog.Error("高级公式——通过权重随机数值 example: S_RND_VALUE_BY_WEIGHT@value1：weight1；value2：weight2... Error Str: " + _reader.getSrcString());
            return null;
        }
    }
}

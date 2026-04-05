package NPGameRes.GameObjs.PlayerVariable.PlayerVariableObj;

import ALServerLog.ALServerLog;
import NPCommon.CommonObj.NPStringReader;
import NPEnum.ENPPlayerVariableType;
import NPGameRes.GameObjs.Battle.WCGIntRange;
import NPGameRes.GameObjs.PlayerVariable._ANPBasicPlayerVariableObj;

public class PlayerVariable_CS_HERO_NUM extends _ANPBasicPlayerVariableObj
{
    private WCGIntRange _m_irIntRange;

    public WCGIntRange getLvlRng()
    {
        return _m_irIntRange;
    }

    /******************
     * 获取条件类型
     */
    @Override
    public ENPPlayerVariableType variableType()
    {
        return ENPPlayerVariableType.CS_HERO_NUM;
    }

    public static PlayerVariable_CS_HERO_NUM readVariable(NPStringReader _reader)
    {
        PlayerVariable_CS_HERO_NUM variableObj = new PlayerVariable_CS_HERO_NUM();

        try
        {
            int min = -1;
            int max = -1;

            String minS = _reader.readItem('@');
            String maxS = _reader.readItem('@');
            if (null != minS)
                min = Integer.parseInt(minS);
            if (null != maxS)
                max = Integer.parseInt(maxS);

            variableObj._m_irIntRange = new WCGIntRange(min, max);

            return variableObj;
        } catch (Exception e)
        {
            ALServerLog.Error("高级公式——大臣数量 example: CS_HERO_NUM@min_lv@max_lv Error Str: " + _reader.getSrcString());
            return null;
        }
    }
}

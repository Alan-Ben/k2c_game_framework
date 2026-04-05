package NPGameRes.GameObjs.PlayerVariable.PlayerVariableObj;

import ALServerLog.ALServerLog;
import NPCommon.CommonObj.NPStringReader;
import NPEnum.ENPItemType;
import NPEnum.ENPPlayerVariableType;
import NPGameRes.GameObjs.PlayerVariable._ANPBasicPlayerVariableObj;

public class NPPlayerVariable_CS_ITEM_COUNT extends _ANPBasicPlayerVariableObj
{
    private ENPItemType _m_eItemType;//类型
    private long _m_lSubId;

    public ENPItemType itemType()
    {
        return _m_eItemType;
    }

    public long subId()
    {
        return _m_lSubId;
    }

    protected NPPlayerVariable_CS_ITEM_COUNT()
    {
    }

    /******************
     * 获取条件类型
     */
    @Override
    public ENPPlayerVariableType variableType()
    {
        return ENPPlayerVariableType.CS_ITEM_COUNT;
    }


    public static NPPlayerVariable_CS_ITEM_COUNT readVariable(NPStringReader _reader)
    {
        NPPlayerVariable_CS_ITEM_COUNT variableObj = new NPPlayerVariable_CS_ITEM_COUNT();

        //解析字符串
        String itemTypeS = _reader.readItem('@');
        String subIdS = _reader.readItem('@');
        //逐个判断
        if (null == itemTypeS || null == subIdS)
        {
            ALServerLog.Error("高级公式——物品数量 - item_count example: enum@itemType@subId Error Str: " + _reader.getSrcString());
            return null;
        }

        try
        {
            //解析品质列表，品质配置至少1种
            variableObj._m_eItemType = ENPItemType.valueOf(itemTypeS.toUpperCase());
            variableObj._m_lSubId = Long.parseLong(subIdS);

            return variableObj;
        } catch (Exception e)
        {
            ALServerLog.Error("高级公式——物品数量 - item_count example: enum@itemType@subId Error Str: " + _reader.getSrcString());
            return null;
        }
    }
}

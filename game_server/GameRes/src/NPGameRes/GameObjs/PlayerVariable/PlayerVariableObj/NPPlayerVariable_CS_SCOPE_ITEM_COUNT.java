package NPGameRes.GameObjs.PlayerVariable.PlayerVariableObj;

import ALServerLog.ALServerLog;
import NPCommon.CommonObj.NPStringReader;
import NPEnum.ENPItemType;
import NPEnum.ENPPlayerVariableType;
import NPGameRes.GameObjs.PlayerVariable._ANPBasicPlayerVariableObj;

public class NPPlayerVariable_CS_SCOPE_ITEM_COUNT extends _ANPBasicPlayerVariableObj
{
    private ENPItemType _m_eItemType;//类型
    private long _m_lStartSubId;
    private long _m_lEndSubId;

    protected NPPlayerVariable_CS_SCOPE_ITEM_COUNT()
    {
    }

    public ENPItemType itemType()
    {
        return _m_eItemType;
    }

    public long startSubId()
    {
        return _m_lStartSubId;
    }

    public long endSubId()
    {
        return _m_lEndSubId;
    }

    /******************
     * 获取条件类型
     */
    @Override
    public ENPPlayerVariableType variableType()
    {
        return ENPPlayerVariableType.CS_SCOPE_ITEM_COUNT;
    }


    public static NPPlayerVariable_CS_SCOPE_ITEM_COUNT readVariable(NPStringReader _reader)
    {
        NPPlayerVariable_CS_SCOPE_ITEM_COUNT variableObj = new NPPlayerVariable_CS_SCOPE_ITEM_COUNT();

        //解析字符串
        String itemTypeS = _reader.readItem('@');
        String startSubIdS = _reader.readItem(':');
        String endSubIdS = _reader.readItem(':');
        //逐个判断
        if (null == itemTypeS || null == startSubIdS || null == endSubIdS)
        {
            ALServerLog.Error("高级公式——多物品数量 - many_item_count example: enum@itemType@startSubIdS:endSubIdS Error Str: " + _reader.getSrcString());
            return null;
        }

        try
        {
            //解析物品列表
            variableObj._m_eItemType = ENPItemType.valueOf(itemTypeS.toUpperCase());
            variableObj._m_lStartSubId = Long.parseLong(startSubIdS);
            variableObj._m_lEndSubId = Long.parseLong(endSubIdS);

            return variableObj;
        } catch (Exception e)
        {
            ALServerLog.Error("高级公式——多物品数量 - many_item_count example: enum@itemType@startSubIdS:endSubIdS Error Str: " + _reader.getSrcString());
            return null;
        }
    }
}

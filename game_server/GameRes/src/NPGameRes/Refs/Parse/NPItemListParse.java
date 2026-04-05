package NPGameRes.Refs.Parse;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData._IParseFromStringable;
import NPCommon.Util.StringFunc;

import java.util.ArrayList;
import java.util.List;

public class NPItemListParse implements _IParseFromStringable
{
    private ArrayList<NPCommonCostItem> _m_alItemObjList;

    public NPItemListParse()
    {
        _m_alItemObjList = new ArrayList<>();
    }

    public ArrayList<NPCommonCostItem> getItemTypeObjList()
    {
        return _m_alItemObjList;
    }

    @Override
    public boolean parseFromString(String sValue)
    {
        if (null == sValue || sValue.trim().isEmpty())
            return true;

        List<NPCommonCostItem> itemList = StringFunc.listFromString(sValue, () -> new NPCommonCostItem());
        _m_alItemObjList.clear();
        _m_alItemObjList.addAll(itemList);
        return true;
    }
}

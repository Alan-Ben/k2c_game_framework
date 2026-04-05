package NPGameRes.Refs.Parse;

import NPCommon.RefData._IParseFromStringable;
import NPCommon.Util.CommonFunc;
import NPEnum.ENPItemType;
import NPGameRes.Refs.Parse.Obj.NPItemProductObj;

import java.util.ArrayList;

public class NPItemProductListParse implements _IParseFromStringable
{
    private ArrayList<NPItemProductObj> _m_alItemTypeObjList;

    public NPItemProductListParse()
    {
        _m_alItemTypeObjList = new ArrayList<NPItemProductObj>();
    }

    @Override
    public boolean parseFromString(String sValue)
    {
        if (null == sValue || sValue.trim().isEmpty())
            return true;

        String[] subStrs = CommonFunc.charSplit(sValue, ';');

        for (int i = 0; i < subStrs.length; i++)
        {
            String[] tmpSubStrs = CommonFunc.charSplit(subStrs[i], ':');
            if (tmpSubStrs.length != 3)
                return false;

            ENPItemType itemType = ENPItemType.valueOf(tmpSubStrs[0].toUpperCase());
            long itemId = Long.parseLong(tmpSubStrs[1]);
            int basicCount = Integer.parseInt(tmpSubStrs[2]);

            NPItemProductObj obj = new NPItemProductObj(itemType, itemId, basicCount);
            _m_alItemTypeObjList.add(obj);
        }

        return true;
    }

    public ArrayList<NPItemProductObj> getItemTypeObjList()
    {
        return _m_alItemTypeObjList;
    }
}

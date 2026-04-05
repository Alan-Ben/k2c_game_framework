package NPGameRes.GameObjs.Arena;

import NPCommon.RefData._IParseFromStringable;
import NPCommon.Util.CommonFunc;

import java.util.ArrayList;

public class ArenaHeroObjList implements _IParseFromStringable
{
    private ArrayList<ArenaHeroObj> _m_alHeroList;

    public ArenaHeroObjList()
    {
        _m_alHeroList = new ArrayList<>();
    }

    public ArrayList<ArenaHeroObj> getArenaHeroList()
    {
        return _m_alHeroList;
    }

    @Override
    public boolean parseFromString(String sValue)
    {
        try
        {
            String[] strs = CommonFunc.charSplit(sValue, ';');

            for(int i = 0; i < strs.length; i++)
            {
                if(null == strs[i] || strs[i].isEmpty())
                    continue;

                ArenaHeroObj heroObj = new ArenaHeroObj();
                if(!heroObj.parseFromString(strs[i]))
                    return false;

                _m_alHeroList.add(heroObj);
            }

            return true;
        } catch (Exception e)
        {
            return false;
        }
    }
}

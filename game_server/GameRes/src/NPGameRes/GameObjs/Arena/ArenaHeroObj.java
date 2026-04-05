package NPGameRes.GameObjs.Arena;

import NPCommon.RefData._IParseFromStringable;
import NPCommon.Util.CommonFunc;

public class ArenaHeroObj implements _IParseFromStringable
{
    /** 大臣id */
    private long _m_lHeroId;
    /** 皮肤id */
    private long _m_lSkinId;
    /** 等级 */
    private int _m_iLevel;
    /** 实力 */
    private long _m_iPower;

    public long getHeroId() { return _m_lHeroId; }
    public long getSkinId() { return _m_lSkinId; }
    public int getLevel() { return _m_iLevel; }
    public long getPower() { return _m_iPower; }

    @Override
    public boolean parseFromString(String sValue)
    {
        try
        {
            String[] strs = CommonFunc.charSplit(sValue, ':');

            this._m_lHeroId = Long.parseLong(strs[0]);
            this._m_lSkinId = Long.parseLong(strs[1]);
            this._m_iLevel = Integer.parseInt(strs[2]);
            this._m_iPower = Long.parseLong(strs[3]);

            return true;
        } catch (Exception e)
        {
            return false;
        }
    }
}

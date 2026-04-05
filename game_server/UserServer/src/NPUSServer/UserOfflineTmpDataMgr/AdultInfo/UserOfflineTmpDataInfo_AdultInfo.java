package NPUSServer.UserOfflineTmpDataMgr.AdultInfo;

import NPUSServer.UserOfflineTmpDataMgr._IUserOfflineTmpDataInfo;
import USDB.Bo.PlayerAdultBO;

/**
 * 子嗣数据处理对象
 */
public class UserOfflineTmpDataInfo_AdultInfo implements _IUserOfflineTmpDataInfo
{
    private long _m_lAdultId;
    //是否卷王
    private boolean _m_bIsGiftde;
    //是否已婚
    private boolean _m_bIsMarried;

    public UserOfflineTmpDataInfo_AdultInfo(PlayerAdultBO _bo)
    {
        _m_lAdultId = _bo.getId();
        _m_bIsGiftde = _bo.getIsGiftde();
        _m_bIsMarried = _bo.getIsMarried();
    }

    public long getAdultId() {return _m_lAdultId;}
    public boolean getIsGifted() {return _m_bIsGiftde;}
    public boolean getIsMarried() {return _m_bIsMarried;}

    /**
     * 返回数据Id，用于在管理器中校验数据匹配
     * @return
     */
    public long getDataId()
    {
        return _m_lAdultId;
    }
}

package NPUSServer.NPUSUserMgr.UserComp.TitleComp;

import Common.NpPlayerInfoObj.PlayerInfo_Title;
import NPCommon.Util.CommonFunc;
import NPGameRes.Refs.Title.RefPlayerTitle;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.ExpiredItemComp._AExpiredItemInfo;
import NPUSServer.NPUserServer;
import USDB.Bo.PlayerTitleBO;

/*****************
 * 玩家称号信息存储对象
 * @author mj
 *
 */
public class PlayerTitleInfo extends _AExpiredItemInfo<RefPlayerTitle>
{
	//玩家数据对象
	private NPUSUserData _m_usUserData;
	
	//数据Bo
	private PlayerTitleBO _m_boTitle;

    public PlayerTitleInfo(NPUSUserData _userData, RefPlayerTitle _ref, PlayerTitleBO _bo)
    {
        super(_ref, _bo.getId(), _bo.getExpireTimeS(), _bo.getViewed(), false);
        
        _m_usUserData = _userData;

        _m_boTitle = _bo;
    }
    
    //玩家对象
    public NPUSUserData getUserData() {return _m_usUserData;}
    public NPUserServer getUSServer() {return _m_usUserData.getUSServer();}
    
    //称号数据
    public PlayerTitleBO getBo() {return _m_boTitle;}
    public int getLastGainTs() {return _m_boTitle.getLastGainTs();}
    public int getGainCount() {return _m_boTitle.getGainCount();}
    
    /**
     * 增加获取次数
     */
    @Override
    protected void _onGainAgain()
    {
    	_m_boTitle.setLastGainTs(getUSServer().getBM(), CommonFunc.getNowTimeSec());
    	_m_boTitle.setGainCount(getUSServer().getBM(), getGainCount() + 1);
    	_m_boTitle.saveAll(getUSServer().getBM());
    }

    @Override
    protected void saveExpiredTimeSecChg()
    {
    	_m_boTitle.setExpireTimeS(getUSServer().getBM(), getExpireTimeSec());
    	_m_boTitle.saveAll(getUSServer().getBM());
    }

    @Override
    protected void saveViewedTagChg()
    {
    	_m_boTitle.setViewed(getUSServer().getBM(), getViewed());
    	_m_boTitle.saveAll(getUSServer().getBM());
    }

    @Override
    protected void saveCheckNeedSendMailTagChg()
    {
    }

    @Override
    public void del()
    {
    	_m_boTitle.del(getUSServer().getBM());
    }

    /**************
     * 构造协议对象
     * @return
     */
    public PlayerInfo_Title makeProto()
    {
        PlayerInfo_Title obj = new PlayerInfo_Title();
        obj.setId(getRefId());
        obj.setExpireTimeTagS(getExpireTimeSec());
        obj.setViewed(getViewed());
        obj.setLastGainTs(getLastGainTs());
        obj.setGainCount(getGainCount());
        
        return obj;
    }
}

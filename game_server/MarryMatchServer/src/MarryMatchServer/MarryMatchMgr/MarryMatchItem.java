package MarryMatchServer.MarryMatchMgr;

import Common.ServerObj.ServerObj_AdultMarryGroupApplyInfo;
import NPCommon.Util.CommonFunc;

public class MarryMatchItem 
{
	//申请子嗣ID
	private long _m_lApplyAdultId;
	//申请玩家CID
	private long _m_lApplyCid;
	//匹配ID
	private int _m_iMatchId;
	//子嗣收益
	private long _m_lBonus;

    //允许对方子嗣最小收益
    private long _m_lMinBonus;

	//所在的US服务器ID
	private int _m_iUsId;
	
	public MarryMatchItem(ServerObj_AdultMarryGroupApplyInfo _apply)
	{
		_m_lApplyAdultId = _apply.getApplyAdultId();
		_m_lApplyCid = _apply.getApplyCid();
		_m_iMatchId = _apply.getMatchId();
		_m_lBonus = _apply.getBonus();

        _m_lMinBonus = _apply.getMinBonus();
		
		_m_iUsId = CommonFunc.parseServerTypeIdFromCid(_m_lApplyCid);
	}
	
	public long getApplyAdultId() {return _m_lApplyAdultId;}
	public long getApplyCid() {return _m_lApplyCid;}
	public int getMatchId() {return _m_iMatchId;}
	public long getBonus()
	{
		return _m_lBonus;
	}
    public long getMinBonus() {return _m_lMinBonus;}

	public int getUsId() {return _m_iUsId;}
}

package NPUSServer.Guild.GuildBox;

import Common.GuildObj.Guild_BoxInfo;
import NPCommon.DB.BM.BM;
import NPGameRes.Refs.Guild.RefGuildBox;
import NPUSServer.Guild.GuildInfo;
import NPUSServer.NPUserServer;
import USDB.Bo.GuildBoxBO;

public class GuildBoxInfo 
{
	//公会数据对象
    private GuildInfo _m_guildInfo;
    //联盟宝箱数据
	private GuildBoxBO _m_bo;
	//配置数据
	private RefGuildBox _m_ref;
    
    public GuildBoxInfo(GuildInfo _guild, GuildBoxBO _bo)
    {
    	_m_guildInfo = _guild;
    	_m_bo = _bo;
    	
    	_m_ref = RefGuildBox.getMgr().get(_m_bo.getBoxId());
    }
    public GuildBoxInfo(GuildInfo _guild, GuildBoxBO _bo, RefGuildBox _ref)
    {
    	_m_guildInfo = _guild;
    	_m_bo = _bo;
    	_m_ref = _ref;
    }
    
    public GuildInfo getGuild() {return _m_guildInfo;}
    public NPUserServer getUSServer() {return getGuild().getGuildMgr().getServer();}
    public BM getBM() {return getUSServer().getBM();}
    
    public GuildBoxBO getBo() {return _m_bo;}
    public long getId() {return getBo().getId();}
    public long getBoxId() {return getBo().getBoxId();}
    public long getShareCid() {return getBo().getShareCid();}
    public long getEndTime() {return getBo().getEndTime();}
    public boolean isAnonymous() {return getBo().getIsAnonymous();}
    
    public RefGuildBox getRef() {return _m_ref;}
    
    protected	void _del()
    {
    	getBo().del(getBM());
    }
    
    public Guild_BoxInfo toProto()
    {
    	Guild_BoxInfo proto = new Guild_BoxInfo();
    	proto.setId(getId());
    	proto.setBoxId(getBoxId());
    	proto.setShareCid(getShareCid());
    	proto.setEndMs(getEndTime());
    	
    	return proto;
    }
}

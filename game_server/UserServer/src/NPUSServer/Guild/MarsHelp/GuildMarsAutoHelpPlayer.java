package NPUSServer.Guild.MarsHelp;

/**
 * 可以自动互助其他盟友的玩家列表数据
 * @author mj
 *
 */
public class GuildMarsAutoHelpPlayer 
{
    //自动互助玩家CID
    private long _m_lCid;
    //自动互助截至时间戳（毫秒）
    private long _m_lEndMs;
    
    public GuildMarsAutoHelpPlayer(long _cid, long _endMs)
    {
    	_m_lCid = _cid;
    	_m_lEndMs = _endMs;
    }
    
    public long getCid() {return _m_lCid;}
    public long getEndMs() {return _m_lEndMs;}
    
    public void setEndMs(long _endMs)
    {
    	_m_lEndMs = _endMs;
    }
}

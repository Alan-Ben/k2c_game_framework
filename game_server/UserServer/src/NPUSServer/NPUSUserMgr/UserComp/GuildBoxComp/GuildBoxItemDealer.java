package NPUSServer.NPUSUserMgr.UserComp.GuildBoxComp;

import NPCommon.DB.BM.BM;
import NPEnum.ENPItemType;
import NPGameRes.Refs.Guild.RefGuildBox;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.ItemDealer._IUserItemBasicDealer;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;

/**
 * 单独联盟宝箱获取处理对象，该对象不挂载在其他对象下
 * @author mj
 *
 */
public class GuildBoxItemDealer implements _IUserItemBasicDealer
{
	//玩家数据对象
	private NPUSUserData _m_udUserData;

	public GuildBoxItemDealer(NPUSUserData _userData)
	{
		_m_udUserData = _userData;
	}
	
	public NPUSUserData getUserData() {return _m_udUserData;}
    public NPUserServer getUSServer() {return getUserData().getUSServer();}
    public long getCid() {return getUserData().getCid();}
    public BM getBM() {return getUserData().getUSServer().getBM();}
	
	@Override
	public ENPItemType getItemType() {return ENPItemType.GUILD_BOX;}

	@Override
	public long getItemCount(long _itemId) {return 0;}

	@Override
	public boolean hasItem(long _itemId, long _count) {return false;}

	@Override
	public void initGainItem(long _itemId, long _count, NPPlayerContext _context) {}

	/**
	 * 增加联盟宝箱
	 * 规则：count - 0 匿名
	 */
	@Override
	public void gainItem(long _itemId, long _count, boolean _isNotMerge, NPPlayerContext _context) 
	{
		//需要检查是否当前参加公会，没有公会不能发送公会宝箱
		if(getUserData().getGuildComponent().getGuildId() <= 0)
			return;
		
		RefGuildBox ref = RefGuildBox.getMgr().get(_itemId);
		if(null == ref)
		{
			USLog.error(getUSServer(), "player:{} box:{} guild box gain fail, not find ref.", getUserData().getCid(), _itemId);
			return;
		}

		getUserData().getGuildComponent().addGuildBox(ref.type, _itemId, _count, _context);
	}

	@Override
	public boolean spendItem(long _itemId, long _count, NPPlayerContext _context) 
	{
		return false;
	}	
}

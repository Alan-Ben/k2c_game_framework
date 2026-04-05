package NPUSServer.NPUSUserMgr.UserComp.PlayerComp;

import NPEnum.ENPItemType;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.ItemDealer._IUserItemBasicDealer;
import NPUSServer.NPUSUserMgr.NPUSUserData;

public class PlayerSysInfoDealer implements _IUserItemBasicDealer
{
    private final NPUSUserData _m_userData;

    public PlayerSysInfoDealer(NPUSUserData _userData)
    {
        _m_userData = _userData;
    }

    public NPUSUserData getUserData() {return _m_userData;}
	
	@Override
	public ENPItemType getItemType() 
	{
		return ENPItemType.SYS_INFO;
	}

	@Override
	public long getItemCount(long _itemId) 
	{
		return 0;
	}

	@Override
	public boolean hasItem(long _itemId, long _count) 
	{
		return false;
	}

	@Override
	public void initGainItem(long _itemId, long _count, NPPlayerContext _context) 
	{
	}

	@Override
	public void gainItem(long _itemId, long _count, boolean _isNotMerge, NPPlayerContext _context) 
	{
        //放入数据
        _context.collectItem(getItemType(), _itemId, _count, _isNotMerge);
	}

	@Override
	public boolean spendItem(long _itemId, long _count, NPPlayerContext _context) 
	{
		return false;
	}
}

package NPUSServer.NPUSUserMgr.UserComp.Common;

import NPEnum.ENPItemType;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.ItemDealer._IUserItemBasicDealer;

/****************
 * 针对NONE类型的item处理
 */
public class UserItemBasicDealer_NONE implements _IUserItemBasicDealer
{
	public static UserItemBasicDealer_NONE _g_instance = new UserItemBasicDealer_NONE();
	public static UserItemBasicDealer_NONE getInstance() {return _g_instance;}

	@Override
	public ENPItemType getItemType() 
	{
		return ENPItemType.NONE;
	}

	@Override
	public long getItemCount(long _itemId) 
	{
		return 0;
	}

	@Override
	public boolean hasItem(long _itemId, long _count) 
	{
		return true;
	}

	@Override
	public void initGainItem(long _itemId, long _count, NPPlayerContext _context) 
	{
	}

	@Override
	public void gainItem(long _itemId, long _count, boolean _isNotMerge, NPPlayerContext _context) 
	{
	}

	@Override
	public boolean spendItem(long _itemId, long _count, NPPlayerContext _context) 
	{
		return true;
	}
}

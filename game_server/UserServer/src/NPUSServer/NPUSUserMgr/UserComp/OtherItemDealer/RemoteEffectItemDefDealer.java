package NPUSServer.NPUSUserMgr.UserComp.OtherItemDealer;

import NPEnum.ENPItemType;
import NPGameRes.Refs.RefRemoteEffect;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.ConditionDealer.NPPlayerConditionDealerMgr;
import NPUSServer.NPUSUserMgr.EffectDealer.NPPlayerEffectDealer;
import NPUSServer.NPUSUserMgr.ItemDealer._IUserItemBasicDealer;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.USLog;

/**
 * @description: 分享道具处理器
 * @author: mark
 * @date: 2022-10-14 09:41:29
 */
public class RemoteEffectItemDefDealer implements _IUserItemBasicDealer
{
    private final NPUSUserData _m_userData;

    public RemoteEffectItemDefDealer(NPUSUserData _userData)
    {
        _m_userData = _userData;
    }

    public NPUSUserData getUserData()
    {
        return _m_userData;
    }

    @Override
    public ENPItemType getItemType()
    {
        return ENPItemType.REMOTE_EFFECT;
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
    	RefRemoteEffect ref = RefRemoteEffect.getMgr().get(_itemId);
    	if(null == ref)
    	{
    		USLog.error(_m_userData.getUSServer(), "player:{} init gain remote-effect:{} item ref is null.", _m_userData.getCid(), _itemId);
    		return;
    	}
    	
    	for(int i = 0; i < _count; i++)
    	{
    		_execRemoteEffect(ref, _context);
    	}
    }

    @Override
    public void gainItem(long _itemId, long _count, boolean _isNotMerge, NPPlayerContext _context)
    {
    	RefRemoteEffect ref = RefRemoteEffect.getMgr().get(_itemId);
    	if(null == ref)
    	{
    		USLog.error(_m_userData.getUSServer(), "player:{} gain remote-effect:{} item ref is null.", _m_userData.getCid(), _itemId);
    		return;
    	}
    	
    	for(int i = 0; i < _count; i++)
    	{
    		_execRemoteEffect(ref, _context);
    	}
    }

    @Override
    public boolean spendItem(long _itemId, long _count, NPPlayerContext _context)
    {
        return false;
    }

    /**
     * 执行远程效果
     * @param _ref
     * @param _context
     */
    private void _execRemoteEffect(RefRemoteEffect _ref, NPPlayerContext _context)
    {
        //检查条件是否满足（默认是全部满足）
        if (!NPPlayerConditionDealerMgr.IsEnable(_ref.condition, getUserData(), null))
            return;

        //检查是否可以足额扣除消耗
        if (!getUserData().hasCostItemList(_ref.cost_item_list.getItemTypeObjList())
                || !getUserData().spendItem(_ref.cost_item_list.getItemTypeObjList(), _context))
            return;

        //执行效果
        NPPlayerEffectDealer.dealEffect(_ref.effect_list.getPlayerEffectList(), getUserData(), null, _context);
    }
}

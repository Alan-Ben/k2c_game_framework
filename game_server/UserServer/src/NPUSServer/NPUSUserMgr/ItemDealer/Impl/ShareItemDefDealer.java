package NPUSServer.NPUSUserMgr.ItemDealer.Impl;

import NPEnum.ENPItemType;
import NPGameRes.Refs.Share.RefShare;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.ItemDealer.Share.ShareItemDealerMgr;
import NPUSServer.NPUSUserMgr.ItemDealer.Share._AShareItemDealer;
import NPUSServer.NPUSUserMgr.ItemDealer._IUserItemBasicDealer;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.USLog;

/**
 * @description: 分享道具处理器
 * @author: mark
 * @date: 2022-10-14 09:41:29
 */
public class ShareItemDefDealer implements _IUserItemBasicDealer
{
    private final NPUSUserData _m_userData;

    public ShareItemDefDealer(NPUSUserData _userData)
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
        return ENPItemType.SHARE;
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
        //分享主表检查
        RefShare shareRef = RefShare.getMgr().get(_itemId);
        if (null == shareRef)
        {
            USLog.error(_m_userData.getUSServer(), "can not gain share item, shareRef error, cid:{} shareType:{} itemId:{} refId:{}"
                    , _m_userData.getCid(), _itemId, _count);
            return;
        }

        //分享子类型
        _AShareItemDealer dealer = ShareItemDealerMgr.getInstance().getDealer(shareRef.share_item_type);
        if (null == dealer)
        {
            USLog.error(_m_userData.getUSServer(), "can not gain share item, itemType error, cid:{} shareType:{}"
                    , _m_userData.getCid(), _itemId);
            return;
        }

        //子项处理
        dealer.gainItem(_m_userData, shareRef, _count, _context);

        //放入数据
        _context.collectItem(getItemType(), _itemId, _count, _isNotMerge);

    }

    @Override
    public boolean spendItem(long _itemId, long _count, NPPlayerContext _context)
    {
        return false;
    }

}

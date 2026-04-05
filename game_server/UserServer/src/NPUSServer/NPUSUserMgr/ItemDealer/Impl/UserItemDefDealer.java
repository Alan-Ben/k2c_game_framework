package NPUSServer.NPUSUserMgr.ItemDealer.Impl;

import NPEnum.ENPItemType;
import NPGameRes.Refs.RefItemDef;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Common.Event.Events.Event_P_GAIN_ITEM_DEF;
import NPUSServer.NPUSUserMgr.ItemDealer._IUserItemBasicDealer;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.VariableDealer.NPPlayerVariableDeal;
import NPUSServer.USLog;

/**
 * @description: 预定义道具处理器
 * @author: ricci
 * @date: 2022-10-14 09:41:29
 */
public class UserItemDefDealer implements _IUserItemBasicDealer
{
    private final NPUSUserData _m_userData;

    public UserItemDefDealer(NPUSUserData _userData)
    {
        _m_userData = _userData;
    }

    @Override
    public ENPItemType getItemType()
    {
        return ENPItemType.ITEM_DEF;
    }

    @Override
    public long getItemCount(long _itemId)
    {
        RefItemDef ref = RefItemDef.getMgr().get(_itemId);
        if (ref == null)
        {
            return 0;
        }

        //根据高级公式获得数量
        long itemCount = NPPlayerVariableDeal.getInstance().CalculateVariableResult(_m_userData, ref.count_formula, null);

        if (itemCount <= 0)
        {
            return 0;
        }
        //不允许递归调用
        if (ref.item.getItemType() == ENPItemType.ITEM_DEF)
        {
            return 0;
        }

        return _m_userData.getItemCount(ref.item.getItemType(), ref.item.getItemId()) / itemCount;
    }

    @Override
    public boolean hasItem(long _itemId, long _count)
    {
        return _count <= getItemCount(_itemId);
    }

    @Override
    public void initGainItem(long _itemId, long _count, NPPlayerContext _context)
    {
    }

    @Override
    public void gainItem(long _itemId, long _count, boolean _isNotMerge, NPPlayerContext _context)
    {
        RefItemDef ref = RefItemDef.getMgr().get(_itemId);
        if (ref == null)
        {
        	USLog.error(_m_userData.getUSServer(), "player:{} gain item-def:{} fail.", _m_userData.getCid(), _itemId);
            return;
        }

        //根据高级公式获得数量
        long itemCount = NPPlayerVariableDeal.getInstance().CalculateVariableResult(_m_userData, ref.count_formula, null);
        //不允许递归调用
        if (ref.item.getItemType() == ENPItemType.ITEM_DEF)
        {
        	USLog.error(_m_userData.getUSServer(), "player:{} gain item-def:{} type:{} fail.", _m_userData.getCid(), _itemId, ref.item.getItemType());
            return;
        }
        if (itemCount > 0)
        {
            _m_userData.gainItem(ref.item.getItemType(), ref.item.getItemId(), itemCount * _count, _isNotMerge, _context);

            _m_userData.onLogicEvent(new Event_P_GAIN_ITEM_DEF(_context, _itemId, _count));
        }
    }

    @Override
    public boolean spendItem(long _itemId, long _count, NPPlayerContext _context)
    {
        RefItemDef ref = RefItemDef.getMgr().get(_itemId);
        if (ref == null)
        {
            return false;
        }

        //根据高级公式获得数量
        long itemCount = NPPlayerVariableDeal.getInstance().CalculateVariableResult(_m_userData, ref.count_formula, null);
        //不允许递归调用
        if (ref.item.getItemType() == ENPItemType.ITEM_DEF)
        {
            return false;
        }
        if (itemCount > 0)
        {
            _m_userData.spendItem(ref.item.getItemType(), ref.item.getItemId(), itemCount * _count, _context);
        }

        return true;
    }

}

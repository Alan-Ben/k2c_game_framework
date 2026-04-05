package NPUSServer.NPUSUserMgr.ItemDealer;

import MJLog.MJLog;
import NPCommon.Enum.NPCommonEnum.ENPInsteadItemType;
import NPEnum.ENPItemType;
import NPGameRes.Refs.RefItemAlter;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.USLog;

import java.util.ArrayList;
import java.util.List;

/**************
 * 通用的Item处理管理类，各处理器通过注册到本管理类中进行相关的通用Item处理
 * @author mj
 *
 */
public class UserItemDealerMgr
{
    private NPUSUserData _m_udUserData;

    //处理对象列表
    private ArrayList<_IUserItemBasicDealer> _m_lItemDealer;

    public UserItemDealerMgr(NPUSUserData _userData)
    {
        _m_udUserData = _userData;

        _m_lItemDealer = new ArrayList<_IUserItemBasicDealer>(ENPItemType.values().length);
        for (int i = 0; i < ENPItemType.values().length; i++)
        {
            _m_lItemDealer.add(null);
        }
    }

    public NPUSUserData getUserData()
    {
        return _m_udUserData;
    }

    protected _IUserItemBasicDealer _getDealer(ENPItemType _itemType)
    {
        return _m_lItemDealer.get(_itemType.ordinal());
    }

    /***************
     * 注册处理对象
     * @param _dealer
     */
    public void regDealer(_IUserItemBasicDealer _dealer)
    {
        if (null == _dealer)
            return;

        ENPItemType itemType = _dealer.getItemType();

        //判断是否有旧数据避免重复设置
        if (null != _m_lItemDealer.get(itemType.ordinal()))
        {
            USLog.fatal(_m_udUserData.getUSServer(), "Multi Reg Item Dealer for Item: " + itemType);
            return;
        }

        _m_lItemDealer.set(itemType.ordinal(), _dealer);
    }

    /*************
     * 释放所有资源
     */
    public void discard()
    {
        if (null != _m_lItemDealer)
            _m_lItemDealer.clear();

        _m_udUserData = null;
    }

    /*************
     * 获取数量
     * @param _itemId
     * @return
     */
    public long getItemCount(ENPItemType _itemType, long _itemId)
    {
        _IUserItemBasicDealer dealer = _getDealer(_itemType);
        if (null == dealer)
        {
            USLog.fatal(getUserData().getUSServer(), "Get Error ItemType: " + _itemType);
            return 0;
        }

        return dealer.getItemCount(_itemId);
    }

    /***************
     * 是否有足够的数量
     * @param _itemId
     * @param _count
     * @return
     */
    public boolean hasItem(ENPItemType _itemType, long _itemId, long _count)
    {
        _IUserItemBasicDealer dealer = _getDealer(_itemType);
        if (null == dealer)
        {
            USLog.fatal(getUserData().getUSServer(), "Get Error ItemType: " + _itemType);
            return false;
        }

        return dealer.hasItem(_itemId, _count);
    }

    /****************
     * 初始化的获取物品处理
     * @param _itemId
     * @param _count
     * @param _context
     * @return
     */
    public void initGainItem(ENPItemType _itemType, long _itemId, long _count, NPPlayerContext _context)
    {
        _IUserItemBasicDealer dealer = _getDealer(_itemType);
        if (null == dealer)
        {
            USLog.fatal(getUserData().getUSServer(), "Get Error ItemType: " + _itemType);
            return;
        }

        long oldVal = dealer.getItemCount(_itemId);

        dealer.initGainItem(_itemId, _count, _context);

        long newVal = dealer.getItemCount(_itemId);

        //梦加运营日志（背包物品由BagItemComponent单独记录）
        if (_itemType != ENPItemType.BAG_ITEM)
        {
            MJLog.logItemChg(getUserData(), _itemType, _itemId, 1, oldVal, _count, newVal, _context.getContextId());
        }
    }

    /****************
     * 获取物品的处理
     * @param _itemId
     * @param _count
     * @param _context
     * @return
     */
    public void gainItem(ENPItemType _itemType, long _itemId, long _count, boolean _isNotMerge, NPPlayerContext _context)
    {
        _IUserItemBasicDealer dealer = _getDealer(_itemType);
        if (null == dealer)
        {
            USLog.fatal(getUserData().getUSServer(), "Get Error ItemType: " + _itemType);
            return;
        }
        
        long oldVal = dealer.getItemCount(_itemId);
        
        dealer.gainItem(_itemId, _count, _isNotMerge, _context);

        long newVal = dealer.getItemCount(_itemId);

        //梦加运营日志（背包物品由BagItemComponent单独记录，包括action 3无效丢失）
        if (_itemType != ENPItemType.BAG_ITEM)
        {
            MJLog.logItemChg(getUserData(), _itemType, _itemId, 1, oldVal, _count, newVal, _context.getContextId());
        }
    }

    /****************
     * 消耗物品的处理
     * @param _itemId
     * @param _count
     * @param _context
     * @return
     */
    public boolean spendItem(ENPItemType _itemType, long _itemId, long _count, NPPlayerContext _context)
    {
        //负数返回失败
        if (_count < 0)
            return false;

        _IUserItemBasicDealer dealer = _getDealer(_itemType);
        if (null == dealer)
        {
            USLog.fatal(getUserData().getUSServer(), "Get Error ItemType: " + _itemType);
            return false;
        }
        long oldVal = dealer.getItemCount(_itemId);

        boolean b = dealer.spendItem(_itemId, _count, _context);

        long newVal = dealer.getItemCount(_itemId);
        if (b)
        {
            //梦加运营日志（背包物品由BagItemComponent单独记录）
            if (_itemType != ENPItemType.BAG_ITEM)
            {
                MJLog.logItemChg(getUserData(), _itemType, _itemId, 2, oldVal, _count, newVal, _context.getContextId());
            }
        }
        return b;
    }

    /****************
     * 使用可替换模式进行扣除处理
     * @param _itemType
     * @param _itemId
     * @param _count
     * @param _alterType
     * @param _context
     * @return
     */
    public boolean spendItem(ENPItemType _itemType, long _itemId, long _count, ENPInsteadItemType _alterType, NPPlayerContext _context)
    {
        //负数返回失败
        if (_count < 0)
            return false;

        long srcItemCount = getItemCount(_itemType, _itemId);
        if (srcItemCount < _count)//物品数量不足，查找可替换物品
        {
            //查询可替代物
            List<RefItemAlter> alterList = RefItemAlter.getMgr().lookupItemAlterEDList(_alterType, _itemType, _itemId);
            //无可替代物则直接返回失败
            if (alterList == null || alterList.isEmpty())
                return false;

            long[] itemCountList = new long[alterList.size()];
            //检测替代品数量
            long allAlterCount = 0;
            //逐个物品判断是否符合匹配要求
            for (int j = 0; j < alterList.size(); j++)
            {
                RefItemAlter refAlter = alterList.get(j);
                if (null == refAlter)
                    continue;

                long count = getItemCount(refAlter.alter_item_type, refAlter.alter_item_id);
                if (count <= 0)
                    continue;

                if (count + allAlterCount + srcItemCount < _count)
                {
                    itemCountList[j] = count;
                    allAlterCount += count;
                } else
                {
                    long costCount = _count - allAlterCount - srcItemCount;
                    itemCountList[j] = costCount;
                    allAlterCount += costCount;
                    break;
                }
            }

            //总数量还是不匹配则返回失败
            if (allAlterCount + srcItemCount < _count)
                return false;

            //扣除源物品
            if (srcItemCount > 0)
            {
                if (!spendItem(_itemType, _itemId, srcItemCount, _context))
                    return false;
            }

            //扣除替代物品
            for (int j = 0; j < alterList.size(); j++)
            {
                RefItemAlter refAlter = alterList.get(j);
                if (null == refAlter)
                    continue;

                //获取扣除数量
                long count = itemCountList[j];
                if (count <= 0)
                    continue;

                if (!spendItem(refAlter.alter_item_type, refAlter.alter_item_id, count, _context))
                    return false;
            }
        } else
        {
            if (!spendItem(_itemType, _itemId, _count, _context))
                return false;
        }

        return true;
    }


}

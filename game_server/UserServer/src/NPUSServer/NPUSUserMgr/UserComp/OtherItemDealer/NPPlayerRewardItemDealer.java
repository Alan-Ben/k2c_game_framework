package NPUSServer.NPUSUserMgr.UserComp.OtherItemDealer;

import NPCommon.CommonObj.NPItemCostCollector_nosafe;
import NPEnum.ENPItemType;
import NPGameRes.GameObjs.Reward.RewardMgr;
import NPGameRes.GameObjs.Reward.RewardObj;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.ItemDealer._IUserItemBasicDealer;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.USLog;


/****************************
 * 用户头像组件
 * @author Administrator
 *
 */
public class NPPlayerRewardItemDealer implements _IUserItemBasicDealer
{
    private NPUSUserData _m_udUserData;

    public NPPlayerRewardItemDealer(NPUSUserData _userData)
    {
        _m_udUserData = _userData;
    }

    //////////////////////////////
    // ItemDealer处理部分
    //////////////////////////////

    /**********
     * 对应处理的类型
     * @return
     */
    public ENPItemType getItemType()
    {
        return ENPItemType.REWARD;
    }

    /*************
     * 获取数量
     * @param _itemId
     * @return
     */
    public long getItemCount(long _itemId)
    {
        return 0;
    }

    /***************
     * 是否有足够的数量
     * @param _itemId
     * @param _count
     * @return
     */
    public boolean hasItem(long _itemId, long _count)
    {
        return false;
    }

    /****************
     * 初始化的获取物品处理
     * @param _itemId
     * @param _count
     * @param _context
     * @return
     */
    public void initGainItem(long _itemId, long _count, NPPlayerContext _context)
    {
        RewardObj rewardObj = RewardMgr.getInstance().lookupReward(_itemId);
        if (null == rewardObj)
        {
            USLog.error(_m_udUserData.getUSServer(), "Can not find reward for id:{}", _itemId, new Exception());
            return;
        }

        // 多次且需要合并，用collector收集后一次性发放
        if (_count > 1 && rewardObj.getRef().is_merge)
        {
            NPItemCostCollector_nosafe collector = new NPItemCostCollector_nosafe();
            for (int i = 0; i < _count; i++)
            {
                collector.addItemList(rewardObj.getItemList());
            }
            _m_udUserData.initGainItem(collector.getItemList(), _context);
            return;
        }

        // 单次或不需要合并，逐次直接发放
        for (int i = 0; i < _count; i++)
        {
            _m_udUserData.initGainItem(rewardObj.getItemList(), _context);
        }
    }

    /****************
     * 获取物品的处理，返回值表示是否触发gameevent事件
     * @param _itemId
     * @param _count
     * @param _context
     * @return
     */
    public void gainItem(long _itemId, long _count, boolean _isNotMerge, NPPlayerContext _context)
    {
        RewardObj rewardObj = RewardMgr.getInstance().lookupReward(_itemId);
        if (null == rewardObj)
        {
            USLog.error(_m_udUserData.getUSServer(), "Can not find reward for id:{}", _itemId, new Exception());
            return;
        }

        // 多次且需要合并，用collector收集后一次性发放
        if (_count > 1 && rewardObj.getRef().is_merge)
        {
            NPItemCostCollector_nosafe collector = new NPItemCostCollector_nosafe();
            for (int i = 0; i < _count; i++)
            {
                collector.addItemList(rewardObj.getItemList());
            }
            _m_udUserData.gainItemList(collector.getItemList(), false, _context);
            return;
        }

        // 单次或不需要合并，逐次直接发放
        for (int i = 0; i < _count; i++)
        {
            _m_udUserData.gainItemList(rewardObj.getItemList(), !rewardObj.getRef().is_merge, _context);
        }
    }

    /****************
     * 消耗物品的处理
     * @param _itemId
     * @param _count
     * @param _context
     * @return
     */
    public boolean spendItem(long _itemId, long _count, NPPlayerContext _context)
    {
        return false;
    }

    //////////////////////////////
    // ItemDealer处理部分结束
    //////////////////////////////
}

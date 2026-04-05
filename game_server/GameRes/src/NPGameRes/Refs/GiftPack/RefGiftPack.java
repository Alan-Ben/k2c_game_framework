package NPGameRes.Refs.GiftPack;

import CommonEnum.EGiftPackLogType;
import CommonEnum.EGiftPackType;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.CommonObj.NPRefreshTimeObj;
import NPCommon.ErrMain.OrderErr;
import NPCommon.ErrMain.Result.ResultOne;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefField;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPEnum.ENPItemType;

import java.util.ArrayList;
import java.util.List;

@RefTable(tableName = "gift_pack")
public class RefGiftPack extends RefBase
{
    private static RefGiftPackMgr _g_mgr = new RefGiftPackMgr();

    public static RefGiftPackMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefGiftPackMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefGiftPackMgr) _mgr;
    }

    public static class RefGiftPackMgr extends RefTableContainer<RefGiftPack>
    {
        @Override
        public void _onTableLoaded()
        {

        }
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefGiftPack newRef = (RefGiftPack) _newRef;
        id = newRef.id;
        type = newRef.type;
        cost_list = newRef.cost_list;
        item_list = newRef.item_list;
        buy_limit_refresh_time = newRef.buy_limit_refresh_time;
        buy_limit_count = newRef.buy_limit_count;
        not_send_vip_exp = newRef.not_send_vip_exp;
        log_type = newRef.log_type;
        is_web_gift_pack = newRef.is_web_gift_pack;
    }

    /**
     * 获取对象数据Id，尽量唯一
     */
    @Override
    public long Id()
    {
        return id;
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public long id; // 礼包ID
    public EGiftPackType type; // 礼包类型
    public List<NPCommonCostItem> cost_list = new ArrayList<>(); // 消耗列表
    public List<NPCommonCostItem> item_list = new ArrayList<>(); // 奖励物品列表
    public NPRefreshTimeObj buy_limit_refresh_time = new NPRefreshTimeObj(); // 限购次数刷新时间（ENPTimeRefreshType）
    public int buy_limit_count; // 限购次数 0=不限制次数
    public boolean not_send_vip_exp; // 是否不发送VIP经验
    public EGiftPackLogType log_type = EGiftPackLogType.NONE; // 日志类型
    public boolean is_web_gift_pack; // 是否是网页充值礼包

    @RefField(isIgnore = true)
    private List<Long> relativeActivityIdList = new ArrayList<>(); // 关联的活动ID列表
    public List<Long> getRelativeActivityIdList()
    {
        return relativeActivityIdList;
    }
    public void setRelativeActivityIdList(List<Long> activityIdList)
    {
        relativeActivityIdList = activityIdList;
    }

    @RefField(isIgnore = true)
    private List<RefGiftPackExtraGain> refGiftPackExtraGainList = new ArrayList<>(); // 关联的额外奖励列表
    public List<RefGiftPackExtraGain> getRefGiftPackExtraGainList()
    {
        return refGiftPackExtraGainList;
    }
    public void setRefGiftPackExtraGainList(List<RefGiftPackExtraGain> extraGainList)
    {
        refGiftPackExtraGainList = extraGainList;
    }

    /**
     * 获取支付ID
     * @return
     */
    public ResultOne<Long> getPayId()
    {
        for (NPCommonCostItem costItem : cost_list)
        {
            if (costItem.getItemType() == ENPItemType.PAY)
                return ResultOne.succ(costItem.getItemId());
        }

        return ResultOne.failed(OrderErr.NOT_SUPPORT_PAY_TYPE);
    }

    /**
     * 填充额外奖励物品列表
     * @param _hadBuyCount
     * @param _itemList
     */
    public void fillExtraGainItemList(int _hadBuyCount, List<NPCommonCostItem> _itemList)
    {
        if (refGiftPackExtraGainList.isEmpty())
            return;

        for (RefGiftPackExtraGain ref : refGiftPackExtraGainList)
        {
            if (!ref.buy_times_range.inRange(_hadBuyCount))
                continue;

            _itemList.addAll(ref.extra_gain_list);
        }
    }

}
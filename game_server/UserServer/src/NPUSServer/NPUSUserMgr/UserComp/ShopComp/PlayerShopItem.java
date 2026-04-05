package NPUSServer.NPUSUserMgr.UserComp.ShopComp;

import Common.ShopObj.Shop_ItemInfo;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.Util.CommonFunc;
import NPGameRes.Refs.Shop.RefShopItem;
import NPGameRes.Refs.Shop.RefShopItemDiscount;
import NPGameRes.Refs.Shop.RefShopItemGroup;
import NPUSServer.Common.UsFunc;
import NPUSServer.NPUSUserMgr.ConditionDealer.NPPlayerConditionDealerMgr;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_030_ShopOp;
import USDB.Bo.PlayerShopBuyRecordBO;
import USDB.Bo.PlayerShopItemBO;

import java.util.ArrayList;
import java.util.List;

/**
 * @description: 商店商品数据
 * @author: ricci
 * @date: 2022-11-11 11:26:08
 */
public class PlayerShopItem
{
    //商店对象
    private PlayerShopInfo _m_shopInfo;
    //商品数据库数据
    private PlayerShopItemBO _m_bo;
    //商品购买记录数据
    private PlayerShopBuyRecord _m_buyRecord;

    //商品配置
    private RefShopItem _m_ref;
    //商品打折配置，可以为空，表示不打折
    private RefShopItemDiscount _m_discountRef;


    public PlayerShopItem(PlayerShopInfo _shopInfo, PlayerShopItemBO _bo, RefShopItem _ref)
    {
        this._m_shopInfo = _shopInfo;
        this._m_bo = _bo;
        _m_ref = _ref;
    }

    public PlayerShopItem(PlayerShopInfo _shopInfo, PlayerShopItemBO _bo,
                          RefShopItem _ref, RefShopItemDiscount _discountRef)
    {
        this._m_shopInfo = _shopInfo;
        this._m_bo = _bo;
        _m_ref = _ref;
        _m_discountRef = _discountRef;
    }

    public RefShopItem getRef()
    {
        return _m_ref;
    }

    public RefShopItemDiscount getDiscountRef()
    {
        return _m_discountRef;
    }

    /**
     * 返回商品折扣
     */
    public long getDiscount()
    {
        if (_m_discountRef != null && _m_discountRef.discount > 0)
            return _m_discountRef.discount;

        return 10000;
    }

    public PlayerShopItemBO getBo()
    {
        return _m_bo;
    }

    public long getId()
    {
        return _m_bo.getId();
    }

    public long getShopRefId()
    {
        return _m_shopInfo.getShopId();
    }

    public NPUSUserData getUserData()
    {
        return _m_shopInfo.getUserData();
    }

    public int getHasBuyCount()
    {
        return _m_buyRecord == null ? 0 : _m_buyRecord.getBuyCount() + _m_shopInfo.getForeverBuyCount(getRef().id);
    }

    public void initBuyRecord(PlayerShopBuyRecordBO _recordBo)
    {
        _m_buyRecord = new PlayerShopBuyRecord(_recordBo);
    }

    public void cleanBuyRecord()
    {
        _m_buyRecord = null;
    }

    /**
     * 返回商品价格
     * @param _goodsCount 购买数量
     * @return ArrayList<NPCommonCostItem> 购买消耗
     */
    public List<NPCommonCostItem> calBuyCost(int _goodsCount)
    {
        List<NPCommonCostItem> costItemList = new ArrayList<>();

        RefShopItem ref = getRef();
        if (ref.times_price_type_id > 0)
        {
            //计算购买消耗，这里的次数要使用购买的次数，因此是从1开始的
            for (int i = 1; i <= _goodsCount; i++)
            {
                NPCommonCostItem costItem = UsFunc.calCostPrice(getUserData(), ref.times_price_type_id, getHasBuyCount() + i);
                if (costItem == null)
                    continue;

                CommonFunc.mergeCostItem(costItemList, costItem);
            }
        } else
        {
            costItemList.addAll(CommonFunc.itemMultiple(ref.cost_item, _goodsCount));

            //计算折扣 只有固定消耗需要计算折扣 【GOB-6161】
            if (getDiscount() != 10000)
                costItemList = CommonFunc.costItemDiscount(costItemList, getDiscount());
        }

        return costItemList;
    }

    /**
     * 购买 如果需要记录永久购买记录，购买次数将不再本对象上记录
     * @param _goodsCount    商品数量
     * @param _goodsItemList 获得商品
     * @return 是否购买成功
     */
    protected boolean _buy(int _goodsCount, ArrayList<NPCommonCostItem> _goodsItemList)
    {
        //判断是否满足购买条件
        RefShopItemGroup refShopItemGroup = RefShopItemGroup.getMgr().get(getBo().getShopItemGroupId());
        if (!NPPlayerConditionDealerMgr.IsEnable(refShopItemGroup.buy_condition, getUserData(), null))
            return false;

        //计划购买后总数量
        int hasBuyCount = getHasBuyCount() + _goodsCount;
        if (hasBuyCount > getBo().getCanBuyNum())
            return false;

        if (getRef().is_forever)
        {
            _m_shopInfo.addForeverBuyCount(getRef().id, _goodsCount);
        } else
        {
            //购买成功，记录购买次数
            recordBuyCount(hasBuyCount);
        }

        _goodsItemList.add(CommonFunc.itemMultiple(getRef().item, _goodsCount));
        pushChg();
        return true;
    }

    /**
     * 记录购买次数
     * @param _hasBuyNum
     */
    private void recordBuyCount(int _hasBuyNum)
    {
        if (_m_buyRecord == null)
        {
            PlayerShopBuyRecordBO bo = new PlayerShopBuyRecordBO();
            bo.setCid(getUserData().getUSServer().getBM(), getUserData().getCid());
            bo.setShopDbId(getUserData().getUSServer().getBM(), _m_shopInfo.getShopDbId());
            bo.setShopItemDbId(getUserData().getUSServer().getBM(), getId());
            bo.setBuyCount(getUserData().getUSServer().getBM(), _hasBuyNum);
            bo.insert(getUserData().getUSServer().getBM());
            _m_buyRecord = new PlayerShopBuyRecord(bo);
        }else
        {
            _m_buyRecord.recordBuyCount(getUserData().getUSServer().getBM(), _hasBuyNum);
        }
    }

    /**
     * 构造商品数据协议信息
     * @return Shop_ItemInfo
     */
    public Shop_ItemInfo makeProto()
    {
        Shop_ItemInfo proto = new Shop_ItemInfo();
        proto.setInstanceId(getId());
        proto.setShopItemRefId(_m_bo.getShopItemRefId());
        proto.setHasBuyNum(getHasBuyCount());
        proto.setDiscountRefId(getDiscountRef() == null ? 0 : getDiscountRef().id);
        proto.setShopItemGroupId(getBo().getShopItemGroupId());
        proto.setCanBuyNum(getBo().getCanBuyNum());
        return proto;
    }

    /**
     * 推送商品变更
     */
    public void pushChg()
    {
        getUserData().sendMsgToGC(US2GCWriter_030_ShopOp.make_051_OnShopItemChg(this));
    }

    @Override
    public String toString()
    {
        return "{" +
                ", id = " + getId() +
                ", refId = " + getRef().id +
                ", discount = " + getDiscount() +
                ", buyCount = " + getHasBuyCount() +
                '}';
    }
}

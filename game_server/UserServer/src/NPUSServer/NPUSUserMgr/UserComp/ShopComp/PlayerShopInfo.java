package NPUSServer.NPUSUserMgr.UserComp.ShopComp;

import Common.ShopObj.Shop_Info;
import EventSystem.NPHandlerEntry;
import NPCommon.DB.BM.BM;
import NPCommon.Log.CommLog;
import NPCommon.NPLogDB.CommLogDB;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.Delegate.HandlerTwo;
import NPCommon.Util.Delegate._IHandlerHolder;
import NPCommon.Util.Pair.WCGPairLong;
import NPGameRes.LogicEvent.MetaData.EventMeta;
import NPGameRes.LogicEvent.MetaData.EventMetaMgr_Refdata;
import NPGameRes.LogicEvent._ALogicEventBase;
import NPGameRes.Refs.Shop.RefShop;
import NPGameRes.Refs.Shop.RefShopItem;
import NPGameRes.Refs.Shop.RefShopItemDiscount;
import NPGameRes.Refs.Shop.ShopItemGroupDrop;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.ConditionDealer.NPPlayerConditionDealerMgr;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.VariableDealer.NPPlayerVariableDeal;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_030_ShopOp;
import NPUSServer.USLog;
import USDB.Bo.PlayerForeverShopBuyRecordBO;
import USDB.Bo.PlayerShopBO;
import USDB.Bo.PlayerShopBuyRecordBO;
import USDB.Bo.PlayerShopItemBO;
import USLOGDB.Bo.LogShopRefreshBO;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

/**
 * @description: 商店数据
 * @author: ricci
 * @date: 2022-11-11 11:24:49
 */
public class PlayerShopInfo implements _IHandlerHolder
{
    //商店配置
    private RefShop _m_ref;
    //商店数据库数据
    private PlayerShopBO _m_bo;
    //商品列表
    private List<PlayerShopItem> _m_goodsList;
    //商店组件
    private PlayerShopComponent _m_shopComp;
    //终身限购记录 key: 商品配置ID, value: 购买记录对象
    private Map<Long, PlayerForeverShopBuyRecord> _m_foreverBuyRecords;

    private NPHandlerEntry<NPUSUserData> _m_evtEntry;

    public PlayerShopInfo(PlayerShopComponent _comp, RefShop _ref, PlayerShopBO _bo)
    {
        _m_goodsList = new ArrayList<>();
        _m_shopComp = _comp;
        _m_ref = _ref;
        _m_bo = _bo;
        _m_foreverBuyRecords = new HashMap<>();
        _m_evtEntry = null;
    }

    public long getShopDbId()
    {
        return getBo().getId();
    }

    public long getShopId()
    {
        return _m_ref.id;
    }

    public PlayerShopBO getBo()
    {
        return _m_bo;
    }

    public long getNextFreshTimeMs()
    {
        return _m_bo.getNextFreshTimeMs();
    }

    public long getCid()
    {
        return getBo().getCid();
    }

    public RefShop getRef()
    {
        return _m_ref;
    }

    public NPUSUserData getUserData()
    {
        return _m_shopComp.getUserData();
    }

    public int getRefreshNum()
    {
        return getBo().getHasRefreshNum();
    }

    /**
     * 从数据库初始化商品数据
     * @param _shopBo 商店数据库对象
     */
    public void initShopItemFromDB(PlayerShopItemBO _shopBo)
    {
        //查询商店商品配置
        RefShopItem refShopItem = RefShopItem.getMgr().get(_shopBo.getShopItemRefId());
        if (refShopItem == null)
        {
            USLog.error(_m_shopComp.getUSServer(), "initShopItemFromDB fail cid:{} shopRefId:{} shopItemRefId:{}",
                    getCid(), getShopId(), _shopBo.getShopItemRefId());
            return;
        }

        //查询折扣配置，折扣配置可以为空
        RefShopItemDiscount refShopItemDiscount = RefShopItemDiscount.getMgr().get(_shopBo.getDiscountRefId());

        //创建新的商品数据并放入map管理
        PlayerShopItem shopItem = new PlayerShopItem(this, _shopBo, refShopItem, refShopItemDiscount);

        _m_goodsList.add(shopItem);
    }

    /**
     * 从数据库初始化商品数据
     * @param _recordBo 商店数据库对象
     */
    public void initShopBuyRecordFromDB(PlayerShopBuyRecordBO _recordBo)
    {
        PlayerShopItem item = lookup(_recordBo.getShopItemDbId());
        if (item == null)
        {
            USLog.error(_m_shopComp.getUSServer(), "initShopBuyRecordFromDB item not found, cid:{} shopRefId:{} shopItemDbId:{}",
                    getCid(), getShopId(), _recordBo.getShopItemDbId());
            return;
        }

        item.initBuyRecord(_recordBo);
    }

    /**
     * 从数据库初始化终身限购记录数据
     * @param _recordBo 终身限购记录数据库对象
     */
    public void initForeverBuyRecordFromDB(PlayerForeverShopBuyRecordBO _recordBo)
    {
        PlayerForeverShopBuyRecord record = new PlayerForeverShopBuyRecord(_recordBo);
        _m_foreverBuyRecords.put(_recordBo.getShopItemRefId(), record);
    }

    /**
     * 获取终身限购商品的已购买次数
     * @param _shopItemRefId 商品配置ID
     * @return 已购买次数
     */
    public int getForeverBuyCount(long _shopItemRefId)
    {
        PlayerForeverShopBuyRecord record = _m_foreverBuyRecords.get(_shopItemRefId);
        return record == null ? 0 : record.getBuyCount();
    }

    /**
     * 更新终身限购商品的购买次数
     * @param _shopItemRefId 商品配置ID
     * @param _addCount 增加的购买次数
     */
    public void addForeverBuyCount(long _shopItemRefId, int _addCount)
    {
        PlayerForeverShopBuyRecord record = _m_foreverBuyRecords.get(_shopItemRefId);
        int newCount;
        
        BM bmObj = _m_shopComp.getUSServer().getBM();
        
        if (record == null)
        {
            // 创建新记录
            newCount = _addCount;
            PlayerForeverShopBuyRecordBO bo = new PlayerForeverShopBuyRecordBO();
            bo.setCid(bmObj, getCid());
            bo.setShopRefId(bmObj, getShopId());
            bo.setShopItemRefId(bmObj, _shopItemRefId);
            bo.setBuyCount(bmObj, newCount);
            bo.insert(bmObj);
            
            // 创建数据对象并缓存
            record = new PlayerForeverShopBuyRecord(bo);
            _m_foreverBuyRecords.put(_shopItemRefId, record);
        }
        else
        {
            // 更新现有记录
            newCount = record.getBuyCount() + _addCount;
            record.recordBuyCount(bmObj, newCount);
        }
    }

    /**
     * 查询商店商品数据
     * @param _goodsDBId 商品唯一id
     * @return NPPlayerShopGoods
     */
    protected PlayerShopItem lookup(long _goodsDBId)
    {
        for (PlayerShopItem playerShopItem : _m_goodsList)
        {
            if (playerShopItem.getId() == _goodsDBId)
            {
                return playerShopItem;
            }
        }
        return null;
    }

    /**
     * 增加刷新次数
     * @param _addVal 增加值
     */
    public void addFreshNum(int _addVal)
    {
        getBo().saveHasRefreshNum(_m_shopComp.getUSServer().getBM(), getRefreshNum() + _addVal);
    }

    /**
     * 尝试执行刷新
     * @param _nowTimeMS 当前时间
     */
    protected void _tryRefresh(long _nowTimeMS)
    {
        //检查刷新时间限制
        long nextFreshTimeMs = getNextFreshTimeMs();
        if (nextFreshTimeMs < 0)
        {
            //认为是永远不需要刷新的数据
            return;
        }
        if (nextFreshTimeMs > _nowTimeMS)
        {
            //未达到刷新时间
            return;
        }

        //执行商品刷新逻辑
        _refreshShop(_nowTimeMS, false);
    }

    /**
     * 执行商店整体刷新逻辑
     * @param _nowTimeMs 刷新时刻
     * @param _isInit
     * @return boolean 是否成功刷新
     */
    protected void _refreshShop(long _nowTimeMs, boolean _isInit)
    {
        //更新刷新时间数据
        long nextFreshTimeTagMS = getRef().refresh_clock.getNextFreshTimeTagMS(_nowTimeMs);

        BM bmObj = _m_shopComp.getUSServer().getBM();

        getBo().setNextFreshTimeMs(bmObj, nextFreshTimeTagMS);
        getBo().setHasRefreshNum(bmObj, 0);
        getBo().saveAllMarked(bmObj);

        //刷新逻辑(需要区分是否刷新商品, 如果不刷新商品, 则只清理购买限制)
        if (_isInit || getRef().need_refresh_item)
        {
            _refreshGoods();

            //商店刷新日志
            LogShopRefreshBO logBo = new LogShopRefreshBO();
            logBo.setCid(bmObj, getCid());
            logBo.setShopId(bmObj, getShopId());
            logBo.setRefreshResult(bmObj, makeRefreshResult());
            CommLogDB.log(bmObj, logBo, null);
        }else
        {
            _cleanGoodsBuyLimit();
        }
    }

    /**
     * 清理商品购买限制（注意：不清理终身限购记录）
     */
    private void _cleanGoodsBuyLimit()
    {
        getUserData().getUSServer().getBM().getBM(PlayerShopBuyRecordBO.class).delAll("shop_db_id",getShopDbId());

        for (PlayerShopItem item : _m_goodsList)
        {
            item.cleanBuyRecord();
        }

        pushChg();
    }

    /**
     * 构造刷新结果信息
     */
    public String makeRefreshResult()
    {
        List<WCGPairLong> list = new ArrayList<>();
        for (PlayerShopItem goods : _m_goodsList)
        {
            list.add(new WCGPairLong(goods.getRef().id, goods.getDiscount()));
        }
        return CommonFunc.list2String(list, ';');
    }

    /**
     * 执行刷新商品列表
     */
    protected void _refreshGoods()
    {
        BM bmObj = _m_shopComp.getUSServer().getBM();

        //清空商店商品数据和购买记录数据
        _m_goodsList.clear();
        bmObj.getBM(PlayerShopItemBO.class).delAll("shop_db_id",getShopDbId());
        bmObj.getBM(PlayerShopBuyRecordBO.class).delAll("shop_db_id",getShopDbId());

        //商品组:商品基础数量:折扣商品基础数量
        List<ShopItemGroupDrop> dropList = getRef().discountWeightList;
        if (dropList == null || dropList.isEmpty())
        {
            USLog.error(_m_shopComp.getUSServer(), "PlayerShopInfo _refreshGoods dropList is empty shopRefId:{}", getShopId());
            return;
        }

        for (ShopItemGroupDrop dropItem : dropList)
        {
            //判断商品组是否生效
            if (!NPPlayerConditionDealerMgr.IsEnable(dropItem.getRef().enable_cond, getUserData(), null))
                continue;

            //计算商品组额外掉落数量
            long extNum = NPPlayerVariableDeal.getInstance().CalculateVariableResult(getUserData(), dropItem.getRef().ext_num, null);
            //计算商品组额外打折数量
            long extDiscountNum = NPPlayerVariableDeal.getInstance().CalculateVariableResult(getUserData(), dropItem.getRef().discount_ext_num, null);

            List<RefShopItem> discountItemList = new ArrayList<>();
            List<RefShopItem> normalItemList = new ArrayList<>();

            //计算商品组掉落道具id列表
            dropItem.drop(getUserData().getCid(), extNum, extDiscountNum, discountItemList, normalItemList);

            //计算商品组折扣道具id列表
            for (RefShopItem refShopItem : discountItemList)
            {
                PlayerShopItemBO bo = new PlayerShopItemBO();
                bo.setCid(bmObj, getCid());
                bo.setShopDbId(bmObj, getShopDbId());
                bo.setShopItemRefId(bmObj, refShopItem.id);
                RefShopItemDiscount refDiscount = refShopItem.discountWeightList.random();
                bo.setDiscountRefId(bmObj, refDiscount == null ? 0 : refDiscount.id);
                bo.setShopItemGroupId(bmObj, dropItem.getRef().group_id);
                bo.setCanBuyNum(bmObj, _calCanBuyNum(refShopItem));
                bo.insert(bmObj);

                _m_goodsList.add(new PlayerShopItem(this, bo, refShopItem, refDiscount));
            }

            //计算商品组无折扣道具id列表
            for (RefShopItem refShopItem : normalItemList)
            {
                PlayerShopItemBO bo = new PlayerShopItemBO();
                bo.setCid(bmObj, getCid());
                bo.setShopDbId(bmObj, getShopDbId());
                bo.setShopItemRefId(bmObj, refShopItem.id);
                bo.setShopItemGroupId(bmObj, dropItem.getRef().group_id);
                bo.setCanBuyNum(bmObj, _calCanBuyNum(refShopItem));
                bo.insert(bmObj);

                _m_goodsList.add(new PlayerShopItem(this, bo, refShopItem));
            }
        }

        pushChg();
    }

    /**
     * 计算商品可购买数量
     * @param _refShopItem 商品配置
     * @return 可购买数量
     */
    private long _calCanBuyNum(RefShopItem _refShopItem)
    {
        return _refShopItem.base_buy_num + NPPlayerVariableDeal.getInstance().CalculateVariableResult(getUserData(), _refShopItem.ext_buy_num, null);
    }

    /**
     * 构造商店数据协议信息
     * @return Shop_Info
     */
    public Shop_Info makeProto()
    {
        Shop_Info proto = new Shop_Info();
        proto.setShopRefId(getShopId());
        proto.setNextRefreshTimeMs(getNextFreshTimeMs());
        proto.setRefreshNum(getRefreshNum());
        for (PlayerShopItem playerShopItem : _m_goodsList)
        {
            proto.addGoodsList(playerShopItem.makeProto());
        }
        return proto;
    }

    /**
     * 推送商店变更
     */
    public void pushChg()
    {
        getUserData().sendMsgToGC(US2GCWriter_030_ShopOp.make_050_OnShopRefresh(this));
    }

    /**
     * 注册刷新事件
     */
    public void regRefreshEvent()
    {
        if (_m_ref.refresh_shop_event_type == null || _m_ref.refresh_shop_event_type.isEmpty())
            return;

        EventMeta eventMeta = EventMetaMgr_Refdata.getInstance().lookupMetaByEventName(
                _m_ref.refresh_shop_event_type.toUpperCase());
        if (eventMeta == null)
        {
            CommLog.error("PlayerShopInfo regRefreshEvent fail unknown eventType:{} shopRefId:{}",
                    _m_ref.refresh_shop_event_type, _m_ref.id);
            return;
        }

        // 注册事件监听
        NPHandlerEntry<NPUSUserData> evtEntry = _m_shopComp.getUserData().getEventHandlerMgr().regHandler(
                eventMeta.getEventId(), this, new HandlerTwo<_ALogicEventBase, NPUSUserData>()
                {
                    @Override
                    public void handle(_ALogicEventBase _evt, NPUSUserData _userData)
                    {
                        NPPlayerContext context = (NPPlayerContext) _evt.getContext();

                        BM bmObj = _m_shopComp.getUSServer().getBM();

                        //刷新逻辑(需要区分是否刷新商品, 如果不刷新商品, 则只清理购买限制)
                        if (getRef().need_refresh_item)
                        {
                            _refreshGoods();

                            //商店刷新日志
                            LogShopRefreshBO logBo = new LogShopRefreshBO();
                            logBo.setCid(bmObj, getCid());
                            logBo.setShopId(bmObj, getShopId());
                            logBo.setRefreshResult(bmObj, makeRefreshResult());
                            CommLogDB.log(bmObj, logBo, context);
                        }else
                        {
                            _cleanGoodsBuyLimit();
                        }
                    }
                });

        _m_evtEntry = evtEntry;
    }

    public void unregRefreshEvent()
    {
        if (_m_evtEntry != null)
        {
            _m_shopComp.getUserData().getEventHandlerMgr().unregHandler(_m_evtEntry);
            _m_evtEntry = null;
        }
    }

    @Override
    public String toString()
    {
        StringBuilder sb = new StringBuilder();
        for (PlayerShopItem goods : _m_goodsList)
        {
            sb.append("\n").append("[").append(goods).append("]");
        }
        return "{"
                + "refId: " + _m_ref.id
                + "goods: " + sb
                + "}";
    }
}

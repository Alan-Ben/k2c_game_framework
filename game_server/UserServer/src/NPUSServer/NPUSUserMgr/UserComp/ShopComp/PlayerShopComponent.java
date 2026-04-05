package NPUSServer.NPUSUserMgr.UserComp.ShopComp;

import ALBasicServer.ALProcess.ALProcess;
import ALBasicServer.ALProcess._ITALProcessAction;
import Common.ShopObj.Shop_Info;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.CommonProcess._IEZProcessMonitor;
import NPCommon.DB.BM.BM;
import NPCommon.DB._ASelectCallback;
import NPCommon.Enum.NPCommonEnum.ENPPlayerCompType;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.Pair.WCGPair;
import NPGameRes.Refs.Shop.RefShop;
import NPGameRes.Refs.Shop.RefShopItem;
import NPUSServer.NPUSUserMgr.ConditionDealer.NPPlayerConditionDealerMgr;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp._ANPUserComponent;
import NPUSServer.USLog;
import USDB.Bo.PlayerForeverShopBuyRecordBO;
import USDB.Bo.PlayerShopBO;
import USDB.Bo.PlayerShopBuyRecordBO;
import USDB.Bo.PlayerShopItemBO;

import java.util.ArrayList;
import java.util.List;

/**
 * @description: 玩家商店组件
 * @author: ricci
 * @date: 2022-11-11 11:08:38
 */
public class PlayerShopComponent extends _ANPUserComponent
{
    //商店列表
    private List<PlayerShopInfo> _m_shopList;

    public PlayerShopComponent(NPUSUserData _userData)
    {
        super(_userData, ENPPlayerCompType.SHOP);
        _m_shopList = new ArrayList<>();
    }

    protected void _lock()
    {
        getUserData().lockUser();
    }

    protected void _unlock()
    {
        getUserData().unlockUser();
    }

    public long getCid()
    {
        return getUserData().getCid();
    }

    /**
     * 从配置文件初始化
     */
    private void __initShopFromRef()
    {
        for (RefShop ref : RefShop.getMgr().getList())
        {
            if (ref == null)
                continue;

            long nowTimeMS = CommonFunc.getNowTimeMS();

            PlayerShopInfo shop = lookup(ref.id);
            if (shop == null)
            {
                BM bmObj = getUSServer().getBM();

                PlayerShopBO bo = new PlayerShopBO();
                bo.setCid(bmObj, getCid());
                bo.setShopRefId(bmObj, ref.id);
                bo.setNextFreshTimeMs(bmObj, 0);
                bo.setHasRefreshNum(bmObj, 0);
                bo.insert(bmObj);
                //放入商店列表
                shop = new PlayerShopInfo(this, ref, bo);
                //首次刷新商店列表
                shop._refreshShop(nowTimeMS, true);
                _m_shopList.add(shop);
            }
        }
    }

    /**
     * 通过商店配置id 查找商店数据
     * @param _id 配置id
     * @return NPPlayerShopInfo
     */
    public PlayerShopInfo lookup(long _id)
    {
        _lock();
        try
        {
            for (PlayerShopInfo shop : _m_shopList)
            {
                if (shop == null)
                {
                    continue;
                }
                if (shop.getShopId() == _id)
                {
                    return shop;
                }
            }
        } finally
        {
            _unlock();
        }
        return null;
    }

    /**
     * 通过商店数据id 查找商店数据
     * @param _dbId 数据id
     * @return NPPlayerShopInfo
     */
    public PlayerShopInfo lookupByShopDbId(long _dbId)
    {
        _lock();
        try
        {
            for (PlayerShopInfo shop : _m_shopList)
            {
                if (shop == null)
                {
                    continue;
                }
                if (shop.getShopDbId() == _dbId)
                {
                    return shop;
                }
            }
        } finally
        {
            _unlock();
        }
        return null;
    }

    /**
     * 从数据库初始化商店
     * @param _action process回调
     */
    public void initShopFromDB(_ITALProcessAction<Boolean> _action)
    {
        getUSServer().getBM().getBM(PlayerShopBO.class).findAll("cid", getCid(), new _ASelectCallback<List<PlayerShopBO>>()
        {
            @Override
            public void dealSuc(List<PlayerShopBO> _boList)
            {
                for (PlayerShopBO bo : _boList)
                {
                    if (bo == null)
                    {
                        continue;
                    }
                    RefShop ref = RefShop.getMgr().get(bo.getShopRefId());
                    if (ref == null)
                    {
                        USLog.info(getUSServer(), "shop initShopFromDB get null ref :{}", bo.getShopRefId());
                        continue;
                    }

                    PlayerShopInfo shop = new PlayerShopInfo(PlayerShopComponent.this, ref, bo);
                    //首次刷新商店列表
                    _m_shopList.add(shop);
                }
                _action.dealAction(true);
            }

            @Override
            public void dealFail()
            {
                _action.dealAction(false);
            }
        });
    }

    /**
     * 从数据库初始化商店商品
     * @param _action process回调
     */
    private void initShopItemFromDB(_ITALProcessAction<Boolean> _action)
    {
        getUSServer().getBM().getBM(PlayerShopItemBO.class).findAll("cid", getCid(), new _ASelectCallback<List<PlayerShopItemBO>>()
        {
            @Override
            public void dealSuc(List<PlayerShopItemBO> _boList)
            {
                for (PlayerShopItemBO bo : _boList)
                {
                    RefShopItem refShopItem = RefShopItem.getMgr().get(bo.getShopItemRefId());
                    if (refShopItem == null)
                    {
                        USLog.error(getUSServer(), "shopComp initShopItemFromDB get null Ref cid:{} refShopItemId:{}",
                                getCid(), bo.getShopItemRefId());

                        continue;
                    }
                    PlayerShopInfo shop = lookupByShopDbId(bo.getShopDbId());
                    if (shop == null)
                    {
                        USLog.error(getUSServer(), "shopComp initShopItemFromDB shop not exist cid:{} shopDbId:{}",
                                getCid(), bo.getShopDbId());
                        continue;
                    }
                    shop.initShopItemFromDB(bo);
                }
                _action.dealAction(true);
            }

            @Override
            public void dealFail()
            {
                _action.dealAction(false);
            }
        });
    }

    /**
     * 从数据库初始化商店商品
     * @param _action process回调
     */
    private void initShopBuyRecordFromDB(_ITALProcessAction<Boolean> _action)
    {
        getUSServer().getBM().getBM(PlayerShopBuyRecordBO.class).findAll("cid", getCid(), new _ASelectCallback<List<PlayerShopBuyRecordBO>>()
        {
            @Override
            public void dealSuc(List<PlayerShopBuyRecordBO> _boList)
            {
                for (PlayerShopBuyRecordBO bo : _boList)
                {
                    PlayerShopInfo shop = lookupByShopDbId(bo.getShopDbId());
                    if (shop == null)
                    {
                        USLog.error(getUSServer(), "shopComp initShopBuyRecordFromDB shop not exist cid:{} shopDbId:{}",
                                getCid(), bo.getShopDbId());
                        continue;
                    }
                    shop.initShopBuyRecordFromDB(bo);
                }
                _action.dealAction(true);
            }

            @Override
            public void dealFail()
            {
                _action.dealAction(false);
            }
        });
    }
    
    /**
     * 从数据库初始化终身限购记录
     * @param _action process回调
     */
    private void initForeverBuyRecordFromDB(_ITALProcessAction<Boolean> _action)
    {
        getUSServer().getBM().getBM(PlayerForeverShopBuyRecordBO.class).findAll("cid", getCid(), new _ASelectCallback<List<PlayerForeverShopBuyRecordBO>>()
        {
            @Override
            public void dealSuc(List<PlayerForeverShopBuyRecordBO> _boList)
            {
                for (PlayerForeverShopBuyRecordBO bo : _boList)
                {
                    // 根据商店ID找到对应的商店进行初始化
                    PlayerShopInfo shop = lookup(bo.getShopRefId());
                    if (shop != null)
                    {
                        shop.initForeverBuyRecordFromDB(bo);
                    }
                    else
                    {
                        USLog.error(getUSServer(), "shopComp initForeverBuyRecordFromDB shop not found cid:{} shopRefId:{}",
                                getCid(), bo.getShopRefId());
                    }
                }
                _action.dealAction(true);
            }

            @Override
            public void dealFail()
            {
                _action.dealAction(false);
            }
        });
    }

    /**
     * 尝试更新所有商店
     * @param _nowTimeMS 当前时间
     */
    private void __tryRefreshAll(long _nowTimeMS)
    {
        _lock();
        try
        {
            for (PlayerShopInfo shop : _m_shopList)
            {
                if (shop == null)
                {
                    continue;
                }
                //刷新商店商品数据
                shop._tryRefresh(_nowTimeMS);
            }
        } finally
        {
            _unlock();
        }
    }


    /**
     * 客户端尝试刷新商店数据
     * @param _shopRefId 商店配置id
     */
    public void tryRefreshShop(long _shopRefId)
    {
        _lock();
        try
        {
            PlayerShopInfo shop = lookup(_shopRefId);
            if (shop == null)
            {
                return;
            }
            long nowTimeMS = CommonFunc.getNowTimeMS();
            shop._tryRefresh(nowTimeMS);
        } finally
        {
            _unlock();
        }
    }

    /**
     * 客户端尝试刷新商店数据
     * @param _shopRefId 商店配置id
     */
    public void refreshShop(long _shopRefId)
    {
        _lock();
        try
        {
            PlayerShopInfo shop = lookup(_shopRefId);
            if (shop == null)
            {
                return;
            }
            long nowTimeMS = CommonFunc.getNowTimeMS();
            shop._refreshShop(nowTimeMS, false);
        } finally
        {
            _unlock();
        }
    }

    /**
     * 客户端刷新商品
     * @param _shopRefId 商店id
     */
    public void refreshGoods(long _shopRefId)
    {
        _lock();
        try
        {
            PlayerShopInfo shop = lookup(_shopRefId);
            if (shop == null)
            {
                return;
            }
            shop._refreshGoods();
        } finally
        {
            _unlock();
        }
    }

    /**
     * 计算商品价格
     * @param _shopRefId  商店配置id
     * @param _goodsDBId  商品唯一id
     * @param _goodsCount 购买数量
     */
    public List<NPCommonCostItem> calCost(long _shopRefId, long _goodsDBId, int _goodsCount)
    {
        _lock();
        try
        {
            PlayerShopInfo shop = lookup(_shopRefId);
            if (shop == null)
                return null;

            long nowTimeMS = CommonFunc.getNowTimeMS();
            //先刷新商店数据
            shop._tryRefresh(nowTimeMS);

            //查询商店商品
            PlayerShopItem goods = shop.lookup(_goodsDBId);
            if (goods == null)
                return null;

            return goods.calBuyCost(_goodsCount);
        } finally
        {
            _unlock();
        }
    }

    /**
     * 购买商品
     * @param _shopRefId     商店配置id
     * @param _goodsDBId     商品唯一id
     * @param _goodsCount    购买数量
     * @param _goodsItemList 获得商品
     * @param _logInfoPair   日志所需信息
     * @return 是否购买成功
     */
    public boolean buyGoods(long _shopRefId, long _goodsDBId, int _goodsCount, ArrayList<NPCommonCostItem> _goodsItemList, WCGPair<Long, Long> _logInfoPair)
    {
        _lock();
        try
        {
            PlayerShopInfo shop = lookup(_shopRefId);
            if (shop == null)
                return false;

            if (!NPPlayerConditionDealerMgr.IsEnable(shop.getRef().unlock_cond, getUserData(), null))
                return false;

            long nowTimeMS = CommonFunc.getNowTimeMS();
            //先刷新商店数据
            shop._tryRefresh(nowTimeMS);
            //查询商店商品
            PlayerShopItem goods = shop.lookup(_goodsDBId);
            if (goods == null)
                return false;

            //日志所需信息
            if (_logInfoPair != null)
            {
                _logInfoPair.first = goods.getRef().id;//商品配置
                _logInfoPair.second = goods.getDiscount();//商品折扣
            }

            return goods._buy(_goodsCount, _goodsItemList);
        } finally
        {
            _unlock();
        }
    }

    /**
     * 构造初始化协议
     * @param _shopList 商店列表
     */
    public void makeProto(ArrayList<Shop_Info> _shopList)
    {
        for (PlayerShopInfo shop : _m_shopList)
        {
            if (shop == null)
            {
                continue;
            }
            _shopList.add(shop.makeProto());
        }
    }

    @Override
    protected void _init()
    {
        ALProcess process = ALProcess.CreateProcess();
        process.addResDelegateProcess(this::initShopFromDB, "initShopFromDB", null, false);
        process.addResDelegateProcess(this::initShopItemFromDB, "initShopItemFromDB", null, false);
        process.addResDelegateProcess(this::initShopBuyRecordFromDB, "initShopBuyRecordFromDB", null, false);
        process.addResDelegateProcess(this::initForeverBuyRecordFromDB, "initForeverBuyRecordFromDB", null, false);

        process.dealProcess(new _IEZProcessMonitor()
        {
            @Override
            public void onRootProecssStop()
            {
                getUserData().setDataLoadFail();
            }

            @Override
            public void onRootProecssSuc()
            {
                setInited();
            }
        });
    }

    @Override
    public ENPPlayerCompType[] getDependCompList()
    {
        return null;
    }

    @Override
    public void onInited()
    {
        //从配表初始化商店
        __initShopFromRef();
        long nowTimeMS = CommonFunc.getNowTimeMS();
        //尝试刷新所有商店的商品数据
        __tryRefreshAll(nowTimeMS);

        for (PlayerShopInfo shopInfo : _m_shopList)
        {
            shopInfo.regRefreshEvent();
        }
    }


    @Override
    public void dispose()
    {
        for (PlayerShopInfo shopInfo : _m_shopList)
        {
            shopInfo.unregRefreshEvent();
        }
    }

    @Override
    public String toString()
    {
        StringBuilder sb = new StringBuilder();
        sb.append("=========");
        for (PlayerShopInfo shop : _m_shopList)
        {
            sb.append("\n").append(shop);
        }
        return sb.toString();
    }

}

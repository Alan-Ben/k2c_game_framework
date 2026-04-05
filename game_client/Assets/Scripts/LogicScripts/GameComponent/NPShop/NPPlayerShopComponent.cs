using System.Collections.Generic;
using System;
using ALPackage;
using JetBrains.Annotations;
using GS2GC.p002_InitOp;
using Common.ShopObj;
using GS2GC.p030_ShopOp;

namespace GOE
{
    // 商店模块管理类
    public partial class NPPlayerShopComponent : _ANPBasicPlayerComponent
    {
        //商店列表
        [NotNull] private List<NPPlayerShop> _m_shopList;
        private GShopRemarkInfo _m_remarkInfo;

        private RedTipDealer _m_redDealer;
        //构造函数
        public NPPlayerShopComponent(NPPlayerComponentMgr _compMgr) : base(_compMgr)
        {
            _m_shopList = new List<NPPlayerShop>();
            _m_redDealer = new RedTipDealer(this);
            _m_remarkInfo = new GShopRemarkInfo();
        }

        #region override
        public override bool isMustInit { get { return true; } }
        public override ENPPlayerCompType compType { get { return ENPPlayerCompType.SHOP; } }
        public override ENPPlayerCompType[] dependCompList { get { return null; } }
        public GShopRemarkInfo remarkInfo { get { return _m_remarkInfo; } }
        #endregion


        #region override 方法
        /// <summary>
        /// 是否允许提前初始化。提前初始化的意思是在依赖项没有完成初始化之前就进行初始化操作（一般是提前发送消息）
        /// 在初始化结果消息返回的时候，通过特殊的初始化函数dealPreInitFunc进行处理函数注册，再依赖项完成之后才进行初始化处理
        /// </summary>
        public override bool canPreInit { get { return true; } }


        /// <summary>
        /// 发送初始化协议提前申请内容
        /// </summary>
        public override void presendInitProtocol()
        {
            _reqShopInitData();
        }

        protected override void _dealInit()
        {

        }

        //组件加载完成时的调用
        protected override void _onInitDone()
        {
        }

        //组件初始化失败的处理
        protected override void _onInitFail()
        {
            ALLog.Error("NPPlayerShopComponent init Fail!!!");
        }

        //释放资源函数
        protected override void _discard()
        {
            _m_shopList.Clear();
            _m_redDealer?.clear();
        }

        #endregion


        /// <summary>
        /// 根据id获取对应商店
        /// </summary>
        /// <param name="_shopId"></param>
        /// <returns></returns>
        public NPPlayerShop getShop(long _shopId)
        {
            NPPlayerShop temp = null;
            for (int i = 0; i < _m_shopList.Count; i++)
            {
                temp = _m_shopList[i];
                if (null == temp)
                    continue;
                if (temp.shopRefId == _shopId)
                    return temp;
            }
            return null;
        }

        #region S2C
        public void retShopInitData(GS2GC_002_051_RetShopInit _msg)
        {
            if (null == _msg)
                return;

            _m_shopList.Clear();
            if (null != _msg.getShopList())
            {
                Shop_Info temp = null;
                for (int i = 0; i < _msg.getShopList().Count; i++)
                {
                    temp = _msg.getShopList()[i];
                    if (null == temp)
                        continue;

                    NPPlayerShop shop = new NPPlayerShop(temp);
                    _m_shopList.Add(shop);
                }
            }
            _m_remarkInfo.sendRequest(() =>
            {
                //刷新红点
                _m_redDealer.init();
            });

            setInitDone();
        }

        /// <summary>
        /// 更新商店
        /// </summary>
        /// <param name="_info"></param>
        public void retOnShopRefresh(Shop_Info _info)
        {
            if (null == _info)
                return;

            NPPlayerShop shop = getShop(_info.getShopRefId());
            if (null == shop)
                return;

            shop.update(_info);
            setShopNextRefreshTimeMs(_info.getShopRefId(), _info.getNextRefreshTimeMs());
            _m_redDealer?.refrshRedTip(shop);
        }

        /// <summary>
        /// 更新商品
        /// </summary>
        /// <param name="_item"></param>
        public void retOnShopItemChg(long _shopId,Shop_ItemInfo _item)
        {
            if (null == _item)
                return;

            NPPlayerShop shop = getShop(_shopId);
            if (null == shop)
                return;

            shop.updateItem(_item);
        }

        #endregion

        #region C2S

        /// <summary>
        /// 商店组件初始化
        /// </summary>
        private void _reqShopInitData()
        {
            NPGSClientListener.sendMsgByLog(GSWriter_002_InitOp.make_002_051_ReqShopInit());
        }

        /// <summary>
        ///  购买商品
        /// </summary>
        /// <param name="_shopId">商店id</param>
        /// <param name="_instanceId">商品实例id</param>
        /// <param name="_count">商品数量</param>
        /// <param name="_backAction"></param>
        public void reqBuyShopItem(long _shopId, long _instanceId,int _count,Action _backAction = null)
        {
            NPGSClientListener.sendRequestByLog(GSWriter_030_ShopOp.make_001_ReqBuyShopItem(_shopId, _instanceId, _count),
                 new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_030_001_RetBuyShopItem>((info) =>
                 {
                     WinMsg.SendMsg(WinMsgType.SHOP_ITEM_CHG, _instanceId);
                     if (null != _backAction)
                         _backAction();
                 }));
        }

        /// <summary>
        /// 免费刷新
        /// </summary>
        public void reqFreeRefreshShop(long _shopId, Action _backAction = null)
        {
            NPGSClientListener.sendRequestByLog(GSWriter_030_ShopOp.make_002_ReqFreeRefreshShop(_shopId),
                 new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_030_002_RetFreeRefreshShop>((info) =>
                 {
                     if (null != _backAction)
                         _backAction();
                 }));
        }

        /// <summary>
        /// 付费刷新
        /// </summary>
        public void reqRefreshShop(long _shopId, Action _backAction = null)
        {
            NPGSClientListener.sendRequestByLog(GSWriter_030_ShopOp.make_003_ReqRefreshShop(_shopId),
                 new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_030_003_RetRefreshShop>((info) =>
                 {
                     if (null != _backAction)
                         _backAction();
                 }));
        }

        /// <summary>
        /// 到点自动刷新
        /// </summary>
        public void reqAutoRefreshShop(long _shopId, Action _backAction = null)
        {
            NPGSClientListener.sendRequestByLog(GSWriter_030_ShopOp.make_004_ReqAutoRefreshShop(_shopId),
                 new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_030_004_RetAutoRefreshShop>((info) =>
                 {
                     if (null != _backAction)
                         _backAction();
                 }));
        }

        #endregion

        /// <summary>
        /// 设置商店最后一次显示时间
        /// </summary>
        /// <param name="_shopRefId"></param>
        /// <param name="_showTimeS"></param>
        public void setShopShowTimeS(long _shopRefId, long _showTimeS)
        {
            remarkInfo?.setShopShowTimeS(_shopRefId, _showTimeS);
            _m_redDealer?.refrshRedTip(getShop(_shopRefId));
        }

        /// <summary>
        /// 设置商店下次刷新时间
        /// </summary>
        /// <param name="_shopRefId"></param>
        /// <param name="_showTimeS"></param>
        public void setShopNextRefreshTimeMs(long _shopRefId, long _showTimeS)
        {
            remarkInfo?.setShopNextRefreshTimeMs(_shopRefId, _showTimeS);
        }
    }
}

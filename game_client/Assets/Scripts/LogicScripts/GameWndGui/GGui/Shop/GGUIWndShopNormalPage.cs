using UnityEngine;
using ALPackage;

namespace GOE
{
    //普通商店界面
    public class GGUIWndShopNormalPage : _ANPGGUIBasicLoadPrefabSubWnd<GGUIMonoShopNormalPage>
    {
        //子页签列表
        private GGUIWndShopSubTabContainer _m_wSubTabContainer;
        //付费刷新消耗
        private NPGGUIWndCommonItem _m_costCommonItemWnd;
        //免费刷新点数恢复倒计时控件
        private NPGGUIWndCommonCountDown _m_freeCountDownWnd;
        //自动刷新倒计时控件
        private NPGGUIWndCommonCountDown _m_autoCountDownWnd;
        //容器
        private GGUIWndShopNormalPageGrid _m_wndListGrid;
        //窗口对象的资源 加载路径id
        private long _m_assetPathId;
        //商店配表id
        private long _m_shopRefId;
        //商店主表id
        private long _m_shopMainId;
        //刷新成功播放音效
        private long _m_soundResId;
        //是否需要切换页签时才刷新红点
        private bool _m_bNeedSwitchRefreshRedTip;

        public GGUIWndShopNormalPage(long _assetPathInfoId, Transform _parent) : base(_parent)
        {
            _m_assetPathId = _assetPathInfoId;
        }

        /// <summary>
        /// 商店主表ID
        /// </summary>
        public long shopMainRefId { get { return _m_shopMainId; } }

        /********************
       * 获取资源所在资源加载文件名称
       **/
        protected override string _monoAssetPath { get { return UIResPathAssistant.getAssetPath(_m_assetPathId); } }
        protected override string _monoObjName { get { return UIResPathAssistant.getObjName(_m_assetPathId); } }

        /**************
         * 获取用于加载资源的管理对象
         **/
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        #region override
        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.SHOP_CHG, _shopChgMsg);
            WinMsg.RegisterMsg(WinMsgType.SHOP_LIST_SCROLL_MOVE_TO_ITEM, _scrollMoveToItem);
        }

        protected override void _onHideWnd()
        {
            _m_wSubTabContainer?.hideWnd();
            _m_costCommonItemWnd?.hideWnd();
            _m_freeCountDownWnd?.hideWnd();
            _m_autoCountDownWnd?.hideWnd();
            _m_wndListGrid?.hideWnd();

            WinMsg.UnregisterMsg(WinMsgType.SHOP_CHG, _shopChgMsg);
            WinMsg.UnregisterMsg(WinMsgType.SHOP_LIST_SCROLL_MOVE_TO_ITEM, _scrollMoveToItem);
            //设置当前看过的时间
            NPPlayer.instance.shopComp.setShopShowTimeS(_m_shopRefId, FpsAndPingMgr.instance.serverTimeTagS);
            _m_bNeedSwitchRefreshRedTip = false;

            PlayAudioMgr.instance.stopClip(_m_soundResId);
        }

        protected override void _onReset()
        {
            _m_wSubTabContainer?.resetWnd();
            _m_costCommonItemWnd?.resetWnd();
            _m_freeCountDownWnd?.resetWnd();
            _m_autoCountDownWnd?.resetWnd();
            _m_wndListGrid?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_wSubTabContainer?.discard();
            _m_wSubTabContainer = null;

            _m_costCommonItemWnd?.discard();
            _m_costCommonItemWnd = null;

            _m_wndListGrid?.discard();
            _m_wndListGrid = null;

            _m_autoCountDownWnd?.discard();
            _m_autoCountDownWnd = null;

            _m_freeCountDownWnd?.discard();
            _m_freeCountDownWnd = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.freeRefreshBtn, _freeRefreshBtnDidClick);
            ALUGUICommon.uncombineBtnClick(wnd.costRefreshBtn, _costRefreshBtnDidClick);
        }

        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;

            if (null != wnd.monoSubTabContainer)
            {
                _m_wSubTabContainer = new GGUIWndShopSubTabContainer(wnd.monoSubTabContainer);
                _m_wSubTabContainer.onClickItem += _onClickTabItem;
            }

            if (null != wnd.costCommonItemMono)
                _m_costCommonItemWnd = new NPGGUIWndCommonItem(wnd.costCommonItemMono);

            if (null != wnd.shopGridMono)
                _m_wndListGrid = new GGUIWndShopNormalPageGrid(wnd.shopGridMono);

            if (null != wnd.autoCountDownMono)
                _m_autoCountDownWnd = new NPGGUIWndCommonCountDown(wnd.autoCountDownMono);

            if (null != wnd.freeCountDownMono)
                _m_freeCountDownWnd = new NPGGUIWndCommonCountDown(wnd.freeCountDownMono);

            ALUGUICommon.combineBtnClick(wnd.freeRefreshBtn, _freeRefreshBtnDidClick);
            ALUGUICommon.combineBtnClick(wnd.costRefreshBtn, _costRefreshBtnDidClick);
        }

        #endregion

        /// <summary>
        /// 设置商店主id
        /// </summary>
        /// <param name="_shopMainId"></param>
        public void setShopMainId(long _shopMainId, long _targetShopId)
        {
            _m_shopMainId = _shopMainId;
            ShopMainRefObj shopMainRef = GRefdataCoreMgr.instance.shopMainRefCore.getRef(_shopMainId);
            if (shopMainRef == null)
                return;

            _m_wSubTabContainer?.showWnd();
            _m_wSubTabContainer?.setInfo(shopMainRef.shop_id_list, _targetShopId);

            //如果只有一个页签，设置是否显示页签
            if(shopMainRef.shop_id_list != null && shopMainRef.shop_id_list.Count == 1 && wnd != null && !wnd.oneTabNeedShow)
                _m_wSubTabContainer?.hideWnd();
        }

        /// <summary>
        /// 根据下标获取商店item的RectTransform
        /// </summary>
        /// <param name="_index"></param>
        /// <returns></returns>
        public RectTransform getShopItemRectTransformByIndex(int _index)
        {
            if (_m_wndListGrid == null)
                return null;

            return _m_wndListGrid.getShopItemRectTransformByIndex(_index);
        }

        /// <summary>
        /// 设置商店id
        /// </summary>
        /// <param name="_shopId"></param>
        private void _setShopId(long _shopId)
        {
            _m_shopRefId = _shopId;
            _refresh();
        }

        private void _refresh()
        {
            if (wnd == null)
                return;

            NPPlayerShop shopInfo = NPPlayer.instance.shopComp.getShop(_m_shopRefId);
            if (null == shopInfo || shopInfo.shopRefObj == null)
                return;

            //设置当前看过的时间
            if (!_m_bNeedSwitchRefreshRedTip)
                NPPlayer.instance.shopComp.setShopShowTimeS(_m_shopRefId, FpsAndPingMgr.instance.serverTimeTagS);

            _checkNeedRefresh(shopInfo);

            NPCommonCostItem costItem = shopInfo.getRefreshCostItem();
            bool isHideCost = null == costItem || costItem.count == 0;
            ALUGUICommon.setGameObjEnable(wnd.costShowGoList, !isHideCost);

            //设置显隐
            long nextRefreshTimeMs = shopInfo.nextRefreshTimeMs;
            //如果没有刷新时间，获取活动时间来展示倒计时
            if (nextRefreshTimeMs <= 0 && shopInfo.shopRefObj.relate_activity_id_list != null)
            {
                for (int i = 0; i < shopInfo.shopRefObj.relate_activity_id_list.Count; i++)
                {
                    _ABaseActivityInfo activityInfo = NPPlayer.instance.commonActivityComp.getValidActivityInfoByActivityId(shopInfo.shopRefObj.relate_activity_id_list[i]);
                    if (activityInfo != null && activityInfo.isEnable)
                    {
                        nextRefreshTimeMs = activityInfo.endTimeMs;
                        break;
                    }
                }
            }
            //设置显隐
            bool isAuto = nextRefreshTimeMs > 0;
            ALUGUICommon.setGameObjEnable(wnd.autoShowGoList, isAuto);
            //自动刷新倒计时
            if (isAuto && null != _m_autoCountDownWnd)
            {
                long marginMs = nextRefreshTimeMs - FpsAndPingMgr.instance.serverTimeTag;
                _m_autoCountDownWnd.showWnd();
                _m_autoCountDownWnd.setInfo(TimeUtil.msToSecCeiling(marginMs), null, () =>
                {
                    _m_bNeedSwitchRefreshRedTip = true;
                    //有刷新时间才需要刷新商店
                    if(shopInfo.nextRefreshTimeMs > 0)
                        NPPlayer.instance.shopComp.reqAutoRefreshShop(_m_shopRefId, () =>
                        {
                            WinMsg.SendMsg(WinMsgType.SHOP_AUTO_REFRESH);
                        });
                });
            }

            _refreshFreeGo();

            //付费消耗
            if (!isHideCost && null != _m_costCommonItemWnd)
            {
                _m_costCommonItemWnd.setItem(costItem);
            }

            bool isHideFree = shopInfo.freeRefreshMaxCount == 0;
            if (isHideFree && isHideCost)
                ALUGUICommon.setGameObjEnable(wnd.autoNeedHideGoList, false);

            //商品
            if (null != _m_wndListGrid)
                _m_wndListGrid.refresh(shopInfo);

            //刷新页签红点
            _m_wSubTabContainer?.refrshRedTip();
        }

        //免费刷新点数相关显示
        private void _refreshFreeGo()
        {
            NPPlayerShop shop = NPPlayer.instance.shopComp.getShop(_m_shopRefId);
            if (null == wnd || null == shop)
                return;

            EProcessStat stat = EProcessStat.EMPTY;
            if (shop.freeRefreshCount > 0 && shop.freeRefreshCount < shop.freeRefreshMaxCount)
            {
                stat = EProcessStat.PART;
            }
            else if ((shop.freeRefreshCount > 0 && shop.freeRefreshCount >= shop.freeRefreshMaxCount))
            {
                stat = EProcessStat.FULL;
            }
            else if(shop.freeRefreshMaxCount == 0)
            {
                stat = EProcessStat.NONE;
            }
            NPCommonEnumStatInfo<EProcessStat>.setStat(wnd.freeProcessStatList,stat);

            //还没达到最大值，显示倒计时
            if (shop.getRemainMs() > 0 &&( stat == EProcessStat.PART || stat == EProcessStat.EMPTY) && null != _m_freeCountDownWnd)
            {
                _m_freeCountDownWnd.setInfo(shop.getRemainMs() / 1000, null, () =>
                {
                    _refreshFreeGo();
                });
            }

            ALUGUICommon.setLabelTxt(wnd.freeRefreshCount, TextTranslate.instance.getLanguage(TransKeyConst.shop_free_refresh_count, shop.freeRefreshCount, shop.freeRefreshMaxCount));
        }
        /// <summary>
        /// 检测是否到点需要刷新
        /// </summary>
        private void _checkNeedRefresh(NPPlayerShop _shop)
        {
            if (null == _shop)
                return;

            if (_shop.nextRefreshTimeMs > 0 && FpsAndPingMgr.instance.serverTimeTag >= _shop.nextRefreshTimeMs)
            {
                NPPlayer.instance.shopComp.reqAutoRefreshShop(_m_shopRefId, () =>
                {
                    WinMsg.SendMsg(WinMsgType.SHOP_AUTO_REFRESH);

                    NPPlayerShop shopInfo = NPPlayer.instance.shopComp.getShop(_m_shopRefId);
                    if (shopInfo == null)
                        return;

                    //获取下次刷新日期
                    int nextRefreshDate = TimeUtil.getTimeByYYYYMM(shopInfo.nextRefreshTimeMs);
                    //如果当前日期还没展示过提示弹窗，则弹窗提示商店已刷新
                    if (AccountSettingMgr.instance.accountSetting.getShopAutoRefreshPopWndDate() != nextRefreshDate)
                    {
                        AccountSettingMgr.instance.accountSetting.setShopAutoRefreshPopWndDate(nextRefreshDate);
                        NPMesMgr.instance.showOneBtnMes(TextTranslate.instance.getLanguage(TransKeyConst.shop_allAutoRefresh_none), 
                            TextTranslate.instance.getLanguage(TransKeyConst.confirm), null);
                    }
                });
            }
        }

        /// <summary>
        /// 刷新成功播放音效
        /// </summary>
        private void _playRefreshSucSound()
        {
            if (null == wnd)
                return;

            _m_soundResId = PlayAudioMgr.instance.playClip(wnd.refreshSucSoundId);
        }

        /// <summary>
        /// 商店变动
        /// </summary>
        private void _shopChgMsg(params object[] _objs)
        {
            if (null == wnd || null == _objs || _objs.Length == 0)
                return;

            long shopRefId = (long)_objs[0];
            if (shopRefId != _m_shopRefId)
                return;

            _refresh();
        }

        /// <summary>
        /// 滚动列表移动到指定item
        /// </summary>
        /// <param name="_objs"></param>
        private void _scrollMoveToItem(params object[] _objs)
        {
            if (_objs == null || _objs.Length < 1 || _objs[0] == null || _m_wndListGrid == null)
                return;

            long shopItemId = (long)_objs[0];
            float moveTime = 0f;
            if (_objs.Length >= 2 && _objs[1] != null)
            {
                if (_objs[1] is float)
                    moveTime = (float)_objs[1];
                else if (_objs[1] is string)
                    moveTime = ALCommon.ParseFloat((string)_objs[1]);
            }
            _m_wndListGrid.scrollMoveToTargetShopItem(shopItemId, moveTime);
        }

        /// <summary>
        /// 点击免费刷新
        /// </summary>
        /// <param name="_go"></param>
        private void _freeRefreshBtnDidClick(GameObject _go)
        {
            NPPlayerShop shop = NPPlayer.instance.shopComp.getShop(_m_shopRefId);
            if (null == shop)
                return;
            //免费刷新弹窗
            if (shop.freeRefreshCount > 0)
            {
                //未保存，二次确认
                bool needConfirm = AccountSettingMgr.instance.warningTipSaver.needShowWarningTip(ENPWarningType.SHOP_FREE_REFRESH_CONFIRM);
                if (needConfirm)
                {
                    NPMesMgr.instance.showTwoBtnTogMes(
                        (_toggle) =>
                        {
                            NPPlayer.instance.shopComp.reqFreeRefreshShop(_m_shopRefId, _playRefreshSucSound);
                            if (_toggle)
                            {
                                AccountSettingMgr.instance.warningTipSaver.setTodayIgnoreWarningTip(ENPWarningType.SHOP_FREE_REFRESH_CONFIRM);
                            }
                        },null,
                        TransKeyConst.shop_freeRefresh_title_none,
                        TextTranslate.instance.getLanguage(TransKeyConst.shop_freeRefresh_desc_num, shop.freeRefreshCount));
                }
                else
                {
                    //直接刷新
                    NPPlayer.instance.shopComp.reqFreeRefreshShop(_m_shopRefId, _playRefreshSucSound);
                }
            }
            else
            {
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.shop_freeRefreshIsMax_none);
            }
        }

        /// <summary>
        /// 点击付费刷新
        /// </summary>
        /// <param name="_go"></param>
        private void _costRefreshBtnDidClick(GameObject _go)
        {
            NPPlayerShop shop = NPPlayer.instance.shopComp.getShop(_m_shopRefId);
            if (null == shop)
                return;

            //先判断消耗
            NPCommonCostItem costItem = shop.getRefreshCostItem();
            if (!GCommon.isItemEnough(costItem, true))
                return;

            //付费刷新弹窗
            if (shop.refreshCount < shop.refreshMaxCount)
            {
                //未保存，二次确认
                bool needConfirm = AccountSettingMgr.instance.warningTipSaver.needShowWarningTip(ENPWarningType.SHOP_COST_REFRESH_CONFIRM);
                if (needConfirm)
                {
                    //剩余付费刷新次数
                    int lastCount = shop.refreshMaxCount - shop.refreshCount;
                    //刷新消耗
                    string costItemName = GCommon.getItemName(costItem.item.itemType, costItem.item.itemId);
                    NPMesMgr.instance.showCostItemTogMes(costItem, (_toggle) =>
                        {
                            _reqRefreshShop(shop);

                            //记录今日不再提醒
                            if (_toggle)
                            {
                                AccountSettingMgr.instance.warningTipSaver.setTodayIgnoreWarningTip(ENPWarningType.SHOP_COST_REFRESH_CONFIRM);
                            }

                        },null,
                        TransKeyConst.shop_costRefresh_title_none,
                        TextTranslate.instance.getLanguage(TransKeyConst.shop_costRefresh_desc_num_str_num, costItem.count, costItemName, lastCount));
                }
                else
                {
                    _reqRefreshShop(shop);
                }
            }
            //付费刷新达到上限
            else
            {
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.shop_costRefreshIsMax_none);
            }
        }

        //点击页签
        private void _onClickTabItem(GGUIWndShopSubTabContainerItem _item)
        {
            if (_item == null || _item.shopRef == null)
                return;

            //如果当前页签是需要切换才刷新红点的，这里刷新一次红点
            if (_m_bNeedSwitchRefreshRedTip)
                NPPlayer.instance.shopComp.setShopShowTimeS(_m_shopRefId, FpsAndPingMgr.instance.serverTimeTagS);
            _m_bNeedSwitchRefreshRedTip = false;

            //切换商店
            _setShopId(_item.shopRef.id);
        }

        //请求刷新商店
        private void _reqRefreshShop(NPPlayerShop _shop)
        {
            NPCommonCostItem costItem = _shop.getRefreshCostItem();

            //玩家拥有的数量不足
            if (!GCommon.isItemEnough(costItem, false))
            {
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.shop_refresh_noEnough_none);
            }
            else
            {
                NPPlayer.instance.shopComp.reqRefreshShop(_m_shopRefId, () =>
                {
                    _playRefreshSucSound();
                    NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.shop_costRefreshSuc_none);

                });
            }
        }
    }
}

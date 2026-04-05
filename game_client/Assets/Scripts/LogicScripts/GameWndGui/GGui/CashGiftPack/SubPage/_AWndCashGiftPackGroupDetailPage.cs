using UnityEngine;
using ALPackage;
using Common.ActivityEnum;

namespace GOE
{
    /// <summary>
    /// 礼包组详情页面-基础
    /// </summary>
    public abstract class _AWndCashGiftPackGroupDetailPage<T> : _ATALBasicLoadPrefabSubUIWnd<T> where T : GGUIMonoCashGiftPackGroupDetailPage
    {
        //资源id
        private long _m_lUIResId;
        //横幅图片
        private NPGGuiWndTexture _m_wGiftPackBanner;
        //定时任务
        private ALCommonEnableTaskController _m_iTickTask;
        //礼包组数据
        protected GiftPackGroupRefObj _m_giftPackGroupRefObj;
        //礼包组标题
        private GGUIWndSubCashGiftPackGroupTitle _m_wGiftPackGroupTitle;

        public _AWndCashGiftPackGroupDetailPage(long _uiResId, Transform _parent) : base(_parent)
        {
            _m_lUIResId = _uiResId;
        }

        protected override string _monoAssetPath { get { return UIResPathAssistant.getAssetPath(_m_lUIResId); } }
        protected override string _monoObjName { get { return UIResPathAssistant.getObjName(_m_lUIResId); } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected sealed override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.ON_GIFT_PACK_ADD, _onGiftPackChg);
            WinMsg.RegisterMsg(WinMsgType.ON_GIFT_PACK_CHG, _onGiftPackChg);
            WinMsg.RegisterMsg(WinMsgType.ON_GIFT_PACK_REMOVE, _onGiftPackChg);
            WinMsg.RegisterMsg(WinMsgType.ON_GIFT_PACK_LIST_REFRESH, _onGiftPackListRefresh);
            _onShowWndEx();
        }

        protected sealed override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_GIFT_PACK_ADD, _onGiftPackChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_GIFT_PACK_CHG, _onGiftPackChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_GIFT_PACK_REMOVE, _onGiftPackChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_GIFT_PACK_LIST_REFRESH, _onGiftPackListRefresh);
            _m_wGiftPackBanner?.hideWnd();
            _m_iTickTask.setDisable();
            if (_m_giftPackGroupRefObj != null && _m_giftPackGroupRefObj.title_ui_res_id > 0)
                GCashGiftPackGroupTitleCacheMgr.instance.pushBackCacheItem(_m_giftPackGroupRefObj.title_ui_res_id, _m_wGiftPackGroupTitle);
            _m_wGiftPackGroupTitle = null;
            _onHideWndEx();
        }

        protected sealed override void _onReset()
        {
            _m_wGiftPackBanner?.discardTexture();
            _onResetEx();
        }

        protected sealed override void _onDiscard()
        {
            _m_wGiftPackBanner?.discard();
            _m_wGiftPackBanner = null;
            _onDiscardEx();
        }

        protected sealed override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.imgBanner != null)
                _m_wGiftPackBanner = new NPGGuiWndTexture(wnd.imgBanner);

            _onWndInitDoneEx();
        }

        /// <summary>
        /// 设置礼包组数据
        /// </summary>
        /// <param name="_giftPackGroupRef"></param>
        public void setInfo(GiftPackGroupRefObj _giftPackGroupRef, bool _goodsContainerMoveToTop)
        {
            _m_giftPackGroupRefObj = _giftPackGroupRef;
            _m_iTickTask.setDisable();
            _m_iTickTask = ALCommonTaskController.CommonEnableDurationActionAddMonoTask(_tick, 0.2f);
            _refreshWnd();
            if (_goodsContainerMoveToTop)
                _dealGoodsContainerMoveToTop();
        }

        //刷新窗口
        private void _refreshWnd()
        {
            if (wnd == null || _m_giftPackGroupRefObj == null)
                return;

            //礼包名称
            long titleUiResId = _m_giftPackGroupRefObj.title_ui_res_id;
            if (titleUiResId > 0 && wnd.giftPackNameParent != null)
            {
                GCashGiftPackGroupTitleCacheMgr.instance.popItem(titleUiResId, wnd.giftPackNameParent,
                    _item =>
                    {
                        if (!isShow || wnd == null)
                        {
                            GCashGiftPackGroupTitleCacheMgr.instance.pushBackCacheItem(titleUiResId, _item);
                            return;
                        }
                        _item?.showWnd();
                        _item?.setInfo(_m_giftPackGroupRefObj.name);
                        _m_wGiftPackGroupTitle = _item;
                    });
            }

            //横幅图片
            _m_wGiftPackBanner?.showWnd();
            _m_wGiftPackBanner?.setTexture(_m_giftPackGroupRefObj.tex_banner);

            //刷新描述
            ALUGUICommon.setLabelTxt(wnd.txtRefreshDesc, TextTranslate.instance.getLanguage(_m_giftPackGroupRefObj.refresh_desc));

            //刷新子类
            _refreshWndEx();
        }

        //获取剩余时间（秒）
        private long _getLeftTimeMs()
        {
            if(_m_giftPackGroupRefObj == null)
                return -1;

            //获取第一个礼包
            GiftPackRefObj giftPackRefObj = null;
            if(_m_giftPackGroupRefObj.gift_pack_id_list != null && _m_giftPackGroupRefObj.gift_pack_id_list.Count > 0)
                giftPackRefObj = GRefdataCoreMgr.instance.giftPackRefCore.getRef(_m_giftPackGroupRefObj.gift_pack_id_list[0]);

            //根据类型计算剩余时间
            switch (_m_giftPackGroupRefObj.show_type)
            {
                // ============ 活动类型 ============
                case EGiftPackGroupShowType.ACTIVITY:
                case EGiftPackGroupShowType.RANK_RUSH:
                    //获取活动剩余时间
                    _ABaseActivityInfo activityInfo = NPPlayer.instance.commonActivityComp.getValidActivityInfoByActivityId(_m_giftPackGroupRefObj.activity_id);
                    if (activityInfo == null || !activityInfo.isPlaying)
                        return 0;
                    else
                        return activityInfo.endTimeMs - FpsAndPingMgr.instance.serverTimeTag;

                // ============ 每日类型、每周类型 ============
                case EGiftPackGroupShowType.DAILY:
                case EGiftPackGroupShowType.WEEKLY:
                case EGiftPackGroupShowType.MARS_DAILY:
                case EGiftPackGroupShowType.MARS_WEEKLY:
                    //每日类型和每周类型的礼包，直接获取第一个礼包的刷新时间
                    if (giftPackRefObj == null)
                        return 0;

                    return giftPackRefObj.buy_limit_refresh_time.getNextRefreshTimeTagMs() - FpsAndPingMgr.instance.serverTimeTag;

                // ============ 联盟类型 ============
                case EGiftPackGroupShowType.GUILD:
                    break;
                default:
                    return -1;
            }
            return -1;
        }

        //每秒倒计时
        private void _tick()
        {
            if (wnd == null)
                return;

            long leftTimeMs = _getLeftTimeMs();
            if (leftTimeMs < 0)
                ALUGUICommon.setLabelTxt(wnd.txtLeftTime, "");
            else
                ALUGUICommon.setLabelTxt(wnd.txtLeftTime, TimeUtil.millisecondsToTime_hms(leftTimeMs, TransKeyConst.giftPack_leftTime_num_num_num));
        }


        protected abstract void _onShowWndEx();
        protected abstract void _onHideWndEx();
        protected abstract void _onResetEx();
        protected abstract void _onDiscardEx();
        protected abstract void _onWndInitDoneEx();
        /// <summary>
        /// 消息通知礼包变化
        /// </summary>
        /// <param name="_objects"></param>
        protected abstract void _onGiftPackChg(params object[] _objects);
        /// <summary>
        /// 消息通知礼包列表刷新
        /// </summary>
        /// <param name="_objects"></param>
        protected abstract void _onGiftPackListRefresh(params object[] _objects);
        /// <summary>
        /// 刷新窗口
        /// </summary>
        protected virtual void _refreshWndEx(){}
        /// <summary>
        /// 商品列表移动到顶部
        /// </summary>
        protected virtual void _dealGoodsContainerMoveToTop(){}
    }
}

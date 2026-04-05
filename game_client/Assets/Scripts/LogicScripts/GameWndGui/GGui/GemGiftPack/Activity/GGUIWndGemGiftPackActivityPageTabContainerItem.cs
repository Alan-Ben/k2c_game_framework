using ALPackage;
using System;

namespace GOE
{
    /// <summary>
    /// 钻石礼包页签列表item
    /// </summary>
    public class GGUIWndGemGiftPackActivityPageTabContainerItem : _ATALBasicUISubWnd<GGUIMonoGemGiftPackActivityPageTabContainerItem>, _IContainerSideRedTipItemInfo
    {
        //活动信息
        private _ABaseActivityInfo _m_activityInfo;
        //页签按钮
        private NPGGUIWndCommonTab _m_wTab;
        //点击item事件
        private Action<GGUIWndGemGiftPackActivityPageTabContainerItem, bool> _m_aOnClickItem;

        /// <summary>
        /// 活动信息
        /// </summary>
        public _ABaseActivityInfo activityInfo { get { return _m_activityInfo; } }

        /// <summary>
        /// 点击item事件
        /// </summary>
        public Action<GGUIWndGemGiftPackActivityPageTabContainerItem, bool> onClickItem
        {
            get { return _m_aOnClickItem; }
            set { _m_aOnClickItem = value; }
        }

        /// <summary>
        /// 是否有免费红点
        /// </summary>
        public bool haveRedTip
        {
            get
            {
                return _m_activityInfo != null && 
                       _m_activityInfo.crystalGiftPackInfo != null && 
                       _m_activityInfo.crystalGiftPackInfo.haveFreeGiftPackCanBuy();
            }
        }

        public GGUIWndGemGiftPackActivityPageTabContainerItem(GGUIMonoGemGiftPackActivityPageTabContainerItem _mono) : base(_mono)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.ON_ACTIVITY_CRYSTAL_GIFT_PACK_BUY_RECORD_CHG, _onGiftPackBugRecordChg);
            WinMsg.RegisterMsg(WinMsgType.ON_ACTIVITY_CRYSTAL_GIFT_PACK_REFRESH, _onGiftPackRefresh);
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_ACTIVITY_CRYSTAL_GIFT_PACK_BUY_RECORD_CHG, _onGiftPackBugRecordChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_ACTIVITY_CRYSTAL_GIFT_PACK_REFRESH, _onGiftPackRefresh);
        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
            _m_wTab?.discard();
            _m_wTab = null;
            _m_aOnClickItem = null;
            _m_activityInfo = null;
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoTab != null)
            {
                _m_wTab = new NPGGUIWndCommonTab(wnd.monoTab);
                _m_wTab.clickDelegate += _onClickTab;
            }
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        public void setInfo(_ABaseActivityInfo _activityInfo, bool _isFirst)
        {
            if (wnd == null || _activityInfo == null)
                return;

            _m_activityInfo = _activityInfo;
            ALUGUICommon.setLabelTxt(wnd.txtTabName, TextTranslate.instance.getLanguage(_m_activityInfo.crystalGiftPackInfo?.giftPackGroupRef?.tab_name));
            ALUGUICommon.setLabelTxt(wnd.txtTabName2, TextTranslate.instance.getLanguage(_m_activityInfo.crystalGiftPackInfo?.giftPackGroupRef?.tab_name));
            ALUGUICommon.setGameObjEnable(wnd.goFirstHideList, !_isFirst);
            ALUGUICommon.setGameObjEnable(wnd.goFirstShowList, _isFirst);
            _refreshRedTip();
        }

        //刷新红点
        private void _refreshRedTip()
        {

            if (wnd == null || _m_activityInfo == null || _m_activityInfo.crystalGiftPackInfo == null)
                return;

            ALUGUICommon.setGameObjEnable(wnd.goRedTip, _m_activityInfo.crystalGiftPackInfo.haveFreeGiftPackCanBuy());
        }

        /// <summary>
        /// 设置选中状态
        /// </summary>
        /// <param name="_isSelect"></param>
        public void setSelect(bool _isSelect)
        {
            _m_wTab?.setSelected(_isSelect);
        }

        //点击页签
        private void _onClickTab(bool _isSelect)
        {
            _m_aOnClickItem?.Invoke(this, true);
        }

        //礼包购买记录变化
        private void _onGiftPackBugRecordChg(params object[] _objects)
        {
            if (_objects == null || _objects.Length < 3 || _m_activityInfo == null || _m_activityInfo.crystalGiftPackInfo == null)
                return;

            long instanceId = (long) _objects[0];
            long giftPackGroupId = (long) _objects[1];

            if (instanceId != _m_activityInfo.instanceId || giftPackGroupId != _m_activityInfo.crystalGiftPackInfo.giftPackGroupId)
                return;

            //刷新红点
            _refreshRedTip();
        }

        //礼包刷新
        private void _onGiftPackRefresh(params object[] _objects)
        {
            if (_objects == null || _objects.Length < 1 || _m_activityInfo == null || _m_activityInfo.crystalGiftPackInfo == null)
                return;

            long instanceId = (long)_objects[0];
            if(_m_activityInfo.instanceId == instanceId)
                _refreshRedTip();
        }
    }
}
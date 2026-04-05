using System;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 活动中心页签列表item
    /// </summary>
    public class GGUIWndActivityCenterTabContainerItem : _ATALBasicUISubWnd<GGUIMonoActivityCenterTabContainerItem>, _IContainerSideRedTipItemInfo
    {
        //配置数据
        private ActivityCenterRefObj _m_activityCenterRef;
        //页签图标
        private NPGGuiWndTexture _m_wIcon;
        //页签
        private NPGGUIWndCommonTab _m_wTab;
        //点击item事件
        private Action<GGUIWndActivityCenterTabContainerItem> _m_aOnClickItem;

        /// <summary>
        /// 配置数据
        /// </summary>
        public ActivityCenterRefObj activityCenterRef { get { return _m_activityCenterRef; } }
        /// <summary>
        /// 点击item事件
        /// </summary>
        public Action<GGUIWndActivityCenterTabContainerItem> onClickItem { get { return _m_aOnClickItem; } set { _m_aOnClickItem = value; } }
        /// <summary>
        /// 是否有红点提示
        /// </summary>
        public bool haveRedTip
        {
            get
            {
                if (_m_activityCenterRef != null && _m_activityCenterRef.red_tip_id > 0)
                {

                    _ARedTipNode redTipNode = RedTipMgr.instance.getNodeByRefRedTipId(_m_activityCenterRef.red_tip_id);
                    return redTipNode != null && redTipNode.needShow();
                }
                return false;
            }
        }

        public GGUIWndActivityCenterTabContainerItem(GGUIMonoActivityCenterTabContainerItem _mono) : base(_mono)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.ON_RED_TIP_CHANGE, _onRedTipChg);
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_RED_TIP_CHANGE, _onRedTipChg);
            _m_wIcon?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wIcon?.discardTexture();
        }

        protected override void _onDiscard()
        {
            _m_wIcon?.discard();
            _m_wIcon = null;

            _m_wTab?.discard();
            _m_wTab = null;
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.imgIcon != null)
                _m_wIcon = new NPGGuiWndTexture(wnd.imgIcon);

            if (wnd.monoTab != null)
            {
                _m_wTab = new NPGGUIWndCommonTab(wnd.monoTab);
                _m_wTab.clickDelegate += _onClickItem;
            }
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_refObj"></param>
        public void setInfo(ActivityCenterRefObj _refObj)
        {
            if (wnd == null)
                return;

            _m_activityCenterRef = _refObj;
            _refreshWnd();
            setSelect(false);
        }

        /// <summary>
        /// 设置是否选中
        /// </summary>
        /// <param name="_isSelect"></param>
        public void setSelect(bool _isSelect)
        {
            _m_wTab?.setSelected(_isSelect);
        }

        //刷新窗口
        private void _refreshWnd()
        {
            if (wnd == null || _m_activityCenterRef == null)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtTabName, TextTranslate.instance.getLanguage(_m_activityCenterRef.name));
            ALUGUICommon.setLabelTxt(wnd.txtTabName2, TextTranslate.instance.getLanguage(_m_activityCenterRef.name));

            _m_wIcon?.showWnd();
            _m_wIcon?.setTexture(_m_activityCenterRef.icon);
            
            _refreshRedTip();
        }

        //刷新红点
        private void _refreshRedTip()
        {
            if (wnd == null || _m_activityCenterRef == null)
                return;

            _ARedTipNode nodeItem = null;
            if(_m_activityCenterRef.red_tip_id > 0)
                nodeItem = RedTipMgr.instance.getNodeByRefRedTipId(_m_activityCenterRef.red_tip_id);
            _m_wTab?.showRedTipNum(nodeItem != null && nodeItem.needShow() ? (int)nodeItem.getCount() : 0);
        }

        //点击item事件
        private void _onClickItem(bool _isSelect)
        {
            _m_aOnClickItem?.Invoke(this);
        }

        //红点变更
        private void _onRedTipChg(params object[] _objects)
        {
            _refreshRedTip();
        }
    }
}
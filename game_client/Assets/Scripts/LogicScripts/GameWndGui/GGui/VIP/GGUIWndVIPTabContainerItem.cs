using ALPackage;
using NPEnum;
using System;
using CommonEnum;

namespace GOE
{
    /// <summary>
    /// VIP页签列表item
    /// </summary>
    public class GGUIWndVIPTabContainerItem : _ATALBasicUISubWnd<GGUIMonoVIPTabContainerItem>
    {
        //VIP配置
        private VipRefObj _m_vipRef;
        //页签图标
        private NPGGuiWndTexture _m_wIcon;
        //页签背景
        private NPGGuiWndTexture _m_wBg;
        //页签
        private NPGGUIWndCommonTab _m_wTab;
        //点击item事件
        private Action<GGUIWndVIPTabContainerItem> _m_aOnClickItem;

        /// <summary>
        /// VIP配置
        /// </summary>
        public VipRefObj vipRef { get { return _m_vipRef; } }
        /// <summary>
        /// 点击item事件
        /// </summary>
        public Action<GGUIWndVIPTabContainerItem> onClickItem { get { return _m_aOnClickItem; } set { _m_aOnClickItem = value; } }

        public GGUIWndVIPTabContainerItem(GGUIMonoVIPTabContainerItem _mono) : base(_mono)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.ON_PLAYER_PARAM_CHANGE, _onParamChg);
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_PLAYER_PARAM_CHANGE, _onParamChg);
            _m_wIcon?.hideWnd();
            _m_wBg?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wIcon?.discardTexture();
            _m_wBg?.discardTexture();
        }

        protected override void _onDiscard()
        {
            _m_wIcon?.discard();
            _m_wIcon = null;

            _m_wBg?.discard();
            _m_wBg = null;

            _m_wTab?.discard();
            _m_wTab = null;
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.imgIcon != null)
                _m_wIcon = new NPGGuiWndTexture(wnd.imgIcon);

            if(wnd.imgBg != null)
                _m_wBg = new NPGGuiWndTexture(wnd.imgBg);

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
        public void setInfo(VipRefObj _refObj)
        {
            if (wnd == null)
                return;

            _m_vipRef = _refObj;
            _refreshWnd();
            setSelect(false);
        }

        /// <summary>
        /// 设置是否选中
        /// </summary>
        /// <param name="_isSelect"></param>
        public void setSelect(bool _isSelect)
        {
            if (wnd == null || _m_vipRef == null)
                return;

            _m_wTab?.setSelected(_isSelect);

            //VIP等级
            string vipLevelStr = TextTranslate.instance.getLanguage(TransKeyConst.vip_tabVIPLevel_num, _m_vipRef.vip_lvl);
            vipLevelStr = GCommon.addColorForRichText(vipLevelStr, (_isSelect ? wnd.selectTextColor : wnd.unSelectTextColor));
            ALUGUICommon.setLabelTxt(wnd.txtVIPLevel, vipLevelStr);
        }

        //刷新窗口
        private void _refreshWnd()
        {
            if (wnd == null || _m_vipRef == null)
                return;

            //默认获取第一个特殊奖励
            NPCommonItem actorItem = null;
            if(_m_vipRef.show_special_reward_list != null && _m_vipRef.show_special_reward_list.Count > 0)
                actorItem = _m_vipRef.show_special_reward_list[0];

            //设置图标
            if (actorItem != null)
            {
                _m_wIcon?.showWnd();
                _m_wIcon?.setTexture(GCommon.getItemTexIcon(actorItem.itemType, actorItem.itemId));

                NPQualityExtRefObj qualityExtRefObj = GCommon.getQualityExtRefObj(actorItem.itemType, actorItem.itemId);
                _m_wBg?.showWnd();
                _m_wBg?.setTexture(qualityExtRefObj?.vip_tab_bg);
            }

            //刷新红点
            _refreshRedTip();
        }

        //刷新红点
        private void _refreshRedTip()
        {
            int redTipCount = 0;
            long vipLevel = NPPlayer.instance.playerInfo.getValue(ENPPlayerParam.VIP_LVL);
            long vipLExp = GCommon.getItemCount(ENPItemType.CURRENCY, (long) ECurrency.VIP_EXP);
            if(_m_vipRef != null && vipLevel >= _m_vipRef.vip_lvl && vipLExp >= _m_vipRef.vip_exp && !NPPlayer.instance.playerInfo.isGetVipReward((int) _m_vipRef.vip_lvl))
                redTipCount = 1;
            _m_wTab?.showRedTipNum(redTipCount);
        }

        //点击item事件
        private void _onClickItem(bool _isSelect)
        {
            _m_aOnClickItem?.Invoke(this);
        }

        //玩家参数变化
        private void _onParamChg(params object[] _objects)
        {
            if (_objects == null || _objects.Length == 0)
                return;

            int paramIndex = (int)_objects[0];
            if((ENPPlayerParam)paramIndex == ENPPlayerParam.HAD_DRAW_VIP_REWARD_LIST)
                _refreshRedTip();
        }
    }
}
using ALPackage;
using System;

namespace GOE
{

    /// <summary>
    /// 伙伴列表item
    /// </summary>
    public class GGUIWndHeroListGridItem : _ANPGGUIBasicGridItemWnd<GGUIMonoHeroListGridItem>
    {
        //伙伴通用卡牌展示子窗口
        private GGUIWndHeroCommonCardItem _m_wndCommonCard;
        //点击回调
        private Action<GGUIWndHeroListGridItem> _m_dClickDelegate;
        //数据信息
        private HeroCardShowInfo _m_heroShowInfo;
        //检查是否是特殊展示红点的伙伴方法
        private Func<long, bool> _m_checkIsSpecialHeroFunc;

        public GGUIWndHeroListGridItem(GGUIMonoHeroListGridItem _wnd, Func<long, bool> _checkIsSpecialHeroFunc)
            : base(_wnd)
        {
            initWnd();
            _m_checkIsSpecialHeroFunc = _checkIsSpecialHeroFunc;
        }

        public Action<GGUIWndHeroListGridItem> clickDelegate { get { return _m_dClickDelegate; } set { _m_dClickDelegate = value; } }

        /// <summary>
        /// 卡牌数据信息
        /// </summary>
        public HeroCardShowInfo heroShowInfo { get { return _m_heroShowInfo; } }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
        }

        protected override void _onReset()
        {
            if (null != _m_wndCommonCard)
                _m_wndCommonCard.resetWnd();
        }

        protected override void _resetGridItem()
        {
            if (null != _m_wndCommonCard)
                _m_wndCommonCard.resetWnd();
        }

        protected override void _onDiscard()
        {
            if (null != _m_wndCommonCard)
                _m_wndCommonCard.discard();
            _m_wndCommonCard = null;

            _m_dClickDelegate = null;
        }

        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;

            if(wnd.monoCardItem != null)
            {
                _m_wndCommonCard = new GGUIWndHeroCommonCardItem(wnd.monoCardItem);
                _m_wndCommonCard.ClickAction += _onClickItem;
            }
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        public void setInfo(HeroCardShowInfo _heroShowInfo)
        {
            if (null == _heroShowInfo)
                return;

            _m_heroShowInfo = _heroShowInfo;

            //刷新卡牌显示
            if (null != _m_wndCommonCard)
            {
                _m_wndCommonCard.showWnd();
                _m_wndCommonCard.setInfo(_m_heroShowInfo);
            }

            //刷新红点展示
            _refreshRedTip();
        }

        //刷新红点展示
        private void _refreshRedTip()
        {
            if (wnd == null || wnd.redTipList == null || _m_heroShowInfo == null)
                return;

            bool hasShowRedTip = false;//是否已经显示了红点，红点互斥，显示第一个
            for (int i = 0; i < wnd.redTipList.Count; i++)
            {
                HeroCardRedTipParam temp = wnd.redTipList[i];
                if (temp == null)
                    continue;

                //已经显示了其他红点
                if (hasShowRedTip)
                {
                    ALUGUICommon.setGameObjEnable(temp.goShowList, false);
                    continue;
                }

                //判断条件满足
                //特殊伙伴所有红点都展示
                //其他伙伴只展示新获取红点
                if (_m_heroShowInfo.heroInfo != null &&
                    ((_m_checkIsSpecialHeroFunc != null && _m_checkIsSpecialHeroFunc(_m_heroShowInfo.id)) ||
                     temp.redId == RedTipConst.RED_HERO_FIRST_GET))
                    hasShowRedTip = NPPlayer.instance.heroComponent.needShowRedTip(temp.redId, _m_heroShowInfo.id);
                ALUGUICommon.setGameObjEnable(temp.goShowList, hasShowRedTip);
            }
        }

        //点击item
        private void _onClickItem(GGUIWndHeroCommonCardItem _commonCard)
        {
            if (null != _m_dClickDelegate)
                _m_dClickDelegate(this);
        }
    }
}

using ALPackage;
using System;

namespace GOE
{
    /// <summary>
    /// 竞技场选择伙伴列表item
    /// </summary>
    public class GGUIWndArenaBattleSelectHeroGridItem : _ANPGGUIBasicGridItemWnd<GGUIMonoArenaBattleSelectHeroGridItem>
    {
        //伙伴通用卡牌展示子窗口
        private GGUIWndHeroCommonCardItem _m_wndCommonCard;
        //点击回调
        private Action<GGUIWndArenaBattleSelectHeroGridItem> _m_dClickDelegate;
        //数据信息
        private HeroInfo _m_heroInfo;
        //是否是指定谈判
        private bool _m_bIsSelectAttack;
        //是否选中
        private bool _m_bIsSelect;

        public GGUIWndArenaBattleSelectHeroGridItem(GGUIMonoArenaBattleSelectHeroGridItem _wnd)
            : base(_wnd)
        {
            initWnd();
        }

        public Action<GGUIWndArenaBattleSelectHeroGridItem> clickDelegate { get { return _m_dClickDelegate; } set { _m_dClickDelegate = value; } }

        /// <summary>
        /// 伙伴数据信息
        /// </summary>
        public HeroInfo heroInfo { get { return _m_heroInfo; } }
        /// <summary>
        /// 是否选中
        /// </summary>
        public bool isSelect { get { return _m_bIsSelect; } }

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
        /// <param name="_heroInfo">伙伴信息</param>
        /// <param name="_isSelectAttack">是否是指定谈判</param>
        public void setInfo(HeroInfo _heroInfo, bool _isSelectAttack)
        {
            if (wnd == null || null == _heroInfo)
                return;

            _m_heroInfo = _heroInfo;
            _m_bIsSelectAttack = _isSelectAttack;
            _m_bIsSelect = false;

            //刷新卡牌显示
            if (null != _m_wndCommonCard)
            {
                _m_wndCommonCard.showWnd();
                _m_wndCommonCard.setInfo(_m_heroInfo);
            }

            //设置实力
            ALUGUICommon.setLabelTxt(wnd.txtAttackPower,
                TextTranslate.instance.getLanguage(TransKeyConst.arena_battleHeroPower_num,
                    _heroInfo.getArenaBasePower()));

            //设置是否已经战斗过
            bool isAlreadyFight = NPPlayer.instance.arenaComp.canSelectHeroAttack(_m_heroInfo.id, _isSelectAttack);
            ALUGUICommon.setGameObjEnable(wnd.goAlreadyFightShowList, isAlreadyFight);
            ALUGUICommon.setGameObjEnable(wnd.goAlreadyFightHideList, !isAlreadyFight);
        }

        //设置选中
        public void setSelect(bool _isSelect)
        {
            if (wnd == null)
                return;

            _m_bIsSelect = _isSelect;
            ALUGUICommon.setGameObjEnable(wnd.goSelectShowList, _m_bIsSelect);
            ALUGUICommon.setGameObjEnable(wnd.goSelectHideList, !_m_bIsSelect);
        }

        //点击item
        private void _onClickItem(GGUIWndHeroCommonCardItem _commonCard)
        {
            bool isAlreadyFight = NPPlayer.instance.arenaComp.canSelectHeroAttack(_m_heroInfo.id, _m_bIsSelectAttack);
            if (isAlreadyFight)
                return;

            setSelect(!_m_bIsSelect);
            if (null != _m_dClickDelegate)
                _m_dClickDelegate(this);
        }
    }
}

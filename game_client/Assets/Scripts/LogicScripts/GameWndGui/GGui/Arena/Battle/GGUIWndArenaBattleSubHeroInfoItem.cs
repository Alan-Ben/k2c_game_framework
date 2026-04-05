using System;
using ALPackage;
using Common.HeroObj;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 竞技场战斗伙伴展示信息附加窗口
    /// </summary>
    public class GGUIWndArenaBattleSubHeroInfoItem : _ATALBasicUISubWnd<GGUIMonoArenaBattleSubHeroInfoItem>
    {
        //对手伙伴信息
        private Hero_ArenaShowInfo _m_arenaHeroInfo;
        //伙伴头像
        private NPGGuiWndTexture _m_wHeroIcon;
        //伙伴形象品质背景
        private GGuiWndSprite _m_wHeroIconBg;
        //伙伴半身像
        private NPGGuiWndTexture _m_wHeroCard;
        //选中需要播放的动画
        private CommonAnimationSingleInfo _m_selectAni;
        //点击谈判回调
        private Action<GGUIWndArenaBattleSubHeroInfoItem> _m_aOnClickFight;

        /// <summary>
        /// 对手伙伴信息
        /// </summary>
        public Hero_ArenaShowInfo arenaHeroInfo => _m_arenaHeroInfo;
        /// <summary>
        /// 选中需要播放的动画
        /// </summary>
        public CommonAnimationSingleInfo selectAni => _m_selectAni;
        /// <summary>
        /// 点击谈判回调
        /// </summary>
        public Action<GGUIWndArenaBattleSubHeroInfoItem> onClickFight
        {
            get => _m_aOnClickFight;
            set => _m_aOnClickFight = value;
        }

        public GGUIWndArenaBattleSubHeroInfoItem(GGUIMonoArenaBattleSubHeroInfoItem _wnd, CommonAnimationSingleInfo _selectAni = null) : base(_wnd)
        {
            _m_selectAni = _selectAni;
            initWnd();
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wHeroIcon?.hideWnd();
            _m_wHeroIconBg?.hideWnd();
            _m_wHeroCard?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wHeroIcon?.discardShowTexture();
            _m_wHeroIconBg?.discardShowTexture();
            _m_wHeroCard?.discardShowTexture();
        }

        protected override void _onDiscard()
        {
            _m_wHeroIcon?.discard();
            _m_wHeroIcon = null;

            _m_wHeroIconBg?.discard();
            _m_wHeroIconBg = null;

            _m_wHeroCard?.discard();
            _m_wHeroCard = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnFight, _onClickFight);//点击谈判按钮
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if(wnd.imgHero != null)
                _m_wHeroIcon = new NPGGuiWndTexture(wnd.imgHero);

            if (wnd.imgBg != null)
                _m_wHeroIconBg = new GGuiWndSprite(wnd.imgBg);

            if (wnd.imgHeroCard != null)
                _m_wHeroCard = new NPGGuiWndTexture(wnd.imgHeroCard);

            ALUGUICommon.combineBtnClick(wnd.btnFight, _onClickFight);//点击谈判按钮
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        public void setInfo(Hero_ArenaShowInfo _arenaHeroInfo)
        {
            _m_arenaHeroInfo  = _arenaHeroInfo;
            _refreshWnd();
        }

        /// <summary>
        /// 重置选中动画
        /// </summary>
        public void resetSelectAni()
        {
            _m_selectAni?.resetAni();
        }

        //刷新显示
        private void _refreshWnd()
        {
            if (wnd == null || arenaHeroInfo == null) 
                return;

            HeroRefObj heroRef = GRefdataCoreMgr.instance.heroRefCore.getRef(arenaHeroInfo.getHeroId());
            if (heroRef == null)
                return;

            long skinId = arenaHeroInfo.getSkinId() != 0 ? arenaHeroInfo.getSkinId() : heroRef.default_skin_id;

            //设置名称
            ALUGUICommon.setLabelTxt(wnd.txtHeroName, heroRef.transName);

            //设置伙伴头像
            if (_m_wHeroIcon != null)
            {
                _m_wHeroIcon.showWnd();
                _m_wHeroIcon.setTexture(GCommon.getItemTexIcon(ENPItemType.HERO_SKIN, skinId));
            }

            //设置头像品质背景
            if (_m_wHeroIconBg != null)
            {
                _m_wHeroIconBg.showWnd();
                _m_wHeroIconBg.setTexture(GCommon.getQualityExtRefObj(ENPItemType.HERO, arenaHeroInfo.getHeroId())?.hero_head_bg);
            }

            //设置伙伴半身像
            if (_m_wHeroCard != null)
            {
                HeroSkinRefObj heroSkinRef = GRefdataCoreMgr.instance.heroSkinRefCore.getRef(skinId);
                _m_wHeroCard.showWnd();
                _m_wHeroCard.setTexture(heroSkinRef?.card_image);
            }

            //设置等级
            ALUGUICommon.setLabelTxt(wnd.txtLevel,TextTranslate.instance.getLanguage(TransKeyConst.common_level_num,arenaHeroInfo.getLevel()));

            //重置动画
            resetSelectAni();
        }

        //点击谈判
        private void _onClickFight(GameObject _go)
        {
            _m_aOnClickFight?.Invoke(this);
        }
    }
}

using System;
using ALPackage;
using Common.HeroObj;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 竞技场战斗展示界面
    /// </summary>
    public class GGUIWndArenaBattleShow : _ANPGGUIBasicWnd<GGUIMonoArenaBattleShow>
    {
        private static GGUIWndArenaBattleShow _g_instance;
        public static GGUIWndArenaBattleShow instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new GGUIWndArenaBattleShow();
                return _g_instance;
            }
        }

        //战斗画面
        private NPGGUIWndCommonShowCase _m_wFightShowCase;
        //自己的伙伴展示
        private GGUIWndArenaBattleShowSubHero _m_wSelfSubHero;
        //对手的伙伴展示
        private GGUIWndArenaBattleShowSubHero _m_wOpponentSubHero;
        //战斗信息
        private ArenaBattleInfo _m_battleInfo;
        //对手伙伴信息
        private Hero_ArenaShowInfo _m_opponentHeroInfo;
        //是否胜利
        private bool _m_bIsWin;
        //显示序列号
        private long _m_lShowSerialize;
        //关闭回调
        private Action _m_aOnClose;

        public GGUIWndArenaBattleShow() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoArenaBattleShow.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoArenaBattleShow.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
            if (wnd == null)
                return;

            _m_lShowSerialize = ALSerializeOpMgr.next();
            long serialize = _m_lShowSerialize;
            
            //设置自动延时关闭
            ALCommonActionMonoTask.addMonoTask(() =>
            {
                if (wnd == null || !isShow || serialize != _m_lShowSerialize)
                    return;

                QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_ARENA_BATTLE_SHOW);
            },wnd.autoCloseDelayTimeSec);
        }

        protected override void _onHideWnd()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();
            _m_wSelfSubHero?.hideWnd();
            _m_wOpponentSubHero?.hideWnd();
            _m_wFightShowCase?.hideWnd();

            _m_aOnClose?.Invoke();
            _m_aOnClose = null;
        }

        protected override void _onReset()
        {
            _m_wSelfSubHero?.resetWnd();
            _m_wOpponentSubHero?.resetWnd();
            _m_wFightShowCase?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_wSelfSubHero?.discard();
            _m_wSelfSubHero = null;
            _m_wOpponentSubHero?.discard();
            _m_wOpponentSubHero = null;
            _m_wFightShowCase?.discard();
            _m_wFightShowCase = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoSelfSubHero != null)
                _m_wSelfSubHero = new GGUIWndArenaBattleShowSubHero(wnd.monoSelfSubHero);

            if (wnd.monoOpponentSubHero != null)
                _m_wOpponentSubHero = new GGUIWndArenaBattleShowSubHero(wnd.monoOpponentSubHero);

            if (wnd.monoFightShowcase != null)
                _m_wFightShowCase = new NPGGUIWndCommonShowCase(wnd.monoFightShowcase);

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        public void setInfo(ArenaBattleInfo _battleInfo, Hero_ArenaShowInfo _opponentHeroInfo, bool _isWin, Action _onClose)
        {
            _m_battleInfo = _battleInfo;
            _m_opponentHeroInfo = _opponentHeroInfo;
            _m_bIsWin = _isWin;
            _m_aOnClose = _onClose;

            _refreshWnd();
        }

        //刷新窗口
        private void _refreshWnd()
        {
            if (wnd == null || _m_battleInfo == null || _m_opponentHeroInfo == null)
                return;

            //====设置自己伙伴信息
            HeroInfo selfHeroInfo = NPPlayer.instance.heroComponent.getHeroInfo(_m_battleInfo.heroId);
            if (selfHeroInfo == null)
                return;

            _m_wSelfSubHero?.showWnd();
            _m_wSelfSubHero?.setInfo(
                selfHeroInfo.heroRefObj, 
                selfHeroInfo.curSkinRefObj, 
                selfHeroInfo.level, 
                _m_battleInfo.getTotalPower(), 
                _m_battleInfo.getCurLeftPower(), 
                _m_battleInfo.getCurLeftPower() + _m_opponentHeroInfo.getPower());

            //====设置对手伙伴信息
            HeroRefObj opponentHeroRef = GRefdataCoreMgr.instance.heroRefCore.getRef(_m_opponentHeroInfo.getHeroId());
            if(opponentHeroRef == null)
                return;
            HeroSkinRefObj opponentHeroSkinRef = GRefdataCoreMgr.instance.heroSkinRefCore.getRef(_m_opponentHeroInfo.getSkinId() != 0? _m_opponentHeroInfo.getSkinId(): opponentHeroRef.default_skin_id);
            //对手伙伴剩余血量 = 对手伙伴总血量 - 自己与对手谈判前的剩余血量
            long opponentCurHeroHp = _m_opponentHeroInfo.getPower() - (_m_battleInfo.getCurLeftPower() + _m_opponentHeroInfo.getPower());
            if (opponentCurHeroHp < 0)
                opponentCurHeroHp = 0;

            _m_wOpponentSubHero?.showWnd();
            _m_wOpponentSubHero?.setInfo(
                opponentHeroRef, 
                opponentHeroSkinRef, 
                _m_opponentHeroInfo.getLevel(), 
                _m_opponentHeroInfo.getPower(), 
                opponentCurHeroHp, 
                _m_opponentHeroInfo.getPower());

            //====播放战斗视频
            NPGGoIndex videoGoIndex = GRefdataCoreMgr.instance.npGeneral.arena_battle_video_go_index;
            if(videoGoIndex != null && videoGoIndex.isValid())
            {
                string aniName = null;
                if (_m_bIsWin)
                    aniName = GRefdataCoreMgr.instance.npGeneral.arena_battle_video_win_ani_name_list.GetRandomItem();
                else
                    aniName = GRefdataCoreMgr.instance.npGeneral.arena_battle_video_lost_ani_name_list.GetRandomItem();

                _m_wFightShowCase?.showWnd(new ShowCaseCommonResUnitInfoObj(videoGoIndex));

                if (!string.IsNullOrEmpty(aniName))
                {
                    _m_wFightShowCase?.regInitDoneDelegate(() =>
                    {
                        _m_wFightShowCase?.playAnim(0, aniName);
                    });
                }
            }
        }

        //点击关闭按钮
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_ARENA_BATTLE_SHOW);
        }
    }
}
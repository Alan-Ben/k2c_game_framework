using System;
using ALPackage;
using Common.ArenaObj;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 竞技场结算弹窗
    /// </summary>
    public class GGUIWndArenaBattleFinalResult : _ANPGGUIBasicWnd<GGUIMonoArenaBattleFinalResult>
    {
        private static GGUIWndArenaBattleFinalResult _g_instance;
        public static GGUIWndArenaBattleFinalResult instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new GGUIWndArenaBattleFinalResult();
                return _g_instance;
            }
        }

        //战斗信息
        private ArenaBattleInfo _m_battleInfo;
        //最终结果
        private Arena_BattleResult _m_finalResult;
        //对手名字
        private string _m_sOpponentName;
        //关闭回调
        private Action _m_aOnClose;
        //伙伴半身像
        private NPGGuiWndTexture _m_wHeroTexture;
        //伙伴形象
        private NPGGUIWndCommonShowCase _m_wHeroShowCase;
        //我方影响力图标
        private NPGGuiWndTexture _m_wSelfInflucenIcon;
        //对方影响力图标
        private NPGGuiWndTexture _m_wOpponentInflucenIcon;
        //奖励列表
        private NPGGUIWndCommonItemContainer _m_wRewradContainer;

        public GGUIWndArenaBattleFinalResult() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoArenaBattleFinalResult.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoArenaBattleFinalResult.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            HeroVoiceMgr.instance.stopAllVoice();
            _m_wSelfInflucenIcon?.hideWnd();
            _m_wOpponentInflucenIcon?.hideWnd();
            _m_wRewradContainer?.hideWnd();
            _m_wHeroTexture?.hideWnd();
            _m_wHeroShowCase?.hideWnd();

            _m_aOnClose?.Invoke();
            _m_aOnClose = null;
        }

        protected override void _onReset()
        {
            _m_wSelfInflucenIcon?.discardTexture();
            _m_wOpponentInflucenIcon?.discardTexture();
            _m_wHeroTexture?.discardTexture();
            _m_wHeroShowCase?.resetWnd();
            _m_wRewradContainer?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_wSelfInflucenIcon?.discard();
            _m_wSelfInflucenIcon = null;
            _m_wOpponentInflucenIcon?.discard();
            _m_wOpponentInflucenIcon = null;
            _m_wRewradContainer?.discard();
            _m_wRewradContainer = null;
            _m_wHeroTexture?.discard();
            _m_wHeroTexture = null;
            _m_wHeroShowCase?.discard();
            _m_wHeroShowCase = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.imgSelfInfluence != null)
                _m_wSelfInflucenIcon = new NPGGuiWndTexture(wnd.imgSelfInfluence);

            if (wnd.imgOpponentInfluence != null)
                _m_wOpponentInflucenIcon = new NPGGuiWndTexture(wnd.imgOpponentInfluence);

            if (wnd.monoCommonItemContainer != null)
                _m_wRewradContainer = new NPGGUIWndCommonItemContainer(wnd.monoCommonItemContainer);

            if (wnd.imgHero != null)
                _m_wHeroTexture = new NPGGuiWndTexture(wnd.imgHero);

            if (wnd.monoHeroShowCase != null)
                _m_wHeroShowCase = new NPGGUIWndCommonShowCase(wnd.monoHeroShowCase);

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_battleInfo"></param>
        /// <param name="_finalResult"></param>
        /// <param name="_opponentName"></param>
        /// <param name="_onClose"></param>
        public void setInfo(ArenaBattleInfo _battleInfo, Arena_BattleResult _finalResult, string _opponentName, Action _onClose)
        {
            _m_battleInfo = _battleInfo;
            _m_finalResult = _finalResult;
            _m_sOpponentName = _opponentName;
            _m_aOnClose = _onClose;

            _refreshWnd();
        }

        //刷新窗口
        private void _refreshWnd()
        {
            if (wnd == null || _m_battleInfo == null || _m_finalResult == null)
                return;

            //是否全部击败
            bool isAllDefeat = _m_finalResult.getOpponentHeroNum() == _m_finalResult.getHadDefeatNum();
            HeroInfo heroInfo = NPPlayer.instance.heroComponent.getHeroInfo(_m_battleInfo.heroId);

            //标题描述
            string title = null;
            if (_m_finalResult.getOpponentHeroNum() <= 0)
            {
                //结算详情
                title = TextTranslate.instance.getLanguage(TransKeyConst.arena_battleFinalResultTitle_none);
                ALUGUICommon.setLabelTxt(wnd.txtTitle, title);
                if(wnd.txtTitleEx != null)
                    wnd.txtTitleEx.text = title;
                //很遗憾，您的伙伴【{0}】未能击败【{1}】的伙伴
                ALUGUICommon.setLabelTxt(wnd.txtDesc, TextTranslate.instance.getLanguage(TransKeyConst.arena_battleFinalResultLostDesc_str_str, heroInfo?.heroRefObj?.transName, _m_sOpponentName));
            }
            else
            {
                if (isAllDefeat)//结算详情
                    title = TextTranslate.instance.getLanguage(TransKeyConst.arena_battleFinalResultAllDefeatTitle_none);
                else//大获全胜
                    title = TextTranslate.instance.getLanguage(TransKeyConst.arena_battleFinalResultTitle_none);

                ALUGUICommon.setLabelTxt(wnd.txtTitle, title);
                if (wnd.txtTitleEx != null)
                    wnd.txtTitleEx.text = title;

                //恭喜，您的伙伴【{0}】击败了【{1}】的{2}位伙伴
                ALUGUICommon.setLabelTxt(wnd.txtDesc, TextTranslate.instance.getLanguage(TransKeyConst.arena_battleFinalResultWinDesc_str_str_num, heroInfo?.heroRefObj?.transName, _m_sOpponentName, _m_finalResult.getHadDefeatNum()));
            }

            //伙伴半身像
            if (_m_wHeroTexture != null)
            {
                _m_wHeroTexture.showWnd();
                _m_wHeroTexture.setTexture(heroInfo?.getCardImage());
            }

            //伙伴形象
            if (_m_wHeroShowCase != null)
            {
                _m_wHeroShowCase.showWnd(new ShowCaseCommonResUnitInfoObj(heroInfo?.getTdShow()));
            }

            //设置影响力图标
            NPCommonItem influenceItem = GRefdataCoreMgr.instance.npGeneral.arena_influence_sys_info;
            if (influenceItem != null)
            {
                _m_wSelfInflucenIcon?.showWnd();
                _m_wSelfInflucenIcon?.setTexture(GCommon.getItemTexIcon(influenceItem.itemType, influenceItem.itemId));
                _m_wOpponentInflucenIcon?.showWnd();
                _m_wOpponentInflucenIcon?.setTexture(GCommon.getItemTexIcon(influenceItem.itemType, influenceItem.itemId));
            }

            //影响力变化
            if (_m_finalResult.getGainInfluence() > 0)
                ALUGUICommon.setLabelTxt(wnd.txtSelfInfluenceChg, TextTranslate.instance.getLanguage(TransKeyConst.common_add_num, _m_finalResult.getGainInfluence()));
            else
                ALUGUICommon.setLabelTxt(wnd.txtSelfInfluenceChg, _m_finalResult.getGainInfluence());

            if (_m_finalResult.getOpponentDeductinfluence() > 0)
                ALUGUICommon.setLabelTxt(wnd.txtOpponentInfluenceChg, TextTranslate.instance.getLanguage(TransKeyConst.common_reduced_num, _m_finalResult.getOpponentDeductinfluence()));
            else
                ALUGUICommon.setLabelTxt(wnd.txtOpponentInfluenceChg, _m_finalResult.getOpponentDeductinfluence());

            //奖励列表
            bool needShowReward = _m_finalResult.getHadDefeatNum() > 0;
            if (_m_finalResult.getGainItem() != null && _m_finalResult.getGainItem().Count > 0)
            {
                _m_wRewradContainer?.showWnd();
                _m_wRewradContainer?.showItemList(_m_finalResult.getGainItem().toItemDataList());
            }
            else
                _m_wRewradContainer?.hideWnd();
            ALUGUICommon.setGameObjEnable(wnd.goHaveRewardShowList, needShowReward);
            ALUGUICommon.setGameObjEnable(wnd.goHaveRewardHideList, !needShowReward);

            //播放配音
            HeroVoiceMgr.instance.playVoice(heroInfo.id, EHeroVoiceType.KILL_BLOW);
        }

        //点击关闭按钮
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_ARENA_BATTLE_FINAL_RESULT);
        }
    }
}
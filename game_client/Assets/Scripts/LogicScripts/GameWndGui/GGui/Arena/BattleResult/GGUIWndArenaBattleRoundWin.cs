using System;
using ALPackage;
using Common.ArenaObj;
using CommonEnum;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 竞技场回合胜利弹窗
    /// </summary>
    public class GGUIWndArenaBattleRoundWin : _ANPGGUIBasicWnd<GGUIMonoArenaBattleRoundWin>
    {
        private static GGUIWndArenaBattleRoundWin _g_instance;
        public static GGUIWndArenaBattleRoundWin instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new GGUIWndArenaBattleRoundWin();
                return _g_instance;
            }
        }

        //回合结果
        private Arena_RoundResult _m_roundResult;
        //关闭回调
        private Action _m_aOnClose;
        //银币图标
        private NPGGuiWndTexture _m_wCoinIcon;
        //我方影响力图标
        private NPGGuiWndTexture _m_wSlefInfluenceIcon;
        //对方影响力图标
        private NPGGuiWndTexture _m_wOpponentInfluenceIcon;

        public GGUIWndArenaBattleRoundWin() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoArenaBattleRoundWin.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoArenaBattleRoundWin.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wCoinIcon?.hideWnd();
            _m_wSlefInfluenceIcon?.hideWnd();
            _m_wOpponentInfluenceIcon?.hideWnd();

            _m_aOnClose?.Invoke();
            _m_aOnClose = null;
        }

        protected override void _onReset()
        {
            _m_wCoinIcon?.discardTexture();
            _m_wSlefInfluenceIcon?.discardTexture();
            _m_wOpponentInfluenceIcon?.discardTexture();
        }

        protected override void _onDiscard()
        {
            _m_wCoinIcon?.discard();
            _m_wCoinIcon = null;
            _m_wSlefInfluenceIcon?.discard();
            _m_wSlefInfluenceIcon = null;
            _m_wOpponentInfluenceIcon?.discard();
            _m_wOpponentInfluenceIcon = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.imgCoin != null)
                _m_wCoinIcon = new NPGGuiWndTexture(wnd.imgCoin);

            if (wnd.imgSelfInfluence != null)
                _m_wSlefInfluenceIcon = new NPGGuiWndTexture(wnd.imgSelfInfluence);

            if (wnd.imgOpponentInfluence != null)
                _m_wOpponentInfluenceIcon = new NPGGuiWndTexture(wnd.imgOpponentInfluence);

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_roundResult"></param>
        /// <param name="_onClose"></param>
        public void setInfo(Arena_RoundResult _roundResult, Action _onClose)
        {
            _m_roundResult = _roundResult;
            _m_aOnClose = _onClose;

            _refreshWnd();
        }

        //刷新窗口
        private void _refreshWnd()
        {
            if (wnd == null || _m_roundResult == null)
                return;

            //当前回合数
            long curRound = _m_roundResult.getRound();

            //设置图标
            _m_wCoinIcon?.showWnd();
            _m_wCoinIcon?.setTexture(GCommon.getItemTexIcon(ENPItemType.CURRENCY,(long)ECurrency.ARENA_COIN));
            NPCommonItem influenceItem = GRefdataCoreMgr.instance.npGeneral.arena_influence_sys_info;
            if (influenceItem != null)
            {
                _m_wSlefInfluenceIcon?.showWnd();
                _m_wSlefInfluenceIcon?.setTexture(GCommon.getItemTexIcon(influenceItem.itemType, influenceItem.itemId));
                _m_wOpponentInfluenceIcon?.showWnd();
                _m_wOpponentInfluenceIcon?.setTexture(GCommon.getItemTexIcon(influenceItem.itemType, influenceItem.itemId));
            }

            //设置连胜奖励描述
            ArenaRoundRewardRefObj arenaRoundRewardRef = GRefdataCoreMgr.instance.arenaRoundRewardRefCore.getRef(curRound);
            if (arenaRoundRewardRef == null)
            {
                //还未达到连胜奖励
                long leftRound = 0;
                for (int i = 0; i < GRefdataCoreMgr.instance.arenaRoundRewardRefCore.refList.Count; i++)
                {
                    if (GRefdataCoreMgr.instance.arenaRoundRewardRefCore.refList[i].round > curRound)
                    {
                        leftRound = GRefdataCoreMgr.instance.arenaRoundRewardRefCore.refList[i].round - curRound;
                        break;
                    }
                }

                //恭喜您获得{0}连胜，再连胜{1}场可获得连胜奖励
                ALUGUICommon.setLabelTxt(wnd.txtGainRewardDesc,
                    TextTranslate.instance.getLanguage(TransKeyConst.arena_battleRoundWinNoReachRewardDesc_num_num,
                        curRound, leftRound));
            }
            else
            {
                //达到连胜奖励
                //恭喜您获得{0}连胜，可获得连胜奖励
                ALUGUICommon.setLabelTxt(wnd.txtGainRewardDesc,
                    TextTranslate.instance.getLanguage(TransKeyConst.arena_battleRoundWinReachRewardDesc_num, curRound));
            }

            //设置连胜次数
            ALUGUICommon.setLabelTxt(wnd.txtWinCount, TextTranslate.instance.getLanguage(TransKeyConst.common_multiple_num, curRound));

            //设置硬币影响力
            ALUGUICommon.setLabelTxt(wnd.txtGainCoin,
                TextTranslate.instance.getLanguage(TransKeyConst.common_add_num, _m_roundResult.getGainCoin()));
            ALUGUICommon.setLabelTxt(wnd.txtSelfInfluenceChg,
                TextTranslate.instance.getLanguage(TransKeyConst.common_add_num, _m_roundResult.getGainInfluence()));
            ALUGUICommon.setLabelTxt(wnd.txtOpponentInfluenceChg,
                TextTranslate.instance.getLanguage(TransKeyConst.common_reduced_num, _m_roundResult.getOpponentDeductinfluence()));
        }

        //点击关闭
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_ARENA_BATTLE_ROUND_WIN);
        }
    }
}
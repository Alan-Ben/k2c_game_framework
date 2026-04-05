using System.Collections.Generic;
using ALPackage;
using Common.RankObj;
using CommonEnum;
using GOE;
using JetBrains.Annotations;
using UnityEngine;

namespace Hotfix
{
    /// <summary>
    /// 2048 得分详情界面
    /// </summary>
    public class GGUIWndNumMergeGameScoreDetail : _AHotfixBaseWnd<GGUIMonoNumMergeGameScoreDetail>
    {
        [NotNull] public static GGUIWndNumMergeGameScoreDetail instance { get { return _g_instance ??= new GGUIWndNumMergeGameScoreDetail(); } }
        private static GGUIWndNumMergeGameScoreDetail _g_instance;


        //活动id
        private int _m_showSerialize;


        private GGUIWndNumMergeGameScoreDetail()
            : base(EALUIWndLayer.ADDITION)
        {
        }


        protected override string _monoAssetPath { get { return UIResPathAssistant.getAssetPath(8604); } }
        protected override string _monoObjName { get { return UIResPathAssistant.getObjName(8604); } }



        protected override void _onShowWnd()
        {
            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_showSerialize = ALSerializeOpMgr.next();
        }
        protected override void _onReset()
        {
        }
        protected override void _onDiscard()
        {
            if (hotfixWnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(hotfixWnd.btnClose, _onClickClose);
        }
        protected override void _onWndInitDoneHotfix()
        {
            if (hotfixWnd == null)
                return;

            ALUGUICommon.combineBtnClick(hotfixWnd.btnClose, _onClickClose);
        }


        //刷新界面
        public void refreshWnd()
        {
            if (hotfixWnd == null || !_m_bIsShow)
                return;

            _refreshHighestScore(null, null);
            _refreshTotalScore(null, null);
            
            int serialize = _m_showSerialize;
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(1);

            long activityInstanceId = HotfixNPPlayer.instance.numMergeComponent.activityInstanceId;
            long highestScoreRankId = HotfixRefdataCoreMgr.instance.numMergeOtherRefObj.highest_score_rank_id;
            long totalScoreRankId = HotfixRefdataCoreMgr.instance.numMergeOtherRefObj.total_score_rank_id;
            
            Rank_BaseItem myHighestScoreRank = null;
            Rank_BaseItem highestScoreNeighborRank = null;
            stepCounter.chgTotalStepCount(1);
            NPPlayer.instance.commonActivityComp.reqActivityRankBaseInfoByKey(activityInstanceId, highestScoreRankId, NPPlayer.instance.playerInfo.CID, false, _myMsg =>
            {
                if (serialize != _m_showSerialize)
                {
                    stepCounter.addDoneStepCount();
                    return;
                }
                
                myHighestScoreRank = _myMsg?.getBaseItem();
                if (myHighestScoreRank != null && myHighestScoreRank.getRank() > 1)
                {
                    stepCounter.chgTotalStepCount(1);
                    NPPlayer.instance.commonActivityComp.reqActivityRankBaseInfoByRank(activityInstanceId, highestScoreRankId, myHighestScoreRank.getRank() - 1, false, _otherMsg =>
                    {
                        if (serialize != _m_showSerialize)
                        {
                            stepCounter.addDoneStepCount();
                            return;
                        }
                        
                        highestScoreNeighborRank = _otherMsg?.getBaseItem();
                        stepCounter.addDoneStepCount();
                    });
                }
                stepCounter.addDoneStepCount();
            });
            
            Rank_BaseItem myTotalScoreRank = null;
            Rank_BaseItem totalScoreNeighborRank = null;
            stepCounter.chgTotalStepCount(1);
            NPPlayer.instance.commonActivityComp.reqActivityRankBaseInfoByKey(activityInstanceId, totalScoreRankId, NPPlayer.instance.playerInfo.CID, false, _myMsg =>
            {
                if (serialize != _m_showSerialize)
                {
                    stepCounter.addDoneStepCount();
                    return;
                }
                
                myTotalScoreRank = _myMsg?.getBaseItem();
                if (myTotalScoreRank != null && myTotalScoreRank.getRank() > 1)
                {
                    stepCounter.chgTotalStepCount(1);
                    NPPlayer.instance.commonActivityComp.reqActivityRankBaseInfoByRank(activityInstanceId, totalScoreRankId, myTotalScoreRank.getRank() - 1, false, _otherMsg =>
                    {
                        if (serialize != _m_showSerialize)
                        {
                            stepCounter.addDoneStepCount();
                            return;
                        }
                        
                        totalScoreNeighborRank = _otherMsg?.getBaseItem();
                        stepCounter.addDoneStepCount();
                    });
                }
                stepCounter.addDoneStepCount();
            });
            
            stepCounter.regAllDoneDelegate(() =>
            {
                if (serialize != _m_showSerialize)
                    return;

                _refreshHighestScore(myHighestScoreRank, highestScoreNeighborRank);
                _refreshTotalScore(myTotalScoreRank, totalScoreNeighborRank);
            });
            stepCounter.addDoneStepCount();
        }


        //刷新历史最高得分
        private void _refreshHighestScore(Rank_BaseItem _myRank, Rank_BaseItem _neighborRank)
        {
            if (hotfixWnd == null)
                return;

            string highestScore = _myRank == null ? "" : _myRank.getScore().ToString();
            ALUGUICommon.setLabelTxt(hotfixWnd.txtHighestScore, TextTranslate.instance.getLanguage(HotfixTransKeyConst.numMerge_highestScore_value, highestScore));
            string highestScoreRank = _myRank == null ? "" : _myRank.getRank().ToString();
            ALUGUICommon.setLabelTxt(hotfixWnd.txtHighestScoreRank, TextTranslate.instance.getLanguage(HotfixTransKeyConst.numMerge_rank_value, highestScoreRank));
            string highestScoreGap = _myRank == null || _neighborRank == null ? "" : (_neighborRank.getScore() - _myRank.getScore()).ToString();
            ALUGUICommon.setLabelTxt(hotfixWnd.txtHighestScoreGap, TextTranslate.instance.getLanguage(HotfixTransKeyConst.numMerge_scoreGap_value, highestScoreGap));
            //是否是第一名
            bool isFirstPlace = _myRank != null && _myRank.getRank() == 1;
            ALUGUICommon.setGameObjEnable(hotfixWnd.listHighestScoreFirstPlaceHide, !isFirstPlace);
        }
        //刷新累计得分
        private void _refreshTotalScore(Rank_BaseItem _myRank, Rank_BaseItem _neighborRank)
        {
            if (hotfixWnd == null)
                return;

            string totalScore = _myRank == null ? "" : _myRank.getScore().ToString();
            ALUGUICommon.setLabelTxt(hotfixWnd.txtTotalScore, TextTranslate.instance.getLanguage(HotfixTransKeyConst.numMerge_totalScore_value, totalScore));
            string totalScoreRank = _myRank == null ? "" : _myRank.getRank().ToString();
            ALUGUICommon.setLabelTxt(hotfixWnd.txtTotalScoreRank, TextTranslate.instance.getLanguage(HotfixTransKeyConst.numMerge_rank_value, totalScoreRank));
            string totalScoreGap = _myRank == null || _neighborRank == null ? "" : (_neighborRank.getScore() - _myRank.getScore()).ToString();
            ALUGUICommon.setLabelTxt(hotfixWnd.txtTotalScoreGap, TextTranslate.instance.getLanguage(HotfixTransKeyConst.numMerge_scoreGap_value, totalScoreGap));
            //是否是第一名
            bool isFirstPlace = _myRank != null && _myRank.getRank() == 1;
            ALUGUICommon.setGameObjEnable(hotfixWnd.listTotalScoreFirstPlaceHide, !isFirstPlace);
        }
        //点击关闭按钮
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(HotfixUINodeTagConst.NUMMERGE_GAME_SCORE_DETAIL);
        }
    }
}

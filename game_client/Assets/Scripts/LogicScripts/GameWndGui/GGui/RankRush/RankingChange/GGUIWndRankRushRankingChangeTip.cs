using System;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 冲榜排名变化上浮提示
    /// </summary>
    public class GGUIWndRankRushRankingChangeTip : _ANPGGUIBasicWnd<GGUIMonoRankRushRankingChangeTip>
    {
        private static GGUIWndRankRushRankingChangeTip _g_instance = new GGUIWndRankRushRankingChangeTip();
        public static GGUIWndRankRushRankingChangeTip instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndRankRushRankingChangeTip();
                return _g_instance;
            }
        }

        //隐藏回调
        private Action _m_aOnHide;

        protected GGUIWndRankRushRankingChangeTip() : base(EALUIWndLayer.NOTICE)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoRankRushRankingChangeTip.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoRankRushRankingChangeTip.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            Action onHide = _m_aOnHide;
            _m_aOnHide = null;
            onHide?.Invoke();
        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
        }

        protected override void _onWndInitDone()
        {
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_rankrushId"></param>
        /// <param name="_lastRanking"></param>
        /// <param name="_curRanking"></param>
        /// <param name="_onHide"></param>
        public void setInfo(long _rankrushId, long _lastRanking, long _curRanking, Action _onHide)
        {
            if (wnd == null)
                return;

            //设置隐藏回调
            _m_aOnHide = _onHide;

            //计算排名变化
            long rankingChgValue = _curRanking - _lastRanking;

            //排名没有变化，不展示
            if (rankingChgValue == 0)
            {
                hideWnd();
                return;
            }

            ActivityRankRushRefObj refObj = GRefdataCoreMgr.instance.activityRankRushRefCore.getRef(_rankrushId);
            if (refObj == null)
            {
                hideWnd();
                return;
            }

            //排名描述
            NPRankRefObj rankRefObj = GRefdataCoreMgr.instance.rankCommonRefCore.getRef(refObj.rank_id);
            string rankingName = rankRefObj?.nameStr;
            string curRanking = _curRanking.ToString();
            string rankingChgStr = "";
            if (_lastRanking > 0)
            {
                //非首次上榜排名变动
                if (rankingChgValue > 0)
                {
                    //排名下降
                    rankingName = GCommon.addColorForRichText(rankingName, wnd.reduceDescColor);
                    curRanking = GCommon.addColorForRichText(curRanking, wnd.reduceDescColor);
                    rankingChgStr = TextTranslate.instance.getLanguage(TransKeyConst.common_reduced_num, rankingChgValue);
                    rankingChgStr = GCommon.addColorForRichText(rankingChgStr, wnd.reduceNumColor);
                }
                else
                {
                    //排名上升
                    curRanking = GCommon.addColorForRichText(curRanking, wnd.increateDescColor);
                    rankingName = GCommon.addColorForRichText(rankingName, wnd.increateDescColor);
                    rankingChgStr = TextTranslate.instance.getLanguage(TransKeyConst.common_add_num, -rankingChgValue);
                    rankingChgStr = GCommon.addColorForRichText(rankingChgStr, wnd.increateNumColor);
                }
            }
            else
            {
                //首次上榜
                curRanking = GCommon.addColorForRichText(curRanking, wnd.haveRankingDescColor);
                rankingName = GCommon.addColorForRichText(rankingName, wnd.haveRankingDescColor);
            }

            ALUGUICommon.setLabelTxt(wnd.txtRankName, rankingName);
            ALUGUICommon.setLabelTxt(wnd.txtRanking, curRanking);
            ALUGUICommon.setLabelTxt(wnd.txtChangeNum, rankingChgStr);

            //设置显隐
            ALUGUICommon.setGameObjEnable(wnd.goHaveRankingShowList, false);
            ALUGUICommon.setGameObjEnable(wnd.goAddShowList, false);
            ALUGUICommon.setGameObjEnable(wnd.goReduceShowList, false);
            if(_lastRanking <= 0)
                ALUGUICommon.setGameObjEnable(wnd.goHaveRankingShowList, true);
            else
            {
                if(rankingChgValue > 0)
                    ALUGUICommon.setGameObjEnable(wnd.goReduceShowList, true);
                else
                    ALUGUICommon.setGameObjEnable(wnd.goAddShowList, true);
            }

            //定时隐藏
            ALCommonActionMonoTask.addMonoTask(() =>
            {
                hideWnd();
            }, wnd.disableTime);
        }
    }
}

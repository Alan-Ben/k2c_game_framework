using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 排行详情窗口
    /// </summary>
    public class GGUIWndEveningDungeonRankDetail : _ANPGGUIBasicWnd<GGUIMonoEveningDungeonRankDetail>
    {
        private static GGUIWndEveningDungeonRankDetail _g_instance;
        public static GGUIWndEveningDungeonRankDetail instance { get { return _g_instance ??= new GGUIWndEveningDungeonRankDetail(); } }

        //排行榜列表
        private List<EveningDungeonRankInfo> _m_lRankShowInfoList;
        // 自己排行数据
        private Common.RankObj.Rank_BaseItem _m_SelfRankShowInfo;
        
        private GGUIWndEveningDungeonRankDetailGrid _m_wRankGrid;//排行榜列表
        
        public GGUIWndEveningDungeonRankDetail() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoEveningDungeonRankDetail.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoEveningDungeonRankDetail.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.monoRankGrid != null)
                _m_wRankGrid = new GGUIWndEveningDungeonRankDetailGrid(wnd.monoRankGrid);
            
            ALUGUICommon.combineBtnClick(wnd.btnClose, _closeBtnClick);
        }
        
        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnClose, _closeBtnClick);
            }
            
            _m_wRankGrid?.discard();
            _m_wRankGrid = null;
            
            _m_lRankShowInfoList?.Clear();
            _m_lRankShowInfoList = null;
            
            _m_SelfRankShowInfo = null;
        }
        
        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wRankGrid?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wRankGrid?.resetWnd();
        }

        public void setData(List<EveningDungeonRankInfo> _rankList, Common.RankObj.Rank_BaseItem _selfRankInfo)
        {
            _m_lRankShowInfoList = _rankList;
            _m_SelfRankShowInfo = _selfRankInfo;
            
            _refreshWnd();
        }
        
        /// <summary>
        /// 刷新窗口显示
        /// </summary>
        private void _refreshWnd()
        {
            if(wnd == null || !isShow)
                return;

            // 刷新排行榜信息
            if (_m_wRankGrid != null)
            {
                _m_wRankGrid.showWnd();
                _m_wRankGrid.setInfo(_m_lRankShowInfoList);
            }
            
            // 刷新自身排行信息
            int selfRank = _m_SelfRankShowInfo?.getRank() ?? 0;
            //设置排名
            if (selfRank > 0)
                ALUGUICommon.setLabelTxt(wnd.txtMyRank, TextTranslate.instance.getLanguage(TransKeyConst.eveningDungeon_myRank_str, selfRank));
            else
                ALUGUICommon.setLabelTxt(wnd.txtMyRank, TextTranslate.instance.getLanguage(TransKeyConst.eveningDungeon_myRank_str, TransKeyConst.eveningDungeon_notOnTheRankingList_none));

            //设置分数
            ALUGUICommon.setLabelTxt(wnd.txtMyScore, TextTranslate.instance.getLanguage(TransKeyConst.eveningDungeon_myScore_str_num, 
                GCommon.getValueFormatStr(EValueFormatType.NORMAL, _m_SelfRankShowInfo?.getScore() ?? 0)));
        }
        
        /// <summary>
        /// 点击关闭按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _closeBtnClick(GameObject _go)
        { 
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_EVENING_DUNGEON_RANK_DETAIL);
        }
    }
}
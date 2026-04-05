using ALPackage;
using GOE;
using System.Collections.Generic;
using UnityEngine;

namespace Hotfix
{
    /// <summary>
    /// 万能活动附加排行榜本服页面
    /// </summary>
    public class GGUIWndRegularEventSubRankSelfPage : _AHotfixBaseSubPrefabWnd<GGUIMonoRegularEventSubRankSelfPage>
    {
        private string _m_sAssetPath;
        private string _m_sObjName;

        //活动id
        private long _m_lActivityId;
        //活动信息
        private _ABaseActivityInfo _m_activityInfo;
        //排行榜列表
        private GGUIWndRankRushDetailRankGrid _m_wRankGrid;
        //排行榜列表
        private List<NPRankCommonShowInfo> _m_lRankShowInfoList;
        //我的排名
        private long _m_lMyRank;
        //我的分数
        private long _m_lMyScore;
        //显示序列号
        private long _m_lShowSerialize;

        public GGUIWndRegularEventSubRankSelfPage(NPCommonAssetPathInfo _assetPathInfo, Transform _parent) : base(_parent)
        {
            if (_assetPathInfo == null)
                return;

            _m_sAssetPath = _assetPathInfo.asset_path;
            _m_sObjName = _assetPathInfo.obj_name;
        }
        
        protected override string _monoAssetPath { get { return _m_sAssetPath; } }
        protected override string _monoObjName { get { return _m_sObjName; } }

        protected override void _onShowWnd()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();
        }

        protected override void _onHideWnd()
        {
            _m_wRankGrid?.hideWnd();
            _m_lShowSerialize = ALSerializeOpMgr.next();
        }

        protected override void _onReset()
        {
            _m_wRankGrid?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_wRankGrid?.discard();
            _m_wRankGrid = null;
            _m_lRankShowInfoList?.Clear();
            _m_lRankShowInfoList = null;
            _m_lMyRank = 0;
            _m_lMyScore = 0;
        }
        
        protected override void _onWndInitDoneHotfix()
        {
            if (hotfixWnd == null)
                return;

            if(hotfixWnd.monoRankGrid != null)
                _m_wRankGrid = new GGUIWndRankRushDetailRankGrid(hotfixWnd.monoRankGrid);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_activityId"></param>
        public void setInfo(long _activityId)
        {
            _m_lActivityId = _activityId;
            _m_activityInfo = NPPlayer.instance.commonActivityComp.getValidActivityInfoByActivityId(_m_lActivityId);
            _refreshWnd();
        }

        //刷新窗口
        private void _refreshWnd()
        {
            _refreshRankList();
            _refreshSelfRank();
        }

        //刷新排行榜列表
        private void _refreshRankList()
        {
            if (hotfixWnd == null || _m_activityInfo == null)
                return;

            //先设置默认显示
            _m_wRankGrid?.showWnd();
            _m_wRankGrid?.setInfo(_m_lRankShowInfoList);

            GActivityMainRefObj activityMainRef = GRefdataCoreMgr.instance.activityMainRefCore.getRef(_m_lActivityId);
            if (activityMainRef == null || activityMainRef.rank_id_list == null || activityMainRef.rank_id_list.Count == 0)
                return;

            //设置分数标题
            NPRankRefObj rankRef = GRefdataCoreMgr.instance.rankCommonRefCore.getRef(activityMainRef.rank_id_list[0]);
            ALUGUICommon.setLabelTxt(hotfixWnd.txtRankScoreTitle, TextTranslate.instance.getLanguage(rankRef?.score_name));

            //请求最新排行榜
            long serialize = _m_lShowSerialize;
            NPPlayer.instance.commonActivityComp.reqActivityRankBaseList(_m_activityInfo.instanceId, activityMainRef.rank_id_list[0], false,
                _msg =>
                {
                    if (_msg == null || wnd == null || !isShow || _m_lShowSerialize != serialize)
                        return;

                    if (_m_lRankShowInfoList == null)
                        _m_lRankShowInfoList = new List<NPRankCommonShowInfo>();
                    _m_lRankShowInfoList.Clear();

                    for (int i = 0; i < _msg.getBaseItemlist().Count; i++)
                    {
                        NPRankCommonShowInfo rankCommonShowInfo = new NPRankCommonShowInfo(_msg.getBaseItemlist()[i], false);
                        rankCommonShowInfo.setRankId(activityMainRef.rank_id_list[0], 0);
                        _m_lRankShowInfoList.Add(rankCommonShowInfo);
                    }
                    _m_lRankShowInfoList.Sort((_a, _b) => _a.rankSortId.CompareTo(_b.rankSortId));

                    //刷新列表
                    _m_wRankGrid?.showWnd();
                    _m_wRankGrid?.setInfo(_m_lRankShowInfoList);
                });
        }

        //刷新自己的排名信息
        private void _refreshSelfRank()
        {
            if (hotfixWnd == null || _m_activityInfo == null)
                return;

            GActivityMainRefObj activityMainRef = GRefdataCoreMgr.instance.activityMainRefCore.getRef(_m_lActivityId);
            if (activityMainRef == null || activityMainRef.rank_id_list == null || activityMainRef.rank_id_list.Count == 0)
                return;
            NPRankRefObj rankRef = GRefdataCoreMgr.instance.rankCommonRefCore.getRef(activityMainRef.rank_id_list[0]);

            //先重置展示
            if (_m_lMyRank <= 0)
                ALUGUICommon.setLabelTxt(hotfixWnd.txtSelfRank, TextTranslate.instance.getLanguage(HotfixTransKeyConst.regularEvent_myRank_num.replaceActivity(_m_lActivityId), TransKeyConst.rankRush_notOnTheRankingList_none));
            else
                ALUGUICommon.setLabelTxt(hotfixWnd.txtSelfRank, TextTranslate.instance.getLanguage(HotfixTransKeyConst.regularEvent_myRank_num.replaceActivity(_m_lActivityId), _m_lMyRank));
            ALUGUICommon.setLabelTxt(hotfixWnd.txtSelfScore, _m_lMyScore.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT));


            long serialize = _m_lShowSerialize;
            //请求自己的排名信息
            NPPlayer.instance.commonActivityComp.reqActivityRankBaseInfoByKey(_m_activityInfo.instanceId, activityMainRef.rank_id_list[0], NPPlayer.instance.playerInfo.CID, false,
                _msg =>
                {
                    if (_msg == null || _msg.getBaseItem() == null || hotfixWnd == null || !isShow || _m_lShowSerialize != serialize)
                        return;

                    _m_lMyRank = _msg.getBaseItem().getRank();
                    _m_lMyScore = _msg.getBaseItem().getScore();
                    if (_m_lMyRank <= 0)
                        ALUGUICommon.setLabelTxt(hotfixWnd.txtSelfRank, TextTranslate.instance.getLanguage(HotfixTransKeyConst.regularEvent_myRank_num.replaceActivity(_m_lActivityId), TransKeyConst.rankRush_notOnTheRankingList_none));
                    else
                        ALUGUICommon.setLabelTxt(hotfixWnd.txtSelfRank, TextTranslate.instance.getLanguage(HotfixTransKeyConst.regularEvent_myRank_num.replaceActivity(_m_lActivityId), _m_lMyRank));

                    ALUGUICommon.setLabelTxt(hotfixWnd.txtSelfScore, _m_lMyScore.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT));
                });
        }
    }
}
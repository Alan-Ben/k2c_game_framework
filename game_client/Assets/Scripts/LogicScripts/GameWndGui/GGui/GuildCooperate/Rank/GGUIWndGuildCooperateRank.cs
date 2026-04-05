using ALPackage;
using Common.RankObj;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 公会协作排行榜弹窗
    /// </summary>
    public class GGUIWndGuildCooperateRank : _ANPGGUIBasicWnd<GGUIMonoGuildCooperateRank>
    {
        private static GGUIWndGuildCooperateRank _g_instance;
        public static GGUIWndGuildCooperateRank instance
        {
            get
            {
                if(_g_instance == null)
                    _g_instance = new GGUIWndGuildCooperateRank();
                return _g_instance;
            }
        }

        //排名列表
        private GGUIWndRankFixedDetailRankGrid _m_wRankGrid;
        //显示序列
        private long _m_lShowSerialize;

        public GGUIWndGuildCooperateRank() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoGuildCooperateRank.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoGuildCooperateRank.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        protected override bool isShowAniPlayOnlyOne { get { return true; } }

        protected override void _onShowWnd()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();
            _m_wRankGrid?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wRankGrid?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_wRankGrid?.discard();
            _m_wRankGrid = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoRankListGrid != null)
                _m_wRankGrid = new GGUIWndRankFixedDetailRankGrid(wnd.monoRankListGrid);

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
        }

        /// <summary>
        /// 刷新界面
        /// </summary>
        private void _refreshWnd()
        {
            if (wnd == null)
                return;

            long serialize = _m_lShowSerialize;

            //先设置空，等待请求数据
            ALUGUICommon.setLabelTxt(wnd.txtMyRank, "");
            ALUGUICommon.setLabelTxt(wnd.txtMyValue, "");
            _m_wRankGrid?.hideWnd();

            //请求数据
            NPPlayer.instance.guildCooperateComp.reqGuildCooperateDamageRank(_msg =>
            {
                if (wnd == null || !isShow || serialize != _m_lShowSerialize || _msg == null || _msg.getRankList() == null)
                    return;

                // 排行榜展示列表数据
                List<NPRankCommonShowInfo> showInfoList = new List<NPRankCommonShowInfo>();
                // 自己的信息
                NPRankCommonShowInfo selfInfo = null;
                for (int i = 0; i < _msg.getRankList().Count; i++)
                {
                    Rank_BaseItem rankInfo = _msg.getRankList()[i];
                    if (rankInfo == null)
                        continue;

                    NPRankCommonShowInfo commonRankInfo = new NPRankCommonShowInfo(rankInfo, false);
                    showInfoList.Add(commonRankInfo);

                    if (commonRankInfo.cid == NPPlayer.instance.playerInfo.CID)
                        selfInfo = commonRankInfo;
                }
                showInfoList.Sort((_a,_b)=>_a.rankSortId.CompareTo(_b.rankSortId));

                // 设置排行列表
                _m_wRankGrid?.showWnd();
                _m_wRankGrid?.setInfo(showInfoList);

                // 设置自己信息
                if (selfInfo != null)
                {
                    ALUGUICommon.setLabelTxt(wnd.txtMyRank, selfInfo.rankSortId);
                    ALUGUICommon.setLabelTxt(wnd.txtMyValue, selfInfo.rankScore.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT));
                }
            });
        }

        /// <summary>
        /// 点击关闭按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_GUIlD_COOPERATE_RANK);
        }
    }
}
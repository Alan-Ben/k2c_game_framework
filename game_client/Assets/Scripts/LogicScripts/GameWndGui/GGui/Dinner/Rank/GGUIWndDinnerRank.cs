using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 宴会排行榜界面
    /// </summary>
    public class GGUIWndDinnerRank : _ANPGGUIBasicWnd<GGUIMonoDinnerRank>
    {
        private static GGUIWndDinnerRank _g_instance;
        public static GGUIWndDinnerRank instance
        {
            get
            {
                if(_g_instance == null)
                    _g_instance = new GGUIWndDinnerRank();
                return _g_instance;
            }
        }

        //排行榜列表
        private GGUIWndDinnerRankGrid _m_rankGrid;
        //显示序列
        private long _m_lShowSerialize;

        public GGUIWndDinnerRank() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoDinnerRank.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoDinnerRank.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();
            _m_rankGrid?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();
            _m_rankGrid?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_rankGrid?.discard();
            _m_rankGrid = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if(wnd.monoRankGrid != null)
                _m_rankGrid = new GGUIWndDinnerRankGrid(wnd.monoRankGrid);

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
        }

        //刷新界面
        private void _refreshWnd()
        {
            if(wnd == null)
                return;

            long rankFixId = GRefdataCoreMgr.instance.npGeneral.dinner_rank_fixed_id;
            long serialize = _m_lShowSerialize;

            //先设置空，等待请求数据
            ALUGUICommon.setLabelTxt(wnd.txtRankScore, "");
            ALUGUICommon.setLabelTxt(wnd.txtSelfRank, "");
            ALUGUICommon.setGameObjEnable(wnd.noScoreShowList, true);
            ALUGUICommon.setGameObjEnable(wnd.noScoreHideList, false);
            _m_rankGrid?.hideWnd();

            //请求自己的数据
            NPPlayer.instance.rankCommonComp.reqInRankPlayerInfoByCid(rankFixId, NPPlayer.instance.playerInfo.CID, _info =>
            {
                if (wnd == null || !isShow || serialize != _m_lShowSerialize || _info == null)
                    return;

                //有无积分时切换显示，
                bool noScore = _info.getScore() <= 0;
                ALUGUICommon.setGameObjEnable(wnd.noScoreShowList, noScore);
                ALUGUICommon.setGameObjEnable(wnd.noScoreHideList, !noScore);
                //设置自己的排名分数
                ALUGUICommon.setLabelTxt(wnd.txtRankScore, _info.getScore().ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT));
                ALUGUICommon.setLabelTxt(wnd.txtSelfRank,  _info.getRank());
            });


            //请求排行榜列表数据
            NPPlayer.instance.rankCommonComp.reqRankFixedBaseList(rankFixId, _msg =>
            {
                if (wnd == null || !isShow || serialize != _m_lShowSerialize || _msg == null || _msg.getBaseItemlist() == null)
                    return;

                List<NPRankCommonShowInfo> showInfoList = new List<NPRankCommonShowInfo>();
                for (int i = 0; i < _msg.getBaseItemlist().Count; i++)
                {
                    showInfoList.Add(new NPRankCommonShowInfo(_msg.getBaseItemlist()[i], false));
                }

                //设置排行榜列表
                _m_rankGrid?.showWnd();
                _m_rankGrid?.setInfo(showInfoList);
            });
        }

        //点击关闭
        private void _onClickClose(GameObject _obj)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_DINNER_RANK);
        }
    }
}
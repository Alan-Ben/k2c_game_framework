using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 竞技场排行榜界面
    /// </summary>
    public class GGUIWndArenaRank : _ANPGGUIBasicWnd<GGUIMonoArenaRank>
    {
        private static GGUIWndArenaRank _g_instance;
        public static GGUIWndArenaRank instance
        {
            get
            {
                if(_g_instance == null)
                    _g_instance = new GGUIWndArenaRank();
                return _g_instance;
            }
        }

        //排行榜列表
        private GGUIWndArenaRankGrid _m_rankGrid;
        //显示序列
        private long _m_lShowSerialize;

        public GGUIWndArenaRank() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoArenaRank.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoArenaRank.objName; } }
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
                _m_rankGrid = new GGUIWndArenaRankGrid(wnd.monoRankGrid);

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
        }

        //刷新界面
        private void _refreshWnd()
        {
            if(wnd == null)
                return;

            long rankFixId = GRefdataCoreMgr.instance.npGeneral.arena_rank_fixed_id;
            long serialize = _m_lShowSerialize;

            //先设置空，等待请求数据
            ALUGUICommon.setLabelTxt(wnd.txtSelfInfluence, "");
            ALUGUICommon.setLabelTxt(wnd.txtSelfRank, "");
            _m_rankGrid?.hideWnd();

            //请求自己的数据
            NPPlayer.instance.rankCommonComp.reqInRankPlayerInfoByCid(rankFixId, NPPlayer.instance.playerInfo.CID, _info =>
            {
                if (wnd == null || !isShow || serialize != _m_lShowSerialize || _info == null)
                    return;

                //设置自己的排名分数
                ALUGUICommon.setLabelTxt(wnd.txtSelfInfluence, TextTranslate.instance.getLanguage(TransKeyConst.arena_selfInfluence_num, _info.getScore().ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT)));
                ALUGUICommon.setLabelTxt(wnd.txtSelfRank, TextTranslate.instance.getLanguage(TransKeyConst.arena_selfRank_num, _info.getRank()));
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
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_ARENA_RANK);
        }
    }
}
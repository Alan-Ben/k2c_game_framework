using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIPrefabSubWndMiddayDungeonRank : _ANPGGUIBasicLoadPrefabSubWnd<GGUIPrefabSubMonoMiddayDungeonRank>
    {
        private NPCommonAssetPathInfo _m_assetPathInfo;

        //排行榜详情前几名信息附加窗口
        private List<GGUIWndRankFixedTopPlayerInfo> _m_lTopPlayerInfoList;
        //排行榜列表
        private GGUIWndMiddayDungeonRankGrid _m_rankGrid;
        //显示序列
        private long _m_lShowSerialize;
        
        public GGUIPrefabSubWndMiddayDungeonRank(NPCommonAssetPathInfo _assetPathInfo, Transform _parent) : base(_parent)
        {
            _m_assetPathInfo = _assetPathInfo;
        }

        protected override string _monoAssetPath { get { return _m_assetPathInfo?.asset_path; } }
        protected override string _monoObjName { get { return _m_assetPathInfo?.obj_name; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if(wnd.monoRankGrid != null)
                _m_rankGrid = new GGUIWndMiddayDungeonRankGrid(wnd.monoRankGrid);
            
            _m_lTopPlayerInfoList = new List<GGUIWndRankFixedTopPlayerInfo>();
            if (wnd.monoTopPlayerList != null)
            {
                for (int i = 0; i < wnd.monoTopPlayerList.Count; i++)
                {
                    _m_lTopPlayerInfoList.Add(new GGUIWndRankFixedTopPlayerInfo(wnd.monoTopPlayerList[i]));
                }
            }
        }
        
        protected override void _onDiscard()
        {
            _m_rankGrid?.discard();
            _m_rankGrid = null;

            if (_m_lTopPlayerInfoList != null)
            {
                foreach (GGUIWndRankFixedTopPlayerInfo subTopPlayerInfo in _m_lTopPlayerInfoList)
                {
                    subTopPlayerInfo?.discard();
                }
                _m_lTopPlayerInfoList.Clear();
                _m_lTopPlayerInfoList = null;
            }
            
            if (wnd == null)
                return;
        }
        
        protected override void _onShowWnd()
        {
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();
            _m_rankGrid?.hideWnd();
            
            if (_m_lTopPlayerInfoList != null)
            {
                foreach (GGUIWndRankFixedTopPlayerInfo subTopPlayerInfo in _m_lTopPlayerInfoList)
                {
                    subTopPlayerInfo?.hideWnd();
                }
            }
        }

        protected override void _onReset()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();
            _m_rankGrid?.resetWnd();
            
            if (_m_lTopPlayerInfoList != null)
            {
                foreach (GGUIWndRankFixedTopPlayerInfo subTopPlayerInfo in _m_lTopPlayerInfoList)
                {
                    subTopPlayerInfo?.resetWnd();
                }
            }
        }
        
        //刷新界面
        private void _refreshWnd()
        {
            if(wnd == null)
                return;

            long rankFixId = GRefdataCoreMgr.instance.npGeneral.midday_dungeon_rank_fixed_id;
            long serialize = _m_lShowSerialize;

            //先设置空，等待请求数据
            ALUGUICommon.setLabelTxt(wnd.txtRankScore, TextTranslate.instance.getLanguage(TransKeyConst.midddayDungeon_myScore_str_num ,""));
            ALUGUICommon.setLabelTxt(wnd.txtSelfRank,  TextTranslate.instance.getLanguage(TransKeyConst.midddayDungeon_myRank_str , ""));
            ALUGUICommon.setGameObjEnable(wnd.noScoreShowList, true);
            ALUGUICommon.setGameObjEnable(wnd.noScoreHideList, false);
            _m_rankGrid?.hideWnd();

            //请求自己的数据
            NPPlayer.instance.rankCommonComp.reqInRankPlayerInfoByCid(rankFixId, NPPlayer.instance.playerInfo.CID, _info =>
            {
                if (wnd == null || !isShow || serialize != _m_lShowSerialize || _info == null)
                    return;

                //有无积分时切换显示，
                bool noScore = _info.getScore() > 0;
                ALUGUICommon.setGameObjEnable(wnd.noScoreShowList, noScore);
                ALUGUICommon.setGameObjEnable(wnd.noScoreHideList, !noScore);
                //设置自己的排名分数
                ALUGUICommon.setLabelTxt(wnd.txtRankScore, TextTranslate.instance.getLanguage(TransKeyConst.midddayDungeon_myScore_str_num ,_info.getScore().ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT)));
                ALUGUICommon.setLabelTxt(wnd.txtSelfRank,  TextTranslate.instance.getLanguage(TransKeyConst.midddayDungeon_myRank_str ,_info.getRank()));
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
                _m_rankGrid?.setInfo(showInfoList, false);

                GGUIWndRankFixedTopPlayerInfo topPlayerInfoWnd = null;
                if (_m_lTopPlayerInfoList != null)
                {
                    for (int i = 0; i < _m_lTopPlayerInfoList.Count; i++)
                    {
                        topPlayerInfoWnd = _m_lTopPlayerInfoList[i];
                        if(topPlayerInfoWnd == null)
                            continue;
                        
                        topPlayerInfoWnd.showWnd();
                        topPlayerInfoWnd.setInfo(showInfoList.SafeGet(i));
                    }
                }
            });
        }
    }
}
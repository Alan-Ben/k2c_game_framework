using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 联盟冲榜详情奖励页面
    /// </summary>
    public class GGUIWndGuildRankRushDetailRewardPage : _ATALBasicLoadPrefabSubUIWnd<GGUIMonoGuildRankRushDetailRewardPage>
    {
        private string _m_sAssetPath;//窗口对象的资源 加载路径
        private string _m_sObjName;//窗口对象的资源 名字
        private long _m_lSelfRank;//自己的排名
        private GGUIWndGuildRankRushDetailRewardContainer _m_wRewardContainer;//奖励列表

        public GGUIWndGuildRankRushDetailRewardPage(NPCommonAssetPathInfo _assetPathInfo, Transform _parent) : base(_parent)
        {
            _m_sAssetPath = _assetPathInfo.asset_path;
            _m_sObjName = _assetPathInfo.obj_name;
        }

        protected override string _monoAssetPath { get { return _m_sAssetPath; } }
        protected override string _monoObjName { get { return _m_sObjName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        
        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wRewardContainer?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wRewardContainer?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_wRewardContainer?.discard();
            _m_wRewardContainer = null;
            _m_lSelfRank = 0;
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoRewardContainer != null)
                _m_wRewardContainer = new GGUIWndGuildRankRushDetailRewardContainer(wnd.monoRewardContainer);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_rankId"></param>
        public void setInfo(long _rankId, long _selfRank)
        {
            if (wnd == null)
                return;

            List<GActivityRankRewardRefObj> rewardList = new List<GActivityRankRewardRefObj>();
            GRefdataCoreMgr.instance.getRankRewardRefList(_rankId, rewardList);
            _m_lSelfRank = _selfRank;

            _m_wRewardContainer?.showWnd();
            _m_wRewardContainer?.setInfo(rewardList);
            _m_wRewardContainer?.refreshBySelfRank(_m_lSelfRank);
        }

        /// <summary>
        /// 根据自己的排名刷新显隐
        /// </summary>
        /// <param name="_rank"></param>
        public void setSelfRank(long _rank)
        {
            _m_lSelfRank = _rank;
            _m_wRewardContainer?.refreshBySelfRank(_rank);
        }
    }
}
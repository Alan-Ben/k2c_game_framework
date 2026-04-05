using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIWndEveningDungeonRankRewardPage : _ATALBasicLoadPrefabSubUIWnd<GGUIMonoEveningDungeonRankRewardPage>
    {
        private string _m_sAssetPath;//窗口对象的资源 加载路径
        private string _m_sObjName;//窗口对象的资源 名字
        
        private long _m_lSelfRank;//自己的排名
        
        private NPGGUIWndCommonItemContainer _m_wRoundFirstKillReward;//每轮首次击杀奖励列表
        private NPGGUIWndCommonItemContainer _m_wNotFirstKillReward;//非首次击杀奖励列表
        private GGUIWndEveningDungeonRankRewardContainer _m_wRankRewardContainer;//奖励列表

        public GGUIWndEveningDungeonRankRewardPage(NPCommonAssetPathInfo _assetPathInfo, Transform _parent) : base(_parent)
        {
            _m_sAssetPath = _assetPathInfo?.asset_path;
            _m_sObjName = _assetPathInfo?.obj_name;
        }

        protected override string _monoAssetPath { get { return _m_sAssetPath; } }
        protected override string _monoObjName { get { return _m_sObjName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        
        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wRoundFirstKillReward?.hideWnd();
            _m_wNotFirstKillReward?.hideWnd();
            _m_wRankRewardContainer?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wRoundFirstKillReward?.resetWnd();
            _m_wNotFirstKillReward?.resetWnd();
            _m_wRankRewardContainer?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_wRoundFirstKillReward?.discard();
            _m_wRoundFirstKillReward = null;

            _m_wNotFirstKillReward?.discard();
            _m_wNotFirstKillReward = null;
            
            _m_wRankRewardContainer?.discard();
            _m_wRankRewardContainer = null;
            _m_lSelfRank = 0;
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.roundFirstKillRewardContainer != null)
                _m_wRoundFirstKillReward = new NPGGUIWndCommonItemContainer(wnd.roundFirstKillRewardContainer);
            
            if (wnd.notFirstKillRewardContainer != null)
                _m_wNotFirstKillReward = new NPGGUIWndCommonItemContainer(wnd.notFirstKillRewardContainer);
            
            if (wnd.monoRewardContainer != null)
                _m_wRankRewardContainer = new GGUIWndEveningDungeonRankRewardContainer(wnd.monoRewardContainer);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_rankId"></param>
        public void setInfo(long _selfRank)
        {
            if (wnd == null)
                return;

            List<EveningDungeonRankRewardRefObj> rewardList = new List<EveningDungeonRankRewardRefObj>();
            rewardList.AddRange(GRefdataCoreMgr.instance.eveningDungeonRankRewardRefCore.refList);
            _m_lSelfRank = _selfRank;

            if (_m_wRoundFirstKillReward != null)
            {
                _m_wRoundFirstKillReward.showWnd();
                _m_wRoundFirstKillReward.showItemList(GRefdataCoreMgr.instance.npGeneral.evening_dungeon_day_first_kill_boss_reward);
            }

            if (_m_wNotFirstKillReward != null)
            {
                _m_wNotFirstKillReward.showWnd();
                _m_wNotFirstKillReward.showItemList(GRefdataCoreMgr.instance.npGeneral.evening_dungeon_day_kill_boss_reward);
            }
            
            if (_m_wRankRewardContainer != null)
            {
                _m_wRankRewardContainer.showWnd();
                _m_wRankRewardContainer.setInfo(rewardList);
                _m_wRankRewardContainer.refreshBySelfRank(_m_lSelfRank);       
            }
        }

        /// <summary>
        /// 根据自己的排名刷新显隐
        /// </summary>
        /// <param name="_rank"></param>
        public void setSelfRank(long _rank)
        {
            _m_lSelfRank = _rank;
            _m_wRankRewardContainer?.refreshBySelfRank(_rank);
        }
    }
}
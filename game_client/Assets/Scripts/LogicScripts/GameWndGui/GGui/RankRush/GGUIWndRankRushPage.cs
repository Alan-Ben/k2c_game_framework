using System.Collections.Generic;
using ALPackage;
using Common.ActivityEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 限时冲榜主界面
    /// </summary>
    public class GGUIWndRankRushPage : _ATALBasicLoadPrefabSubUIWnd<GGUIMonoRankRushPage>
    {
        private string _m_sAssetPath;//窗口对象的资源 加载路径
        private string _m_sObjName;//窗口对象的资源 名字
        private GGUIWndRankRushContainer _m_wRankRushContainer;//限时冲榜列表

        public GGUIWndRankRushPage(NPCommonAssetPathInfo _assetPathInfo, Transform _parent) : base(_parent)
        {
            _m_sAssetPath = _assetPathInfo.asset_path;
            _m_sObjName = _assetPathInfo.obj_name;
        }

        protected override string _monoAssetPath { get { return _m_sAssetPath; } }
        protected override string _monoObjName { get { return _m_sObjName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        
        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.ON_COMMON_ACTIVITY_ADD, _onActivityChg);
            WinMsg.RegisterMsg(WinMsgType.ON_COMMON_ACTIVITY_UPDATE, _onActivityChg);
            WinMsg.RegisterMsg(WinMsgType.ON_COMMON_ACTIVITY_STATE_CHG, _onActivityChg);
            WinMsg.RegisterMsg(WinMsgType.ON_COMMON_ACTIVITY_CLOSE, _onActivityChg);

            _m_wRankRushContainer?.showWnd();
            _refreshWnd();
            _playAudio();
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_COMMON_ACTIVITY_ADD, _onActivityChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_COMMON_ACTIVITY_UPDATE, _onActivityChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_COMMON_ACTIVITY_STATE_CHG, _onActivityChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_COMMON_ACTIVITY_CLOSE, _onActivityChg);

            _m_wRankRushContainer?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wRankRushContainer?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_wRankRushContainer?.discard();
            _m_wRankRushContainer = null;
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoRankRushContainer != null)
                _m_wRankRushContainer = new GGUIWndRankRushContainer(wnd.monoRankRushContainer);
        }

        //刷新窗口
        private void _refreshWnd()
        {
            List<ActivityRankRushInfo> rankRushInfoList = new List<ActivityRankRushInfo>();
            NPPlayer.instance.commonActivityComp.getActivityRankRushInfoList(rankRushInfoList, (_info) =>
            {
                return _info != null && _info.isShowInRankRushWnd;
            });
            rankRushInfoList?.Sort(_sortRankRushList);

            _m_wRankRushContainer?.setInfo(rankRushInfoList);
        }

        //播放有列表时展示音效
        private void _playAudio()
        {
            if (wnd == null)
                return;

            List<ActivityRankRushInfo> rankRushInfoList = new List<ActivityRankRushInfo>();
            NPPlayer.instance.commonActivityComp.getActivityRankRushInfoList(rankRushInfoList, (_info) =>
            {
                return _info != null && _info.isShowInRankRushWnd;
            });
            if (rankRushInfoList.Count > 0 && wnd.haveListAudioId > 0)
                PlayAudioMgr.instance.playClip(wnd.haveListAudioId);
        }

        //排序限时冲榜列表：冲榜阶段>结算阶段>领奖阶段，同阶段根据配表顺序展示
        private int _sortRankRushList(ActivityRankRushInfo _a, ActivityRankRushInfo _b)
        {
            if (_a == null || _a.activityInfo == null || _a.activityRankRushRefObj == null || _b == null || _b.activityInfo == null || _b.activityRankRushRefObj == null)
                return 0;

            EActivityState stateA = _a.activityInfo.activityState;
            EActivityState stateB = _b.activityInfo.activityState;
            if(stateA != stateB)
                return stateA.CompareTo(stateB);

            return _a.activityRankRushRefObj.id.CompareTo(_b.activityRankRushRefObj.id);
        }

        //活动变更
        private void _onActivityChg(params object[] _objects)
        {
            if (_objects == null || _objects.Length < 2)
                return;

            long activityId = (long) _objects[0];
            List<long> rankRushList = GRefdataCoreMgr.instance.getRankRushActivityIdList();

            //是否是冲榜活动
            if(rankRushList != null && rankRushList.Contains(activityId))
                _refreshWnd();
        }
    }
}
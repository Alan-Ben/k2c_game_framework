using System.Collections.Generic;
using ALPackage;
using Common.ActivityEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 冲榜获取途径界面
    /// </summary>
    public class GGUIWndRankRushAccess : _ANPGGUIBasicWnd<GGUIMonoRankRushAccess>
    {
        private static GGUIWndRankRushAccess _g_instance;
        public static GGUIWndRankRushAccess instance
        {
            get
            {
                if(_g_instance == null)
                    _g_instance = new GGUIWndRankRushAccess();
                return _g_instance;
            }
        }

        //冲榜实例id
        private long _m_lInstanceId;
        //获取途径列表
        private GGUIWndRankRushAccessContainer _m_accessContainer;

        public GGUIWndRankRushAccess() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoRankRushAccess.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoRankRushAccess.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.ON_COMMON_ACTIVITY_STATE_CHG, _onActivityStateChg);
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_COMMON_ACTIVITY_STATE_CHG, _onActivityStateChg);
            _m_accessContainer?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_accessContainer?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_accessContainer?.discard();
            _m_accessContainer = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoAccseeContainer != null)
                _m_accessContainer = new GGUIWndRankRushAccessContainer(wnd.monoAccseeContainer);

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_accessIdList"></param>
        public void setInfo(long _instanceId, List<long> _accessIdList)
        {
            _m_lInstanceId = _instanceId;
            _m_accessContainer?.showWnd();
            _m_accessContainer?.setInfo(_accessIdList);
        }

        //点击关闭
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_RANK_RUSH_ACCESS);
        }

        //活动状态变更
        private void _onActivityStateChg(params object[] _objects)
        {
            if (_objects == null || _objects.Length < 4)
                return;

            long instanceId = (long)_objects[1];
            EActivityState oriState = (EActivityState)_objects[2];
            EActivityState curState = (EActivityState)_objects[3];

            if (_m_lInstanceId == instanceId)
            {
                //如果是关闭或者可丢弃状态，直接关闭界面
                if (curState == EActivityState.CLOSED || curState == EActivityState.CAN_DISCARD)
                {
                    QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_RANK_RUSH_ACCESS);
                }
            }
        }
    }
}
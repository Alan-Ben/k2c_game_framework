
using System;
using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    public class GGUIWndRushExchangeFollowerController : _ATALGGUICommonFollowItemController<GGUIMonoRushExchangeFollower, GGUIWndRushExchangeFollower>
    {
        private readonly GResPathIndex _m_resIndex;
        
        public GGUIWndRushExchangeFollowerController()
        {
            _m_resIndex = new GResPathIndex(9001);
        }
        
        
        public override _AALBasicLoadResIndexInfo followItemIndex { get { return _m_resIndex; } }
        

        protected override GGUIWndRushExchangeFollower _createItemWnd(GGUIMonoRushExchangeFollower _wndMono)
        {
            GGUIWndRushExchangeFollower wnd = new GGUIWndRushExchangeFollower(_wndMono);
            wnd.showWnd();
            return wnd;
        }
        
        public void refreshWnd()
        {
            wnd?.refreshWnd();
        }
        
    }
    public class GGUIWndRushExchangeFollower : _ATALGGUIWndCommonFollowItem<GGUIMonoRushExchangeFollower>
    {
        private int _m_timeDownSer;
        private long _m_timeCdMs;

        private ERushExchangeState _m_state;
        private Material _m_txtMeshProMat;
        private static readonly int _g_glowColor = Shader.PropertyToID("_GlowColor");

        public GGUIWndRushExchangeFollower(GGUIMonoRushExchangeFollower _wnd) 
            : base(_wnd)
        {
            initWnd();
        }
        
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
        }

        protected override void _onDiscard()
        {
            if (wnd == null)
                return;
            _m_txtMeshProMat = null;
        }

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsgAct(WinMsgType.ON_RUSH_EXCHANGE_CHG, _refreshWnd);
            refreshWnd();
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.ON_RUSH_EXCHANGE_CHG, _refreshWnd);
            _m_timeDownSer = ALSerializeOpMgr.next();
        }

        protected override void _onReset()
        {
        }

        
        public void refreshWnd()
        {
            if (wnd == null) return;
            
            if (NPPlayer.instance.rushExchangeComp.rushExchangeState == ERushExchangeState.Lock)
            {
                NPPlayer.instance.rushExchangeComp.checkRefresh();
            }
            else
            {
                RushExchangeRefObj rushExchangeRef = GRefdataCoreMgr.instance.rushExchangeRefCore.getRef(NPPlayer.instance.rushExchangeComp.rushExchangeRefId);
                if (rushExchangeRef == null)
                    NPPlayer.instance.rushExchangeComp.checkRefresh();
            }

            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if (wnd == null) return;
            _m_timeCdMs = NPPlayer.instance.rushExchangeComp.rushExchangeShowTime;
            
            _m_state = NPPlayer.instance.rushExchangeComp.rushExchangeState;
            _m_timeDownSer = ALSerializeOpMgr.next();
            _refreshTimeDown(_m_timeDownSer);
            if (_m_txtMeshProMat == null && wnd.txtMeshProTime != null && wnd.txtMeshProTime.fontSharedMaterial != null)
            {
                _m_txtMeshProMat = wnd.txtMeshProTime.fontMaterial;
                // wnd.txtMeshProTime.fontMaterial = _m_txtMeshProMat;
            }

            if (_m_txtMeshProMat != null)
            {
                switch (_m_state)
                {
                    case ERushExchangeState.Lock:
                        break;
                    case ERushExchangeState.Ready:
                        _m_txtMeshProMat.SetColor(_g_glowColor, wnd.stateReadyCol);
                        break;
                    case ERushExchangeState.Exchange:
                        _m_txtMeshProMat.SetColor(_g_glowColor, wnd.stateExchangeCol);
                        break;
                    case ERushExchangeState.Reward:
                        _m_txtMeshProMat.SetColor(_g_glowColor, wnd.stateRewardCol);
                        break;
                    case ERushExchangeState.Wait:
                        _m_txtMeshProMat.SetColor(_g_glowColor, wnd.stateWaitCol);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            RushExchangeStateInfo.setState(wnd.stateInfoList, _m_state);
        }
        /// <summary>
        /// 刷新倒计时
        /// </summary>
        private void _refreshTimeDown(int _timeDownSer)
        {
            if (null == wnd)
                return;

            if (_timeDownSer != _m_timeDownSer)
                return;

            if (_m_state == ERushExchangeState.Ready && _m_timeCdMs < FpsAndPingMgr.instance.serverTimeTag)
            {
                long dis = FpsAndPingMgr.instance.serverTimeTag - _m_timeCdMs;
                long needAddCount = 0;
                if(GRefdataCoreMgr.instance.npGeneral.rush_exchange_refresh_sec > 0)
                    needAddCount = dis / (GRefdataCoreMgr.instance.npGeneral.rush_exchange_refresh_sec * 1000) + 1;
                _m_timeCdMs += needAddCount * GRefdataCoreMgr.instance.npGeneral.rush_exchange_refresh_sec * 1000;
            }
            if (_m_state == ERushExchangeState.Wait && FpsAndPingMgr.instance.serverTimeTag > _m_timeCdMs)
            {
                NPPlayer.instance.rushExchangeComp.reqRushExchangeRefresh();
            }

            string timeStr = TimeUtil.millisecondsToTime_hms(_m_timeCdMs - FpsAndPingMgr.instance.serverTimeTag);
            ALUGUICommon.setLabelTxt(wnd.txtTime,  timeStr);
            ALUGUICommon.setLabelTxt(wnd.txtMeshProTime, timeStr);

            ALCommonTaskController.CommonActionAddMonoTask(() =>
            {
                _refreshTimeDown(_timeDownSer);
            },1f);
        }
    }
}
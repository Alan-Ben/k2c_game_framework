using ALPackage;
using GC2GS.p004_PlayerOp;
using UnityEngine;

namespace GOE
{
    public class GGUIWndPlayerInfo_DailyRewardShow
    {
        private readonly GGUIMonoPlayerInfoDailyRewardShow _m_wnd;
        private bool _m_bIsShow;
        
        
        public GGUIWndPlayerInfo_DailyRewardShow(GGUIMonoPlayerInfoDailyRewardShow _wnd)
        {
            _m_wnd = _wnd;

            initWnd();
        }
        
        
        public GGUIMonoPlayerInfoDailyRewardShow wnd { get { return _m_wnd; } }


        public void showWnd()
        {
            if (_m_bIsShow)
                return;

            _m_bIsShow = true;

            refreshWnd();
        }
        public void hideWnd()
        {
            if (!_m_bIsShow)
                return;

            _m_bIsShow = false;
        }
        public void resetWnd()
        {
        }
        public void discard()
        {
            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnGainReward, _btnGainRewardDidClick);
        }
        public void initWnd()
        {
            if (wnd == null)
                return;
            
            ALUGUICommon.combineBtnClick(wnd.btnGainReward, _btnGainRewardDidClick);
        }


        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow)
                return;
            
            wnd.setHasReward(NPPlayer.instance.playerInfo.curLevelRef.daily_reward_item.Count != 0, NPPlayer.instance.playerInfo.isDailyRewardDrawn());
        }


        private void _btnGainRewardDidClick(GameObject _)
        {
            if (NPPlayer.instance.playerInfo.isDailyRewardDrawn())
            {
                GGUIWndPlayerDailyRewardPreview.instance.refreshWnd(NPPlayer.instance.playerInfo.curLevelRef.daily_reward_item);
                QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndPlayerDailyRewardPreview.instance, GGUIWndPlayerDailyRewardPreview.instance.showWnd, 
                    EUIQueueStageType.MAIN, string.Empty, true, false);
                return;
            }
            
            NPGSClientListener.sendMsgByLog(new GC2GS_004_019_ReqDrawDailyReward());
        }
    }
}
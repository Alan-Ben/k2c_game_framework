using UnityEngine;
using System.Collections;
using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 卧室界面
    /// </summary>
    public class NPGGUIWndMainRoom : _ANPGGUIBasicResBarWnd<NPGGUIMonoMainRoom>
    {
        private static NPGGUIWndMainRoom _g_instance = new NPGGUIWndMainRoom();
        public static NPGGUIWndMainRoom instance
        {
            get
            {
                if(null == _g_instance)
                    _g_instance = new NPGGUIWndMainRoom();
                return _g_instance;
            }
        }

        //显示序列
        private long _m_lShowSerialize;
        //是否已经发送帧率埋点
        private bool _m_bIsSendFPSReport;

        private NPGGUISubWndMiniChat _m_chatMiniWnd; // 聊天入口
        private GGUIWndCommonSideBar _m_belowSideBar;//下面的收纳栏
        private bool _m_bIsSetBelowSideBar;//是否已经设置收纳栏状态

        private NPGGuiWndTexture _m_wRoomBg;//卧室背景
        
        public NPGGUIWndMainRoom() : base(EALUIWndLayer.NORMAL)
        {
        }

        protected override string _monoAssetPath { get { return NPGGUIMonoMainRoom.assetPath; } }
        protected override string _monoObjName { get { return NPGGUIMonoMainRoom.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
            //显示聊天栏
            if (null != _m_chatMiniWnd)
                _m_chatMiniWnd.showWnd();

            //显示收纳栏
            if (_m_belowSideBar != null)
            {
                _m_belowSideBar.showWnd();
                bool isSpread = false;
                //第一次进入设置收起收纳栏，之后保留玩家操作的状态.
                if (!_m_bIsSetBelowSideBar)
                {
                    isSpread = false;
                    _m_bIsSetBelowSideBar = true;
                }
                else
                    isSpread = AccountSettingMgr.instance.accountSetting.isSideBarSpread(ESideBarType.WIN_MAIN_BELOW);
                
                _m_belowSideBar.setInfo(ESideBarType.WIN_MAIN_BELOW, isSpread, true);

                //根据是否展示弹窗来设置收纳栏的显示状态
                AnnouncementMgr.instance.regInitDone(() =>
                {
                    isSpread = AnnouncementMgr.instance.canShowNotice();
                    if(isSpread)
                        _m_belowSideBar?.setInfo(ESideBarType.WIN_MAIN_BELOW, isSpread, true);
                });
            }

            //由于每日任务有展示条件，某些操作可能会有新的每日任务可展示，这里打开界面时刷新每日任务红点
            NPPlayer.instance.dailyQuestComp.refreshDailyRewardRedTip();
            //由于可能跨天，组件没有实时计算，这里再刷新一次排行榜入口红点
            NPPlayer.instance.rankCommonComp.refreshRedTip();
            
            WinMsg.SendMsg(WinMsgType.MAIN_ROOM_WND_SHOW);

            //发送当前帧率值埋点
            _dealSendFPSReport();
        }

        protected override void _onHideWnd()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();
            
            _m_chatMiniWnd?.hideWnd();
            _m_belowSideBar?.hideWnd();
            _m_wRoomBg?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_chatMiniWnd?.resetWnd();
            _m_belowSideBar?.resetWnd();
            _m_wRoomBg?.discardTexture();

        }

        protected override void _onDiscard()
        {
            if (null != _m_chatMiniWnd)
            {
                _m_chatMiniWnd.discard();
            }


            _m_belowSideBar?.discard();
            _m_belowSideBar = null;

            _m_wRoomBg?.discard();
            _m_wRoomBg = null;
            
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnReturn, _onClickReturnBtn);
            }
        }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            _m_bIsSendFPSReport = false;
            
            if (null != wnd.chatMiniWndMono)
            {
                _m_chatMiniWnd = new NPGGUISubWndMiniChat(wnd.chatMiniWndMono);
            }

            if (null != wnd.belowSideBarMono)
                _m_belowSideBar = new GGUIWndCommonSideBar(wnd.belowSideBarMono);
            
            if(wnd.roomSkinBg != null)
                _m_wRoomBg = new NPGGuiWndTexture(wnd.roomSkinBg);
            
            ALUGUICommon.combineBtnClick(wnd.btnReturn, _onClickReturnBtn);
        }

        //发送当前帧率值埋点
        private void _dealSendFPSReport()
        {
            //已发送不再发送
            if (_m_bIsSendFPSReport)
                return;

            _m_lShowSerialize = ALSerializeOpMgr.next();
            long serialize = _m_lShowSerialize;

            //延迟5秒后如果还在当前界面就发送埋点
            ALCommonActionMonoTask.addMonoTask(() =>
            {
                if (!isShow || serialize != _m_lShowSerialize || _m_bIsSendFPSReport)
                    return;

                _m_bIsSendFPSReport = true;
                // //发送卧室帧率值埋点
                // GCommon.sendStepReport(TraceConst.ROOM_FPS.setMark($"{(int)FpsAndPingMgr.instance.fpsValue}"));
            }, 5f);
        }

        private void _onClickReturnBtn(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_NODE_ROOM);
        }

        public void setTDSceneLoading(bool _isLoading)
        {
            if (wnd == null)
                return;

            if (_isLoading)
            {
                _m_wRoomBg?.showWnd();
                _m_wRoomBg?.setTexture(NPPlayer.instance.roomSkinComp.currentRoomSkinRefObj?.bg_img);
            }
            else
            {
                _m_wRoomBg?.hideWnd();
            }
            
            ALUGUICommon.setGameObjEnable(wnd.onTDLoadingShow, _isLoading);
            ALUGUICommon.setGameObjEnable(wnd.onTDLoadingHide, !_isLoading);
        }
    }
}

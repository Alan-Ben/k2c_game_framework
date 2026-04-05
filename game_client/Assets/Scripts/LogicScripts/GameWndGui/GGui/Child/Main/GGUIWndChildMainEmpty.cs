using ALPackage;
using GC2GS.p014_ChildOp;
using GS2GC.p014_ChildOp;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public partial class GGUIWndChildMain
    {
        public class GGUIWndChildMainEmpty
        {
            [NotNull] private readonly GGUIMonoChildMainEmpty _m_wnd;
            private bool _m_bIsShow;
            private ChildViewMgr _m_viewMgr;
            
            
            public GGUIWndChildMainEmpty([NotNull] GGUIMonoChildMainEmpty _wnd)
            {
                _m_wnd = _wnd;
                initWnd();
            }
            
            
            public void showWnd()
            {
                _m_bIsShow = true;
                
                WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_SCHOOL_GET_CHILD, _onSimulateClickSchoolGetChild);//模拟点击学校招生按钮

                refreshWnd();
            }  
            public void hideWnd()
            {
                WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_SCHOOL_GET_CHILD, _onSimulateClickSchoolGetChild);//模拟点击学校招生按钮
                
                _m_bIsShow = false;
            }
            public void resetWnd()
            {
            }
            public void discard()
            {
                ALUGUICommon.uncombineBtnClick(_m_wnd.btnGoto, _onBtnGotoClick);
                ALUGUICommon.uncombineBtnClick(_m_wnd.btnGetChild, _onBtnGetChild);
            }
            public void initWnd()
            {
                ALUGUICommon.combineBtnClick(_m_wnd.btnGoto, _onBtnGotoClick);
                ALUGUICommon.combineBtnClick(_m_wnd.btnGetChild, _onBtnGetChild);
            }


            public void refreshWnd(ChildViewMgr _viewMgr)
            {
                _m_viewMgr = _viewMgr;
                refreshWnd();
            }
            public void refreshWnd()
            {
                if (!_m_bIsShow)
                    return;
            }
            
            
            private void _onBtnGotoClick(GameObject _)
            {
                NPMesMgr.instance.showTwoBtnMes(
                    TextTranslate.instance.getLanguage(TransKeyConst.child_emptyRoomToConsortTip_none),
                    TextTranslate.instance.getLanguage(TransKeyConst.cancel),
                    null,
                    TextTranslate.instance.getLanguage(TransKeyConst.confirm),
                    () =>
                    {
                        QueueMgr.instance.AddNode(new GNodeConsortChatMain(EConsortChatMainPage.CONSORT_LIST));
                    });
            }
            private void _onBtnGetChild(GameObject _)
            {
                SeatInfo seatInfo = _m_viewMgr?.curSelectSeatInfo;
                if (seatInfo == null)
                    return;
                
                NPGSClientListener.sendRequestByLog(new GC2GS_014_019_ReqRandomGainChild((int) seatInfo.id),
                    new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_014_019_RetRandomGainChild>((_msg) =>
                    {
                        GGUIWndChildMain.instance.refreshWnd();
                        GNodeChild childNode = QueueMgr.instance.findLastNode(UINodeTagConst_Child.C_MAIN_CHILD_NODE) as GNodeChild;
                        childNode?.viewMgr?.refreshClassroomView();
                    }));
            }
            //模拟点击学校招生按钮
            private void _onSimulateClickSchoolGetChild()
            {
                _onBtnGetChild(null);
            }
        }
    }
}
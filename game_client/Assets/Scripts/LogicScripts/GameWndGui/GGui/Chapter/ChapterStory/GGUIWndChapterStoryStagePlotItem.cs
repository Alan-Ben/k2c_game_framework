using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIWndChapterStoryStagePlotItem : _ATALBasicUISubWnd<GGUIMonoChapterStoryStagePlotItem>
    {
        private ChapterStoryStagePlotShowInfo _m_iStagePlotShowInfo;
        
        public GGUIWndChapterStoryStagePlotItem(GGUIMonoChapterStoryStagePlotItem _wnd) : base(_wnd)
        {
        }

        public event Action<ChapterStoryStagePlotShowInfo> onClickDrawRewardBtn;
        
        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;
         
            ALUGUICommon.combineBtnClick(wnd.btnPlay, _onPlayBtnClick);
        }
        
        protected override void _onDiscard()
        {
            onClickDrawRewardBtn = null;
            
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnPlay, _onPlayBtnClick);
            }
        }
        
        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
        }

        protected override void _onReset()
        {
        }

        public void setData(ChapterStoryStagePlotShowInfo _stagePlotShowInfo)
        {
            _m_iStagePlotShowInfo = _stagePlotShowInfo;

            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if(wnd == null || _m_iStagePlotShowInfo == null || _m_iStagePlotShowInfo.stagePlotRefObj == null)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtPlotName, TextTranslate.instance.getLanguage(_m_iStagePlotShowInfo.stagePlotRefObj.name, _m_iStagePlotShowInfo.stagePlotRefObj.name_args));

            EGameCommonUnlockRewardType plotState = _m_iStagePlotShowInfo.getState();
            if(wnd.statInfoList != null)
                NPCommonEnumStatInfo<EGameCommonUnlockRewardType>.setStat(wnd.statInfoList, plotState);
        }

        /// <summary>
        /// 点击查看按钮
        /// </summary>
        private void _onPlayBtnClick(GameObject _go)
        {
            if (_m_iStagePlotShowInfo == null || _m_iStagePlotShowInfo.stagePlotRefObj == null)
                return;
            
            EGameCommonUnlockRewardType plotState = _m_iStagePlotShowInfo.getState();
            if(plotState == EGameCommonUnlockRewardType.LOCK)
                return;

            if (plotState == EGameCommonUnlockRewardType.UNLOCK_HAS_GET)
            {
                NPUINoticeMgr.instance.addDealer(new NoticeDealer_EnterDialogue(_m_iStagePlotShowInfo.stagePlotRefObj.dialog_id));
            }
            else if (plotState == EGameCommonUnlockRewardType.UNLOCK_UN_GET)
            {
                onClickDrawRewardBtn?.Invoke(_m_iStagePlotShowInfo);
            }
            // GCommon.enterDialogueNode(_m_rStagePlotRefObj.dialog_id, null, false);
        }
    }
}
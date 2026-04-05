using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 中文窗口注释
    /// </summary>
    public class GGUIWndConsortMomentInteractionGridItem : _ANPGGUIBasicGridItemWnd<GGUIMonoConsortMomentInteractionGridItem>
    {
        private GGUIWndConsortIconItem _m_consortCardItem;
        private GGUIWndMomentsImageItem _m_momentsImageItem;

        private ConsortMomentConsortAICommentData _m_msgInfo;
        // <AutoGen:WndDeclaration>
        // </AutoGen:WndDeclaration>
        
        public GGUIWndConsortMomentInteractionGridItem(GGUIMonoConsortMomentInteractionGridItem _wnd) : base(_wnd)
        {
            initWnd();
        }
        
        protected override void _onShowWnd()
        {
            _refreshWnd();
        }
    
        protected override void _onHideWnd()
        {
        }
    
        protected override void _onReset()
        {
        }
    
        protected override void _onDiscard()
        {
            _m_msgInfo = null;
            _m_consortCardItem?.discard();
            _m_consortCardItem = null;
        }
    
        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            
            if (wnd.consortIconItem != null)
                _m_consortCardItem = new GGUIWndConsortIconItem(wnd.consortIconItem);
            
            if (wnd.momentsImageItem != null) 
                _m_momentsImageItem = new GGUIWndMomentsImageItem(wnd.momentsImageItem);
            
            ALUGUICommon.combineBtnClick(wnd.btnClickDetail, _onBtnDetailClick);
        }

        private void _onBtnDetailClick(GameObject _obj)
        {
            if (_m_msgInfo != null)
            {
                GGUIWndConsortMomentDetail.instance.setInfo(_m_msgInfo.momentInstanceId);
                QueueMgr.instance.addNode_InGame_MainUIMainWnd(GGUIWndConsortMomentDetail.instance, UINodeTagConst.C_CONSORT_CHAT_MOMENT_DETAIL, null, null, 0);
            }
        }

        protected override void _resetGridItem()
        {
        }

        /// <summary>
        /// 设置显示信息
        /// </summary>
        public void setInfo(ConsortMomentConsortAICommentData _data)
        {
            _m_msgInfo = _data;
            _refreshWnd();
        }

        /// <summary>
        /// 刷新界面显示
        /// </summary>
        private void _refreshWnd()
        {
            if (null == wnd)
                return;
            
            if(_m_msgInfo == null)
                return;
            
            ALUGUICommon.setLabelTxt(wnd.txtContent, _m_msgInfo.consortCommentData.getContent());
            ALUGUICommon.setLabelTxt(wnd.txtTime, _m_msgInfo.consortCommentData.commentTimeTag);

            GGottenConsortInfo consortInfo = NPPlayer.instance.consortComp.getConsortInfo(_m_msgInfo.consortCommentData.senderId);
            _m_consortCardItem?.showWnd();
            _m_consortCardItem?.setInfo(consortInfo, 0);

            ConsortMomentSaver momentSaver = AccountSettingMgr.instance.consortMomentSaverMgr.getMomentSaver(_m_msgInfo.momentInstanceId);
            if (_m_momentsImageItem != null && momentSaver != null && momentSaver.momentData != null && momentSaver.momentData.imageList != null && momentSaver.momentData.imageList.Count > 0)
            {
                _m_momentsImageItem.setInfo(new List<ConsortMomentImageData>(){momentSaver.momentData.imageList[0]}, 0);
            }
        }
        
        // <AutoGen:Method>
        
        // </AutoGen:Method>
    }
}


using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUISubWndMarsExplorePvPLogGridItem : _ATALUGUIBasicGridItemWnd<GGUIMonoMarsExplorePvPLogGridItem>
    {
        private GGUIWndMarsExplorePvPLog._ALogData _m_logData;
        private List<GGUIWndMarsExplorePvPLog._ALogData> _m_logDataList;
        
        private NPGGuiWndTexture _m_iconWnd;
        private NPGGUIWndCommonItem _m_itemWnd;
        private int _m_showSerialize;


        public GGUISubWndMarsExplorePvPLogGridItem(GGUIMonoMarsExplorePvPLogGridItem _wnd)
            : base(_wnd)
        {
            initWnd();
        }


        protected override void _onShowWnd()
        {
            _m_iconWnd?.showWnd();
            _m_itemWnd?.showWnd();
            
            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_iconWnd?.hideWnd();
            _m_itemWnd?.hideWnd();

            _m_showSerialize = ALSerializeOpMgr.next();
        }
        protected override void _onReset()
        {
            _m_iconWnd?.discardTexture();
            _m_itemWnd?.resetWnd();
        }
        protected override void _onDiscard()
        {
            _m_iconWnd?.discard();
            _m_iconWnd = null;
            _m_itemWnd?.discard();
            _m_itemWnd = null;

            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnDetail, _onBtnDetailClick);
        }
        protected override void _resetGridItem()
        {
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            if (wnd.imgIcon != null)
                _m_iconWnd = new NPGGuiWndTexture(wnd.imgIcon);
            if (wnd.monoItem != null)
                _m_itemWnd = new NPGGUIWndCommonItem(wnd.monoItem);
            
            ALUGUICommon.combineBtnClick(wnd.btnDetail, _onBtnDetailClick);
        }


        public void refreshWnd(GGUIWndMarsExplorePvPLog._ALogData _logData, List<GGUIWndMarsExplorePvPLog._ALogData> _logDataList)
        {
            _m_logData = _logData;
            _m_logDataList = _logDataList;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_logData == null)
                return;

            int serialize = _m_showSerialize;
            
            wnd.setState(_m_logData.isSuccess, _m_logData.showItem != null, _m_logData.needShowDetailBtn);
            _m_iconWnd?.setTexture(_m_logData.icon);
            _m_itemWnd?.setItem(_m_logData.showItem);
            ALUGUICommon.setLabelTxt(wnd.txtTitle, _m_logData.title);
            ALUGUICommon.setLabelTxt(wnd.txtTime, TimeUtil.DateTime2StringMDYHMS(TimeUtil.FromUTCMilliseconds(_m_logData.getCreatedAt())));
            ALUGUICommon.setLabelTxt(wnd.txtDesc, string.Empty);
            _m_logData.getDesc(_desc =>
            {
                if (serialize != _m_showSerialize)
                    return;
                
                ALUGUICommon.setLabelTxt(wnd.txtDesc, _desc);
            });
        }


        private void _onBtnDetailClick(GameObject _)
        {
            GGUIWndMarsExplorePvPDetail.instance.refreshWnd(_m_logData, _m_logDataList);
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndMarsExplorePvPDetail.instance, GGUIWndMarsExplorePvPDetail.instance.showWnd, UINodeTagConst.C_MARS_EXPLORE_PVP_DETAIL);
        }
    }
}

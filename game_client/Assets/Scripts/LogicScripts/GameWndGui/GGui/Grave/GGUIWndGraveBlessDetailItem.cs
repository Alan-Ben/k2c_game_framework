using System;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 祝福详情界面
    /// </summary>
    public class GGUIWndGraveBlessDetailContainerItem : _ATALBasicUISubWnd<GGUIMonoGraveBlessDetailContainerItem>
    {
        // <AutoGen:WndDeclaration>
        
        // </AutoGen:WndDeclaration>
        
        private NPPlayerBuffInfo _m_buffInfo;
        private long _m_buffId;
        public GGUIWndGraveBlessDetailContainerItem(GGUIMonoGraveBlessDetailContainerItem _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
            _refreshWnd();
            // <AutoGen:_onShowWnd>
            
            // </AutoGen:_onShowWnd>
        }
    
        protected override void _onHideWnd()
        {
            // <AutoGen:_onHideWnd>
            
            // </AutoGen:_onHideWnd>
        }
    
        protected override void _onReset()
        {
            // <AutoGen:_onReset>
            
            // </AutoGen:_onReset>
        }
    
        protected override void _onDiscard()
        {
            // <AutoGen:_onDiscard>
            
            // </AutoGen:_onDiscard>
        }
    
        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            // <AutoGen:_onWndInitDone>
            
            // </AutoGen:_onWndInitDone>
        }

        public void setInfo(long _buffId, NPPlayerBuffInfo _buffInfo = null)
        {
            _m_buffId = _buffId;
            _m_buffInfo = _buffInfo;
            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if (null == wnd)
                return;
            // <AutoGen:_refreshWnd>
            // <UserCode name="txtDesc">
            NPPlayerBuffRefObj buffRefObj = GRefdataCoreMgr.instance.playerBuffMap.getRef(_m_buffId);
            if(buffRefObj == null)
                return;
            if (_m_buffInfo != null)
            {
                ALUGUICommon.setLabelTxt(wnd.txtTitle, TextTranslate.instance.getLanguage(buffRefObj.name));
                ALUGUICommon.setLabelTxt(wnd.txtDesc, TextTranslate.instance.getLanguage(
                    TransKeyConst.grave_bless_on_desc, 
                    TextTranslate.instance.getLanguage(buffRefObj.desc, buffRefObj.desc_args),
                    _m_buffInfo.layer, buffRefObj.max_layer));
            }
            else
            {
                ALUGUICommon.setLabelTxt(wnd.txtTitle, TextTranslate.instance.getLanguage(buffRefObj.name));
                ALUGUICommon.setLabelTxt(wnd.txtDesc, TextTranslate.instance.getLanguage(
                    TransKeyConst.grave_bless_off_desc, 
                    TextTranslate.instance.getLanguage(buffRefObj.desc, buffRefObj.desc_args)));
            }
            // </UserCode>
            // </AutoGen:_refreshWnd>
        }
        
        // <AutoGen:Method>
        
        // </AutoGen:Method>
    }
}

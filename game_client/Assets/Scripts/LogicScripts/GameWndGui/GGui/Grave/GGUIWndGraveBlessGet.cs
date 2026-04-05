using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 杰出者祝福
    /// </summary>
    public class GGUIWndGraveBlessGet : _ATALBasicUIWnd<GGUIMonoGraveBlessGet>
    {
        private static GGUIWndGraveBlessGet _g_instance = new GGUIWndGraveBlessGet();
    
        public static GGUIWndGraveBlessGet instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndGraveBlessGet();
                return _g_instance;
            }
        }
        
        // <AutoGen:WndDeclaration>
        
        // </AutoGen:WndDeclaration>
        private long _m_buffId;
        private Action _m_setDealerDone;

        public GGUIWndGraveBlessGet() : base(EALUIWndLayer.ADDITION)
        {
        }
    
        protected override string _monoAssetPath { get => GGUIMonoGraveBlessGet.assetPath; }
        protected override string _monoObjName { get => GGUIMonoGraveBlessGet.objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }
    
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
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickbtnClose);
            // </AutoGen:_onDiscard>
        }
    
        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            // <AutoGen:_onWndInitDone>
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickbtnClose);
            // </AutoGen:_onWndInitDone>
        }
        
        public void setInfo(long _buffId, Action _setDealerDone)
        {
            _m_buffId = _buffId;
            _m_setDealerDone = _setDealerDone;
            _refreshWnd();
        }
        
        /// <summary>
        /// 刷新界面
        /// </summary>
        private void _refreshWnd()
        {
            if(null == wnd)
                return;

            NPPlayerBuffInfo buffInfo = NPPlayer.instance.playerBuffComp.lookup(_m_buffId);
            NPPlayerBuffRefObj buffRefObj = GRefdataCoreMgr.instance.playerBuffMap.getRef(_m_buffId);

            if(buffRefObj == null)
                return;
            if (buffInfo != null)
            {
                ALUGUICommon.setLabelTxt(wnd.txtBlessDesc, TextTranslate.instance.getLanguage(
                    TransKeyConst.grave_bless_get_desc, 
                    TextTranslate.instance.getLanguage(buffRefObj.desc, buffRefObj.desc_args),
                    buffInfo.layer, buffRefObj.max_layer));
            }
            
            // <AutoGen:_refreshWnd>
            // <UserCode name="txtBlessName">
            ALUGUICommon.setLabelTxt(wnd.txtBlessName, TextTranslate.instance.getLanguage(buffRefObj.name));
            // </UserCode>
            // <UserCode name="txtBlessDesc">
            // </UserCode>
            // </AutoGen:_refreshWnd>
        }
        
        // <AutoGen:Method>
        // 关闭按钮点击事件
        private void _onClickbtnClose(GameObject go)
        {
            // <UserCode name="btnClose">
            _m_setDealerDone?.Invoke();
            // </UserCode>
        }
        // </AutoGen:Method>
    }
}
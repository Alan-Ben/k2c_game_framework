using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 祝福详情界面
    /// </summary>
    public class GGUIWndGraveBlessDetail : _ATALBasicUIWnd<GGUIMonoGraveBlessDetail>
    {
        private static GGUIWndGraveBlessDetail _g_instance = new GGUIWndGraveBlessDetail();
    
        public static GGUIWndGraveBlessDetail instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndGraveBlessDetail();
                return _g_instance;
            }
        }
        
        private GGUIWndGraveBlessDetailContainer _m_itemContainerWnd;
        private List<object> _m_itemDataList = new List<object>();

        // <AutoGen:WndDeclaration>
        // </AutoGen:WndDeclaration>

        public GGUIWndGraveBlessDetail() : base(EALUIWndLayer.ADDITION)
        {
        }
    
        protected override string _monoAssetPath { get => GGUIMonoGraveBlessDetail.assetPath; }
        protected override string _monoObjName { get => GGUIMonoGraveBlessDetail.objName; }
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
            _m_itemContainerWnd?.discard();
            _m_itemContainerWnd = null;
            // <AutoGen:_onDiscard>
            // </AutoGen:_onDiscard>
        }
    
        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
                
            if (wnd.itemContainer != null)
                _m_itemContainerWnd = new GGUIWndGraveBlessDetailContainer(wnd.itemContainer);
                
            // <AutoGen:_onWndInitDone>
            // </AutoGen:_onWndInitDone>
        }
    
        /// <summary>
        /// 刷新界面
        /// </summary>
        private void _refreshWnd()
        {
            if(null == wnd)
                return;
                
            _m_itemContainerWnd?.showWnd();
            _m_itemContainerWnd?.showItemList(GRefdataCoreMgr.instance.npGeneral.grave_buff_list);
            
            // <AutoGen:_refreshWnd>
            // </AutoGen:_refreshWnd>
        }
        
        // <AutoGen:Method>
        
        // </AutoGen:Method>
    }
}

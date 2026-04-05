using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 中文窗口注释
    /// </summary>
    public class GGUIWndConsortMomentInteraction : _ATALBasicUIWnd<GGUIMonoConsortMomentInteraction>
    {
        private static GGUIWndConsortMomentInteraction _g_instance = new GGUIWndConsortMomentInteraction();
    
        public static GGUIWndConsortMomentInteraction instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndConsortMomentInteraction();
                return _g_instance;
            }
        }
        
        private GGUIWndConsortMomentInteractionGrid _m_itemGridWnd;
        private List<ConsortMomentConsortAICommentData> _m_itemDataList = new List<ConsortMomentConsortAICommentData>();

        // <AutoGen:WndDeclaration>
        // </AutoGen:WndDeclaration>

        public GGUIWndConsortMomentInteraction() : base(EALUIWndLayer.NORMAL)
        {
        }
    
        protected override string _monoAssetPath { get => GGUIMonoConsortMomentInteraction.assetPath; }
        protected override string _monoObjName { get => GGUIMonoConsortMomentInteraction.objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }
    
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
            _m_itemGridWnd?.discard();
            _m_itemGridWnd = null;
            if (_m_itemDataList != null) 
                _m_itemDataList.Clear();
            
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onBtnCloseClick);
        }
    
        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
                
            if (wnd.itemGrid != null)
                _m_itemGridWnd = new GGUIWndConsortMomentInteractionGrid(wnd.itemGrid);
            
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onBtnCloseClick);
        }

        public void setInfo(List<ConsortMomentConsortAICommentData> _itemDataList)
        {
            _m_itemDataList = _itemDataList;
        }
    
        /// <summary>
        /// 刷新界面
        /// </summary>
        private void _refreshWnd()
        {
            if(null == wnd)
                return;
                
            _m_itemGridWnd?.showWnd();
            _m_itemGridWnd?.showItemList(_m_itemDataList);
        }
        
        // <AutoGen:Method>
        
        // </AutoGen:Method>
        

        private void _onBtnCloseClick(GameObject _obj)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_CONSORT_CHAT_MOMENT_INTERATION);
        }

    }
}